using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace Game.Common
{
    public static class LifetimeScopeExt
    {
        public static void InstallScriptableObjects(
            this IContainerBuilder builder, IReadOnlyList<ScriptableObjectInstaller> installers)
        {
            if (installers == null || installers.Count == 0) return;
            
            foreach (var installer in installers)
            {
                
                installer.Configure(builder);
            }
        }
        
       
    }
}