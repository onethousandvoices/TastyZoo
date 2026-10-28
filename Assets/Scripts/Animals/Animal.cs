using TastyZoo.Animals.Behaviours;
using TastyZoo.Animals.Movement;
using TastyZoo.World;
using UnityEngine;

namespace TastyZoo.Animals
{
    public sealed class Animal
    {
        private readonly IAnimalMovement _movement;
        private readonly IAnimalBehaviour _behaviour;
        private readonly ArenaBounds _bounds;

        public int SpawnOrder { get; }
        public AnimalConfig Config { get; }
        public AnimalView View { get; }
        public Vector3 Direction { get; private set; }
        public AnimalRole Role => Config.Role;
        public Vector3 Position => View.Position;

        public Animal(int spawnOrder, AnimalConfig config, AnimalView view, IAnimalMovement movement, IAnimalBehaviour behaviour, ArenaBounds bounds, Vector3 direction)
        {
            SpawnOrder = spawnOrder;
            Config = config;
            View = view;
            _movement = movement;
            _behaviour = behaviour;
            _bounds = bounds;
            Direction = direction;
        }

        public void Tick(float deltaTime)
        {
            Direction = _behaviour.Steer(this, deltaTime);
            if (_bounds.IsLeaving(Position, Direction))
                TurnTowards(_bounds.RandomPoint());

            _movement.Tick(Direction, deltaTime);
        }

        public void BounceOff(Vector3 point)
        {
            var away = Position - point;
            away.y = 0f;
            View.BounceOff(point);
            Reflect(away.normalized);
        }

        public void Reflect(Vector3 normal)
        {
            if (Vector3.Dot(Direction, normal) >= 0f)
                return;

            var direction = Vector3.Reflect(Direction, normal);
            direction.y = 0f;
            Direction = direction.normalized;
        }

        public bool CanEat(Animal other)
            => Config.FoodGroup.Eats(other.Config.FoodGroup);

        private void TurnTowards(Vector3 point)
        {
            var direction = point - Position;
            direction.y = 0f;
            Direction = direction.normalized;
        }
    }
}
