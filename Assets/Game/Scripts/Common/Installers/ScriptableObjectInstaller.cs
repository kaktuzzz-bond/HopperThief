using UnityEngine;
using VContainer;

namespace Game.Common
{
   
    public abstract class ScriptableObjectInstaller : ScriptableObject
    {
        public abstract void Configure(IContainerBuilder builder);
    }
}