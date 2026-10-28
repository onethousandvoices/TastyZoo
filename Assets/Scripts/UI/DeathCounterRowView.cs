using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TastyZoo.UI
{
    public sealed class DeathCounterRowView : MonoBehaviour
    {
        private const float PUNCH_SCALE = 0.25f;
        private const float PUNCH_DURATION = 0.3f;
        private const int PUNCH_VIBRATO = 1;

        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _count;

        private Tween _punch;

        private void OnDestroy()
            => _punch?.Kill();

        public void SetTitle(string title)
            => _title.SetText(title);

        public void SetCount(int count)
        {
            _count.SetText("{0}", count);
            _punch?.Kill(true);
            _punch = _count.transform.DOPunchScale(Vector3.one * PUNCH_SCALE, PUNCH_DURATION, PUNCH_VIBRATO);
        }
    }
}
