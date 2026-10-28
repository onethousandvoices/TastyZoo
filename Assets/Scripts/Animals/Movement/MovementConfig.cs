using UnityEngine;

namespace TastyZoo.Animals.Movement
{
    public abstract class MovementConfig : ScriptableObject
    {
        public abstract IAnimalMovement Create(AnimalView view);
    }
}
