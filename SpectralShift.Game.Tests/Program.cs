using osu.Framework;
using osu.Framework.Platform;

namespace SpectralShift.Game.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost("visual-tests"))
            using (var game = new SpectralShiftTestBrowser())
                host.Run(game);
        }
    }
}
