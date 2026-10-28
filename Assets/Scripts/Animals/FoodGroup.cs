using System;
using UnityEngine;

namespace TastyZoo.Animals
{
    [CreateAssetMenu(fileName = "FoodGroup", menuName = "TastyZoo/Food Group")]
    public sealed class FoodGroup : ScriptableObject
    {
        [SerializeField] private FoodGroup[] _diet = Array.Empty<FoodGroup>();

        public bool Eats(FoodGroup group)
            => Array.IndexOf(_diet, group) >= 0;
    }
}
