using UnityEngine;

namespace Game.Views
{
    public class GameCameraView: MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        public void SetTargetPosition(Vector3 position)
        {
            target.position = position;
        }
    }
}