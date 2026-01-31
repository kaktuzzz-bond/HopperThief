using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Gameplay
{
    public class InputController : MonoBehaviour
    {
        [SerializeField]
        private Camera inputCamera;

        private void Update()
        {
            if (!Touchscreen.current.primaryTouch.press.isPressed) return;
            
            var primaryTouch = Touchscreen.current.primaryTouch.position.ReadValue();
            
            var worldPoint = inputCamera.ScreenToWorldPoint(primaryTouch);
            
            Debug.Log(worldPoint);
        }
    }
}