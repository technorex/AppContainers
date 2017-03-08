using System;

namespace TechnoRex.AppDomainContainers.Host.App
{
    [Serializable()]
    public class AppHostConfig
    {
        public AppDomain Domain { get; set; }
        public string PluginAssemblyPath { get; set; }
        public string PluginRefDir { get; set; }
    }
}