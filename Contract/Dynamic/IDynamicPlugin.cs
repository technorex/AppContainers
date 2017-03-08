using System;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Contract.Dynamic
{
    public interface IDynamicPlugin
    {
        Result<object> Run(Func<object, Result<object>> func, object param = null);
    }
}