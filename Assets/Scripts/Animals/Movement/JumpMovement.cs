using UnityEngine;

namespace TastyZoo.Animals.Movement
{
    public sealed class JumpMovement : IAnimalMovement
    {
        private static readonly int _jumpHash = Animator.StringToHash("Jump");
        private static readonly int _flightSpeedHash = Animator.StringToHash("FlightSpeed");

        private readonly Rigidbody _body;
        private readonly Animator _animator;
        private readonly JumpArc _arc;
        private readonly float _interval;
        private readonly float _windup;
        private readonly float _turnSpeed;

        private float _cooldown;
        private float _airTime;
        private bool _crouching;
        private Vector3 _heading;

        public JumpMovement(AnimalView view, JumpMovementConfig config)
        {
            _body = view.Body;
            _animator = view.Animator;
            _arc = JumpArc.From(config.Height, config.Distance, -Physics.gravity.y);
            _interval = config.Interval;
            _windup = config.Windup;
            _turnSpeed = config.TurnSpeed;
            _cooldown = config.Interval;
            _heading = view.transform.forward;
        }

        public void Tick(Vector3 direction, float deltaTime)
        {
            Face(deltaTime);
            Land(deltaTime);
            _cooldown -= deltaTime;
            if (_cooldown > 0f)
                return;

            if (_crouching)
                Launch();
            else
                Crouch(direction);
        }

        private void Face(float deltaTime)
            => _body.MoveRotation(Quaternion.RotateTowards(_body.rotation, Quaternion.LookRotation(_heading), _turnSpeed * deltaTime));

        private void Land(float deltaTime)
        {
            if (_airTime <= 0f)
                return;

            _airTime -= deltaTime;
            if (_airTime <= 0f)
                _body.linearVelocity = Vector3.up * _body.linearVelocity.y;
        }

        private void Crouch(Vector3 direction)
        {
            _crouching = true;
            _cooldown += _windup;
            _heading = direction;
            _animator.SetTrigger(_jumpHash);
        }

        private void Launch()
        {
            _crouching = false;
            _cooldown += _interval - _windup;
            _airTime = _arc.FlightTime;
            _body.linearVelocity = _heading * _arc.HorizontalSpeed + Vector3.up * _arc.VerticalSpeed;
            _animator.SetFloat(_flightSpeedHash, 1f / _arc.FlightTime);
        }
    }
}
