using System;
using osuTK;

namespace SpectralShift.Game
{
    public class Util
    {
        public static Vector2 Refract(Vector2 incident, Vector2 normal, float eta)
        {
            float iDotN = Vector2.Dot(incident, normal);
            float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);

            if (k < 0)
                return Vector2.Zero;

            return eta * incident - (eta * iDotN + (float)Math.Sqrt(k)) * normal;
        }

        public static Vector2 Reflect(Vector2 incident, Vector2 normal)
        {
            return incident - 2 * Vector2.Dot(incident, normal) * normal;
        }
    }
}
