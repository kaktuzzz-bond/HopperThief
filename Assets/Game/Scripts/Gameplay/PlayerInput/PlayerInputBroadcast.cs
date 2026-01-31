using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;
using Vector2 = UnityEngine.Vector2;

namespace Game.Gameplay
{
    public interface IPlayerInputBroadcast
    {
        event Action<Vector2> OnScreenTouchStarted;
        event Action<Vector2> OnScreenTouchFinished;
        event Action<Vector2> OnTapPerformed;
        event Action<Vector2> OnScreenHeld;

        bool IsInputEnabled { get; }

        void EnableInput();

        void DisableInput();
    }


    public class PlayerInputBroadcast : IPlayerInputBroadcast, ITickable
    {
        private readonly PlayerInput _playerInput;

        public PlayerInputBroadcast(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public event Action<Vector2> OnScreenTouchStarted;
        public event Action<Vector2> OnScreenTouchFinished;
        public event Action<Vector2> OnTapPerformed;
        public event Action<Vector2> OnScreenHeld;

        private bool _isInputEnabled;
        private bool _isHeld;


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

        public void Tick() =>
            NotifyIfScreenHeld();

        public void EnableInput()
        {
            _playerInput.Touch.Press.started += NotifyOnTouchStarted;
            _playerInput.Touch.Press.canceled += NotifyOnTouchFinished;

            _playerInput.Touch.Tap.performed += NotifyOnTapPerformed;

            _playerInput.Touch.Hold.performed += _ => _isHeld = true;
            _playerInput.Touch.Hold.canceled += _ => _isHeld = false;

            IsInputEnabled = true;
        }

        public void DisableInput()
        {
            IsInputEnabled = false;
        }


        private void NotifyOnTouchStarted(InputAction.CallbackContext _) =>
            OnScreenTouchStarted?.Invoke(ScreenTouchPosition);

        private void NotifyOnTouchFinished(InputAction.CallbackContext _) =>
            OnScreenTouchFinished?.Invoke(ScreenTouchPosition);

        private void NotifyOnTapPerformed(InputAction.CallbackContext _) =>
            OnTapPerformed?.Invoke(ScreenTouchPosition);

        private void NotifyIfScreenHeld()
        {
            if (_isHeld)
                OnScreenHeld?.Invoke(ScreenTouchPosition);
        }

        private static void SendMessage(string message) =>
            Debug.Log($"<color=cyan>Player Input: <b>{message}</b></color>");
    }
}