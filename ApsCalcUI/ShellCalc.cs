﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using PenCalc;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    public struct ModuleCount
    {
        public int HeadIndex;
        public float Var0Count;
        public float Var1Count;
        public float Var2Count;
        public float Var3Count;
        public float Var4Count;
        public float Var5Count;
        public float Var6Count;
        public float Var7Count;
        public float Var8Count;
        public float GPCount;
        public float RGCount;
    }

    // Damage types.  Enum is faster than strings.
    public enum DamageType : int
    {
        Kinetic,
        Emp,
        FlaK,
        Frag,
        HE,
        HEAT,
        Pendepth,
        Disruptor
    }


    public class ShellCalc
    {
        /// <summary>
        /// Takes shell parameters and calculates performance of shell permutations.
        /// </summary>
        /// <param name="barrelCount">Number of barrels</param>
        /// <param name="gauge">Desired gauge in mm</param>
        /// <param name="headList">List of module indices for every module to be used as head</param>
        /// <param name="baseModule">The special base module, if any</param>
        /// <param name="fixedModuleCounts">An array of integers representing number of shells at that index in module list</param>
        /// <param name="fixedModuleTotal">Minimum number of modules on every shell</param>
        /// <param name="variableModuleIndices">Module indices of modules to be used in varying numbers in testing</param>
        /// <param name="maxGPInput">Max desired number of gunpowder casings</param>
        /// <param name="maxRGInput">Max desired number of railgun casings</param>
        /// <param name="minShellLengthInput">Min desired shell length in mm, exclusive</param>
        /// <param name="maxShellLengthInput">Max desired shell length in mm, inclusive</param>
        /// <param name="maxDrawInput">Max desired rail draw</param>
        /// <param name="maxRecoilInput">Max desired recoil, including rail and GP</param>
        /// <param name="minVelocityInput">Min desired velocity</param>
        /// <param name="minEffectiveRangeInput">Min desired effective range</param>
        /// <param name="targetAC">Armor class of target for kinetic damage calculations</param>
        /// <param name="damageType">DamageType Enum, determines which damage type is optimized</param>
        /// <param name="targetArmorScheme">Target armor scheme, from Pencalc namespace</param>
        /// <param name="testType">0 for DPS per volume, 1 for DPS per cost</param>
        /// <param name="labels">True if row headers should be printed on every line</param>
        /// <param name="testInterval">Test interval in min</param>
        /// <param name="storagePerVolume">Material storage per container volume</param>
        /// <param name="storagePerCost">Material storage per container cost</param>
        /// <param name="ppm">Engine power per material</param>
        /// <param name="ppv">Engine power per volume</param>
        /// <param name="ppc">Engine power per block cost</param>
        /// <param name="fuel">Whether engine uses special Fuel storage</param>
        public ShellCalc(
            int barrelCount,
            float gauge,
            List<int> headList,
            Module baseModule,
            float[] fixedModuleCounts,
            float fixedModuleTotal,
            int[] variableModuleIndices,
            float maxGPInput,
            float maxRGInput,
            float minShellLengthInput,
            float maxShellLengthInput,
            float maxDrawInput,
            float maxRecoilInput,
            float minVelocityInput,
            float minEffectiveRangeInput,
            float targetAC,
            DamageType damageType,
            Scheme targetArmorScheme,
            int testType,
            bool labels,
            int testInterval,
            float storagePerVolume,
            float storagePerCost,
            float ppm,
            float ppv,
            float ppc,
            bool fuel
            )
        {
            BarrelCount = barrelCount;
            Gauge = gauge;
            HeadList = headList;
            BaseModule = baseModule;
            FixedModuleCounts = fixedModuleCounts;
            FixedModuleTotal = fixedModuleTotal;
            VariableModuleIndices = variableModuleIndices;
            MaxGPInput = maxGPInput;
            MaxRGInput = maxRGInput;
            MinShellLength = minShellLengthInput;
            MaxShellLength = maxShellLengthInput;
            MaxDrawInput = maxDrawInput;
            MaxRecoilInput = maxRecoilInput;
            MinVelocityInput = minVelocityInput;
            MinEffectiveRangeInput = minEffectiveRangeInput;
            TargetAC = targetAC;
            DamageType = damageType;
            TargetArmorScheme = targetArmorScheme;
            TestType = testType;
            Labels = labels;
            TestInterval = testInterval;
            TestIntervalSeconds = testInterval * 60;
            StoragePerVolume = storagePerVolume;
            StoragePerCost = storagePerCost;
            Ppm = ppm;
            Ppv = ppv;
            Ppc = ppc;
            Fuel = fuel;
        }


        public int BarrelCount { get; }
        public float Gauge { get; }
        public List<int> HeadList { get; }
        public Module BaseModule { get; }
        public float[] FixedModuleCounts { get; }
        public float FixedModuleTotal { get; }
        public int[] VariableModuleIndices { get; }
        public float MaxGPInput { get; }
        public float MaxRGInput { get; }
        public float MinShellLength { get; }
        public float MaxShellLength { get; }
        public float MaxDrawInput { get; }
        public float MaxRecoilInput { get; }
        public float MinVelocityInput { get; }
        public float MinEffectiveRangeInput { get; }
        public float TargetAC { get; }
        public DamageType DamageType { get; }
        public Scheme TargetArmorScheme { get; }
        public int TestType { get; }
        public bool Labels { get; }
        public int TestInterval { get; }
        public int TestIntervalSeconds { get; }
        public float StoragePerVolume { get; }
        public float StoragePerCost { get; }
        public float Ppm { get; }
        public float Ppv { get; }
        public float Ppc { get; }
        public bool Fuel { get; }


        // Store top-DPS shells by loader length
        public Shell TopBelt { get; set; } = new();
        public Shell Top1000 { get; set; } = new();
        public Shell Top2000 { get; set; } = new();
        public Shell Top3000 { get; set; } = new();
        public Shell Top4000 { get; set; } = new();
        public Shell Top5000 { get; set; } = new();
        public Shell Top6000 { get; set; } = new();
        public Shell Top7000 { get; set; } = new();
        public Shell Top8000 { get; set; } = new();

        public Dictionary<string, Shell> TopDpsShells { get; set; } = new Dictionary<string, Shell>();
        public List<Shell> TopShellsLocal { get; set; } = new List<Shell>();


        /// <summary>
        /// The iterable generator for shells.  Generates all shell possible permutations of shell within given parameters.
        /// The "var_Max" and such represent number of each variable module. During test parameter creation, user selects variable
        /// module types; these are represented by indices corresponding to position in Module.AllModules array
        /// There must always be 9 indices; unassigned/unused indices are set to value of first index (index 0) - this allows
        /// GenerateModuleCounts() to ignore unused indices to avoid generating duplicate shell configurations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModuleCount> GenerateModuleCounts()
        {
            float var0Max = 20f - FixedModuleTotal;
            float var1Max;
            float var2Max;
            float var3Max;
            float var4Max;
            float var5Max;
            float var6Max;
            float var7Max;
            float var8Max;
            float gpMax;
            float rgMax;

            foreach (int index in HeadList)
            {
                for (float var0Count = 0; var0Count <= var0Max; var0Count++)
                {
                    if (VariableModuleIndices[1] == VariableModuleIndices[0])
                    {
                        var1Max = 0; // No need to add duplicates
                    }
                    else
                    {
                        var1Max = 20f - (FixedModuleTotal + var0Count);
                    }

                    for (float var1Count = 0; var1Count <= var1Max; var1Count++)
                    {
                        if (VariableModuleIndices[2] == VariableModuleIndices[0])
                        {
                            var2Max = 0; // No need to add duplicates
                        }
                        else
                        {
                            var2Max = 20f - (FixedModuleTotal + var0Count + var1Count);
                        }

                        for (float var2Count = 0; var2Count <= var2Max; var2Count++)
                        {
                            if (VariableModuleIndices[3] == VariableModuleIndices[0])
                            {
                                var3Max = 0; // No need to add duplicates
                            }
                            else
                            {
                                var3Max = 20f - (FixedModuleTotal + var0Count + var1Count + var2Count);
                            }

                            for (float var3Count = 0; var3Count <= var3Max; var3Count++)
                            {
                                if (VariableModuleIndices[4] == VariableModuleIndices[0])
                                {
                                    var4Max = 0; // No need to add duplicates
                                }
                                else
                                {
                                    var4Max = 20f - (FixedModuleTotal + var0Count + var1Count + var2Count + var3Count);
                                }

                                for (float var4Count = 0; var4Count <= var4Max; var4Count++)
                                {
                                    if (VariableModuleIndices[5] == VariableModuleIndices[0])
                                    {
                                        var5Max = 0; // No need to add duplicates
                                    }
                                    else
                                    {
                                        var5Max = 20f - (FixedModuleTotal + var0Count + var1Count + var2Count + var3Count + var4Count);
                                    }

                                    for (float var5Count = 0; var5Count <= var5Max; var5Count++)
                                    {
                                        if (VariableModuleIndices[6] == VariableModuleIndices[0])
                                        {
                                            var6Max = 0; // No need to add duplicates
                                        }
                                        else
                                        {
                                            var6Max = 20f - (FixedModuleTotal + var0Count + var1Count + var2Count + var3Count + var4Count + var5Count);
                                        }

                                        for (float var6Count = 0; var6Count <= var6Max; var6Count++)
                                        {
                                            if (VariableModuleIndices[7] == VariableModuleIndices[0])
                                            {
                                                var7Max = 0; // No need to add duplicates
                                            }
                                            else
                                            {
                                                var7Max = 20f - (FixedModuleTotal
                                                    + var0Count
                                                    + var1Count
                                                    + var2Count
                                                    + var3Count
                                                    + var4Count
                                                    + var5Count
                                                    + var6Count);
                                            }

                                            for (float var7Count = 0; var7Count <= var7Max; var7Count++)
                                            {
                                                if (VariableModuleIndices[8] == VariableModuleIndices[0])
                                                {
                                                    var8Max = 0; // No need to add duplicates
                                                }
                                                else
                                                {
                                                    var8Max = 20f - (FixedModuleTotal
                                                        + var0Count
                                                        + var1Count
                                                        + var2Count
                                                        + var3Count
                                                        + var4Count
                                                        + var5Count
                                                        + var6Count
                                                        + var7Count);
                                                }

                                                for (float var8Count = 0; var8Count <= var8Max; var8Count++)
                                                {
                                                    gpMax = MathF.Min(20f - (FixedModuleTotal + var0Count + var1Count), MaxGPInput);

                                                    for (float gpCount = 0; gpCount <= gpMax; gpCount += 0.01f)
                                                    {
                                                        rgMax = MathF.Min(20f - (FixedModuleTotal + var0Count + var1Count + gpCount), MaxRGInput);

                                                        for (float rgCount = 0; rgCount <= rgMax; rgCount++)
                                                        {
                                                            yield return new ModuleCount
                                                            {
                                                                HeadIndex = index,
                                                                Var0Count = var0Count,
                                                                Var1Count = var1Count,
                                                                Var2Count = var2Count,
                                                                Var3Count = var3Count,
                                                                Var4Count = var4Count,
                                                                Var5Count = var5Count,
                                                                Var6Count = var6Count,
                                                                Var7Count = var7Count,
                                                                Var8Count = var8Count,
                                                                GPCount = gpCount,
                                                                RGCount = rgCount
                                                            };
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Iterates over possible configurations and stores the best according to test parameters
        /// </summary>
        public void ShellTest()
        {
            foreach (ModuleCount counts in GenerateModuleCounts())
            {
                Shell shellUnderTesting = new();
                shellUnderTesting.BarrelCount = BarrelCount;
                shellUnderTesting.HeadModule = Module.AllModules[counts.HeadIndex];
                shellUnderTesting.BaseModule = BaseModule;
                FixedModuleCounts.CopyTo(shellUnderTesting.BodyModuleCounts, 0);

                shellUnderTesting.Gauge = Gauge;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[0]] += counts.Var0Count;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[1]] += counts.Var1Count;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[2]] += counts.Var2Count;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[3]] += counts.Var3Count;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[4]] += counts.Var4Count;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[5]] += counts.Var5Count;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[6]] += counts.Var6Count;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[7]] += counts.Var7Count;
                shellUnderTesting.BodyModuleCounts[VariableModuleIndices[8]] += counts.Var8Count;
                shellUnderTesting.GPCasingCount = counts.GPCount;
                shellUnderTesting.RGCasingCount = counts.RGCount;

                shellUnderTesting.CalculateLengths();

                if (shellUnderTesting.TotalLength > MinShellLength && shellUnderTesting.TotalLength <= MaxShellLength)
                {
                    shellUnderTesting.CalculateVelocityModifier();
                    shellUnderTesting.CalculateRecoil();
                    shellUnderTesting.CalculateMaxDraw();

                    float maxDraw = MathF.Min(shellUnderTesting.MaxDraw, MaxDrawInput);
                    maxDraw = MathF.Min(maxDraw, MaxRecoilInput - shellUnderTesting.GPRecoil);
                    float minDraw = shellUnderTesting.CalculateMinimumDrawForVelocityandRange(MinVelocityInput, MinEffectiveRangeInput);

                    if (maxDraw >= minDraw)
                    {
                        shellUnderTesting.CalculateReloadTime();
                        shellUnderTesting.CalculateDamageModifierByType(DamageType);
                        shellUnderTesting.CalculateDamageByType(DamageType);
                        shellUnderTesting.CalculateCooldownTime();
                        shellUnderTesting.CalculateCoolerVolumeAndCost();
                        shellUnderTesting.CalculateLoaderVolumeAndCost();
                        shellUnderTesting.CalculateVariableVolumesAndCosts(TestIntervalSeconds, StoragePerVolume, StoragePerCost);


                        float optimalDraw = 0;
                        if (maxDraw > 0)
                        {
                            float bottomScore = 0;
                            float topScore = 0;
                            float midRangeLower = 0;
                            float midRangeLowerScore = 0;
                            float midRangeUpper = 0;
                            float midRangeUpperScore = 0;


                            shellUnderTesting.RailDraw = minDraw;
                            shellUnderTesting.CalculateDpsByType(
                                DamageType,
                                TargetAC,
                                TargetArmorScheme,
                                TestIntervalSeconds,
                                StoragePerVolume,
                                StoragePerCost,
                                Ppm,
                                Ppv,
                                Ppc,
                                Fuel);
                            if (TestType == 0)
                            {
                                bottomScore = shellUnderTesting.DpsPerVolumeDict[DamageType];
                            }
                            else if (TestType == 1)
                            {
                                bottomScore = shellUnderTesting.DpsPerCostDict[DamageType];
                            }

                            shellUnderTesting.RailDraw = maxDraw;
                            shellUnderTesting.CalculateDpsByType(
                                DamageType,
                                TargetAC,
                                TargetArmorScheme,
                                TestIntervalSeconds,
                                StoragePerVolume,
                                StoragePerCost,
                                Ppm,
                                Ppv,
                                Ppc,
                                Fuel);
                            if (TestType == 0)
                            {
                                topScore = shellUnderTesting.DpsPerVolumeDict[DamageType];
                            }
                            else if (TestType == 1)
                            {
                                topScore = shellUnderTesting.DpsPerCostDict[DamageType];
                            }

                            if (topScore > bottomScore)
                            {
                                // Check if max draw is optimal
                                shellUnderTesting.RailDraw = maxDraw - 1f;
                                shellUnderTesting.CalculateDpsByType(
                                    DamageType,
                                    TargetAC,
                                    TargetArmorScheme,
                                    TestIntervalSeconds,
                                    StoragePerVolume,
                                    StoragePerCost,
                                    Ppm,
                                    Ppv,
                                    Ppc,
                                    Fuel);
                                if (TestType == 0)
                                {
                                    bottomScore = shellUnderTesting.DpsPerVolumeDict[DamageType];
                                }
                                else if (TestType == 1)
                                {
                                    bottomScore = shellUnderTesting.DpsPerCostDict[DamageType];
                                }

                                if (topScore > bottomScore)
                                {
                                    optimalDraw = maxDraw;
                                }
                            }
                            else
                            {
                                // Check if min draw is optimal
                                shellUnderTesting.RailDraw = minDraw + 1f;
                                shellUnderTesting.CalculateDpsByType(
                                    DamageType,
                                    TargetAC,
                                    TargetArmorScheme,
                                    TestIntervalSeconds,
                                    StoragePerVolume,
                                    StoragePerCost,
                                    Ppm,
                                    Ppv,
                                    Ppc,
                                    Fuel);
                                if (TestType == 0)
                                {
                                    topScore = shellUnderTesting.DpsPerVolumeDict[DamageType];
                                }
                                else if (TestType == 1)
                                {
                                    topScore = shellUnderTesting.DpsPerCostDict[DamageType];
                                }

                                if (bottomScore > topScore)
                                {
                                    optimalDraw = minDraw;
                                }
                            }

                            if (optimalDraw == 0)
                            {
                                float topOfRange = maxDraw;
                                // Binary search to find optimal draw without testing every value
                                float bottomOfRange = 0;
                                while (topOfRange - bottomOfRange > 1)
                                {

                                    midRangeLower = MathF.Floor((topOfRange + bottomOfRange) / 2f);
                                    midRangeUpper = midRangeLower + 1f;

                                    shellUnderTesting.RailDraw = midRangeLower;
                                    shellUnderTesting.CalculateDpsByType(
                                        DamageType,
                                        TargetAC,
                                        TargetArmorScheme,
                                        TestIntervalSeconds,
                                        StoragePerVolume,
                                        StoragePerCost,
                                        Ppm,
                                        Ppv,
                                        Ppc,
                                        Fuel);
                                    if (TestType == 0)
                                    {
                                        midRangeLowerScore = shellUnderTesting.DpsPerVolumeDict[DamageType];
                                    }
                                    else if (TestType == 1)
                                    {
                                        midRangeLowerScore = shellUnderTesting.DpsPerCostDict[DamageType];
                                    }

                                    shellUnderTesting.RailDraw = midRangeUpper;
                                    shellUnderTesting.CalculateDpsByType(
                                        DamageType,
                                        TargetAC,
                                        TargetArmorScheme,
                                        TestIntervalSeconds,
                                        StoragePerVolume,
                                        StoragePerCost,
                                        Ppm,
                                        Ppv,
                                        Ppc,
                                        Fuel);
                                    if (TestType == 0)
                                    {
                                        midRangeUpperScore = shellUnderTesting.DpsPerVolumeDict[DamageType];
                                    }
                                    else if (TestType == 1)
                                    {
                                        midRangeUpperScore = shellUnderTesting.DpsPerCostDict[DamageType];
                                    }

                                    // Determine which half of range to continue testing
                                    // Midrange upper will equal 0 a lot of time for pendepth
                                    if (midRangeUpperScore == 0)
                                    {
                                        bottomOfRange = midRangeUpper;
                                    }
                                    else if (midRangeLowerScore >= midRangeUpperScore)
                                    {
                                        topOfRange = midRangeLower;
                                    }
                                    else
                                    {
                                        bottomOfRange = midRangeUpper;
                                    }
                                }
                                // Take better of two remaining values
                                if (midRangeLowerScore >= midRangeUpperScore)
                                {
                                    optimalDraw = midRangeLower;
                                }
                                else
                                {
                                    optimalDraw = midRangeUpper;
                                }
                            }
                        }

                        // Check performance against top shells
                        shellUnderTesting.RailDraw = optimalDraw;
                        shellUnderTesting.CalculateDpsByType(
                            DamageType,
                            TargetAC,
                            TargetArmorScheme,
                            TestIntervalSeconds,
                            StoragePerVolume,
                            StoragePerCost,
                            Ppm,
                            Ppv,
                            Ppc,
                            Fuel);
                        if (DamageType == DamageType.Pendepth)
                        {
                            shellUnderTesting.CalculateDpsByType(
                                DamageType.Kinetic,
                                TargetAC,
                                TargetArmorScheme,
                                TestIntervalSeconds,
                                StoragePerVolume,
                                StoragePerCost,
                                Ppm,
                                Ppv,
                                Ppc,
                                Fuel);
                            shellUnderTesting.CalculateDpsByType(
                                DamageType.FlaK,
                                TargetAC,
                                TargetArmorScheme,
                                TestIntervalSeconds,
                                StoragePerVolume,
                                StoragePerCost,
                                Ppm,
                                Ppv,
                                Ppc,
                                Fuel);
                            shellUnderTesting.CalculateDpsByType(
                                DamageType.Frag,
                                TargetAC,
                                TargetArmorScheme,
                                TestIntervalSeconds,
                                StoragePerVolume,
                                StoragePerCost,
                                Ppm,
                                Ppv,
                                Ppc,
                                Fuel);
                            shellUnderTesting.CalculateDpsByType(
                                DamageType.HE,
                                TargetAC,
                                TargetArmorScheme,
                                TestIntervalSeconds,
                                StoragePerVolume,
                                StoragePerCost,
                                Ppm,
                                Ppv,
                                Ppc,
                                Fuel);
                        }
                        shellUnderTesting.CalculateVelocity();
                        shellUnderTesting.CalculateEffectiveRange();

                        if (TestType == 0)
                        {
                            if (shellUnderTesting.TotalLength <= 1000f)
                            {
                                if (shellUnderTesting.DpsPerVolumeDict[DamageType] > Top1000.DpsPerVolumeDict[DamageType])
                                {
                                    Top1000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 2000f)
                            {
                                if (shellUnderTesting.DpsPerVolumeDict[DamageType] > Top2000.DpsPerVolumeDict[DamageType])
                                {
                                    Top2000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 3000f)
                            {
                                if (shellUnderTesting.DpsPerVolumeDict[DamageType] > Top3000.DpsPerVolumeDict[DamageType])
                                {
                                    Top3000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 4000f)
                            {
                                if (shellUnderTesting.DpsPerVolumeDict[DamageType] > Top4000.DpsPerVolumeDict[DamageType])
                                {
                                    Top4000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 5000f)
                            {
                                if (shellUnderTesting.DpsPerVolumeDict[DamageType] > Top5000.DpsPerVolumeDict[DamageType])
                                {
                                    Top5000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 6000f)
                            {
                                if (shellUnderTesting.DpsPerVolumeDict[DamageType] > Top6000.DpsPerVolumeDict[DamageType])
                                {
                                    Top6000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 7000f)
                            {
                                if (shellUnderTesting.DpsPerVolumeDict[DamageType] > Top7000.DpsPerVolumeDict[DamageType])
                                {
                                    Top7000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 8000f)
                            {
                                if (shellUnderTesting.DpsPerVolumeDict[DamageType] > Top8000.DpsPerVolumeDict[DamageType])
                                {
                                    Top8000 = shellUnderTesting;
                                }
                            }
                        }
                        else if (TestType == 1)
                        {
                            if (shellUnderTesting.TotalLength <= 1000f)
                            {
                                if (shellUnderTesting.DpsPerCostDict[DamageType] > Top1000.DpsPerCostDict[DamageType])
                                {
                                    Top1000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 2000f)
                            {
                                if (shellUnderTesting.DpsPerCostDict[DamageType] > Top2000.DpsPerCostDict[DamageType])
                                {
                                    Top2000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 3000f)
                            {
                                if (shellUnderTesting.DpsPerCostDict[DamageType] > Top3000.DpsPerCostDict[DamageType])
                                {
                                    Top3000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 4000f)
                            {
                                if (shellUnderTesting.DpsPerCostDict[DamageType] > Top4000.DpsPerCostDict[DamageType])
                                {
                                    Top4000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 5000f)
                            {
                                if (shellUnderTesting.DpsPerCostDict[DamageType] > Top5000.DpsPerCostDict[DamageType])
                                {
                                    Top5000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 6000f)
                            {
                                if (shellUnderTesting.DpsPerCostDict[DamageType] > Top6000.DpsPerCostDict[DamageType])
                                {
                                    Top6000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 7000f)
                            {
                                if (shellUnderTesting.DpsPerCostDict[DamageType] > Top7000.DpsPerCostDict[DamageType])
                                {
                                    Top7000 = shellUnderTesting;
                                }
                            }
                            else if (shellUnderTesting.TotalLength <= 8000f)
                            {
                                if (shellUnderTesting.DpsPerCostDict[DamageType] > Top8000.DpsPerCostDict[DamageType])
                                {
                                    Top8000 = shellUnderTesting;
                                }
                            }
                        }


                        // Beltfed testing
                        if (shellUnderTesting.TotalLength <= 1000f)
                        {
                            Shell shellUnderTestingBelt = new();
                            shellUnderTestingBelt.BarrelCount = BarrelCount;
                            shellUnderTestingBelt.HeadModule = Module.AllModules[counts.HeadIndex];
                            shellUnderTestingBelt.BaseModule = BaseModule;
                            FixedModuleCounts.CopyTo(shellUnderTestingBelt.BodyModuleCounts, 0);

                            shellUnderTestingBelt.Gauge = Gauge;
                            shellUnderTestingBelt.BodyModuleCounts[VariableModuleIndices[0]] += counts.Var0Count;
                            shellUnderTestingBelt.BodyModuleCounts[VariableModuleIndices[1]] += counts.Var1Count;
                            shellUnderTestingBelt.BodyModuleCounts[VariableModuleIndices[2]] += counts.Var2Count;
                            shellUnderTestingBelt.GPCasingCount = counts.GPCount;
                            shellUnderTestingBelt.RGCasingCount = counts.RGCount;

                            shellUnderTestingBelt.IsBelt = true;
                            shellUnderTestingBelt.CalculateLengths();
                            shellUnderTestingBelt.CalculateVelocityModifier();
                            shellUnderTestingBelt.CalculateRecoil();
                            shellUnderTestingBelt.CalculateMaxDraw();
                            shellUnderTestingBelt.CalculateReloadTime();
                            shellUnderTestingBelt.CalculateReloadTimeBelt();
                            shellUnderTestingBelt.CalculateVariableVolumesAndCosts(TestIntervalSeconds, StoragePerVolume, StoragePerCost);
                            shellUnderTestingBelt.CalculateCooldownTime();
                            shellUnderTestingBelt.CalculateDamageModifierByType(DamageType);
                            shellUnderTestingBelt.CalculateDamageByType(DamageType);
                            shellUnderTestingBelt.CalculateLoaderVolumeAndCost();
                            shellUnderTestingBelt.CalculateCoolerVolumeAndCost();

                            if (maxDraw > 0)
                            {
                                float bottomScore = 0;
                                float topScore = 0;
                                float midRangeLower = 0;
                                float midRangeLowerScore = 0;
                                float midRangeUpper = 0;
                                float midRangeUpperScore = 0;

                                shellUnderTestingBelt.RailDraw = minDraw;
                                shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                    DamageType,
                                    TargetAC,
                                    TargetArmorScheme,
                                    TestIntervalSeconds,
                                    StoragePerVolume,
                                    StoragePerCost,
                                    Ppm,
                                    Ppv,
                                    Ppc,
                                    Fuel);
                                if (TestType == 0)
                                {
                                    bottomScore = shellUnderTestingBelt.DpsPerVolumeDict[DamageType];
                                }
                                else if (TestType == 1)
                                {
                                    bottomScore = shellUnderTestingBelt.DpsPerCostDict[DamageType];
                                }

                                shellUnderTestingBelt.RailDraw = maxDraw;
                                shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                    DamageType,
                                    TargetAC,
                                    TargetArmorScheme,
                                    TestIntervalSeconds,
                                    StoragePerVolume,
                                    StoragePerCost,
                                    Ppm,
                                    Ppv,
                                    Ppc,
                                    Fuel);
                                if (TestType == 0)
                                {
                                    topScore = shellUnderTestingBelt.DpsPerVolumeDict[DamageType];
                                }
                                else if (TestType == 1)
                                {
                                    topScore = shellUnderTestingBelt.DpsPerCostDict[DamageType];
                                }

                                if (topScore > bottomScore)
                                {
                                    // Check if max draw is optimal
                                    shellUnderTestingBelt.RailDraw = maxDraw - 1f;
                                    shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                        DamageType,
                                        TargetAC,
                                        TargetArmorScheme,
                                        TestIntervalSeconds,
                                        StoragePerVolume,
                                        StoragePerCost,
                                        Ppm,
                                        Ppv,
                                        Ppc,
                                        Fuel);
                                    if (TestType == 0)
                                    {
                                        bottomScore = shellUnderTestingBelt.DpsPerVolumeDict[DamageType];
                                    }
                                    else if (TestType == 1)
                                    {
                                        bottomScore = shellUnderTestingBelt.DpsPerCostDict[DamageType];
                                    }

                                    if (topScore > bottomScore)
                                    {
                                        optimalDraw = maxDraw;
                                    }
                                }
                                else
                                {
                                    // Check if min draw is optimal
                                    shellUnderTestingBelt.RailDraw = minDraw + 1f;
                                    shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                        DamageType,
                                        TargetAC,
                                        TargetArmorScheme,
                                        TestIntervalSeconds,
                                        StoragePerVolume,
                                        StoragePerCost,
                                        Ppm,
                                        Ppv,
                                        Ppc,
                                        Fuel);
                                    if (TestType == 0)
                                    {
                                        topScore = shellUnderTestingBelt.DpsPerVolumeDict[DamageType];
                                    }
                                    else if (TestType == 1)
                                    {
                                        topScore = shellUnderTestingBelt.DpsPerCostDict[DamageType];
                                    }

                                    if (bottomScore > topScore)
                                    {
                                        optimalDraw = minDraw;
                                    }
                                }

                                if (optimalDraw == 0)
                                {
                                    float topOfRange = maxDraw;
                                    // Binary search to find optimal draw without testing every value
                                    float bottomOfRange = 0;
                                    while (topOfRange - bottomOfRange > 1)
                                    {
                                        midRangeLower = MathF.Floor((topOfRange + bottomOfRange) / 2f);
                                        midRangeUpper = midRangeLower + 1f;

                                        shellUnderTestingBelt.RailDraw = midRangeLower;
                                        shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                            DamageType,
                                            TargetAC,
                                            TargetArmorScheme,
                                            TestIntervalSeconds,
                                            StoragePerVolume,
                                            StoragePerCost,
                                            Ppm,
                                            Ppv,
                                            Ppc,
                                            Fuel);
                                        if (TestType == 0)
                                        {
                                            midRangeLowerScore = shellUnderTestingBelt.DpsPerVolumeDict[DamageType];
                                        }
                                        else if (TestType == 1)
                                        {
                                            midRangeLowerScore = shellUnderTestingBelt.DpsPerCostDict[DamageType];
                                        }

                                        shellUnderTestingBelt.RailDraw = midRangeUpper;
                                        shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                            DamageType,
                                            TargetAC,
                                            TargetArmorScheme,
                                            TestIntervalSeconds,
                                            StoragePerVolume,
                                            StoragePerCost,
                                            Ppm,
                                            Ppv,
                                            Ppc,
                                            Fuel);
                                        if (TestType == 0)
                                        {
                                            midRangeUpperScore = shellUnderTestingBelt.DpsPerVolumeDict[DamageType];
                                        }
                                        else if (TestType == 1)
                                        {
                                            midRangeUpperScore = shellUnderTestingBelt.DpsPerCostDict[DamageType];
                                        }

                                        // Determine which half of range to continue testing
                                        if (midRangeUpperScore == 0)
                                        {
                                            bottomOfRange = midRangeUpper;
                                        }
                                        else if (midRangeLowerScore >= midRangeUpperScore)
                                        {
                                            topOfRange = midRangeLower;
                                        }
                                        else
                                        {
                                            bottomOfRange = midRangeUpper;
                                        }
                                    }
                                    // Take better of two remaining values
                                    if (midRangeLowerScore >= midRangeUpperScore)
                                    {
                                        optimalDraw = midRangeLower;
                                    }
                                    else
                                    {
                                        optimalDraw = midRangeUpper;
                                    }
                                }
                            }

                            // Check performance against top shells
                            shellUnderTestingBelt.RailDraw = optimalDraw;
                            shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                DamageType,
                                TargetAC,
                                TargetArmorScheme,
                                TestIntervalSeconds,
                                StoragePerVolume,
                                StoragePerCost,
                                Ppm,
                                Ppv,
                                Ppc,
                                Fuel);
                            if (DamageType == DamageType.Pendepth)
                            {
                                shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                    DamageType.Kinetic,
                                    TargetAC,
                                    TargetArmorScheme,
                                    TestIntervalSeconds,
                                    StoragePerVolume,
                                    StoragePerCost,
                                    Ppm,
                                    Ppv,
                                    Ppc,
                                    Fuel);
                                shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                    DamageType.FlaK,
                                    TargetAC,
                                    TargetArmorScheme,
                                    TestIntervalSeconds,
                                    StoragePerVolume,
                                    StoragePerCost,
                                    Ppm,
                                    Ppv,
                                    Ppc,
                                    Fuel);
                                shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                    DamageType.Frag,
                                    TargetAC,
                                    TargetArmorScheme,
                                    TestIntervalSeconds,
                                    StoragePerVolume,
                                    StoragePerCost,
                                    Ppm,
                                    Ppv,
                                    Ppc,
                                    Fuel);
                                shellUnderTestingBelt.CalculateDpsByTypeBelt(
                                    DamageType.HE,
                                    TargetAC,
                                    TargetArmorScheme,
                                    TestIntervalSeconds,
                                    StoragePerVolume,
                                    StoragePerCost,
                                    Ppm,
                                    Ppv,
                                    Ppc,
                                    Fuel);
                            }
                            shellUnderTestingBelt.CalculateVelocity();
                            shellUnderTestingBelt.CalculateEffectiveRange();

                            if (TestType == 0)
                            {
                                if (shellUnderTestingBelt.DpsPerVolumeDict[DamageType] > TopBelt.DpsPerVolumeDict[DamageType])
                                {
                                    TopBelt = shellUnderTestingBelt;
                                }
                            }
                            else if (TestType == 1)
                            {
                                if (shellUnderTestingBelt.DpsPerCostDict[DamageType] > TopBelt.DpsPerCostDict[DamageType])
                                {
                                    TopBelt = shellUnderTestingBelt;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds current top-performing shells to TopShells list for comparison with other lists
        /// Note that DPS is used only to determine whether a shell has been assigned to a particular length slot
        /// </summary>
        public void AddTopShellsToLocalList()
        {
            if (TopBelt.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(TopBelt);
            }

            if (Top1000.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(Top1000);
            }

            if (Top2000.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(Top2000);
            }

            if (Top3000.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(Top3000);
            }

            if (Top4000.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(Top4000);
            }

            if (Top5000.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(Top5000);
            }

            if (Top6000.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(Top6000);
            }

            if (Top7000.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(Top7000);
            }

            if (Top8000.DpsDict[DamageType] > 0)
            {
                TopShellsLocal.Add(Top8000);
            }
        }


        /// <summary>
        /// Adds current top-performing shells to TopShells dictionary for writing to console
        /// Note that DPS is used only to determine whether a shell has been assigned to a length slot
        /// </summary>
        public void AddTopShellsToDictionary()
        {
            if (TopBelt.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("1 m (belt)", TopBelt);
            }

            if (Top1000.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("1 m", Top1000);
            }

            if (Top2000.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("2 m", Top2000);
            }

            if (Top3000.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("3 m", Top3000);
            }

            if (Top4000.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("4 m", Top4000);
            }

            if (Top5000.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("5 m", Top5000);
            }

            if (Top6000.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("6 m", Top6000);
            }

            if (Top7000.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("7 m", Top7000);
            }

            if (Top8000.DpsDict[DamageType] > 0)
            {
                TopDpsShells.Add("8 m", Top8000);
            }
        }


        /// <summary>
        /// Finds top shells in given list.  Used in multithreading.
        /// </summary>
        /// <param name="shellBag"></param>
        public void FindTopShellsInList(ConcurrentBag<Shell> shellBag)
        {
            foreach (Shell rawShell in shellBag)
            {
                if (TestType == 0)
                {
                    if (rawShell.IsBelt)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > TopBelt.DpsPerVolumeDict[DamageType])
                        {
                            TopBelt = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 1000f)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > Top1000.DpsPerVolumeDict[DamageType])
                        {
                            Top1000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 2000f)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > Top2000.DpsPerVolumeDict[DamageType])
                        {
                            Top2000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 3000f)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > Top3000.DpsPerVolumeDict[DamageType])
                        {
                            Top3000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 4000f)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > Top4000.DpsPerVolumeDict[DamageType])
                        {
                            Top4000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 5000f)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > Top5000.DpsPerVolumeDict[DamageType])
                        {
                            Top5000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 6000f)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > Top6000.DpsPerVolumeDict[DamageType])
                        {
                            Top6000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 7000f)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > Top7000.DpsPerVolumeDict[DamageType])
                        {
                            Top7000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 8000f)
                    {
                        if (rawShell.DpsPerVolumeDict[DamageType] > Top8000.DpsPerVolumeDict[DamageType])
                        {
                            Top8000 = rawShell;
                        }
                    }
                }
                else if (TestType == 1)
                {
                    if (rawShell.IsBelt)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > TopBelt.DpsPerCostDict[DamageType])
                        {
                            TopBelt = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 1000f)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > Top1000.DpsPerCostDict[DamageType])
                        {
                            Top1000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 2000f)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > Top2000.DpsPerCostDict[DamageType])
                        {
                            Top2000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 3000f)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > Top3000.DpsPerCostDict[DamageType])
                        {
                            Top3000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 4000f)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > Top4000.DpsPerCostDict[DamageType])
                        {
                            Top4000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 5000f)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > Top5000.DpsPerCostDict[DamageType])
                        {
                            Top5000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 6000f)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > Top6000.DpsPerCostDict[DamageType])
                        {
                            Top6000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 7000f)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > Top7000.DpsPerCostDict[DamageType])
                        {
                            Top7000 = rawShell;
                        }
                    }
                    else if (rawShell.TotalLength <= 8000f)
                    {
                        if (rawShell.DpsPerCostDict[DamageType] > Top8000.DpsPerCostDict[DamageType])
                        {
                            Top8000 = rawShell;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Write top shell information
        /// </summary>
        public void WriteTopShells(float minGauge, float maxGauge)
        {
            bool showGP = false;
            if (MaxGPInput > 0)
            {
                showGP = true;
            }

            bool showRG = false;
            if (MaxRGInput > 0)
            {
                showRG = true;
            }

            bool showDraw = false;
            if (MaxDrawInput > 0)
            {
                showDraw = true;
            }

            // Determine module and damage types to show
            Dictionary<DamageType, bool> dtToShow = new()
            {
                { DamageType.Kinetic, false },
                { DamageType.Emp, false },
                { DamageType.FlaK, false },
                { DamageType.Frag, false },
                { DamageType.HE, false },
                { DamageType.Pendepth, false },
                { DamageType.Disruptor, false }
            };

            List<int> modsToShow = new();

            dtToShow[DamageType] = true; // Always show selected damage type
            if (FixedModuleCounts[0] > 0 || VariableModuleIndices.Contains(0))
            {
                dtToShow[DamageType.Kinetic] = true;
                modsToShow.Add(0);
            }
            if (FixedModuleCounts[1] > 0 || VariableModuleIndices.Contains(1))
            {
                dtToShow[DamageType.Kinetic] = true;
                modsToShow.Add(1);
            }
            if (FixedModuleCounts[2] > 0 || VariableModuleIndices.Contains(2))
            {
                dtToShow[DamageType.Emp] = true;
                modsToShow.Add(2);
            }
            if (FixedModuleCounts[3] > 0 || VariableModuleIndices.Contains(3))
            {
                dtToShow[DamageType.FlaK] = true;
                modsToShow.Add(3);
            }
            if (FixedModuleCounts[4] > 0 || VariableModuleIndices.Contains(4))
            {
                dtToShow[DamageType.Frag] = true;
                modsToShow.Add(4);
            }
            if (FixedModuleCounts[5] > 0 || VariableModuleIndices.Contains(5))
            {
                dtToShow[DamageType.HE] = true;
                modsToShow.Add(5);
            }

            // Check non-damage body mods for inclusion
            for (int index = 6; index < Module.AllModules.Length; index++)
            {
                if (Module.AllModules[index].ModulePosition == Module.Position.Middle &&
                    (FixedModuleCounts[index] > 0 || VariableModuleIndices.Contains(index)))
                {
                    modsToShow.Add(index);
                }
            }

            WriteTopShellsToFile(minGauge, maxGauge, showGP, showRG, showDraw, dtToShow, modsToShow);
        }


        /// <summary>
        /// Write to file statistics of top shells
        /// </summary>
        void WriteTopShellsToFile(
            float minGauge,
            float maxGauge,
            bool showGP,
            bool showRG,
            bool showDraw,
            Dictionary<DamageType, bool> dtToShow,
            List<int> modsToShow)

        {
            // Create filename from current time
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-ff") + ".txt";

            using var writer = new StreamWriter(fileName, append: true);
            FileStream fs = (FileStream)writer.BaseStream;

            writer.WriteLine("\nTest Parameters");
            writer.WriteLine(BarrelCount + " Barrels");
            if (minGauge == maxGauge)
            {
                writer.WriteLine("Gauge: " + minGauge);
            }
            else
            {
                writer.WriteLine("Gauge: " + minGauge + " mm thru " + maxGauge + " mm");
            }

            if (HeadList.Count == 1)
            {
                writer.WriteLine("Head: " + Module.AllModules[HeadList[0]].Name);
            }
            else
            {
                writer.WriteLine("Heads: ");
                foreach (int headIndex in HeadList)
                {
                    writer.WriteLine(Module.AllModules[headIndex].Name);
                }
            }

            if (BaseModule != null)
            {
                writer.WriteLine("Base: " + BaseModule.Name);
            }
            else
            {
                writer.WriteLine("No special base module");
            }

            writer.WriteLine("Fixed module(s): ");

            int modIndex = 0;
            foreach (float modCount in FixedModuleCounts)
            {
                if (modCount > 0)
                {
                    writer.WriteLine(Module.AllModules[modIndex].Name + ": " + modCount);
                }
                modIndex++;
            }

            // Remove duplicate variable mod indices
            List<int> uniqueVarModIndices = VariableModuleIndices.Distinct().ToList();
            writer.WriteLine("Variable module(s):");
            foreach (int index in uniqueVarModIndices)
            {
                writer.WriteLine(Module.AllModules[index].Name);
            }


            writer.WriteLine("Max GP casings: " + MaxGPInput);
            writer.WriteLine("Max RG casings: " + MaxRGInput);
            writer.WriteLine("Max draw: " + MaxDrawInput);
            writer.WriteLine("Max recoil: " + MaxRecoilInput);
            writer.WriteLine("Min length: " + MinShellLength);
            writer.WriteLine("Max length: " + MaxShellLength);
            writer.WriteLine("Min velocity: " + MinVelocityInput);
            writer.WriteLine("Min effective range: " + MinEffectiveRangeInput);

            if (DamageType == DamageType.Kinetic)
            {
                writer.WriteLine("Damage type: kinetic");
                writer.WriteLine("Target AC: " + TargetAC);
            }
            else if (DamageType == DamageType.Pendepth)
            {
                writer.WriteLine("Damage type: pendepth");
                writer.WriteLine("Target armor scheme:");
                foreach (Layer armorLayer in TargetArmorScheme.LayerList)
                {
                    writer.WriteLine(armorLayer.Name);
                }
            }
            else
            {
                writer.WriteLine("Damage type: " + (DamageType)(int)DamageType);
            }

            if (TestType == 0)
            {
                writer.WriteLine("Testing for DPS / volume");
            }
            else if (TestType == 1)
            {
                writer.WriteLine("Testing for DPS / cost");
            }

            writer.WriteLine("Test interval (min): " + TestInterval);
            writer.WriteLine("\n");


            if (!Labels)
            {
                writer.WriteLine("Row headers");
                writer.WriteLine("Gauge (mm)");
                writer.WriteLine("Total length (mm)");
                writer.WriteLine("Length without casings");
                writer.WriteLine("Total modules");
                if (showGP)
                {
                    writer.WriteLine("GP casing");
                }
                if (showRG)
                {
                    writer.WriteLine("RG casing");
                }

                foreach (int index in modsToShow)
                {
                    writer.WriteLine(Module.AllModules[index].Name);
                }
                writer.WriteLine("Head");


                if (showDraw)
                {
                    writer.WriteLine("Rail draw");
                }
                // Recoil = draw if no GP
                if (showGP)
                {
                    writer.WriteLine("Recoil");
                }

                writer.WriteLine("Velocity (m/s)");
                writer.WriteLine("Effective range (m)");

                if (dtToShow[DamageType.Kinetic])
                {
                    writer.WriteLine("Raw KD");
                    writer.WriteLine("AP");
                }
                foreach (DamageType dt in dtToShow.Keys)
                {
                    if (dtToShow[dt])
                    {
                        writer.WriteLine((DamageType)(int)dt + " damage");
                    }
                }


                writer.WriteLine("Reload time");
                foreach (DamageType dt in dtToShow.Keys)
                {
                    if (dtToShow[dt])
                    {
                        writer.WriteLine((DamageType)(int)dt + " DPS");
                    }
                }

                writer.WriteLine("Loader volume");
                writer.WriteLine("Cooler volume");
                writer.WriteLine("Charger volume");
                writer.WriteLine("Engine volume");
                writer.WriteLine("Fuel access volume");
                writer.WriteLine("Fuel storage volume");
                writer.WriteLine("Recoil volume");
                writer.WriteLine("Ammo access volume");
                writer.WriteLine("Ammo storage volume");
                writer.WriteLine("Total volume");
                foreach (DamageType dt in dtToShow.Keys)
                {
                    if (dtToShow[dt])
                    {
                        writer.WriteLine((DamageType)(int)dt + " DPS per volume");
                    }
                }

                writer.WriteLine("Cost per shell");
                writer.WriteLine("Loader cost");
                writer.WriteLine("Cooler cost");
                writer.WriteLine("Charger cost");
                writer.WriteLine("Fuel burned");
                writer.WriteLine("Engine cost");
                writer.WriteLine("Fuel access cost");
                writer.WriteLine("Fuel storage cost");
                writer.WriteLine("Recoil cost");
                writer.WriteLine("Ammo used");
                writer.WriteLine("Ammo access cost");
                writer.WriteLine("Ammo storage cost");
                writer.WriteLine("Total cost");
                foreach (DamageType dt in dtToShow.Keys)
                {
                    if (dtToShow[dt])
                    {
                        writer.WriteLine((DamageType)(int)dt + " DPS per cost");
                    }
                }
            }


            foreach (KeyValuePair<string, Shell> topShell in TopDpsShells)
            {
                writer.WriteLine("\n");
                writer.WriteLine(topShell.Key);
                topShell.Value.GetModuleCounts();
                topShell.Value.WriteShellInfoToFile(writer, Labels, showGP, showRG, showDraw, dtToShow, modsToShow);
            }
        }
    }
}
