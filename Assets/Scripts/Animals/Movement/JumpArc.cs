using UnityEngine;

namespace TastyZoo.Animals.Movement
{
    public readonly record struct JumpArc(float VerticalSpeed, float HorizontalSpeed, float FlightTime)
    {
        public static JumpArc From(float height, float distance, float gravity)
        {
            var verticalSpeed = Mathf.Sqrt(2f * gravity * height);
            var flightTime = 2f * verticalSpeed / gravity;
            return new(verticalSpeed, distance / flightTime, flightTime);
        }
    }
}
