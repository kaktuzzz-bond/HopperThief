using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
            _playerInputBroadcast.OnScreenTapped += OnScreenTapped;
            _playerInputBroadcast.OnScreenHeld += OnScreenHeld;
            
            _playerInputBroadcast.EnableInput();
        }

        public void Dispose()
        {
            _playerInputBroadcast.DisableInput();
            
            _playerInputBroadcast.OnScreenTapped -= OnScreenTapped;
            _playerInputBroadcast.OnScreenHeld -= OnScreenHeld;
        }

        private void OnScreenHeld(Vector2 startPosition, Vector2 currenPosition)
        {
            SendMessage($"HOLD: {startPosition} : {currenPosition}");
        }

        private void OnScreenTapped(Vector2 screenPosition)
        {
            SendMessage($"TOUCHED: {screenPosition}");
        }

      


        private static void SendMessage(string message) =>
            Debug.Log($"<color=orange><b>{message}</b></color>");
    }
}