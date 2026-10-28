using UnityEngine;

namespace TastyZoo.Animals
{
    public readonly record struct AnimalEaten(AnimalRole Food, Vector3 Position);
}
