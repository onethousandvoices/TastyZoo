using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer.Unity;

namespace TastyZoo.Animals
{
    public sealed class AnimalsController : IFixedTickable
    {
        private readonly List<Animal> _animals = new();
        private readonly Dictionary<Rigidbody, Animal> _bodies = new();
        private readonly AnimalFactory _factory;
        private readonly IPublisher<AnimalEaten> _eaten;

        public AnimalsController(AnimalFactory factory, IPublisher<AnimalEaten> eaten)
        {
            _factory = factory;
            _eaten = eaten;
        }

        public void Spawn(AnimalConfig config, Vector3 position, Vector3 direction)
        {
            var animal = _factory.Create(config, position, direction);
            animal.View.HitAnimal += OnHitAnimal;
            animal.View.HitObstacle += OnHitObstacle;
            _animals.Add(animal);
            _bodies.Add(animal.View.Body, animal);
        }

        public void FixedTick()
        {
            var deltaTime = Time.fixedDeltaTime;
            for (var i = 0; i < _animals.Count; i++)
                _animals[i].Tick(deltaTime);
        }

        private void OnHitAnimal(AnimalView view, Rigidbody other)
        {
            if (!_bodies.TryGetValue(other, out var opponent))
                return;

            var self = _bodies[view.Body];
            var eater = FoodChain.FindEater(self, opponent);
            if (eater == null)
            {
                self.BounceOff(opponent.Position);
                return;
            }

            var food = eater == self ? opponent : self;
            Remove(food);
            eater.View.Eat();
            food.View.BeEaten(eater.Position, () => _factory.Release(food));
            _eaten.Publish(new(food.Role, eater.Position));
        }

        private void OnHitObstacle(AnimalView view, Vector3 normal)
            => _bodies[view.Body].Reflect(normal);

        private void Remove(Animal animal)
        {
            animal.View.HitAnimal -= OnHitAnimal;
            animal.View.HitObstacle -= OnHitObstacle;
            _bodies.Remove(animal.View.Body);
            _animals.Remove(animal);
        }
    }
}
