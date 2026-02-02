using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Views
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private ThiefView thiefView;

        [SerializeField]
        private StickView stickView;

        [SerializeField]
        private ShapeCompass compass;

        private void OnEnable()
        {
            stickView.OnNestedPositionChanged += OnNestPositionChanged;
            stickView.OnDirectionChanged += OnDirectionChanged;
        }


        private void OnDisable()
        {
            stickView.OnNestedPositionChanged -= OnNestPositionChanged;
            stickView.OnDirectionChanged -= OnDirectionChanged;
        }

        public void UpdateAngle(Vector2 factor)
        {
            stickView.Bend(factor);

            stickView.SetDirection(-factor.x);

            thiefView.SetPositionAndRotation(stickView.NestPoint, factor);
        }

        public void SetName(string newName) =>
            transform.name = name;

        public void SetParent(Transform parent) =>
            transform.SetParent(parent);

        public void SetPosition(Vector3 position)
        {
            stickView.ResetStick();
            thiefView.SetPosition(position);
        }

        private void OnNestPositionChanged(Vector3 position)
        {
            thiefView.SetPosition(position);
            compass.StartPosition = position;
        }

        private void OnDirectionChanged(Vector3 lookDirection)
        {
            thiefView.SetScale(lookDirection);
        }
    }
}