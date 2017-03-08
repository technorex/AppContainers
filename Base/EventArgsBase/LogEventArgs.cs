using System;

namespace TechnoRex.AppDomainContainers.Base.EventArgsBase
{
    public class LogEventArgs : EventArgs
    {
        public string LogMessage { get; private set; }

        public LogEventArgs(string log)
        {
            this.LogMessage = log;
        }

    }
}