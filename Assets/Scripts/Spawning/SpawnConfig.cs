using System;
using System.Collections.Generic;
using TastyZoo.Animals;
using UnityEngine;

namespace TastyZoo.Spawning
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "TastyZoo/Spawn")]
    public sealed class SpawnConfig : ScriptableObject
    {
        [SerializeField] private AnimalConfig[] _animals = Array.Empty<AnimalConfig>();
        [SerializeField, Min(0.1f)] private float _minDelay = 1f;
        [SerializeField, Min(0.1f)] private float _maxDelay = 2f;
        [SerializeField, Min(0f)] private float _clearance = 1f;
        [SerializeField] private LayerMask _obstacles;

        public IReadOnlyList<AnimalConfig> Animals => _animals;
        public float MinDelay => _minDelay;
        public float MaxDelay => _maxDelay;
        public float Clearance => _clearance;
        public LayerMask Obstacles => _obstacles;
    }
}
