using VContainer;
using VContainer.Unity;

namespace Game.App
{
    public class RootLifetimeScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }
    }
}