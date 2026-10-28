using UnityEngine;

namespace TastyZoo.Animals.Movement
{
    [CreateAssetMenu(fileName = "CrawlMovement", menuName = "TastyZoo/Movement/Crawl")]
    public sealed class CrawlMovementConfig : MovementConfig
    {
        [SerializeField, Min(0.1f)] private float _speed = 2f;
        [SerializeField, Min(1f)] private float _turnSpeed = 360f;
        [SerializeField, Min(0.1f)] private float _acceleration = 12f;

        public float Speed => _speed;
        public float TurnSpeed => _turnSpeed;
        public float Acceleration => _acceleration;

        public override IAnimalMovement Create(AnimalView view)
            => new CrawlMovement(view, this);
    }
}
