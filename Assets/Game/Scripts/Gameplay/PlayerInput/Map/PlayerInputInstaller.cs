using Game.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Gameplay
{
    [CreateAssetMenu(
        fileName = "PLayerInputInstaller",
        menuName = "Game/SO Installers/Player Input Installer")]
    public class PlayerInputInstaller : ScriptableObjectInstaller
    {
        public override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerInput>(Lifetime.Singleton);

            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<PlayerInputBroadcast>();
                entryPoints.Add<PlayerInputController>();
            });
        }
    }
}