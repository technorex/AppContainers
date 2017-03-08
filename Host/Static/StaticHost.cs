using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TechnoRex.AppDomainContainers.Contract.App;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Host.Static
{
    public class StaticHost : MarshalByRefObject, IDisposable
    {
        private Assembly _pluginAssembly;
        private Type _type;
        private List<IAppPlugin> _plugins;



        public StaticHost()
        {
            _plugins = new List<IAppPlugin>();
        }



        public StaticHost SetType(Type pluginType)
        {
            this._type = pluginType;
            return this;
        }


        public Result Initiate(AppDomain domain)
        {
            var pluginAssembly = _type.Assembly;
            domain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                var _domain = sender as AppDomain;
                var path = Path.Combine(new FileInfo(pluginAssembly.Location).Directory.FullName,
                    args.Name.Split(',')[0] + ".dll");
                return _domain.Load(path);
            };


            foreach (var type in pluginAssembly.GetTypes())
            {
                if (!type.IsClass) continue;

                if (type.FindInterfaces(delegate (Type t, object filter) { return t == filter as Type; },
                        typeof(IAppPlugin)).Length > 0)
                {
                    var _staticPlugin = pluginAssembly.CreateInstance(type.ToString()) as IAppPlugin;
                    _plugins.Add(_staticPlugin);
                }
            }

            return new Result();
        }

        public Result<object> Run(string pluginName, object param = null)
        {
            Result<object> serverResponse = new Result<object>();
            var plugin = _plugins.FirstOrDefault(x => x.Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase));
            if (plugin == null) return serverResponse.AddError("Plugin o nazwie [" + pluginName + "] nie istnieje");
            var response = plugin.Run(param);
            serverResponse.AddMessagesFrom(response);
            serverResponse.SetObject(response.Object);
            return serverResponse;
        }


        public void Dispose()
        {
        }
    }
}