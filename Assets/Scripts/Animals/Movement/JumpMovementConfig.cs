using UnityEngine;

namespace TastyZoo.Animals.Movement
{
    [CreateAssetMenu(fileName = "JumpMovement", menuName = "TastyZoo/Movement/Jump")]
    public sealed class JumpMovementConfig : MovementConfig
    {
        [SerializeField, Min(0.1f)] private float _interval = 1.6f;
        [SerializeField, Min(0f)] private float _windup = 0.42f;
        [SerializeField, Min(0.1f)] private float _distance = 2.5f;
        [SerializeField, Min(0.1f)] private float _height = 1f;
        [SerializeField, Min(1f)] private float _turnSpeed = 720f;

        public float Interval => _interval;
        public float Windup => _windup;
        public float Distance => _distance;
        public float Height => _height;
        public float TurnSpeed => _turnSpeed;

        public override IAnimalMovement Create(AnimalView view)
            => new JumpMovement(view, this);
    }
}
