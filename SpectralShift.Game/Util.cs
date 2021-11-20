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

        public static Vector2 ElementwiseMultiply(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X * right.X, left.Y * right.Y);
        }

        public static Vector2 ElementwiseMin(Vector2 left, Vector2 right)
        {
            return new Vector2(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y));
        }

        public static Vector2 ElementwiseMax(Vector2 left, Vector2 right)
        {
            return new Vector2(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y));
        }

        public static float MinElement(Vector2 v)
        {
            return Math.Min(v.X, v.Y);
        }

        public static float MaxElement(Vector2 v)
        {
            return Math.Max(v.X, v.Y);
        }

        public static Vector2 Multiply(Matrix2 m, Vector2 v)
        {
            return m.Column0 * v.X + m.Column1 * v.Y;
            // return new Vector2((m.M11 + m.M21) * v.X, (m.M12 + m.M22) * v.Y);
        }
    }
}
