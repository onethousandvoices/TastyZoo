using System;
using System.Threading;
using TastyZoo.Animals;
using TastyZoo.World;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace TastyZoo.Spawning
{
    public sealed class SpawnController : IStartable, IDisposable
    {
        private readonly CancellationTokenSource _cancellation = new();
        private readonly SpawnConfig _config;
        private readonly AnimalsController _animals;
        private readonly ArenaBounds _bounds;

        public SpawnController(SpawnConfig config, AnimalsController animals, ArenaBounds bounds)
        {
            _config = config;
            _animals = animals;
            _bounds = bounds;
        }

        public async void Start()
        {
            var token = _cancellation.Token;
            try
            {
                while (true)
                {
                    await Awaitable.WaitForSecondsAsync(Random.Range(_config.MinDelay, _config.MaxDelay), token);
                    var animal = _config.Animals[Random.Range(0, _config.Animals.Count)];
                    while (!TrySpawn(animal))
                        await Awaitable.FixedUpdateAsync(token);
                }
            }
            catch (OperationCanceledException) { }
        }

        public void Dispose()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private bool TrySpawn(AnimalConfig animal)
        {
            var position = _bounds.RandomPoint();
            if (Physics.CheckSphere(position + Vector3.up * _config.Clearance, _config.Clearance, _config.Obstacles))
                return false;

            var direction = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) * Vector3.forward;
            _animals.Spawn(animal, position, direction);
            return true;
        }
    }
}
