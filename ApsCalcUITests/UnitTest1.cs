using NUnit.Framework;
using ApsCalcUI;

namespace ApsCalcUITests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ShellTest()
        {
            float[] testModuleCounts = { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
            Shell testShell = new();
            testShell.BarrelCount = 1;
            testShell.HeadModule = Module.APHead;
            testShell.BaseModule = Module.BaseBleeder;
            testModuleCounts.CopyTo(testShell.BodyModuleCounts, 0);
            testShell.Gauge = 490;
            testShell.GPCasingCount = 1.5f;
            testShell.RGCasingCount = 1;
            testShell.IsDif = false;
            testShell.RailDraw = 500;

            testShell.CalculateLengths();
            Assert.AreEqual(testShell.TotalLength, 5055);
            Assert.AreEqual(testShell.ProjectileLength, 3830);
            Assert.AreEqual(testShell.LengthDifferential, 0);
            Assert.AreEqual(testShell.BodyLength, 3340);
            Assert.AreEqual(testShell.CasingLength, 1225);
        }

        [Test]
        public void InaccuracyTest()
        {
            float[] testModuleCounts = { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 };
            Shell testShell = new();
            testShell.BarrelCount = 1;
            testShell.HeadModule = Module.APHead;
            testShell.BaseModule = Module.BaseBleeder;
            testModuleCounts.CopyTo(testShell.BodyModuleCounts, 0);
            testShell.Gauge = 100;
            testShell.IsDif = false;

            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(5.5f, 0.3f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 0.810000062f);

            testShell.Gauge = 300;
            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(16.5f, 0.3f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 0.810000062f);

            testShell.Gauge = 400;
            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(22, 0.3f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 0.944999993f);

            testShell.Gauge = 500;
            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(27.5f, 0.3f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 1.02600002f);
        }
    }
}