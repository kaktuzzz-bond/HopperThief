using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Game.Gameplay
{
    public class PlayerDebugger : MonoBehaviour
    {
        [Inject, ShowInInspector, ReadOnly]
        private IPlayer _player;

        [Inject, ShowInInspector, ReadOnly]
        private IPlayerViewFactory _viewFactory;

        [InfoBox("X should be in (-1,1) range, Y - in (0,1) range")]
       [Button, HideInEditorMode]
        private void UpdateDeviationFactor(Vector2 factor) =>
            _player.UpdateDeviationFactor(factor);

        [Button, HideInEditorMode]
        private void SpawnView(Vector3 position) =>
            _ = _viewFactory.Create(position);

        [Button, HideInEditorMode]
        private void ResetPlayer() =>
            _player.Reset();
    }
}