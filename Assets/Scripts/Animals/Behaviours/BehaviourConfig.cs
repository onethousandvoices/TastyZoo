using UnityEngine;

namespace TastyZoo.Animals.Behaviours
{
    public abstract class BehaviourConfig : ScriptableObject
    {
        public abstract IAnimalBehaviour Create();
    }
}
