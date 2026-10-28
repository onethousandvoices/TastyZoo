using UnityEngine;

namespace TastyZoo.Animals.Movement
{
    public sealed class CrawlMovement : IAnimalMovement
    {
        private readonly Rigidbody _body;
        private readonly float _speed;
        private readonly float _turnSpeed;
        private readonly float _acceleration;

        public CrawlMovement(AnimalView view, CrawlMovementConfig config)
        {
            _body = view.Body;
            _speed = config.Speed;
            _turnSpeed = config.TurnSpeed;
            _acceleration = config.Acceleration;
        }

        public void Tick(Vector3 direction, float deltaTime)
        {
            var rotation = Quaternion.RotateTowards(_body.rotation, Quaternion.LookRotation(direction), _turnSpeed * deltaTime);
            _body.MoveRotation(rotation);
            var velocity = _body.linearVelocity;
            var target = rotation * Vector3.forward * _speed;
            target.y = velocity.y;
            _body.linearVelocity = Vector3.MoveTowards(velocity, target, _acceleration * deltaTime);
        }
    }
}
