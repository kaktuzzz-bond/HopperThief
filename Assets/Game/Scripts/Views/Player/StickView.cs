using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Views
{
    public class StickView : MonoBehaviour
    {
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
        private Tween _shakeTween;

        private readonly Vector2 _idleBend = new(0.5f, 0f);

        private static readonly int VerticalBend = Animator.StringToHash(nameof(VerticalBend));
        private static readonly int HorizontalBend = Animator.StringToHash(nameof(HorizontalBend));
        public event Action<Vector3> OnNestedPositionChanged;
        public event Action<Vector3> OnDirectionChanged;


        public Vector3 NestPoint => nest.position;
        public bool IsLeftPosition => transform.localScale.x > 0;

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
            stick.localEulerAngles = new Vector3(0, 0, angle * transform.localScale.x);

        public void Bend(Vector2 factor)
        {
            animator.SetFloat(VerticalBend, Mathf.Abs(factor.y));
            animator.SetFloat(HorizontalBend,  Mathf.Abs(factor.x));
        }

        public void SetDirection(float value)
        {
            var isLeft = value > 0;

            if (isLeft == IsLeftPosition) return;

            var newScale = new Vector3(isLeft ? 1f : -1f, 1, 1);

            transform.localScale = newScale;
            OnDirectionChanged?.Invoke(newScale);
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