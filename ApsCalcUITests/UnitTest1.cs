using NUnit.Framework;
using ApsCalcUI;
using System;

namespace ApsCalcUITests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void KineticDamageTest()
        {
            float fragConeAngle = 60f;
            float fragAngleMultiplier = (2 + MathF.Sqrt(fragConeAngle)) / 16f;
            float directHitAngleFromPerpendicularDegrees = 0f;
            float nonDirectHitAngleFromPerpendicularDegrees = 15f;

            float[] testModuleCounts = { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            PenCalc.Scheme testScheme = new();
            Shell testShell = new();
            testShell.BarrelCount = 1;
            testShell.HeadModule = Module.APHead;
            testShell.BaseModule = Module.BaseBleeder;
            testModuleCounts.CopyTo(testShell.BodyModuleCounts, 0);
            testShell.Gauge = 490;
            testShell.GaugeCoefficient = MathF.Pow(testShell.Gauge / 500f, 1.8f);
            testShell.GPCasingCount = 1.5f;
            testShell.RGCasingCount = 1;
            testShell.IsDif = false;
            testShell.RailDraw = 500;

            testShell.CalculateLengths();
            testShell.CalculateRecoil();
            testShell.CalculateMaxDraw();
            testShell.CalculateReloadTime(600f);

            // AP head
            testShell.NonSabotAngleMultiplier = MathF.Abs(MathF.Cos(directHitAngleFromPerpendicularDegrees * MathF.PI / 180));
            testShell.SabotAngleMultiplier = MathF.Abs(MathF.Cos(directHitAngleFromPerpendicularDegrees * MathF.PI / 240));
            testShell.CalculateVelocityModifier();
            testShell.CalculateVelocity();
            testShell.CalculateDamageModifierByType(DamageType.Kinetic);
            testShell.CalculateDamageByType(DamageType.Kinetic, fragAngleMultiplier);
            testShell.CalculateDpsByType(
                DamageType.Kinetic, 
                1f, 
                1800, 
                500, 
                250, 
                600, 
                60, 
                5, 
                false, 
                testScheme, 
                directHitAngleFromPerpendicularDegrees);
            float nonSabotDirectHit = testShell.DamageDict[DamageType.Kinetic];

            testShell.NonSabotAngleMultiplier = MathF.Abs(MathF.Cos(nonDirectHitAngleFromPerpendicularDegrees * MathF.PI / 180));
            testShell.SabotAngleMultiplier = MathF.Abs(MathF.Cos(nonDirectHitAngleFromPerpendicularDegrees * MathF.PI / 240));
            testShell.CalculateDpsByType(
                DamageType.Kinetic, 
                1f, 
                1800, 
                500, 
                250, 
                600, 
                60, 
                5, 
                false, 
                testScheme, 
                nonDirectHitAngleFromPerpendicularDegrees);
            float nonSabotNonDirectHit = testShell.DamageDict[DamageType.Kinetic];
            Assert.AreEqual(nonSabotDirectHit, testShell.RawKD);
            Assert.AreEqual(nonSabotDirectHit, 10413.1367f);
            Assert.AreEqual(nonSabotNonDirectHit, 10058.3174f);


            // Sabot head uses 3/4 angle
            testShell.HeadModule = Module.SabotHead;
            testShell.NonSabotAngleMultiplier = MathF.Abs(MathF.Cos(directHitAngleFromPerpendicularDegrees * MathF.PI / 180));
            testShell.SabotAngleMultiplier = MathF.Abs(MathF.Cos(directHitAngleFromPerpendicularDegrees * MathF.PI / 240));
            testShell.CalculateVelocityModifier();
            testShell.CalculateVelocity();
            testShell.CalculateDamageModifierByType(DamageType.Kinetic);
            testShell.CalculateDamageByType(DamageType.Kinetic, fragAngleMultiplier);
            testShell.CalculateDpsByType(
                DamageType.Kinetic,
                1f,
                1800,
                500,
                250,
                600,
                60,
                5,
                false,
                testScheme,
                directHitAngleFromPerpendicularDegrees);
            float sabotDirectHit = testShell.DamageDict[DamageType.Kinetic];

            testShell.NonSabotAngleMultiplier = MathF.Abs(MathF.Cos(nonDirectHitAngleFromPerpendicularDegrees * MathF.PI / 180));
            testShell.SabotAngleMultiplier = MathF.Abs(MathF.Cos(nonDirectHitAngleFromPerpendicularDegrees * MathF.PI / 240));
            testShell.CalculateDpsByType(
                DamageType.Kinetic,
                1f,
                1800,
                500,
                250,
                600,
                60,
                5,
                false,
                testScheme,
                nonDirectHitAngleFromPerpendicularDegrees);
            float sabotNonDirectHit = testShell.DamageDict[DamageType.Kinetic];
            Assert.AreEqual(sabotDirectHit, testShell.RawKD);
            Assert.AreEqual(sabotDirectHit, 8851.16602f);
            Assert.AreEqual(sabotNonDirectHit, 8681.09277f);


            // Hollow point head ignores angle
            testShell.HeadModule = Module.HollowPoint;
            testShell.NonSabotAngleMultiplier = MathF.Abs(MathF.Cos(directHitAngleFromPerpendicularDegrees * MathF.PI / 180));
            testShell.SabotAngleMultiplier = MathF.Abs(MathF.Cos(directHitAngleFromPerpendicularDegrees * MathF.PI / 240));
            testShell.CalculateVelocityModifier();
            testShell.CalculateVelocity();
            testShell.CalculateDamageModifierByType(DamageType.Kinetic);
            testShell.CalculateDamageByType(DamageType.Kinetic, fragAngleMultiplier);
            testShell.CalculateDpsByType(
                DamageType.Kinetic,
                1f,
                1800,
                500,
                250,
                600,
                60,
                5,
                false,
                testScheme,
                directHitAngleFromPerpendicularDegrees);
            float hollowPointDirectHit = testShell.DamageDict[DamageType.Kinetic];

            testShell.NonSabotAngleMultiplier = MathF.Abs(MathF.Cos(nonDirectHitAngleFromPerpendicularDegrees * MathF.PI / 180));
            testShell.SabotAngleMultiplier = MathF.Abs(MathF.Cos(nonDirectHitAngleFromPerpendicularDegrees * MathF.PI / 240));
            testShell.CalculateDpsByType(
                DamageType.Kinetic,
                1f,
                1800,
                500,
                250,
                600,
                60,
                5,
                false,
                testScheme,
                nonDirectHitAngleFromPerpendicularDegrees);
            float hollowPointNonDirectHit = testShell.DamageDict[DamageType.Kinetic];
            Assert.AreEqual(hollowPointDirectHit, testShell.RawKD);
            Assert.AreEqual(hollowPointDirectHit, 9489.92285f);
            Assert.AreEqual(hollowPointNonDirectHit, hollowPointDirectHit);
        }

        [Test]
        public void ChemicalDamageTest()
        {
            float fragConeAngle = 60f;
            float fragAngleMultiplier = (2 + MathF.Sqrt(fragConeAngle)) / 16f;

            float[] testModuleCounts = { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
            PenCalc.Scheme testScheme = new();
            Shell testShell = new();
            testShell.BarrelCount = 1;
            testShell.HeadModule = Module.APHead;
            testShell.BaseModule = Module.BaseBleeder;
            testModuleCounts.CopyTo(testShell.BodyModuleCounts, 0);
            testShell.Gauge = 490;
            testShell.GaugeCoefficient = MathF.Pow(testShell.Gauge / 500f, 1.8f);
            testShell.GPCasingCount = 1.5f;
            testShell.RGCasingCount = 1;
            testShell.IsDif = false;
            testShell.RailDraw = 500;

            testShell.CalculateLengths();
            testShell.CalculateVelocityModifier();
            testShell.CalculateRecoil();
            testShell.CalculateVelocity();
            testShell.CalculateMaxDraw();
            testShell.CalculateReloadTime(600f);
            testShell.CalculateDamageModifierByType(DamageType.HE);
            testShell.CalculateDamageByType(DamageType.Kinetic, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.HE, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.Flak, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.Frag, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.EMP, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.Smoke, fragAngleMultiplier);

            Assert.AreEqual(testShell.TotalLength, 5545);
            Assert.AreEqual(testShell.ProjectileLength, 4320);
            Assert.AreEqual(testShell.LengthDifferential, 0);
            Assert.AreEqual(testShell.BodyLength, 3830);
            Assert.AreEqual(testShell.CasingLength, 1225);
            Assert.AreEqual(testShell.OverallVelocityModifier, 1.78467369f);
            Assert.AreEqual(testShell.TotalRecoil, 4116.08154f);
            Assert.AreEqual(testShell.Velocity, 362.045319f);
            Assert.AreEqual(testShell.MaxDraw, 112295.32f);
            Assert.AreEqual(testShell.ClusterReloadTime, 194.836182f);
            Assert.AreEqual(testShell.OverallChemModifier, 0.25f);
            Assert.AreEqual(testShell.RawHE, 773.504028f);
            Assert.AreEqual(testShell.DamageDict[DamageType.Flak], 703.5271f);
            Assert.AreEqual(testShell.DamageDict[DamageType.Frag], 10132.1357f);
            Assert.AreEqual(testShell.DamageDict[DamageType.EMP], 415.849396f);
            Assert.AreEqual(testShell.DamageDict[DamageType.Smoke], 964.288391f);

            // Note: when testing HEAT damage, ensure HE warhead is directly behind head
            testShell.HeadModule = Module.ShapedChargeHead;
            testShell.CalculateDamageByType(DamageType.HEAT, fragAngleMultiplier);
            Assert.AreEqual(testShell.DamageDict[DamageType.HEAT], 9557.65039f);
        }

        [Test]
        public void InaccuracyTest1()
        {
            float[] testModuleCounts = { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            Shell testShell = new();
            testShell.BarrelCount = 1;
            testShell.HeadModule = Module.APHead;
            testShell.BaseModule = Module.BaseBleeder;
            testModuleCounts.CopyTo(testShell.BodyModuleCounts, 0);
            testShell.Gauge = 100;
            testShell.GaugeCoefficient = MathF.Pow(testShell.Gauge / 500f, 1.8f);
            testShell.IsDif = false;

            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(5.5f, 0.3f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 0.810000062f);

            testShell.Gauge = 300;
            testShell.GaugeCoefficient = MathF.Pow(testShell.Gauge / 500f, 1.8f);
            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(16.5f, 0.3f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 0.810000062f);

            testShell.Gauge = 400;
            testShell.GaugeCoefficient = MathF.Pow(testShell.Gauge / 500f, 1.8f);
            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(22, 0.3f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 0.944999993f);

            testShell.Gauge = 500;
            testShell.GaugeCoefficient = MathF.Pow(testShell.Gauge / 500f, 1.8f);
            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(27.5f, 0.3f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 1.02600002f);
        }

        [Test]
        public void InaccuracyTest2()
        {
            float[] testModuleCounts = { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Shell testShell = new();
            testShell.BarrelCount = 1;
            testShell.HeadModule = Module.APHead;
            testShell.BaseModule = Module.BaseBleeder;
            testModuleCounts.CopyTo(testShell.BodyModuleCounts, 0);
            testShell.Gauge = 392;
            testShell.GaugeCoefficient = MathF.Pow(testShell.Gauge / 500f, 1.8f);
            testShell.IsDif = false;

            testShell.CalculateLengths();
            testShell.CalculateMaxProjectileLengthForInaccuracy(21.56f, 0.2f);
            Assert.AreEqual(testShell.OverallInaccuracyModifier, 1.35f);
            Assert.AreEqual(testShell.CalculateMaxProjectileLengthForInaccuracy(21.56f, 0.2f), 899.569763f);
        }
    }
}