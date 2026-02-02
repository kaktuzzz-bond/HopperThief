using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UIElements;


namespace Game.UI
{
    public class SwipeTrailShapes : OnScreenControl
    {
        private struct TrailPoint
        {
            public Vector2 position;
            public float timestamp;
        }


        [Header("UI Setup")]
        [SerializeField]
        private UIDocument uiDocument;

        [SerializeField]
        private Camera uiCamera;

        [SerializeField]
        private string areaName = "TouchArea";

        [Header("Settings")]
        [SerializeField]
        private float trailExpiry = 0.3f;

        [SerializeField]
        private float thickness = 40f;

        [SerializeField]
        private Color mainColor = Color.white;


        private readonly Color _clearColor = new(1f, 1f, 1f, 0f);

        private VisualElement _area;
        private readonly List<TrailPoint> _points = new();

        private const float TRAIL_MULTIPLIER = 20f;

        private void Awake()
        {
            var root = uiDocument.rootVisualElement;
            _area = root.Q<VisualElement>(areaName);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _area.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _area.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _area.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _area.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _area.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            _area.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }


        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string _controlPath = "<Gamepad>/leftStick";

        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            _area.CapturePointer(evt.pointerId);
            _points.Clear(); // Опционально: очистка при новом нажатии
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_area.HasPointerCapture(evt.pointerId)) return;
            
            _points.Add(new TrailPoint
            {
                position = evt.position,
                timestamp = Time.time
            });
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_area.HasPointerCapture(evt.pointerId))
                _area.ReleasePointer(evt.pointerId);
        }

        private void Update()
        {
            // Очищаем старые точки
            _points.RemoveAll(p => Time.time - p.timestamp > trailExpiry);

            DrawTrail();
        }

        private void DrawTrail()
        {
            if (_points.Count < 2) return;

            using (Draw.Command(uiCamera))
            {
                Draw.Matrix = Matrix4x4.identity;
                Draw.ZTest = UnityEngine.Rendering.CompareFunction.Always;
                Draw.PolylineGeometry = PolylineGeometry.Billboard;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.RadiusSpace = ThicknessSpace.Pixels; // Гарантируем размер в пикселях
                Draw.ThicknessSpace = ThicknessSpace.Pixels;

                using (var path = new PolylinePath())
                {
                    for (var i = 0; i < _points.Count; i++)
                    {
                        var age = Mathf.Clamp01((Time.time - _points[i].timestamp) / trailExpiry);
                        var currentThickness = thickness * TRAIL_MULTIPLIER * (1f - age);
                        var color = Color.Lerp(mainColor, _clearColor, age);

                        path.AddPoint(ScreenToWorld(_points[i].position), currentThickness, color);
                    }

                    Draw.Polyline(path, PolylineJoins.Round);
                }

                // --- Логика диска ---
                // Берем самую последнюю добавленную точку (она же самая свежая)
                var headPoint = _points[^1];
                var headAge = Mathf.Clamp01((Time.time - headPoint.timestamp) / trailExpiry);

                var discFade = 1f - headAge;

                var discPos = ScreenToWorld(headPoint.position);
                Draw.Disc(discPos, thickness * discFade, Color.Lerp(mainColor, _clearColor, headAge));
            }
        }

        private Vector3 ScreenToWorld(Vector2 screenPos)
        {
            // Инвертируем Y для соответствия UI Toolkit -> Unity Screen
            var pos = new Vector3(screenPos.x, Screen.height - screenPos.y, 4f); // 0.5f - расстояние от камеры

            return uiCamera.ScreenToWorldPoint(pos);
        }
    }
}