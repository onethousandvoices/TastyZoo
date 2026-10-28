using System.Collections.Generic;
using TastyZoo.Animals;
using UnityEngine;

namespace TastyZoo.UI
{
    public sealed class DeathCounterView : MonoBehaviour
    {
        private readonly Dictionary<AnimalRole, DeathCounterRowView> _rows = new();

        [SerializeField] private DeathCounterRowView _rowPrefab;
        [SerializeField] private Transform _rowsRoot;

        public void Add(AnimalRole role)
        {
            var row = Instantiate(_rowPrefab, _rowsRoot);
            row.SetTitle(role.Title);
            _rows.Add(role, row);
        }

        public void SetCount(AnimalRole role, int count)
            => _rows[role].SetCount(count);
    }
}
