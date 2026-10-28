using System;
using System.Collections.Generic;
using MessagePipe;
using TastyZoo.Animals;
using TastyZoo.Spawning;
using VContainer.Unity;

namespace TastyZoo.UI
{
    public sealed class DeathCounterController : IStartable, IDisposable
    {
        private readonly Dictionary<AnimalRole, int> _deaths = new();
        private readonly DeathCounterView _view;
        private readonly SpawnConfig _spawnConfig;
        private readonly ISubscriber<AnimalEaten> _eaten;

        private IDisposable _subscription;

        public DeathCounterController(DeathCounterView view, SpawnConfig spawnConfig, ISubscriber<AnimalEaten> eaten)
        {
            _view = view;
            _spawnConfig = spawnConfig;
            _eaten = eaten;
        }

        public void Start()
        {
            var animals = _spawnConfig.Animals;
            for (var i = 0; i < animals.Count; i++)
            {
                var role = animals[i].Role;
                if (_deaths.TryAdd(role, 0))
                    _view.Add(role);
            }

            _subscription = _eaten.Subscribe(OnEaten);
        }

        public void Dispose()
            => _subscription?.Dispose();

        private void OnEaten(AnimalEaten message)
        {
            var role = message.Food;
            var count = _deaths[role] + 1;
            _deaths[role] = count;
            _view.SetCount(role, count);
        }
    }
}
