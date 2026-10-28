using UnityEngine;

namespace TastyZoo.Animals.Behaviours
{
    [CreateAssetMenu(fileName = "WanderBehaviour", menuName = "TastyZoo/Behaviour/Wander")]
    public sealed class WanderBehaviourConfig : BehaviourConfig
    {
        [SerializeField, Min(0.1f)] private float _interval = 3f;
        [SerializeField, Range(0f, 180f)] private float _angle = 45f;

        public float Interval => _interval;
        public float Angle => _angle;

        public override IAnimalBehaviour Create()
            => new WanderBehaviour(this);
    }
}
