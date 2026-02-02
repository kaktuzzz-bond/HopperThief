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
        private ShapeCompas compas;

        [SerializeField, Range(-1, 1)]
        private float deviationAngleFactorX = -0.5f;

        [SerializeField, Range(0, 1)]
        private float deviationAngleFactorY;


        private Vector2 Factor => new(deviationAngleFactorX, deviationAngleFactorY);

        private void OnValidate()
        {
            UpdateAngle(Factor);
        }

        [Button]
        private void Awake()
        {
            stickView.ResetStick();
            thiefView.SetPositionAndRotation(stickView.NestPoint, Factor);
        }

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

        public void OnNestPositionChanged(Vector3 position)
        {
            thiefView.SetPosition(position);
            compas.StartPosition = position;
        }

        public void OnDirectionChanged(Vector3 lookDirection)
        {
            thiefView.SetScale(lookDirection);
        }
        public void UpdateAngle(Vector2 factor)
        {
            stickView.Bend(factor);

            stickView.SetDirection(-factor.x);

            thiefView.SetPositionAndRotation(stickView.NestPoint, factor);
        }
        
        
    }
}