using UnityEngine;

namespace Game.Views
{
    public class ThiefView : MonoBehaviour
    {
        [SerializeField]
        private Transform body;

        private const float PLAYER_ANGLE_MAX = 40f;
        
        private Vector3 _scale = Vector3.one;
        public void SetRotation(float angle) =>
            body.localEulerAngles = new Vector3(0, 0, angle * body.localScale.x);

        public void SetPosition(Vector3 position) =>
            body.position = position;

        public void SetScale(Vector3 scale) =>
            body.localScale = scale;

        public void SetPositionAndRotation(Vector3 position, Vector2 factor)
        {
            SetPosition(position);
            
            var angle = Mathf.Lerp(0, PLAYER_ANGLE_MAX * Mathf.Abs(factor.x), factor.y);

            SetRotation(angle);

            _scale.x = factor.x > 0 ? -1f : 1f;
            SetScale(_scale);
        }
    }
}