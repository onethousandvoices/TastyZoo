using System;
using MessagePipe;
using TastyZoo.Animals;
using UnityEngine;
using UnityEngine.Pool;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace TastyZoo.UI
{
    public sealed class TastyLabelsController : IStartable, IDisposable
    {
        private readonly ObjectPool<TastyLabelView> _pool;
        private readonly Camera _camera;
        private readonly RectTransform _root;
        private readonly ISubscriber<AnimalEaten> _eaten;

        private IDisposable _subscription;

        public TastyLabelsController(Camera camera, TastyLabelView prefab, RectTransform root, ISubscriber<AnimalEaten> eaten)
        {
            _camera = camera;
            _root = root;
            _eaten = eaten;
            _pool = new(() => Object.Instantiate(prefab, root), actionOnRelease: label => label.gameObject.SetActive(false));
        }

        public void Start()
            => _subscription = _eaten.Subscribe(OnEaten);

        public void Dispose()
            => _subscription?.Dispose();

        private void OnEaten(AnimalEaten message)
        {
            var screenPoint = _camera.WorldToScreenPoint(message.Position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_root, screenPoint, null, out var point);
            _pool.Get().Show(point, _pool.Release);
        }
    }
}
