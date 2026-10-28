using UnityEngine;

namespace TastyZoo.Animals.Behaviours
{
    public sealed class WanderBehaviour : IAnimalBehaviour
    {
        private readonly float _interval;
        private readonly float _angle;

        private float _cooldown;

        public WanderBehaviour(WanderBehaviourConfig config)
        {
            _interval = config.Interval;
            _angle = config.Angle;
            _cooldown = config.Interval;
        }

        public Vector3 Steer(Animal animal, float deltaTime)
        {
            _cooldown -= deltaTime;
            if (_cooldown > 0f)
                return animal.Direction;

            _cooldown += _interval;
            return Quaternion.Euler(0f, Random.Range(-_angle, _angle), 0f) * animal.Direction;
        }
    }
}
