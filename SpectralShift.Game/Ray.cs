using osuTK;

namespace SpectralShift.Game
{
    public struct Ray
    {
        public Vector2 Origin;
        public Vector2 Direction;

        public Ray(Vector2 origin, Vector2 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}
