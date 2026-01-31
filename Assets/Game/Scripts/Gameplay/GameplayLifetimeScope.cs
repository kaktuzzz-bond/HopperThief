using Game.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Gameplay
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private ScriptableObjectInstaller[] installers;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.InstallScriptableObjects(installers);
        }
    }
}