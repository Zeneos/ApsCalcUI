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

            float[] testModuleCounts = { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
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
            testShell.CalculateReloadTime();

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
            Assert.AreEqual(nonSabotDirectHit, 9903.79883f);
            Assert.AreEqual(nonSabotNonDirectHit, 9566.33496f);


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
            Assert.AreEqual(sabotDirectHit, 8418.22852f);
            Assert.AreEqual(sabotNonDirectHit, 8256.47461f);


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
            Assert.AreEqual(hollowPointDirectHit, 9025.74219f);
            Assert.AreEqual(hollowPointNonDirectHit, hollowPointDirectHit);
        }

        [Test]
        public void ChemicalDamageTest()
        {
            float fragConeAngle = 60f;
            float fragAngleMultiplier = (2 + MathF.Sqrt(fragConeAngle)) / 16f;

            float[] testModuleCounts = { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
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
            testShell.CalculateReloadTime();
            testShell.CalculateDamageModifierByType(DamageType.HE);
            testShell.CalculateDamageByType(DamageType.Kinetic, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.HE, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.FlaK, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.Frag, fragAngleMultiplier);
            testShell.CalculateDamageByType(DamageType.EMP, fragAngleMultiplier);

            Assert.AreEqual(testShell.TotalLength, 5055);
            Assert.AreEqual(testShell.ProjectileLength, 3830);
            Assert.AreEqual(testShell.LengthDifferential, 0);
            Assert.AreEqual(testShell.BodyLength, 3340);
            Assert.AreEqual(testShell.CasingLength, 1225);
            Assert.AreEqual(testShell.OverallVelocityModifier, 1.78976035f);
            Assert.AreEqual(testShell.TotalRecoil, 4116.08154f);
            Assert.AreEqual(testShell.Velocity, 385.603973f);
            Assert.AreEqual(testShell.MaxDraw, 100241.711f);
            Assert.AreEqual(testShell.ReloadTime, 177.807022f);
            Assert.AreEqual(testShell.OverallChemModifier, 0.25f);
            Assert.AreEqual(testShell.RawHE, 743.169617f);
            Assert.AreEqual(testShell.RawFlaK, 675.936951f);
            Assert.AreEqual(testShell.DamageDict[DamageType.Frag], 9691.6084f);
            Assert.AreEqual(testShell.DamageDict[DamageType.EMP], 397.768982f);
        }

        [Test]
        public void InaccuracyTest1()
        {
            float[] testModuleCounts = { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 };
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
            float[] testModuleCounts = { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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