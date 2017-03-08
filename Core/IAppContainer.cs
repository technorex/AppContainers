using System;
using TechnoRex.AppDomainContainers.Types;
using TechnoRex.ResultProvider;

namespace TechnoRex.AppDomainContainers.Core
{
    public interface IAppContainer : IDisposable
    {
        string Name { get; }
        AppContainerState State { get; }
        bool IsLoaded { get; }

        Result Load();
        Result UnLoad();

        Result<object> Run(string pluginName, object param = null);

    }
}