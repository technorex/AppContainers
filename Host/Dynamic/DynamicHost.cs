using System;
using System.IO;
using TechnoRex.AppDomainContainers.Contract.Dynamic;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Host.Dynamic
{
    public class DynamicHost : MarshalByRefObject, IDisposable
    {
        public Result<object> Run(
            AppDomain domain,
            Func<object, Result<object>> func,
            object param = null)
        {
            
            var pluginAssembly = new DynamicPlugin().GetType().Assembly;
            domain.AssemblyResolve += delegate(object sender, ResolveEventArgs args)
            {
                var _domain = sender as AppDomain;
                var path = Path.Combine(new FileInfo(pluginAssembly.Location).Directory.FullName,
                    args.Name.Split(',')[0] + ".dll");
                return _domain.Load(path);
            };

            foreach (var type in pluginAssembly.GetTypes())
            {
                if (!type.IsClass) continue;

                if (type.FindInterfaces(delegate(Type t, object filter) { return t == filter as Type; },
                        typeof(IDynamicPlugin)).Length > 0)
                {
                    var _dynamicPlugin = pluginAssembly.CreateInstance(type.ToString()) as IDynamicPlugin;
                    return _dynamicPlugin.Run(func, param);
                }
            }
            return null;
        }


        public void Dispose()
        {
        }

    }
}