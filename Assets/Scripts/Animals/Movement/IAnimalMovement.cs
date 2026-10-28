using UnityEngine;

namespace TastyZoo.Animals.Movement
{
    public interface IAnimalMovement
    {
        void Tick(Vector3 direction, float deltaTime);
    }
}
