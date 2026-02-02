using Game.Common;
using Game.Views;
using UnityEngine;
using VContainer;

namespace Game.Gameplay
{
    [CreateAssetMenu(
        fileName = "PlayerInstaller",
        menuName = "Game/SO Installers/Player Installer")]
    public class PlayerInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private PlayerView playerViewPrefab;

        public override void Configure(IContainerBuilder builder)
        {
            builder.Register<IPlayer, Player>(Lifetime.Singleton);
            
            builder.Register<PlayerViewFactory>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .WithParameter(playerViewPrefab);

            builder.Register<PlayerInputController>(Lifetime.Singleton)
                   .AsImplementedInterfaces();
        }
    }
}