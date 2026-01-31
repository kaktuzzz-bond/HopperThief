using VContainer;
using VContainer.Unity;

namespace Game.Gameplay
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
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