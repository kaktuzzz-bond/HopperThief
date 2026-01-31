using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;


namespace Game.UI
{
    public class DynamicJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public GameObject joystickContainer; // Родитель джойстика
        public OnScreenStick stick;          // Сам стик
        private RectTransform _containerRect;

        [SerializeField]
        private TMP_Text _touchPosition;
        
        private void Awake()
        {
            _containerRect = joystickContainer.GetComponent<RectTransform>();
            joystickContainer.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Перемещаем джойстик в точку нажатия
            _containerRect.position = eventData.position;
            joystickContainer.SetActive(true);

            // Передаем событие нажатия в OnScreenStick, чтобы он сразу начал работать
            stick.OnPointerDown(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Передаем данные перетаскивания в OnScreenStick
            stick.OnDrag(eventData);

            _touchPosition.text = eventData.delta.ToString();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Скрываем джойстик и сбрасываем его
            stick.OnPointerUp(eventData);
            joystickContainer.SetActive(false);
        }
        
    }
}