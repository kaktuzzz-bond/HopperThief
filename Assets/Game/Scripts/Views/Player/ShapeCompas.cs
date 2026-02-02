using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Views
{
    [ExecuteAlways]
    public class ShapeCompas : ImmediateModeShapeDrawer
    {
        [Header("Line")]
        [SerializeField, Range(0f, 5f)]
        private float maxLength = 3f;

        [SerializeField, MinMaxSlider(0f, 15, true)]
        private Vector2 thicknessRange = new(2f, 10f);

        [SerializeField]
        private Vector3 offset = Vector3.zero;

        [SerializeField]
        private Vector3 direction = Vector3.up;

        [Header("Triangle")]
        [SerializeField]
        private Vector2 triangleSize = Vector2.one;

        [SerializeField, Range(0, 1)]
        private float cornerRoundness = 0.5f;

        [Space]
        [SerializeField]
        private Gradient colorGradient;

        [SerializeField, Range(0f, 1f)]
        private float progress;

        [ShowInInspector, ReadOnly]
        private Vector3 _startPosition = Vector3.zero;


        private const float MIN_THRESHOLD = 0.2f;

        public Vector3 StartPosition
        {
            get => _startPosition;
            set => _startPosition = value + offset;
        }

        public override void DrawShapes(Camera cam)
        {
            if (progress < MIN_THRESHOLD) return;

            using (Draw.Command(cam))
            {
                Draw.PolylineGeometry = PolylineGeometry.Flat2D;

                using (var path = new PolylinePath())
                {
                    //line
                    var multiplier = progress * progress * progress;
                    var endPosition = StartPosition + direction.normalized * maxLength * multiplier;
                    var startColor = colorGradient.Evaluate(progress - 0.5f);
                    startColor.a = 0;
                    var endColor = colorGradient.Evaluate(progress);

                    path.AddPoint(StartPosition, thicknessRange.x * progress, startColor);
                    path.AddPoint(endPosition, thicknessRange.y * progress, endColor);

                    //triangle
                    var halfWidth = triangleSize.x * 0.5f * progress;
                    var height = triangleSize.y * progress;
                    var a = new Vector3(endPosition.x - halfWidth, endPosition.y, endPosition.z);
                    var b = new Vector3(endPosition.x, endPosition.y + height, endPosition.z);
                    var c = new Vector3(endPosition.x + halfWidth, endPosition.y, endPosition.z);

                    Draw.Polyline(path, PolylineJoins.Round);
                    Draw.Triangle(a, b, c, cornerRoundness, endColor);
                }
            }
        }
    }
}