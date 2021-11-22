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

        public static Vector2 Reflect(Vector2 incident, Vector2 normal) => incident - 2 * Vector2.Dot(incident, normal) * normal;

        public static Vector2 ElementwiseMultiply(Vector2 left, Vector2 right) => new Vector2(left.X * right.X, left.Y * right.Y);

        public static float MinElement(Vector2 v) => Math.Min(v.X, v.Y);

        public static float MaxElement(Vector2 v) => Math.Max(v.X, v.Y);

        public static Vector2 Multiply(Matrix2 m, Vector2 v) => m.Column0 * v.X + m.Column1 * v.Y;

        /// <summary>
        /// Cauchy's equation
        /// </summary>
        /// <param name="a">First coefficient</param>
        /// <param name="b">Second coefficient</param>
        /// <param name="wavelength">Coefficients are usually given for wavlength in micrometres</param>
        public static float Cauchy(float a, float b, float wavelength) => a + b / (wavelength * wavelength);
    }
}
