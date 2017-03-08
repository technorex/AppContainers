namespace TechnoRex.AppDomainContainers.Base.EventArgsBase
{
    public class ProgressEventArgs : System.EventArgs
    {
        public string PluginName { get; set; }
        public int Progress { get; }

        public ProgressEventArgs(string pluginName, int progress)
        {
            this.PluginName = pluginName;
            this.Progress = progress;
        }
    }
}