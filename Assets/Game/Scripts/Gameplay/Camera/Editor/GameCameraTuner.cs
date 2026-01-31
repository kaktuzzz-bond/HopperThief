#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Editor
{
    public class GameCameraTuner : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("Points")]
        private Transform leftPoint;

        [SerializeField, FoldoutGroup("Points")]
        private Transform rightPoint;
        
        [SerializeField, Range(0, 10)]
        private float width = 3.2f;


        private void OnValidate()
        {
            if (leftPoint != null) 
                leftPoint.localPosition = new Vector3(-width, 0, 0);

            if (rightPoint != null) 
                rightPoint.localPosition = new Vector3(width, 0, 0);
            
        }
    }
}
#endif