using System;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Contract.Dynamic
{
    public class DynamicPlugin : IDynamicPlugin
    {
        public Result<object> Run(Func<object, Result<object>> func, object param = null)
        {
            return func(param);
        }
    }
}