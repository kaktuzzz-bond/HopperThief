using UnityEngine;

namespace Game.Views
{
    [CreateAssetMenu(
        fileName = "StickSettings", 
        menuName = "Game/World/Stick Settings", 
        order = 0)]
    public class StickSettings : ScriptableObject
    {
        [SerializeField]
        private float tensionValue = 2;

        [SerializeField]
        private int vibratoValue = 50;
        
        [SerializeField]
        private float shakeDuration = 0.4f;
        
        public Vector3 Strength => new (0, 0, tensionValue);
        public int VibratoValue => vibratoValue;
        public float ShakeDuration => shakeDuration;
    }
}