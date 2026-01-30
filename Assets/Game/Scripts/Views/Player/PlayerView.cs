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


        [SerializeField, MinMaxSlider(0, 90, true)]
        private Vector2 deviationAngleMinMax = new(26.5f, 54f);

        //[HideInEditorMode]
        [SerializeField, Range(0, 1), OnValueChanged(nameof(UpdateAngle))]
        private float deviationAngleFactor;

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
            //stick
            var angle = Mathf.Lerp(deviationAngleMinMax.x, deviationAngleMinMax.y, factor);

            stickView.SetAngle(angle);
            stickView.Bend(factor);

            //thief
            var playerPos = stickView.NestPoint;
            var playerAngle = angle - deviationAngleMinMax.x;

            thiefView.SetPositionAndRotation(playerPos, playerAngle);
        }
    }
}