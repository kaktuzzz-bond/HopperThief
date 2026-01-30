using UnityEngine;

namespace Game.Views
{
    public class ThiefView : MonoBehaviour
    {
        [SerializeField]
        private Transform body;

        public void SetRotation(float angle) =>
            body.localEulerAngles = new Vector3(0, 0, angle);

        public void SetPosition(Vector3 position) =>
            body.position = position;

        public void SetPositionAndRotation(Vector3 position, float angle)
        {
            var ang = new Vector3(0, 0, angle);
            var rotation = Quaternion.Euler(ang);

            body.SetPositionAndRotation(position, rotation);
        }
    }
}