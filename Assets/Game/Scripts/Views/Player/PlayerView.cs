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


        [SerializeField, Range(0, 90f)]
        private float playerAngleMax = 27.5f;
        
        [HideInEditorMode]
        [SerializeField, Range(0, 1), OnValueChanged(nameof(UpdateAngle))]
        private float deviationAngleFactor;

        
        private void Awake()
        {
            stickView.SetAngle(0f);
        }

        private void OnEnable()
        {
            thiefView.SetPosition(stickView.NestPoint);

            stickView.Bend(0f);

            stickView.OnNestedPositionChanged += thiefView.SetPosition;
        }


        private void OnDisable()
        {
            stickView.OnNestedPositionChanged -= thiefView.SetPosition;
        }

        private void UpdateAngle(float factor)
        {
            stickView.Bend(factor);

            var playerPos = stickView.NestPoint;
            var playerAngle = Mathf.Lerp(0, playerAngleMax, factor);

            thiefView.SetPositionAndRotation(playerPos, playerAngle);
        }
    }
}