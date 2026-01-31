using System;
using UnityEngine;
using VContainer.Unity;
using Vector2 = UnityEngine.Vector2;

namespace Game.Gameplay
{
    public delegate void OnScreenHeldCallback(Vector2 startPosition, Vector2 currenPosition);


    public interface IPlayerInputBroadcast
    {
        event Action<Vector2> OnScreenTapped;
        event OnScreenHeldCallback OnScreenHeld;
        
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


        public event Action<Vector2> OnScreenTapped;
        public event OnScreenHeldCallback OnScreenHeld;

        private bool _isInputEnabled;
        private Vector2 _startTouchPosition;
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

        public void EnableInput()
        {
            _playerInput.Touch.Tap.started += _ => _startTouchPosition = ScreenTouchPosition;
            _playerInput.Touch.Tap.performed += _ => OnScreenTapped?.Invoke(ScreenTouchPosition);

            _playerInput.Touch.Hold.performed += _ => _isHeld = true;
            _playerInput.Touch.Hold.canceled += _ => _isHeld = false;

            IsInputEnabled = true;
        }

        public void DisableInput()
        {
            IsInputEnabled = false;
        }


        public void Tick()
        {
            if (!_isHeld) return;
            OnScreenHeld?.Invoke(_startTouchPosition, ScreenTouchPosition);
        }

        private static void SendMessage(string message) =>
            Debug.Log($"<color=cyan>Player Input: <b>{message}</b></color>");
    }
}