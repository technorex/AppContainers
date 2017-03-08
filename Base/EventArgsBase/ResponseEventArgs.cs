using System;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Base.EventArgsBase
{
    public class ResponseEventArgs : EventArgs
    {
        public Result<object> Response { get; set; }
    }
}