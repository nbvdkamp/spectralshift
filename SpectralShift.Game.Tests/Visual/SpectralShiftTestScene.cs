using osu.Framework.Testing;

namespace SpectralShift.Game.Tests.Visual
{
    public class SpectralShiftTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new SpectralShiftTestSceneTestRunner();

        private class SpectralShiftTestSceneTestRunner : SpectralShiftGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
