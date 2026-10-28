using NUnit.Framework;
using TastyZoo.Animals;
using UnityEngine;

namespace TastyZoo.Tests
{
    public sealed class AnimalTests
    {
        private const float TOLERANCE = 0.0001f;

        private AnimalConfig _config;

        [SetUp]
        public void SetUp()
            => _config = ScriptableObject.CreateInstance<AnimalConfig>();

        [TearDown]
        public void TearDown()
            => Object.DestroyImmediate(_config);

        [Test]
        public void Reflect_TowardsWall_MirrorsDirection()
        {
            var animal = Create(new Vector3(1f, 0f, 1f).normalized);

            animal.Reflect(Vector3.left);

            Assert.AreEqual(0f, Vector3.Distance(new Vector3(-1f, 0f, 1f).normalized, animal.Direction), TOLERANCE);
        }

        [Test]
        public void Reflect_AwayFromWall_KeepsDirection()
        {
            var animal = Create(Vector3.left);

            animal.Reflect(Vector3.left);

            Assert.AreEqual(Vector3.left, animal.Direction);
        }

        private Animal Create(Vector3 direction)
            => new(0, _config, null, null, null, null, direction);
    }
}
