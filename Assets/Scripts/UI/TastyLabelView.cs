using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TastyZoo.UI
{
    public sealed class TastyLabelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Vector2 _offset = new(0f, -70f);
        [SerializeField] private float _punchScale = 1.4f;
        [SerializeField] private float _settleDuration = 0.35f;
        [SerializeField] private float _lifetime = 1.1f;
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField] private float _riseHeight = 70f;

        private Sequence _sequence;

        public float Height => _rect.rect.height;

        private void OnDestroy()
            => _sequence?.Kill();

        public bool Overlaps(Vector2 point)
        {
            var delta = (Vector2)_rect.localPosition - _offset - point;
            var size = _rect.rect.size;
            return Mathf.Abs(delta.x) < size.x && Mathf.Abs(delta.y) < size.y;
        }

        public void Show(Vector2 point, Action<TastyLabelView> hidden)
        {
            var start = point + _offset;
            _rect.localPosition = start;
            _rect.localScale = Vector3.one * _punchScale;
            _text.alpha = 1f;
            gameObject.SetActive(true);

            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(_rect.DOLocalMoveY(start.y + _riseHeight, _lifetime).SetEase(Ease.OutQuad))
                .Insert(0f, _rect.DOScale(1f, _settleDuration).SetEase(Ease.OutCubic))
                .Insert(_lifetime - _fadeDuration, DOTween.To(() => _text.alpha, value => _text.alpha = value, 0f, _fadeDuration))
                .OnComplete(() => hidden(this));
        }
    }
}
