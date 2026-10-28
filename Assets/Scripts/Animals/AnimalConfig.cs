using TastyZoo.Animals.Behaviours;
using TastyZoo.Animals.Movement;
using UnityEngine;

namespace TastyZoo.Animals
{
    [CreateAssetMenu(fileName = "AnimalConfig", menuName = "TastyZoo/Animal")]
    public sealed class AnimalConfig : ScriptableObject
    {
        [SerializeField] private AnimalView _prefab;
        [SerializeField] private AnimalRole _role;
        [SerializeField] private FoodGroup _foodGroup;
        [SerializeField] private MovementConfig _movement;
        [SerializeField] private BehaviourConfig _behaviour;

        public AnimalView Prefab => _prefab;
        public AnimalRole Role => _role;
        public FoodGroup FoodGroup => _foodGroup;
        public MovementConfig Movement => _movement;
        public BehaviourConfig Behaviour => _behaviour;
    }
}
