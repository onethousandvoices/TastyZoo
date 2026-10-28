using UnityEngine;

namespace TastyZoo.Animals
{
    [CreateAssetMenu(fileName = "AnimalRole", menuName = "TastyZoo/Animal Role")]
    public sealed class AnimalRole : ScriptableObject
    {
        [SerializeField] private string _title;

        public string Title => _title;
    }
}
