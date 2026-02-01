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
        
        [SerializeField, Range(0, 1)]
        private float deviationAngleFactorY;

        [SerializeField, Range(0, 1)]
        private float deviationAngleFactorX = 0.5f;
        private const float PLAYER_ANGLE_MAX = 40f;

        private void OnValidate()
        {
            UpdateAngle(new Vector2(deviationAngleFactorX, deviationAngleFactorY));
        }

        [Button]
        private void Awake()
        {
            stickView.ResetStick();
            thiefView.SetPosition(stickView.NestPoint);
        }

        private void OnEnable()
        {
            stickView.OnNestedPositionChanged += thiefView.SetPosition;
        }


        private void OnDisable()
        {
            stickView.OnNestedPositionChanged -= thiefView.SetPosition;
        }

        public void UpdateAngle(Vector2 factor)
        {
            stickView.Bend(factor);

            var playerPos = stickView.NestPoint;
            var playerAngle = Mathf.Lerp(0, PLAYER_ANGLE_MAX, factor.y);

            thiefView.SetPositionAndRotation(playerPos, playerAngle);
        }
    }
}