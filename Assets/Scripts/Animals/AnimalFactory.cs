using System.Collections.Generic;
using TastyZoo.World;
using UnityEngine;
using UnityEngine.Pool;

namespace TastyZoo.Animals
{
    public sealed class AnimalFactory
    {
        private readonly Dictionary<AnimalConfig, ObjectPool<AnimalView>> _pools = new();
        private readonly Transform _root;
        private readonly ArenaBounds _bounds;

        private int _spawned;

        public AnimalFactory(Transform root, ArenaBounds bounds)
        {
            _root = root;
            _bounds = bounds;
        }

        public Animal Create(AnimalConfig config, Vector3 position, Vector3 direction)
        {
            var view = GetPool(config).Get();
            view.Appear(position, direction);
            return new(_spawned++, config, view, config.Movement.Create(view), config.Behaviour.Create(), _bounds, direction);
        }

        public void Release(Animal animal)
            => _pools[animal.Config].Release(animal.View);

        private ObjectPool<AnimalView> GetPool(AnimalConfig config)
        {
            if (_pools.TryGetValue(config, out var pool))
                return pool;

            pool = new(() => Object.Instantiate(config.Prefab, _root), actionOnRelease: view => view.gameObject.SetActive(false));
            _pools.Add(config, pool);
            return pool;
        }
    }
}
