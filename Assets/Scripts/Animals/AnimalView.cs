using System;
using DG.Tweening;
using UnityEngine;

namespace TastyZoo.Animals
{
    public sealed class AnimalView : MonoBehaviour
    {
        public event Action<AnimalView, Rigidbody> HitAnimal;
        public event Action<AnimalView, Vector3> HitObstacle;

        private const float MAX_WALL_NORMAL_Y = 0.5f;
        private const int GULP_VIBRATO = 1;

        private static readonly int _eatHash = Animator.StringToHash("Eat");

        [SerializeField] private Rigidbody _body;
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _appearDuration = 0.3f;
        [SerializeField] private float _disappearDuration = 0.25f;
        [SerializeField] private float _bounceImpulse = 3f;
        [SerializeField] private float _bounceLift = 1.5f;
        [SerializeField] private float _gulpScale = 0.2f;
        [SerializeField] private float _gulpDuration = 0.3f;

        private Tween _tween;
        private Tween _gulp;
        private Vector3 _scale;

        public Rigidbody Body => _body;
        public Animator Animator => _animator;
        public Vector3 Position => _body.position;

        private void Awake()
            => _scale = transform.localScale;

        private void OnDestroy()
        {
            _tween?.Kill();
            _gulp?.Kill();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody)
                HitAnimal?.Invoke(this, collision.rigidbody);
            else
                ReportWall(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!collision.rigidbody)
                ReportWall(collision);
        }

        public void Appear(Vector3 position, Vector3 direction)
        {
            _tween?.Kill();
            _gulp?.Kill(true);
            transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));
            transform.localScale = Vector3.zero;
            _collider.enabled = true;
            _body.isKinematic = false;
            _body.interpolation = RigidbodyInterpolation.Interpolate;
            _body.linearVelocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            gameObject.SetActive(true);
            _tween = transform.DOScale(_scale, _appearDuration).SetEase(Ease.OutBack);
        }

        public void Eat()
        {
            _animator.SetTrigger(_eatHash);
            _gulp?.Kill(true);
            _gulp = _animator.transform.DOPunchScale(Vector3.one * _gulpScale, _gulpDuration, GULP_VIBRATO);
        }

        public void BeEaten(Vector3 mouth, Action disappeared)
        {
            _tween?.Kill();
            _collider.enabled = false;
            _body.isKinematic = true;
            _body.interpolation = RigidbodyInterpolation.None;
            _tween = DOTween.Sequence()
                .Join(transform.DOMove(mouth, _disappearDuration).SetEase(Ease.InQuad))
                .Join(transform.DOScale(Vector3.zero, _disappearDuration).SetEase(Ease.InBack))
                .OnComplete(disappeared.Invoke);
        }

        public void Push(Vector3 direction)
            => _body.AddForce(direction * _bounceImpulse + Vector3.up * _bounceLift, ForceMode.VelocityChange);

        private void ReportWall(Collision collision)
        {
            var normal = collision.GetContact(0).normal;
            if (Mathf.Abs(normal.y) < MAX_WALL_NORMAL_Y)
                HitObstacle?.Invoke(this, normal);
        }
    }
}
