using Game.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.App
{
    public class RootLifetimeScope: LifetimeScope
    {
        [SerializeField]
        private ScriptableObjectInstaller[] installers;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.InstallScriptableObjects(installers);
        }
    }
}