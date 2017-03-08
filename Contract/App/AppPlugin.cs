using System;
using TechnoRex.AppDomainContainers.Base.EventArgsBase;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Contract.App
{
    public abstract class AppPlugin : IAppPlugin
    {
        public event EventHandler<ProgressEventArgs> ProgressEvent = delegate { };
        public event EventHandler<LogEventArgs> LogEvent = delegate { };
        public event EventHandler<ResponseEventArgs> ResponseEvent = delegate { };


        public string Name { get; protected set; }
        public string Version { get; protected set; }


        public abstract Result<object> Run(object param);

        protected AppPlugin(string name, string version)
        {
            this.Name = name;
            this.Version = version;
        }


        protected virtual void Log(string logMessage)
        {
            this.LogEvent(this, new LogEventArgs(logMessage));
        }
        protected virtual void Progress(int progress)
        {
            this.ProgressEvent(this, new ProgressEventArgs(Name, progress));
        }
       


        public void Dispose()
        {
        }



    }
}