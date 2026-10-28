using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TastyZoo.UI
{
    public sealed class TastyLabelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Vector2 _offset = new(0f, -70f);
        [SerializeField] private float _riseHeight = 40f;
        [SerializeField] private float _appearDuration = 0.25f;
        [SerializeField] private float _holdDuration = 0.5f;
        [SerializeField] private float _fadeDuration = 0.3f;

        private Sequence _sequence;

        private void OnDestroy()
            => _sequence?.Kill();

        public void Show(Vector2 point, Action<TastyLabelView> hidden)
        {
            var start = point + _offset;
            transform.localPosition = start;
            transform.localScale = Vector3.zero;
            _text.alpha = 1f;
            gameObject.SetActive(true);

            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one, _appearDuration).SetEase(Ease.OutBack))
                .Join(transform.DOLocalMoveY(start.y + _riseHeight, _appearDuration + _holdDuration + _fadeDuration).SetEase(Ease.OutQuad))
                .Insert(_appearDuration + _holdDuration, DOTween.To(() => _text.alpha, value => _text.alpha = value, 0f, _fadeDuration))
                .OnComplete(() => hidden(this));
        }
    }
}
