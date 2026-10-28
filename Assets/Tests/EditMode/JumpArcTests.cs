using NUnit.Framework;
using TastyZoo.Animals.Movement;

namespace TastyZoo.Tests
{
    public sealed class JumpArcTests
    {
        private const float GRAVITY = 9.81f;
        private const float TOLERANCE = 0.0001f;

        [Test]
        public void Flight_CoversDistance()
        {
            var arc = JumpArc.From(1f, 2.5f, GRAVITY);

            Assert.AreEqual(2.5f, arc.HorizontalSpeed * arc.FlightTime, TOLERANCE);
        }

        [Test]
        public void Apex_ReachesHeight()
        {
            var arc = JumpArc.From(1.5f, 3f, GRAVITY);
            var apexTime = arc.FlightTime * 0.5f;
            var apex = arc.VerticalSpeed * apexTime - 0.5f * GRAVITY * apexTime * apexTime;

            Assert.AreEqual(1.5f, apex, TOLERANCE);
        }
    }
}
