using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Views
{
    public class StickView : MonoBehaviour
    {
        private static readonly int VerticalBend = Animator.StringToHash(nameof(VerticalBend));
        private static readonly int HorizontalBend = Animator.StringToHash(nameof(HorizontalBend));
        public event Action<Vector3> OnNestedPositionChanged;

        [SerializeField]
        private StickSettings tapSettings;

        [SerializeField]
        private StickSettings bendSettings;

        [SerializeField]
        private Transform stick;

        [SerializeField]
        private Transform nest;


        [SerializeField]
        private Animator animator;

        private Vector3 _prevNestPosition;

        public Vector3 NestPoint => nest.position;

        private Tween _shakeTween;

        private readonly Vector2 _idleBend = new(0.5f, 0f);
        

        private void LateUpdate()
        {
            if (NestPoint == _prevNestPosition) return;

            OnNestedPositionChanged?.Invoke(NestPoint);

            _prevNestPosition = NestPoint;
        }

        private void OnDestroy()
        {
            DOTween.KillAll();
        }

        public void SetAngle(float angle) =>
            stick.localEulerAngles = new Vector3(0, 0, angle);

        public void Bend(Vector2 factor)
        {
            animator.SetFloat(VerticalBend, factor.y);
            animator.SetFloat(HorizontalBend, factor.x);
        }

        [Button]
        public void PlayTension()
        {
            _shakeTween =
                stick.DOShakeRotation(tapSettings.ShakeDuration, tapSettings.Strength, tapSettings.VibratoValue);
        }

        [Button]
        public void PlayTensionLoop()
        {
            _shakeTween = stick.DOShakeRotation(bendSettings.ShakeDuration, bendSettings.Strength,
                                   bendSettings.VibratoValue, fadeOut: false)
                               .SetLoops(-1, LoopType.Incremental);
        }

        [Button]
        public void KillShake()
        {
            _shakeTween.Kill();
        }

        [Button]
        public void ResetStick()
        {
            SetAngle(0f);
            Bend(_idleBend);
        }
    }
}