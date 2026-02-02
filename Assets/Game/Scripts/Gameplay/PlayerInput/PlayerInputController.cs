using System;
using UnityEngine;
using VContainer.Unity;

namespace Game.Gameplay
{
    public class PlayerInputController : IStartable, IDisposable
    {
        private readonly IPlayerInputBroadcast _playerInputBroadcast;

        public PlayerInputController(IPlayerInputBroadcast playerInputBroadcast)
        {
            _playerInputBroadcast = playerInputBroadcast;
        }


        // private void Update()
        // {
        //     if (!Touchscreen.current.primaryTouch.press.isPressed) return;
        //     
        //     var primaryTouch = Touchscreen.current.primaryTouch.position.ReadValue();
        //     
        //     var worldPoint = inputCamera.ScreenToWorldPoint(primaryTouch);
        //     
        //     Debug.Log(worldPoint);

        // }
        public void Start()
        {
            _playerInputBroadcast.OnScreenTouchStarted += OnTouchStarted;
            _playerInputBroadcast.OnScreenTouchFinished += OnTouchFinished;
            _playerInputBroadcast.OnTapPerformed += OnScreenTapped;
            _playerInputBroadcast.OnScreenHeld += OnScreenHeld;
            _playerInputBroadcast.OnSwipeDetected += OnSwipe;

            _playerInputBroadcast.EnableInput();
        }

        public void Dispose()
        {
            _playerInputBroadcast.DisableInput();

            _playerInputBroadcast.OnScreenTouchStarted -= OnTouchStarted;
            _playerInputBroadcast.OnScreenTouchFinished -= OnTouchFinished;
            _playerInputBroadcast.OnTapPerformed -= OnScreenTapped;
            _playerInputBroadcast.OnScreenHeld -= OnScreenHeld;
            _playerInputBroadcast.OnSwipeDetected -= OnSwipe;
        }

        private void OnSwipe(Vector2 direction)
        {
            SendMessage($"SWIPE: {direction}");
        }


        private void OnTouchStarted(Vector2 position)
        {
            SendMessage($"START: {position}");
        }

        private void OnTouchFinished(Vector2 position)
        {
            SendMessage($"FINISH: {position}");
        }


        private void OnScreenTapped(Vector2 screenPosition)
        {
            SendMessage($"TAP: {screenPosition}");
        }

        private void OnScreenHeld(Vector2 touchPosition, Vector2 joystickMove)
        {
            SendMessage($"HOLD: {touchPosition} |  STICK: {joystickMove}");
        }


        private static void SendMessage(string message) =>
            Debug.Log($"<color=orange><b>{message}</b></color>");
    }
}