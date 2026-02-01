using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UIElements;


namespace Game.UI
{
    public class DynamicJoystickUIT : OnScreenControl
    {
        [Header("UI Setup")]
        [SerializeField]
        private UIDocument uiDocument;

        [SerializeField]
        private string areaName = "TouchArea";

        [SerializeField]
        private string containerName = "JoystickBG";

        [SerializeField]
        private string handleName = "JoystickHandle";

        [Header("Settings")]
        [SerializeField]
        private float movementRange = 300f;

        [SerializeField]
        private float animationDuration = 0.15f;

        [SerializeField]
        private Vector3 idleScale = new Vector3(0.5f, 0.5f, 1f); // Размер в скрытом состоянии

        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string _controlPath = "<Gamepad>/leftStick";

        private VisualElement _container;
        private VisualElement _handle;
        private VisualElement _area;

        private StyleScale _idleScale;
        private StyleScale _defaultScale;

        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        private void Awake()
        {
            var root = uiDocument.rootVisualElement;
            _area = root.Q<VisualElement>(areaName);
            _container = root.Q<VisualElement>(containerName);
            _handle = root.Q<VisualElement>(handleName);

            // Size
            _container.style.position = Position.Absolute;

            var ringSize = movementRange * 2;
            _container.style.width = ringSize;
            _container.style.height = ringSize;

            // Animations
            _idleScale = new StyleScale(new Scale(idleScale));
            _defaultScale = new StyleScale(new Scale(Vector3.one));
            _container.style.transitionDuration = new List<TimeValue> { new(animationDuration) };
            _container.style.transitionProperty = new List<StylePropertyName> { new("opacity"), new("scale") };
            _container.style.transitionTimingFunction = new List<EasingFunction> { new(EasingMode.EaseOutCubic) };
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // Events
            _area.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _area.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _area.RegisterCallback<PointerUpEvent>(OnPointerUp);

            HideJoystick();
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            _area.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _area.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            _area.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            Debug.Log($"Pressed: {evt.target}");

            _area.CapturePointer(evt.pointerId);

            // Используем расчет на основе геометрии самого контейнера
            var containerWidth = _container.layout.width > 0 ? _container.layout.width : movementRange * 2;
            var containerHeight = _container.layout.height > 0 ? _container.layout.height : movementRange * 2;

            _container.style.left = evt.localPosition.x - containerWidth * 0.5f;
            _container.style.top = evt.localPosition.y - containerHeight * 0.5f;

            ShowJoystick();

            UpdateJoystick(evt.localPosition);
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (_area.HasPointerCapture(evt.pointerId))
                UpdateJoystick(evt.localPosition);
        }

        private void UpdateJoystick(Vector2 pointerPos)
        {
            var center = new Vector2(
                _container.layout.x + _container.layout.width / 2,
                _container.layout.y + _container.layout.height / 2
            );

            var delta = pointerPos - center;
            var distance = Mathf.Min(delta.magnitude, movementRange);
            var input = delta.normalized * (distance / movementRange);

            _handle.transform.position = delta.normalized * distance;
            SendValueToControl(input);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!_area.HasPointerCapture(evt.pointerId)) return;

            _area.ReleasePointer(evt.pointerId);

            HideJoystick();

            _handle.transform.position = Vector3.zero;
            SendValueToControl(Vector2.zero);
        }

        private void HideJoystick()
        {
            _container.style.opacity = 0;
            _container.style.scale = _idleScale;
        }

        private void ShowJoystick()
        {
            _container.style.opacity = 1;
            _container.style.scale = _defaultScale;
        }
    }
}