using UnityEngine;

namespace TastyZoo.World
{
    public sealed class ArenaBounds
    {
        private readonly Camera _camera;
        private readonly float _margin;

        private Rect _inner;
        private Vector2Int _screen;

        public ArenaBounds(Camera camera, float margin)
        {
            _camera = camera;
            _margin = margin;
        }

        public bool IsLeaving(Vector3 position, Vector3 direction)
        {
            Refresh();
            return position.x < _inner.xMin && direction.x < 0f
                || position.x > _inner.xMax && direction.x > 0f
                || position.z < _inner.yMin && direction.z < 0f
                || position.z > _inner.yMax && direction.z > 0f;
        }

        public Vector3 RandomPoint()
        {
            Refresh();
            return new(Random.Range(_inner.xMin, _inner.xMax), 0f, Random.Range(_inner.yMin, _inner.yMax));
        }

        private void Refresh()
        {
            var screen = new Vector2Int(Screen.width, Screen.height);
            if (screen == _screen)
                return;

            _screen = screen;
            var min = ProjectToGround(Vector2.zero);
            var max = ProjectToGround(Vector2.one);
            _inner = Rect.MinMaxRect(
                Mathf.Min(min.x, max.x) + _margin,
                Mathf.Min(min.z, max.z) + _margin,
                Mathf.Max(min.x, max.x) - _margin,
                Mathf.Max(min.z, max.z) - _margin);
        }

        private Vector3 ProjectToGround(Vector2 viewport)
        {
            var ray = _camera.ViewportPointToRay(viewport);
            new Plane(Vector3.up, Vector3.zero).Raycast(ray, out var distance);
            return ray.GetPoint(distance);
        }
    }
}
