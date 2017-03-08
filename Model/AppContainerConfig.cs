using System;

namespace TechnoRex.AppDomainContainers.Model
{
    [Serializable()]
    public class AppContainerConfig
    {
        public string ContainerName { get; set; }

        //Ścieżka do dll
        public string AssemblyPath { get; set; }
        //Ścieżka do pliku app.config jeżeli posiada
        public string PluginConfigPath { get; set; }
        //Ściężka do katalogu z zależnościami
        public string PluginRefDirPath { get; set; }
        //Domyślny katalog domeny
        public string AppDomainDir { get; set; }


        protected AppContainerConfig()
        {
                
        }
        public AppContainerConfig(string containerName, string assemblyPath)
        {
            this.ContainerName = containerName;
            this.AssemblyPath = assemblyPath;
        }

    }
}