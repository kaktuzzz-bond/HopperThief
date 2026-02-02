using System;
using System.Collections.Generic;
using Game.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Gameplay
{
    public interface IPlayerViewFactory
    {
        PlayerView Create();

        PlayerView Create(Vector3 position);

        PlayerView Create(Vector3 position, Transform parent);

        PlayerView Create(Vector3 position, Quaternion rotation, Transform parent);
    }


    public class PlayerViewFactory : IPlayerViewFactory, IDisposable
    {
        private readonly IObjectResolver _container;
        private readonly PlayerView _prefab;

        private const string NAME = "Player";

        public PlayerViewFactory(IObjectResolver container, PlayerView prefab)
        {
            _container = container;
            _prefab = prefab;
        }


        private readonly List<Action> _disposables = new();

        public PlayerView Create()
        {
            var view = _container.Instantiate(_prefab);
            Setup(view);

            return view;
        }

        public PlayerView Create(Vector3 position)
        {
            var view = _container.Instantiate(_prefab, position, Quaternion.identity);
            Setup(view);

            return view;
        }

        public PlayerView Create(Vector3 position, Transform parent)
        {
            var view = _container.Instantiate(_prefab, position, Quaternion.identity, parent);
            Setup(view);

            return view;
        }

        public PlayerView Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            var view = _container.Instantiate(_prefab, position, rotation, parent);
            Setup(view);

            return view;
        }

        private void Setup(PlayerView view)
        {
            view.SetName(NAME);

            var player = _container.Resolve<IPlayer>();
            
            player.OnDeviationChanged += view.UpdateAngle;

            _disposables.Add(() => player.OnDeviationChanged -= view.UpdateAngle);
            
            player.Reset();
        }

        public void Dispose()
        {
            foreach (var action in _disposables)
            {
                action.Invoke();
            }

            _disposables.Clear();
        }
    }
}