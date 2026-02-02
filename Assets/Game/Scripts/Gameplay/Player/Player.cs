using System;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IPlayer
    {
        event Action OnReset;
        event Action<Vector2> OnDeviationChanged;

        void UpdateDeviationFactor(Vector2 deviationFactor);

        void Reset();
    }


    public class Player : IPlayer
    {
        private readonly Vector2 _defaultDeviationFactor = new(-0.5f, 0f);
        private Vector2 _currentDeviationFactor;

        public event Action OnReset;

        public event Action<Vector2> OnDeviationChanged;
       

        public void UpdateDeviationFactor(Vector2 deviationFactor)
        {
            if (deviationFactor == _currentDeviationFactor) return;
            _currentDeviationFactor = deviationFactor;
            OnDeviationChanged?.Invoke(_currentDeviationFactor);
        }

        public void Reset()
        {
            UpdateDeviationFactor(_defaultDeviationFactor);
            OnReset?.Invoke();
        }
    }
}