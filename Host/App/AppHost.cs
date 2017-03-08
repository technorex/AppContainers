using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TechnoRex.AppDomainContainers.Base.EventArgsBase;
using TechnoRex.AppDomainContainers.Contract.App;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Host.App
{
    public class AppHost : MarshalByRefObject, IDisposable
    {
        private Assembly _pluginAssembly;
        private List<IAppPlugin> _workerTasks;

        public event EventHandler<ProgressEventArgs> ProgressEvent = delegate { };
        public event EventHandler<LogEventArgs> LogEvent = delegate { };
        public event EventHandler<ResponseEventArgs> ResponseEvent = delegate { };

        public Result Load(AppHostConfig config)
        {
            Result severResponse = new Result();
            _workerTasks = new List<IAppPlugin>();

            try
            {
                _pluginAssembly = config.Domain.Load(AssemblyName.GetAssemblyName(config.PluginAssemblyPath));
                config.Domain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
                {
                    AppDomain domain = sender as AppDomain;
                    string path = Path.Combine(config.PluginRefDir, args.Name.Split(',')[0] + ".dll");
                    return domain.Load(path);
                };


                foreach (Type type in _pluginAssembly.GetTypes())
                {
                    if (!type.IsClass) continue;

                    if (type.FindInterfaces(delegate (Type t, object filter) { return t == filter as Type; }, typeof(IAppPlugin)).Length > 0)
                    {
                        IAppPlugin plugin = _pluginAssembly.CreateInstance(type.ToString()) as IAppPlugin;
                        plugin.LogEvent += LogEvent;
                        plugin.ProgressEvent += ProgressEvent;
                        plugin.ResponseEvent += ResponseEvent;


                        this._workerTasks.Add(plugin);
                    }
                }
            }
            catch (Exception exc)
            {
                severResponse.AddException(exc);
            }
            return severResponse;
        }
        public Result<object> Run(string pluginName, object param = null)
        {
            Result<object> serverResponse = new Result<object>();
            try
            {
                var plugin = _workerTasks.FirstOrDefault(x => x.Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase));
                if (plugin == null) return serverResponse.AddError("Brak pluginu o naziwe [\"" + pluginName + "\"]");
                return serverResponse.AddMessagesFrom(plugin.Run(param));
            }
            catch (Exception exc)
            {
                serverResponse.AddException(exc);
            }
            return serverResponse;
        }


        public void Dispose()
        {
        }
    }

}