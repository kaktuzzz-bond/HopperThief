using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Common
{
    public abstract class LifetimeContext : LifetimeScope
    {
        [SerializeField]
        private ScriptableObjectInstaller[] installers;

        protected override void Configure(IContainerBuilder builder)
        {
            InstallScriptableObjects(builder);

            Install(builder);
        }

        protected virtual void Install(IContainerBuilder builder)
        {
        }

        private void InstallScriptableObjects(IContainerBuilder builder)
        {
            if (installers == null || 
                installers.Length == 0) return;

            foreach (var installer in installers)
                installer.Configure(builder);
        }
    }
}