using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;
using Vector2 = UnityEngine.Vector2;

namespace Game.Gameplay
{
    public delegate void OnScreenHeldCallback(Vector2 touchPosition, Vector2 joystickMove);


    public interface IPlayerInputBroadcast
    {
        event Action<Vector2> OnScreenTouchStarted;
        event Action<Vector2> OnScreenTouchFinished;
        event Action<Vector2> OnTapPerformed;
        event OnScreenHeldCallback OnScreenHeld;
        event Action<Vector2> OnSwipeDetected;

        bool IsInputEnabled { get; }

        void EnableInput();

        void DisableInput();
    }


    public class PlayerInputBroadcast : IPlayerInputBroadcast, ITickable
    {
        private readonly PlayerInput _playerInput;

        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;


        private bool _isInputEnabled;
        private bool _isHeld;
        private const float SWIPE_RESISTANCE_THRESHOLD = 100f;

        public PlayerInputBroadcast(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public event Action<Vector2> OnScreenTouchStarted;
        public event Action<Vector2> OnScreenTouchFinished;
        public event Action<Vector2> OnTapPerformed;
        public event OnScreenHeldCallback OnScreenHeld;
        public event Action<Vector2> OnSwipeDetected;


        public bool IsInputEnabled
        {
            get => _isInputEnabled;
            private set
            {
                if (value == _isInputEnabled)
                    Debug.LogWarning($"PLayer Input: THE SAME VALUE [{value}]");

                if (value) _playerInput.Enable();
                else _playerInput.Disable();

                _isInputEnabled = value;

                SendMessage(value ? "ENABLED" : "DISABLED");
            }
        }

        private Vector2 ScreenTouchPosition => _playerInput.Touch.TouchPosition.ReadValue<Vector2>();
        private Vector2 Delta => _playerInput.Touch.Delta.ReadValue<Vector2>();
        private Vector2 Move => _playerInput.Touch.Move.ReadValue<Vector2>();

        public void Tick() =>
            NotifyIfScreenHeld();

        private void NotifyIfScreenHeld()
        {
            if (!_isHeld) return;

            OnScreenHeld?.Invoke(ScreenTouchPosition, Move);
        }

        public void EnableInput()
        {
            _playerInput.Touch.Press.started += NotifyOnTouchStarted;
            _playerInput.Touch.Press.canceled += NotifyOnTouchFinished;

            _playerInput.Touch.Tap.performed += NotifyOnTapPerformed;

            _playerInput.Touch.Hold.performed += _ => _isHeld = true;
            _playerInput.Touch.Hold.canceled += _ => _isHeld = false;

            IsInputEnabled = true;
        }

        public void DisableInput() =>
            IsInputEnabled = false;


        private void NotifyOnTouchStarted(InputAction.CallbackContext _)
        {
            _startTouchPosition = ScreenTouchPosition;
            OnScreenTouchStarted?.Invoke(_startTouchPosition);
        }

        private void NotifyOnTouchFinished(InputAction.CallbackContext _)
        {
            _endTouchPosition = ScreenTouchPosition;
            OnScreenTouchFinished?.Invoke(_endTouchPosition);

            DetectSwipe();
        }

        private void DetectSwipe()
        {
            var direction = _endTouchPosition - _startTouchPosition;

            if (direction.magnitude < SWIPE_RESISTANCE_THRESHOLD) return;

            var absDirectionX = Mathf.Abs(direction.x);
            var absDirectionY = Mathf.Abs(direction.y);

            if (absDirectionX > absDirectionY)
            {
                OnSwipeDetected?.Invoke(direction.x > 0 ? Vector2.right : Vector2.left);
            }
            else
            {
                OnSwipeDetected?.Invoke(direction.y > 0 ? Vector2.up : Vector2.down);
            }
        }

        private void NotifyOnTapPerformed(InputAction.CallbackContext _)
        {
            OnTapPerformed?.Invoke(ScreenTouchPosition);
        }


        private static void SendMessage(string message) =>
            Debug.Log($"<color=cyan>Player Input: <b>{message}</b></color>");
    }
}