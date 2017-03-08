using System;
using TechnoRex.AppDomainContainers.Base.EventArgsBase;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Contract.App
{
    public interface IAppPlugin : IDisposable
    {
        event EventHandler<ProgressEventArgs> ProgressEvent;
        event EventHandler<LogEventArgs> LogEvent;
        event EventHandler<ResponseEventArgs> ResponseEvent;

        string Name { get; }
        string Version { get; }

        Result<object> Run(object param);

    }
}