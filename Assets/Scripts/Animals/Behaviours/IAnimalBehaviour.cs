using UnityEngine;

namespace TastyZoo.Animals.Behaviours
{
    public interface IAnimalBehaviour
    {
        Vector3 Steer(Animal animal, float deltaTime);
    }
}
