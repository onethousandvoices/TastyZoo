using TastyZoo.Animals;
using TastyZoo.World;
using UnityEngine;
using VContainer.Unity;

namespace TastyZoo.Spawning
{
    public sealed class SpawnController : IFixedTickable
    {
        private readonly SpawnConfig _config;
        private readonly AnimalsController _animals;
        private readonly ArenaBounds _bounds;

        private float _cooldown;

        public SpawnController(SpawnConfig config, AnimalsController animals, ArenaBounds bounds)
        {
            _config = config;
            _animals = animals;
            _bounds = bounds;
            _cooldown = NextDelay();
        }

        public void FixedTick()
        {
            _cooldown -= Time.fixedDeltaTime;
            if (_cooldown > 0f || !TrySpawn())
                return;

            _cooldown = NextDelay();
        }

        private float NextDelay()
            => Random.Range(_config.MinDelay, _config.MaxDelay);

        private bool TrySpawn()
        {
            var position = _bounds.RandomPoint();
            if (Physics.CheckSphere(position + Vector3.up * _config.Clearance, _config.Clearance, _config.Obstacles))
                return false;

            var animal = _config.Animals[Random.Range(0, _config.Animals.Count)];
            var direction = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) * Vector3.forward;
            _animals.Spawn(animal, position, direction);
            return true;
        }
    }
}
