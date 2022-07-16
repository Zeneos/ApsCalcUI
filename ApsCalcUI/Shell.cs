using System;
using System.Collections.Generic;
using PenCalc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    public class Shell
    {
        public Shell() { BaseModule = default; }

        /// <summary>
        /// Sets gauge and simultaneously calculates Gauge Coefficient, which is used in several formulae as a way to scale with gauge.
        /// </summary>
        private float _gauge;
        public float Gauge
        {
            get { return _gauge; }
            set
            {
                _gauge = value;
                GaugeCoefficient = MathF.Pow(Gauge / 500f, 1.8f);
            }
        }
        public float GaugeCoefficient { get; set; } // Expensive to calculate and used in several formulae

        public bool IsBelt;

        // Keep counts of body modules.
        public float[] BodyModuleCounts { get; set; } = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public float ModuleCountTotal { get; set; }


        public Module BaseModule { get; set; } // Optional; is 'null' if no base is chosen by user
        public Module HeadModule { get; set; } // There must always be a Head

        // Gunpowder and Railgun casing counts
        public float GPCasingCount { get; set; }
        public float RGCasingCount { get; set; }


        // Lengths
        public float CasingLength { get; set; }
        public float ProjectileLength { get; set; } // Everything but casings and Head
        public float BodyLength { get; set; } // Everything but casings
        public float TotalLength { get; set; }
        public float ShortLength { get; set; } // Used for penalizing short shells
        public float LengthDifferential { get; set; } // Used for penalizing short shells
        public float EffectiveBodyLength { get; set; } // Used for penalizing short shells
        public float EffectiveBodyModuleCount { get; set; } // Compensate for length-limited modules
        public float EffectiveProjectileModuleCount { get; set; } // Compensate for length-limited modules
        public float BarrelLengthForInaccuracy { get; set; }
        public float BarrelLengthForPropellant { get; set; }

        // Overall modifiers
        public float OverallVelocityModifier { get; set; }
        public float OverallKineticDamageModifier { get; set; }
        public float OverallArmorPierceModifier { get; set; }
        public float OverallInaccuracyModifier { get; set; }
        public float OverallChemModifier { get; set; }


        // Power
        public float GPRecoil { get; set; }
        public float MaxDraw { get; set; }
        public float RailDraw { get; set; }
        public float TotalRecoil { get; set; }
        public float Velocity { get; set; }

        // Reload
        public bool IsDif { get; set; } // Direct-Input Feed doubles reload time
        public float ReloadTime { get; set; }
        public float ReloadTimeBelt { get; set; } // Beltfed Loader
        public float UptimeBelt { get; set; }
        public int BarrelCount { get; set; }
        public float CooldownTime { get; set; }

        // Effective range
        public float EffectiveRange { get; set; }


        // Damage
        public float RawKD { get; set; }
        public float ArmorPierce { get; set; }
        public float SabotAngleMultiplier { get; set; }
        public float NonSabotAngleMultiplier { get; set; }
        public float FragCount { get; set; }
        public float DamagePerFrag { get; set; }
        public float RawHE { get; set; }
        public float HEExplosionRadius { get; set; }
        public float RawFlaK { get; set; }
        public float FlaKExplosionRadius { get; set; }

        public Dictionary<DamageType, float> DamageDict = new()
        {
            { DamageType.Kinetic, 0 },
            { DamageType.EMP, 0 },
            { DamageType.FlaK, 0 },
            { DamageType.Frag, 0 },
            { DamageType.HE, 0 },
            { DamageType.HEAT, 0 },
            { DamageType.Disruptor, 0 }
        };

        public Dictionary<DamageType, float> DpsDict = new()
        {
            { DamageType.Kinetic, 0 },
            { DamageType.EMP, 0 },
            { DamageType.FlaK, 0 },
            { DamageType.Frag, 0 },
            { DamageType.HE, 0 },
            { DamageType.HEAT, 0 },
            { DamageType.Disruptor, 0 }
        };

        public Dictionary<DamageType, float> DpsPerVolumeDict = new()
        {
            { DamageType.Kinetic, 0 },
            { DamageType.EMP, 0 },
            { DamageType.FlaK, 0 },
            { DamageType.Frag, 0 },
            { DamageType.HE, 0 },
            { DamageType.HEAT, 0 },
            { DamageType.Disruptor, 0 }
        };

        public Dictionary<DamageType, float> DpsPerCostDict = new()
        {
            { DamageType.Kinetic, 0 },
            { DamageType.EMP, 0 },
            { DamageType.FlaK, 0 },
            { DamageType.Frag, 0 },
            { DamageType.HE, 0 },
            { DamageType.HEAT, 0 },
            { DamageType.Disruptor, 0 }
        };


        // Volume
        public float LoaderVolume { get; set; }
        public float LoaderVolumeBelt { get; } = 4f; // loader, clip, 2 intakes
        public float RecoilVolume { get; set; }
        public float RecoilVolumeBelt { get; set; }
        public float ChargerVolume { get; set; }
        public float ChargerVolumeBelt { get; set; }
        public float EngineVolume { get; set; }
        public float EngineVolumeBelt { get; set; }
        public float FuelAccessVolume { get; set; }
        public float FuelAccessVolumeBelt { get; set; }
        public float FuelStorageVolume { get; set; }
        public float FuelStorageVolumeBelt { get; set; }
        public float CoolerVolume { get; set; }
        public float CoolerVolumeBelt { get; set; }
        public float AmmoAccessVolume { get; set; }
        public float AmmoAccessVolumeBelt { get; set; }
        public float AmmoStorageVolume { get; set; }
        public float AmmoStorageVolumeBelt { get; set; }
        public float VolumePerIntake { get; set; }
        public float VolumePerIntakeBelt { get; set; }


        // Cost
        public float LoaderCost { get; set; }
        public float LoaderCostBelt { get; } = 860f; // loader, clip, 2 intakes
        public float RecoilCost { get; set; }
        public float RecoilCostBelt { get; set; }
        public float ChargerCost { get; set; }
        public float ChargerCostBelt { get; set; }
        public float EngineCost { get; set; }
        public float EngineCostBelt { get; set; }
        public float FuelBurned { get; set; }
        public float FuelBurnedBelt { get; set; }
        public float FuelAccessCost { get; set; }
        public float FuelAccessCostBelt { get; set; }
        public float FuelStorageCost { get; set; }
        public float FuelStorageCostBelt { get; set; }
        public float CoolerCost { get; set; }
        public float CoolerCostBelt { get; set; }
        public float CostPerShell { get; set; } // Material cost for one shell
        public float ShellCost { get; set; } // Material cost for all shells
        public float ShellCostBelt { get; set; }
        public float AmmoAccessCost { get; set; }
        public float AmmoAccessCostBelt { get; set; }
        public float AmmoStorageCost { get; set; }
        public float AmmoStorageCostBelt { get; set; }
        public float CostPerIntake { get; set; }
        public float CostPerIntakeBelt { get; set; }


        /// <summary>
        /// Calculates body, projectile, casing, and total lengths, as well as length differential, which is used to penalize short shells
        /// </summary>
        public void CalculateLengths()
        {
            BodyLength = 0;
            if (BaseModule != null)
            {
                BodyLength += MathF.Min(Gauge, BaseModule.MaxLength);
            }

            int modIndex = 0;
            foreach (float modCount in BodyModuleCounts)
            {
                float ModuleLength = MathF.Min(Gauge, Module.AllModules[modIndex].MaxLength);
                BodyLength += ModuleLength * modCount;
                modIndex++;
            }

            CasingLength = (GPCasingCount + RGCasingCount) * Gauge;

            float HeadLength = MathF.Min(Gauge, HeadModule.MaxLength);
            ProjectileLength = BodyLength + HeadLength;

            TotalLength = CasingLength + ProjectileLength;

            ShortLength = 2 * Gauge;
            LengthDifferential = MathF.Max(ShortLength - BodyLength, 0);
            EffectiveBodyLength = MathF.Max(2 * Gauge, BodyLength);

            EffectiveBodyModuleCount = BodyLength / Gauge;
            EffectiveProjectileModuleCount = ProjectileLength / Gauge;
        }

        /// <summary>
        /// Calculates recoil from gunpowder casings
        /// </summary>
        public void CalculateRecoil()
        {
            GPRecoil = GaugeCoefficient * GPCasingCount * 2500f;
            TotalRecoil = GPRecoil + RailDraw;
        }

        /// <summary>
        /// Calculates max rail draw of shell
        /// </summary>
        public void CalculateMaxDraw()
        {
            MaxDraw = 12500f * GaugeCoefficient * (EffectiveProjectileModuleCount + (0.5f * RGCasingCount));
        }


        /// <summary>
        /// Calculates velocity modifier
        /// </summary>
        public void CalculateVelocityModifier()
        {
            // Calculate weighted velocity modifier of body
            float weightedVelocityMod = 0f;
            if (BaseModule != null)
            {
                weightedVelocityMod += BaseModule.VelocityMod * MathF.Min(Gauge, BaseModule.MaxLength);
            }

            // Add body module weighted modifiers
            int modIndex = 0;
            foreach (float modCount in BodyModuleCounts)
            {
                float modLength = MathF.Min(Gauge, Module.AllModules[modIndex].MaxLength);
                weightedVelocityMod += modLength * Module.AllModules[modIndex].VelocityMod * modCount;
                modIndex++;
            }

            if (LengthDifferential > 0f) // Add 'ghost' module for penalizing short shells; has no effect if body length >= 2 * gauge
            {
                weightedVelocityMod += 0.7f * LengthDifferential;
            }

            weightedVelocityMod /= EffectiveBodyLength;

            OverallVelocityModifier = weightedVelocityMod * HeadModule.VelocityMod;
            if (BaseModule?.Name == "Base bleeder")
            {
                OverallVelocityModifier += 0.15f;
            }
        }


        /// <summary>
        /// Calculates kinetic damage modifier
        /// </summary>
        void CalculateKDModifier()
        {
            // Calculate weighted KineticDamage modifier of body
            float weightedKineticDamageMod = 0f;
            if (BaseModule != null)
            {
                weightedKineticDamageMod += BaseModule.KineticDamageMod * MathF.Min(Gauge, BaseModule.MaxLength);
            }

            int modIndex = 0;
            foreach (float modCount in BodyModuleCounts)
            {
                float modLength = MathF.Min(Gauge, Module.AllModules[modIndex].MaxLength);
                weightedKineticDamageMod += modLength * Module.AllModules[modIndex].KineticDamageMod * modCount;
                modIndex++;
            }

            if (LengthDifferential > 0f) // Add 'ghost' module for penalizing short shells; has no effect if body length >= 2 * gauge
            {
                weightedKineticDamageMod += LengthDifferential;
            }

            weightedKineticDamageMod /= EffectiveBodyLength;

            OverallKineticDamageModifier = weightedKineticDamageMod * HeadModule.KineticDamageMod;
        }


        /// <summary>
        /// Calculates AP modifier
        /// </summary>
        void CalculateAPModifier()
        {
            // Calculate weighted AP modifier of body
            float weightedArmorPierceMod = 0f;
            if (BaseModule != null)
            {
                weightedArmorPierceMod += BaseModule.ArmorPierceMod * MathF.Min(Gauge, BaseModule.MaxLength);
            }

            int modIndex = 0;
            foreach (float modCount in BodyModuleCounts)
            {
                float modLength = MathF.Min(Gauge, Module.AllModules[modIndex].MaxLength);
                weightedArmorPierceMod += modLength * Module.AllModules[modIndex].ArmorPierceMod * modCount;
                modIndex++;
            }

            if (LengthDifferential > 0f) // Add 'ghost' module for penalizing short shells; has no effect if body length >= 2 * gauge
            {
                weightedArmorPierceMod += LengthDifferential;
            }

            weightedArmorPierceMod /= EffectiveBodyLength;

            OverallArmorPierceModifier = weightedArmorPierceMod * HeadModule.ArmorPierceMod;
        }

        /// <summary>
        /// Calculates inaccuracy modifier
        /// </summary>
        void CalculateInaccuracyModifier()
        {
            // Calculate weighted inaccuracy modifier of body
            float weightedInaccuracyMod = 0f;
            if (BaseModule != null)
            {
                weightedInaccuracyMod += BaseModule.InaccuracyMod * MathF.Min(Gauge, BaseModule.MaxLength);
            }

            // Add body module weighted modifiers
            int modIndex = 0;
            foreach (float modCount in BodyModuleCounts)
            {
                float modLength = MathF.Min(Gauge, Module.AllModules[modIndex].MaxLength);
                weightedInaccuracyMod += modLength * Module.AllModules[modIndex].InaccuracyMod * modCount;
                modIndex++;
            }

            if (LengthDifferential > 0f) // Add 'ghost' module for penalizing short shells; has no effect if body length >= 2 * gauge
            {
                weightedInaccuracyMod += LengthDifferential;
            }

            weightedInaccuracyMod /= EffectiveBodyLength;

            OverallInaccuracyModifier = weightedInaccuracyMod * HeadModule.InaccuracyMod;
            if (BaseModule?.Name == Module.BaseBleeder.Name)
            {
                OverallInaccuracyModifier *= 1.35f;
            }
            else if (BaseModule?.Name == Module.Tracer.Name)
            {
                OverallInaccuracyModifier *= 0.72f; // This is a filler value; actual bonus depends on ROF
            }

            if (BarrelCount > 1)
            {
                OverallInaccuracyModifier *= (BarrelCount - 1) * 0.05f + 1.2f;
            }
        }

        /// <summary>
        /// Calculate max body length for 0.3° inaccuracy
        /// </summary>
        public float CalculateMaxProjectileLengthForInaccuracy(float barrelLength, float desiredInaccuracy)
        {
            CalculateInaccuracyModifier();
            float maxProjectileLength = MathF.Pow(barrelLength / 4f / MathF.Pow(0.3f / desiredInaccuracy * OverallInaccuracyModifier, 2.5f), 4f/3f);

            return maxProjectileLength * 1000f;
        }

        /// <summary>
        /// Calculate min barrel length for inaccuracy and full propellant burn
        /// </summary>
        public void CalculateRequiredBarrelLengths(float desiredInaccuracy = 0.3f)
        {
            CalculateInaccuracyModifier();
            BarrelLengthForInaccuracy = 4 * MathF.Pow(ProjectileLength / 1000f, 0.75f) * MathF.Pow(0.3f / desiredInaccuracy * OverallInaccuracyModifier, 2.5f);

            BarrelLengthForPropellant = 2.2f * GPCasingCount * MathF.Pow(Gauge / 1000f, 0.55f);
        }


        /// <summary>
        /// Calculates chemical payload modifier
        /// </summary>
        void CalculateChemModifier()
        {
            OverallChemModifier = 1f;
            if (BaseModule != null)
            {
                OverallChemModifier = MathF.Min(OverallChemModifier, BaseModule.ChemMod);
            }


            int modIndex = 0;
            foreach (float modCount in BodyModuleCounts)
            {
                if (modCount > 0)
                {
                    OverallChemModifier = MathF.Min(OverallChemModifier, Module.AllModules[modIndex].ChemMod);
                }
                modIndex++;
            }

            if (HeadModule == Module.Disruptor) // Disruptor 50% penalty stacks
            {
                OverallChemModifier *= 0.5f;
            }
            else
            {
                OverallChemModifier = MathF.Min(OverallChemModifier, HeadModule.ChemMod);
            }
        }


        /// <summary>
        /// Calculates shell velocity
        /// </summary>
        public void CalculateVelocity()
        {
            Velocity = MathF.Sqrt(TotalRecoil * 85f * Gauge / (GaugeCoefficient * ProjectileLength)) * OverallVelocityModifier;
        }


        /// <summary>
        /// Calculates minimum rail draw needed to achieve given velocity and effective range
        /// </summary>
        public float CalculateMinimumDrawForVelocityandRange(float minVelocityInput, float minRangeInput)
        {
            CalculateRecoil();
            // Calculate effective time
            float gravityCompensatorCount = 0;
            int modIndex = 0;
            foreach (float modCount in BodyModuleCounts)
            {
                if (Module.AllModules[modIndex] == Module.GravCompensator)
                {
                    gravityCompensatorCount = BodyModuleCounts[modIndex];
                    break;
                }
                else
                {
                    modIndex++;
                }
            }
            float effectiveTime = 10f * OverallVelocityModifier * (ProjectileLength / 1000f) * (1f + gravityCompensatorCount);

            // Determine whether range or velocity is limiting factor
            float minVelocity = MathF.Max(minVelocityInput, minRangeInput / effectiveTime);

            // Calculate draw required for either range or velocity
            float minDrawVelocity = MathF.Pow(minVelocity / OverallVelocityModifier, 2)
                * (GaugeCoefficient * ProjectileLength)
                / (Gauge * 85f)
                - GPRecoil;

            float minDraw = MathF.Max(0, minDrawVelocity);

            return minDraw;
        }


        /// <summary>
        /// Calculates effective range of shell
        /// </summary>
        public void CalculateEffectiveRange()
        {
            float gravityCompensatorCount = 0;
            int modIndex = 0;
            foreach (float modCount in BodyModuleCounts)
            {
                if (Module.AllModules[modIndex] == Module.GravCompensator)
                {
                    gravityCompensatorCount = BodyModuleCounts[modIndex];
                    break;
                }
                else
                {
                    modIndex++;
                }
            }
            float effectiveTime = 10f * OverallVelocityModifier * (ProjectileLength / 1000f) * (1f + gravityCompensatorCount);
            EffectiveRange = Velocity * effectiveTime;
        }


        /// <summary>
        /// Calculate volume of intake and loader
        /// </summary>
        public void CalculateLoaderVolumeAndCost()
        {
            LoaderVolume = 0;
            LoaderCost = 0;

            // DIF can't use loaders, only inputs
            if (!IsDif)
            {
                if (TotalLength <= 1000f)
                {
                    LoaderVolume = 1f;
                    LoaderCost = 240f;
                }
                else if (TotalLength <= 2000f)
                {
                    LoaderVolume = 2f;
                    LoaderCost = 300f;
                }
                else if (TotalLength <= 3000f)
                {
                    LoaderVolume = 3f;
                    LoaderCost = 330f;
                }
                else if (TotalLength <= 4000f)
                {
                    LoaderVolume = 4f;
                    LoaderCost = 360f;
                }
                else if (TotalLength <= 5000f)
                {
                    LoaderVolume = 5f;
                    LoaderCost = 390f;
                }
                else if (TotalLength <= 6000f)
                {
                    LoaderVolume = 6f;
                    LoaderCost = 420f;
                }
                else if (TotalLength <= 7000f)
                {
                    LoaderVolume = 7f;
                    LoaderCost = 450f;
                }
                else if (TotalLength <= 8000f)
                {
                    LoaderVolume = 8f;
                    LoaderCost = 480f;
                }
            }

            LoaderVolume += 1f; // Always have an intake, even for DIF
            LoaderCost += 50f;
        }

        /// <summary>
        /// Calculates volume per intake of recoil absorbers
        /// </summary>
        public void CalculateRecoilVolumeAndCost()
        {
            RecoilVolume = TotalRecoil / (ReloadTime * 120f); // Absorbers absorb 120 per second per metre
            RecoilCost = RecoilVolume * 80f; // Absorbers cost 80 per metre

            if (TotalLength <= 1000f)
            {
                RecoilVolumeBelt = TotalRecoil / (ReloadTimeBelt * 120f);
                RecoilCostBelt = RecoilVolumeBelt * 80f;
            }
            else
            {
                RecoilVolumeBelt = 0f;
                RecoilCostBelt = 0f;
            }
        }

        /// <summary>
        /// Calculates barrel cooldown time
        /// </summary>
        public void CalculateCooldownTime()
        {
            CooldownTime =
                3.75f
                * GaugeCoefficient
                / MathF.Pow(Gauge * Gauge * Gauge / 125000000f, 0.15f)
                * 17.5f
                * MathF.Pow(GPCasingCount, 0.35f)
                / 2;
            CooldownTime = MathF.Max(CooldownTime, 0);
        }

        /// <summary>
        /// Calculates marginal volume of coolers to sustain fire from one additional intake.  Ignores cooling from firing piece
        /// </summary>
        public void CalculateCoolerVolumeAndCost()
        {
            float coolerVolume;
            float coolerCost;

            float coolerVolumeBelt;
            float coolerCostBelt;
            if (GPCasingCount > 0)
            {
                coolerVolume = CooldownTime * MathF.Sqrt(Gauge / 1000) / ReloadTime / (1 + BarrelCount * 0.05f) / 0.176775f;
                coolerCost = coolerVolume * 50f;

                if (TotalLength <= 1000f)
                {
                    coolerVolumeBelt = coolerVolume * ReloadTime / ReloadTimeBelt;
                    coolerCostBelt = coolerVolumeBelt * 50f;
                }
                else
                {
                    coolerVolumeBelt = 0;
                    coolerCostBelt = 0;
                }
            }
            else
            {
                coolerVolume = 0;
                coolerCost = 0;
                coolerVolumeBelt = 0;
                coolerCostBelt = 0;
            }

            CoolerVolume = coolerVolume;
            CoolerCost = coolerCost;

            CoolerVolumeBelt = coolerVolumeBelt;
            CoolerCostBelt = coolerCostBelt;
        }

        /// <summary>
        /// Calculates marginal volume per intake of rail chargers and engines
        /// </summary>
        public void CalculateRailVolumeAndCost(
            int testIntervalSeconds,
            float storagePerVolume,
            float storagePerCost,
            float ppm,
            float ppv,
            float ppc,
            bool fuel)
        {
            if (RailDraw > 0)
            {
                float drawPerSecond = RailDraw / ReloadTime;
                ChargerVolume = drawPerSecond / 200f; // Chargers provide 200 Energy per second
                ChargerCost = ChargerVolume * 400f; // Chargers cost 400 per metre

                // Volume and cost of engine
                EngineVolume = drawPerSecond / ppv;
                EngineCost = drawPerSecond / ppc;

                // Materials burned by engine
                FuelBurned = drawPerSecond * testIntervalSeconds / ppm;

                float fuelStorageNeeded;
                if (fuel)
                {
                    // Volume and cost of special fuel access blocks
                    // 1 fuel per MINUTE = 1/50 m^3 and 0.2 material cost
                    // 1 fuel per SECOND = 60/50 (1.2) m^3 and 12 material cost
                    // 1 m^3 fuel access = 10 material cost
                    float fuelAccessNeeded = drawPerSecond / ppm;
                    FuelAccessVolume = fuelAccessNeeded * 1.2f;
                    FuelAccessCost = FuelAccessVolume * 10;

                    // Fuel access blocks store enough materials to run for 10 minutes
                    fuelStorageNeeded = drawPerSecond * MathF.Max(testIntervalSeconds - 600f, 0) / ppm;
                }
                else
                {
                    fuelStorageNeeded = drawPerSecond * testIntervalSeconds / ppm;
                }

                // Storage for materials burned
                FuelStorageVolume = fuelStorageNeeded / storagePerVolume;
                FuelStorageCost = fuelStorageNeeded / storagePerCost;


                if (TotalLength <= 1000f)
                {
                    float drawPerSecondBelt = RailDraw / ReloadTimeBelt;
                    ChargerVolumeBelt = drawPerSecondBelt / 200f; // Chargers provide 200 Energy per second
                    ChargerCostBelt = ChargerVolumeBelt * 400f; // Chargers cost 400 per metre

                    // Volume and cost of engine
                    EngineVolumeBelt = drawPerSecondBelt / ppv;
                    EngineCostBelt = drawPerSecondBelt / ppc;

                    // Materials burned by engine
                    FuelBurnedBelt = drawPerSecondBelt * testIntervalSeconds / ppm * UptimeBelt;

                    float fuelStorageNeededBelt;
                    if (fuel)
                    {
                        // Volume and cost of special fuel access blocks
                        // 1 fuel per MINUTE = 1/50 m^3 and 0.2 material cost
                        // 1 fuel per SECOND = 60/50 m^3 and 12 material cost
                        float fuelAccessNeededBelt = drawPerSecondBelt / ppm;
                        FuelAccessVolumeBelt = fuelAccessNeededBelt / 50f * 60f;
                        FuelAccessCostBelt = fuelAccessNeededBelt / 12f;

                        // Fuel access blocks store enough materials to run for 10 minutes
                        fuelStorageNeededBelt = drawPerSecondBelt * MathF.Max(testIntervalSeconds - 600f, 0) / ppm * UptimeBelt;
                    }
                    else
                    {
                        fuelStorageNeededBelt = drawPerSecondBelt * testIntervalSeconds / ppm * UptimeBelt;
                    }

                    // Storage for materials burned
                    FuelStorageVolumeBelt = fuelStorageNeededBelt / storagePerVolume;
                    FuelStorageCostBelt = fuelStorageNeededBelt / storagePerCost;
                }
                else
                {
                    ChargerVolumeBelt = 0;
                    ChargerCostBelt = 0;
                    FuelBurnedBelt = 0;
                    EngineVolumeBelt = 0;
                    EngineCostBelt = 0;
                    FuelAccessVolumeBelt = 0;
                    FuelAccessCostBelt = 0;
                    FuelStorageVolumeBelt = 0;
                    FuelStorageCostBelt = 0;
                }
            }
            else
            {
                ChargerVolume = 0;
                ChargerCost = 0;
                ChargerVolumeBelt = 0;
                ChargerCostBelt = 0;
                FuelBurned = 0;
                FuelBurnedBelt = 0;
                EngineVolume = 0;
                EngineCost = 0;
                EngineVolumeBelt = 0;
                EngineCostBelt = 0;
                FuelAccessVolume = 0;
                FuelAccessCost = 0;
                FuelAccessVolumeBelt = 0;
                FuelAccessCostBelt = 0;
                FuelStorageVolume = 0;
                FuelStorageCost = 0;
                FuelStorageVolumeBelt = 0;
                FuelStorageCostBelt = 0;
            }
        }


        /// <summary>
        /// Calculates all volumes and costs dependent on testing interval
        /// </summary>
        /// <param name="testInterval">Test interval in minutes</param>
        public void CalculateVariableVolumesAndCosts(int testIntervalSeconds, float storagePerVolume, float storagePerCost)
        {
            // Calculate cost of shell itself
            CostPerShell = (ProjectileLength + (GPCasingCount + RGCasingCount) / 4)
                * 5f
                * GaugeCoefficient
                / Gauge;

            ShellCost = CostPerShell * testIntervalSeconds / ReloadTime;

            if (TotalLength <= 1000f)
            {
                ShellCostBelt = CostPerShell * testIntervalSeconds / ReloadTimeBelt * UptimeBelt;
            }

            // Calculate volume and cost of ammo crates
            // 1/50 m^3 and 1/5 material cost per material per minute
            float shellCostPerMinute = CostPerShell / ReloadTime * 60f;
            AmmoAccessVolume = shellCostPerMinute / 50f;
            AmmoAccessCost = shellCostPerMinute / 5f;

            if (TotalLength <= 1000f)
            {
                float shellCostPerMinuteBelt = CostPerShell / ReloadTimeBelt * UptimeBelt * 60f;
                AmmoAccessVolumeBelt = shellCostPerMinuteBelt / 50f;
                AmmoAccessCostBelt = shellCostPerMinuteBelt / 5f;
            }

            // Calculate volume and cost of material storage
            // Ammo crates hold enough materials for 10 minutes
            float ammoStorageNeeded = CostPerShell * MathF.Max(testIntervalSeconds - 600, 0) / ReloadTime;
            AmmoStorageVolume = ammoStorageNeeded / storagePerVolume;
            AmmoStorageCost = ammoStorageNeeded / storagePerCost;

            if (TotalLength <= 1000f)
            {
                float ammoStorageNeededBelt = CostPerShell * MathF.Max(testIntervalSeconds - 600, 0) / ReloadTimeBelt * UptimeBelt;
                AmmoStorageVolumeBelt = ammoStorageNeededBelt / storagePerVolume;
                AmmoStorageCostBelt = ammoStorageNeededBelt / storagePerCost;
            }
        }

        /// <summary>
        /// Calculates volume used by shell, including intake, loader, cooling, recoil absorbers, and rail chargers
        /// </summary>
        public void CalculateVolumeAndCostPerIntake()
        {

            VolumePerIntake =
                LoaderVolume
                + RecoilVolume
                + CoolerVolume
                + ChargerVolume
                + AmmoAccessVolume
                + AmmoStorageVolume
                + EngineVolume
                + FuelAccessVolume
                + FuelStorageVolume;

            CostPerIntake =
                LoaderCost
                + RecoilCost
                + CoolerCost
                + ChargerCost
                + ShellCost
                + AmmoAccessCost
                + AmmoStorageCost
                + FuelBurned
                + EngineCost
                + FuelAccessCost
                + FuelStorageCost;

            if (TotalLength <= 1000f)
            {
                VolumePerIntakeBelt =
                    LoaderVolumeBelt
                    + RecoilVolumeBelt
                    + CoolerVolumeBelt
                    + ChargerVolumeBelt
                    + AmmoAccessVolumeBelt
                    + AmmoStorageVolumeBelt
                    + EngineVolumeBelt
                    + FuelAccessVolumeBelt
                    + FuelStorageVolumeBelt;

                CostPerIntakeBelt =
                    LoaderCostBelt
                    + RecoilCostBelt
                    + CoolerCostBelt
                    + ChargerCostBelt
                    + ShellCostBelt
                    + AmmoAccessCostBelt
                    + AmmoStorageCostBelt
                    + FuelBurnedBelt
                    + EngineCostBelt
                    + FuelAccessCostBelt
                    + FuelStorageCostBelt;
            }
        }


        /// <summary>
        /// Calculates reload time; also calculates beltfed reload time for shells 1000 mm or shorter
        /// </summary>
        public void CalculateReloadTime()
        {
            ReloadTime = MathF.Pow(Gauge / 500f, 1.35f)
                * (2f + EffectiveProjectileModuleCount + 0.25f * (RGCasingCount + GPCasingCount))
                * 17.5f;

            // DIF doubles reload time
            if (IsDif)
            {
                ReloadTime *= 2f;
            }
        }

        /// <summary>
        /// Calculates beltfed loader reload time and uptime
        /// </summary>
        public void CalculateReloadTimeBelt()
        {
            if (TotalLength <= 1000f)
            {
                ReloadTimeBelt = ReloadTime * 0.75f * MathF.Pow(Gauge / 1000f, 0.45f);
                float gaugeModifier;
                if (Gauge <= 250f)
                {
                    gaugeModifier = 2f;
                }
                else
                {
                    gaugeModifier = 1f;
                }
                float shellCapacity = 1f * MathF.Min(64f, MathF.Floor(1000f / Gauge) * gaugeModifier) + 1f;
                float firingCycleLength = (shellCapacity - 1f) * ReloadTimeBelt;
                float loadingCycleLength = (shellCapacity - 2f) * ReloadTimeBelt / 2f; // 2 intakes
                UptimeBelt = firingCycleLength / (firingCycleLength + loadingCycleLength);
            }
            else
            {
                ReloadTimeBelt = 0;
            }
        }


        /// <summary>
        /// Calculates raw kinetic damage
        /// </summary>
        void CalculateKineticDamage()
        {
            CalculateVelocity();

            if (HeadModule == Module.HollowPoint)
            {
                RawKD =
                    GaugeCoefficient
                    * EffectiveProjectileModuleCount
                    * Velocity
                    * OverallKineticDamageModifier
                    * 3.5f;
            }
            else
            {
                RawKD =
                    MathF.Pow(500 / MathF.Max(Gauge, 100f), 0.15f)
                    * GaugeCoefficient
                    * EffectiveProjectileModuleCount
                    * Velocity
                    * OverallKineticDamageModifier
                    * 3.5f;
            }
        }

        /// <summary>
        /// Calculates armor pierce rating
        /// </summary>
        void CalculateAP()
        {
            ArmorPierce = Velocity * OverallArmorPierceModifier * 0.0175f;
        }

        /// <summary>
        /// Calculates EMP damage. Used by shield reduction
        /// </summary>
        void CalculateEmpDamage()
        {
            // Get index of EMP body
            int empIndex = int.MaxValue;
            for (int i = 0; i < Module.AllModules.Length; i++)
            {
                if (Module.AllModules[i] == Module.EmpBody)
                {
                    empIndex = i;
                    break;
                }
            }

            float empBodies = BodyModuleCounts[empIndex];

            if (HeadModule == Module.EmpHead || HeadModule == Module.EmpBody || HeadModule == Module.Disruptor)
            {
                empBodies++;
            }
            DamageDict[DamageType.EMP] = GaugeCoefficient * empBodies * OverallChemModifier * 1650;
        }

        /// <summary>
        /// Calculates FlaK damage
        /// </summary>
        void CalculateFlaKDamage()
        {
            // Get index of FlaK body
            int flakIndex = int.MaxValue;
            for (int i = 0; i < Module.AllModules.Length; i++)
            {
                if (Module.AllModules[i] == Module.FlaKBody)
                {
                    flakIndex = i;
                    break;
                }
            }

            float flaKBodies = BodyModuleCounts[flakIndex];

            if (HeadModule == Module.FlaKHead || HeadModule == Module.FlaKBody)
            {
                flaKBodies++;
            }
            RawFlaK = 3000f * MathF.Pow(GaugeCoefficient * flaKBodies * 0.792f * OverallChemModifier, 0.9f);
            FlaKExplosionRadius = MathF.Min(MathF.Pow(RawFlaK, 0.3f) * 3f, 30f);
            // Multiply by volume to approximate applied damage
            float sphereVolume = MathF.Pow(FlaKExplosionRadius, 3) * MathF.PI * 4f / 3f;
            DamageDict[DamageType.FlaK] = RawFlaK * sphereVolume;
        }

        /// <summary>
        /// Calculates damage from Frag
        /// </summary>
        /// <param name="fragAngleMultiplier">(2 + sqrt(cone angle °)) / 16</param>
        void CalculateFragDamage(float fragAngleMultiplier)
        {
            // Get index of frag body
            int fragIndex = int.MaxValue;
            for (int i = 0; i < Module.AllModules.Length; i++)
            {
                if (Module.AllModules[i] == Module.FragBody)
                {
                    fragIndex = i;
                    break;
                }
            }

            float fragBodies = BodyModuleCounts[fragIndex];

            if (HeadModule == Module.FragHead || HeadModule == Module.FragBody)
            {
                fragBodies++;
            }
            DamageDict[DamageType.Frag] = GaugeCoefficient * fragBodies * OverallChemModifier * 66000;
            // Frag count is based on raw damage before angle multiplier
            FragCount = MathF.Floor(MathF.Pow(DamageDict[DamageType.Frag], 0.25f));
            DamageDict[DamageType.Frag] *= fragAngleMultiplier;
            DamagePerFrag = DamageDict[DamageType.Frag] / FragCount;
        }

        /// <summary>
        /// Calculates damage from HE 
        /// </summary>
        void CalculateHEDamage()
        {
            // Get index of HE body
            int heIndex = int.MaxValue;
            for (int i = 0; i < Module.AllModules.Length; i++)
            {
                if (Module.AllModules[i] == Module.HEBody)
                {
                    heIndex = i;
                    break;
                }
            }

            float heBodies = BodyModuleCounts[heIndex];

            if (HeadModule == Module.ShapedChargeHead)
            {
                heBodies += 0.2f;
            }
            else if (HeadModule == Module.HEHead || HeadModule == Module.HEBody)
            {
                heBodies++;
            }
            RawHE = 3000f * MathF.Pow(GaugeCoefficient * heBodies * 0.88f * OverallChemModifier, 0.9f);
            HEExplosionRadius = MathF.Min(MathF.Pow(RawHE, 0.3f), 30f);
            // Multiply by volume to approximate applied damage
            float sphereVolume = MathF.Pow(HEExplosionRadius, 3) * MathF.PI * 4f / 3f;
            DamageDict[DamageType.HE] = RawHE * sphereVolume;
        }

        /// <summary>
        /// Calculates damage from HEAT, assuming special factor of 1 for all HE bodies and a penetration metric of 0.5
        /// HESH damage scales same as HEAT, so optimal configurations work for both types
        /// </summary>
        void CalculateHeatDamage()
        {
            if (HeadModule == Module.ShapedChargeHead)
            {
                // Get index of HE body
                int heIndex = int.MaxValue;
                for (int i = 0; i < Module.AllModules.Length; i++)
                {
                    if (Module.AllModules[i] == Module.HEBody)
                    {
                        heIndex = i;
                        break;
                    }
                }

                float heBodies = BodyModuleCounts[heIndex];
                // Calculate HE damage assuming special factor of 1 for HE bodies
                // Special heads count as HE body with special factor of 0.8, leaving 0.2 body equivalents for actual HE damage
                RawHE = 3000f * MathF.Pow(GaugeCoefficient * 0.2f * 0.88f * OverallChemModifier, 0.9f);
                HEExplosionRadius = MathF.Min(MathF.Pow(RawHE, 0.3f), 30f);
                // Multiply by volume to approximate applied damage
                float sphereVolume = MathF.Pow(HEExplosionRadius, 3) * MathF.PI * 4f / 3f;
                DamageDict[DamageType.HE] = RawHE * sphereVolume;

                DamageDict[DamageType.HEAT] =
                    GaugeCoefficient
                    * (heBodies + 0.8f)
                    * OverallChemModifier
                    * 17447.75f; // 26400 / 16 / sqrt(0.5) * (2 + sqrt(30))
            }
            else
            {
                DamageDict[DamageType.HE] = 0;
                DamageDict[DamageType.HEAT] = 0;
            }
        }

        /// <summary>
        /// Calculates planar shield reduction for shells with disruptor head
        /// </summary>
        void CalculateShieldReduction()
        {
            CalculateEmpDamage();
            if (HeadModule == Module.Disruptor)
            {
                DamageDict[DamageType.Disruptor] = DamageDict[DamageType.EMP] * 0.75f / 1500;
                DamageDict[DamageType.Disruptor] = MathF.Min(DamageDict[DamageType.Disruptor], 1f);
            }
            else
            {
                DamageDict[DamageType.Disruptor] = 0;
            }
        }


        /// <summary>
        /// Calculates applied kinetic damage for a given target armor class and impact angle
        /// </summary>
        void CalculateKineticDps(float targetAC)
        {
            CalculateKineticDamage();
            CalculateAP();

            // Hollow point and CIWS ignore impact angle
            if (HeadModule == Module.HollowPoint || targetAC == 20f)
            {
                DamageDict[DamageType.Kinetic] = RawKD * MathF.Min(1, ArmorPierce / targetAC);
            }
            else if (HeadModule == Module.SabotHead)
            {
                DamageDict[DamageType.Kinetic] = RawKD * MathF.Min(1, ArmorPierce / targetAC) * SabotAngleMultiplier;
            }
            else
            {
                DamageDict[DamageType.Kinetic] = RawKD * MathF.Min(1, ArmorPierce / targetAC) * NonSabotAngleMultiplier;
            }

            DpsDict[DamageType.Kinetic] = DamageDict[DamageType.Kinetic] / ReloadTime;
            DpsPerVolumeDict[DamageType.Kinetic] = DpsDict[DamageType.Kinetic] / VolumePerIntake;
            DpsPerCostDict[DamageType.Kinetic] = DpsDict[DamageType.Kinetic] / CostPerIntake;
        }


        void CalculateKineticDpsBelt(float targetAC)
        {
            if (TotalLength <= 1000f)
            {
                CalculateKineticDamage();
                CalculateAP();

                if (HeadModule == Module.HollowPoint || targetAC == 20f)
                {
                    DamageDict[DamageType.Kinetic] = RawKD * MathF.Min(1, ArmorPierce / targetAC);
                }
                else if (HeadModule == Module.SabotHead)
                {
                    DamageDict[DamageType.Kinetic] = RawKD * MathF.Min(1, ArmorPierce / targetAC) * SabotAngleMultiplier;
                }
                else
                {
                    DamageDict[DamageType.Kinetic] = RawKD * MathF.Min(1, ArmorPierce / targetAC) * NonSabotAngleMultiplier;
                }

                DpsDict[DamageType.Kinetic] = DamageDict[DamageType.Kinetic] / ReloadTimeBelt * UptimeBelt;
                DpsPerVolumeDict[DamageType.Kinetic] = DpsDict[DamageType.Kinetic] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.Kinetic] = DpsDict[DamageType.Kinetic] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.Kinetic] = 0;
                DpsPerVolumeDict[DamageType.Kinetic] = 0;
                DpsPerCostDict[DamageType.Kinetic] = 0;
            }
        }

        /// <summary>
        /// Calculates EMP damage per second
        /// </summary>
        void CalculateEmpDps()
        {
            DpsDict[DamageType.EMP] = DamageDict[DamageType.EMP] / ReloadTime;
            DpsPerVolumeDict[DamageType.EMP] = DpsDict[DamageType.EMP] / VolumePerIntake;
            DpsPerCostDict[DamageType.EMP] = DpsDict[DamageType.EMP] / CostPerIntake;
        }

        /// <summary>
        /// Calculates EMP damage per second for beltfed loaders
        /// </summary>
        void CalculateEmpDpsBelt()
        {
            if (TotalLength <= 1000)
            {
                DpsDict[DamageType.EMP] = DamageDict[DamageType.EMP] / ReloadTimeBelt * UptimeBelt;
                DpsPerVolumeDict[DamageType.EMP] = DpsDict[DamageType.EMP] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.EMP] = DpsDict[DamageType.EMP] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.EMP] = 0;
                DpsPerVolumeDict[DamageType.EMP] = 0;
                DpsPerCostDict[DamageType.EMP] = 0;
            }
        }

        /// <summary>
        /// Calculates FlaK damage per second
        /// </summary>
        void CalculateFlaKDps()
        {
            DpsDict[DamageType.FlaK] = DamageDict[DamageType.FlaK] / ReloadTime;
            DpsPerVolumeDict[DamageType.FlaK] = DpsDict[DamageType.FlaK] / VolumePerIntake;
            DpsPerCostDict[DamageType.FlaK] = DpsDict[DamageType.FlaK] / CostPerIntake;
        }

        /// <summary>
        /// Calculates FlaK damage per second for beltfed loaders
        /// </summary>
        void CalculateFlaKDpsBelt()
        {
            if (TotalLength <= 1000)
            {
                DpsDict[DamageType.FlaK] = DamageDict[DamageType.FlaK] / ReloadTimeBelt * UptimeBelt;
                DpsPerVolumeDict[DamageType.FlaK] = DpsDict[DamageType.FlaK] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.FlaK] = DpsDict[DamageType.FlaK] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.FlaK] = 0;
                DpsPerVolumeDict[DamageType.FlaK] = 0;
                DpsPerCostDict[DamageType.FlaK] = 0;
            }
        }

        /// <summary>
        /// Calculates Frag damage per second
        /// </summary>
        void CalculateFragDps()
        {
            DpsDict[DamageType.Frag] = DamageDict[DamageType.Frag] / ReloadTime;
            DpsPerVolumeDict[DamageType.Frag] = DpsDict[DamageType.Frag] / VolumePerIntake;
            DpsPerCostDict[DamageType.Frag] = DpsDict[DamageType.Frag] / CostPerIntake;
        }

        /// <summary>
        /// Calculates Frag damage per second for beltfed loaders
        /// </summary>
        void CalculateFragDpsBelt()
        {
            if (TotalLength <= 1000)
            {
                DpsDict[DamageType.Frag] = DamageDict[DamageType.Frag] / ReloadTimeBelt * UptimeBelt;
                DpsPerVolumeDict[DamageType.Frag] = DpsDict[DamageType.Frag] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.Frag] = DpsDict[DamageType.Frag] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.Frag] = 0;
                DpsPerVolumeDict[DamageType.Frag] = 0;
                DpsPerCostDict[DamageType.Frag] = 0;
            }
        }

        /// <summary>
        /// Calculates HE damage per second
        /// </summary>
        void CalculateHEDps()
        {
            DpsDict[DamageType.HE] = DamageDict[DamageType.HE] / ReloadTime;
            DpsPerVolumeDict[DamageType.HE] = DpsDict[DamageType.HE] / VolumePerIntake;
            DpsPerCostDict[DamageType.HE] = DpsDict[DamageType.HE] / CostPerIntake;
        }

        /// <summary>
        /// Calculates HE damage per second for beltfed loaders
        /// </summary>
        void CalculateHEDpsBelt()
        {
            if (TotalLength <= 1000)
            {
                DpsDict[DamageType.HE] = DamageDict[DamageType.HE] / ReloadTimeBelt * UptimeBelt;
                DpsPerVolumeDict[DamageType.HE] = DpsDict[DamageType.HE] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.HE] = DpsDict[DamageType.HE] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.HE] = 0;
                DpsPerVolumeDict[DamageType.HE] = 0;
                DpsPerCostDict[DamageType.HE] = 0;
            }
        }

        /// <summary>
        /// Calculates HEAT damage per second (HESH scales similarly)
        /// </summary>
        void CalculateHeatDps()
        {
            if (HeadModule == Module.ShapedChargeHead)
            {
                DpsDict[DamageType.HE] = DamageDict[DamageType.HE] / ReloadTime;
                DpsPerVolumeDict[DamageType.HE] = DpsDict[DamageType.HE] / VolumePerIntake;
                DpsPerCostDict[DamageType.HE] = DpsDict[DamageType.HE] / CostPerIntake;

                DpsDict[DamageType.HEAT] = DamageDict[DamageType.HEAT] / ReloadTime;
                DpsPerVolumeDict[DamageType.HEAT] = DpsDict[DamageType.HEAT] / VolumePerIntake;
                DpsPerCostDict[DamageType.HEAT] = DpsDict[DamageType.HEAT] / CostPerIntake;
            }
            else
            {
                DpsDict[DamageType.HE] = 0;
                DpsPerVolumeDict[DamageType.HE] = 0;
                DpsPerCostDict[DamageType.HE] = 0;

                DpsDict[DamageType.HEAT] = 0;
                DpsPerVolumeDict[DamageType.HEAT] = 0;
                DpsPerCostDict[DamageType.HEAT] = 0;
            }
        }

        /// <summary>
        /// Calculates HEAT damage per second for beltfed loaders
        /// </summary>
        void CalculateHeatDpsBelt()
        {
            if (HeadModule == Module.ShapedChargeHead && TotalLength <= 1000)
            {
                DpsDict[DamageType.HE] = DamageDict[DamageType.HE] / ReloadTimeBelt * UptimeBelt;
                DpsPerVolumeDict[DamageType.HE] = DpsDict[DamageType.HE] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.HE] = DpsDict[DamageType.HE] / CostPerIntakeBelt;

                DpsDict[DamageType.HEAT] = DamageDict[DamageType.HEAT] / ReloadTimeBelt * UptimeBelt;
                DpsPerVolumeDict[DamageType.HEAT] = DpsDict[DamageType.HEAT] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.HEAT] = DpsDict[DamageType.HEAT] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.HE] = 0;
                DpsPerVolumeDict[DamageType.HE] = 0;
                DpsPerCostDict[DamageType.HE] = 0;

                DpsDict[DamageType.HEAT] = 0;
                DpsPerVolumeDict[DamageType.HEAT] = 0;
                DpsPerCostDict[DamageType.HEAT] = 0;
            }
        }

        /// <summary>
        /// Calculates shield disruption, in % reduction per second per volume
        /// </summary>
        void CalculateShieldRps()
        {
            CalculateEmpDps();

            DpsDict[DamageType.Disruptor] = DamageDict[DamageType.Disruptor] / ReloadTime;

            DpsPerVolumeDict[DamageType.Disruptor] = DpsDict[DamageType.Disruptor] / VolumePerIntake;
            DpsPerCostDict[DamageType.Disruptor] = DpsDict[DamageType.Disruptor] / CostPerIntake;
        }

        void CalculateShieldRpsBelt()
        {
            if (TotalLength <= 1000f)
            {
                CalculateEmpDpsBelt();

                DpsDict[DamageType.Disruptor] = DamageDict[DamageType.Disruptor] / ReloadTimeBelt * UptimeBelt;

                DpsPerVolumeDict[DamageType.Disruptor] = DpsDict[DamageType.Disruptor] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.Disruptor] = DpsDict[DamageType.Disruptor] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.Disruptor] = 0;
                DpsPerVolumeDict[DamageType.Disruptor] = 0;
                DpsPerCostDict[DamageType.Disruptor] = 0;
            }
        }


        /// <summary>
        /// Calculate damage modifier according to current DamageType
        /// </summary>
        public void CalculateDamageModifierByType(DamageType dt)
        {
            CalculateKDModifier();
            CalculateAPModifier();
            if (dt != DamageType.Kinetic)
            {
                CalculateChemModifier();
            }
        }


        /// <summary>
        /// Calculates damage according to current damageType
        /// </summary>
        /// <param name="dt">Damage type to test</param>
        /// <param name="fragAngleMultiplier">(2 + sqrt(angle °)) / 16</param>
        public void CalculateDamageByType(DamageType dt, float fragAngleMultiplier)
        {
            if (dt == DamageType.Kinetic)
            {
                CalculateKineticDamage();
                CalculateAP();
            }
            else if (dt == DamageType.EMP)
            {
                CalculateEmpDamage();
            }
            else if (dt == DamageType.FlaK)
            {
                CalculateFlaKDamage();
            }
            else if (dt == DamageType.Frag)
            {
                CalculateFragDamage(fragAngleMultiplier);
            }
            else if (dt == DamageType.HE)
            {
                CalculateHEDamage();
            }
            else if (dt == DamageType.HEAT)
            {
                CalculateHeatDamage();
            }
            else if (dt == DamageType.Disruptor)
            {
                CalculateShieldReduction();
            }
        }


        public void CalculateDpsByType(
            DamageType dt,
            float targetAC,
            int testIntervalSeconds,
            float storagePerVolume,
            float storagePerCost,
            float ppm,
            float ppv,
            float ppc,
            bool fuel,
            Scheme targetScheme,
            float impactAngleFromPerpendicularDegrees)
        {
            CalculateRecoil();
            CalculateRailVolumeAndCost(testIntervalSeconds, storagePerVolume, storagePerCost, ppm, ppv, ppc, fuel);
            CalculateRecoilVolumeAndCost();
            CalculateVariableVolumesAndCosts(testIntervalSeconds, storagePerVolume, storagePerCost);
            CalculateVolumeAndCostPerIntake();

            // Kinetic stats needed for pendepth testing
            CalculateVelocity();
            CalculateKineticDamage();
            CalculateAP();

            if (RawKD >= targetScheme.GetRequiredKD(ArmorPierce, impactAngleFromPerpendicularDegrees, HeadModule == Module.SabotHead)
                || (HeadModule == Module.HollowPoint && RawKD >= targetScheme.GetRequiredThump(ArmorPierce)))
            {
                if (dt == DamageType.Kinetic)
                {
                    CalculateKineticDps(targetAC);
                }
                else if (dt == DamageType.EMP)
                {
                    CalculateEmpDps();
                }
                else if (dt == DamageType.FlaK)
                {
                    CalculateFlaKDps();
                }
                else if (dt == DamageType.Frag)
                {
                    CalculateFragDps();
                }
                else if (dt == DamageType.HE)
                {
                    CalculateHEDps();
                }
                else if (dt == DamageType.HEAT)
                {
                    CalculateHeatDps();
                }
                else if (dt == DamageType.Disruptor)
                {
                    CalculateShieldRps();
                }
            }
            else
            {
                foreach (DamageType dpstype in DpsDict.Keys)
                {
                    DpsDict[dpstype] = 0;
                }
                foreach (DamageType dpstype in DpsPerCostDict.Keys)
                {
                    DpsPerCostDict[dpstype] = 0;
                }
                foreach (DamageType dpstype in DpsPerVolumeDict.Keys)
                {
                    DpsPerVolumeDict[dpstype] = 0;
                }
            }

        }

        public void CalculateDpsByTypeBelt(
            DamageType dt,
            float targetAC,
            int testIntervalSeconds,
            float storagePerVolume,
            float storagePerCost,
            float ppm,
            float ppv,
            float ppc,
            bool fuel,
            Scheme targetScheme,
            float impactAngleFromPerpendicularDegrees)
        {
            CalculateRecoil();
            CalculateRailVolumeAndCost(testIntervalSeconds, storagePerVolume, storagePerCost, ppm, ppv, ppc, fuel);
            CalculateRecoilVolumeAndCost();
            CalculateVolumeAndCostPerIntake();

            // Kinetic stats needed for pendepth testing
            CalculateVelocity();
            CalculateKineticDamage();
            CalculateAP();

            if (RawKD >= targetScheme.GetRequiredKD(ArmorPierce, impactAngleFromPerpendicularDegrees, HeadModule == Module.SabotHead)
                || (HeadModule == Module.HollowPoint && RawKD >= targetScheme.GetRequiredThump(ArmorPierce)))
            {
                if (dt == DamageType.Kinetic)
                {
                    CalculateKineticDpsBelt(targetAC);
                }
                else if (dt == DamageType.EMP)
                {
                    CalculateEmpDpsBelt();
                }
                else if (dt == DamageType.FlaK)
                {
                    CalculateFlaKDpsBelt();
                }
                else if (dt == DamageType.Frag)
                {
                    CalculateFragDpsBelt();
                }
                else if (dt == DamageType.HE)
                {
                    CalculateHEDpsBelt();
                }
                else if (dt == DamageType.HEAT)
                {
                    CalculateHeatDpsBelt();
                }
                else if (dt == DamageType.Disruptor)
                {
                    CalculateShieldRpsBelt();
                }
            }
            else
            {
                foreach (DamageType dpstype in DpsDict.Keys)
                {
                    DpsDict[dpstype] = 0;
                }
                foreach (DamageType dpstype in DpsPerCostDict.Keys)
                {
                    DpsPerCostDict[dpstype] = 0;
                }
                foreach (DamageType dpstype in DpsPerVolumeDict.Keys)
                {
                    DpsPerVolumeDict[dpstype] = 0;
                }
            }
        }


        /// <summary>
        /// Calculates total number of modules in shell
        /// </summary>
        public void GetModuleCounts()
        {
            // ModuleCountTotal starts at 1 for head
            ModuleCountTotal = 1;
            if (BaseModule != null)
            {
                ModuleCountTotal += 1;
            }

            foreach (float modCount in BodyModuleCounts)
            {
                ModuleCountTotal += modCount;
            }

            ModuleCountTotal += MathF.Ceiling(GPCasingCount) + RGCasingCount;
        }


        public void WriteShellInfoToFile(
            StreamWriter writer,
            bool labels,
            bool showGP,
            bool showRG,
            bool showDraw,
            Dictionary<DamageType, bool> dtToShow,
            List<int> modsToShow,
            float targetAC)
        {
            if (labels)
            {
                writer.WriteLine("Gauge (mm): " + Gauge);
                writer.WriteLine("Total length (mm): " + TotalLength);
                writer.WriteLine("Length without casings (mm): " + ProjectileLength);
                writer.WriteLine("Total modules: " + ModuleCountTotal);

                if (showGP)
                {
                    writer.WriteLine("GP casing: " + GPCasingCount);
                }
                if (showRG)
                {
                    writer.WriteLine("RG casing: " + RGCasingCount);
                }

                foreach (int index in modsToShow)
                {
                    writer.WriteLine(Module.AllModules[index].Name + ": " + BodyModuleCounts[index]);
                }
                writer.WriteLine("Head: " + HeadModule.Name);


                if (showDraw)
                {
                    writer.WriteLine("Rail draw: " + RailDraw);
                }
                // Recoil = draw if no GP
                if (showGP)
                {
                    writer.WriteLine("Recoil: " + TotalRecoil);
                }

                writer.WriteLine("Velocity (m/s): " + Velocity);
                writer.WriteLine("Effective range (m): " + EffectiveRange);
                writer.WriteLine("Barrel length for inaccuracy (m): " + BarrelLengthForInaccuracy);
                writer.WriteLine("Barrel length for propellant burn (m): " + BarrelLengthForPropellant);

                foreach (DamageType dt in dtToShow.Keys)
                {
                    if (dtToShow[dt])
                    {
                        if (dt == DamageType.Kinetic)
                        {
                            writer.WriteLine("Raw KD: " + RawKD);
                            writer.WriteLine("AP: " + ArmorPierce);

                            if (HeadModule == Module.HollowPoint || targetAC == 20f)
                            {
                                writer.WriteLine("KD multiplier from angle: 1");
                            }
                            else if (HeadModule == Module.SabotHead)
                            {
                                writer.WriteLine("KD multiplier from angle: " + SabotAngleMultiplier);
                            }
                            else
                            {
                                writer.WriteLine("KD multiplier from angle: " + NonSabotAngleMultiplier);
                            }
                        }
                        else if (dt == DamageType.Frag)
                        {
                            writer.WriteLine("Frag count: " + FragCount);
                            writer.WriteLine("Damage per frag: " + DamagePerFrag);
                        }
                        else if (dt == DamageType.FlaK)
                        {
                            writer.WriteLine("Raw FlaK damage: " + RawFlaK);
                            writer.WriteLine("FlaK explosion radius (m): " + FlaKExplosionRadius);
                        }
                        else if (dt == DamageType.HE)
                        {
                            writer.WriteLine("Raw HE damage: " + RawHE);
                            writer.WriteLine("HE explosion radius (m): " + HEExplosionRadius);
                        }
                        writer.WriteLine((DamageType)(int)dt + " damage: " + DamageDict[dt]);
                    }
                }


                if (IsBelt)
                {
                    writer.WriteLine("Reload time (s): " + ReloadTimeBelt);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine((DamageType)(int)dt + " DPS: " + DpsDict[dt]);
                        }
                    }

                    writer.WriteLine("Loader volume: " + LoaderVolumeBelt);
                    writer.WriteLine("Cooler volume: " + CoolerVolumeBelt);
                    writer.WriteLine("Charger volume: " + ChargerVolumeBelt);
                    writer.WriteLine("Engine volume: " + EngineVolumeBelt);
                    writer.WriteLine("Fuel access volume: " + FuelAccessVolumeBelt);
                    writer.WriteLine("Fuel storage volume: " + FuelStorageVolumeBelt);
                    writer.WriteLine("Recoil volume: " + RecoilVolumeBelt);
                    writer.WriteLine("Ammo access volume: " + AmmoAccessVolumeBelt);
                    writer.WriteLine("Ammo storage volume: " + AmmoStorageVolumeBelt);
                    writer.WriteLine("Total volume: " + VolumePerIntakeBelt);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine((DamageType)(int)dt + " DPS per volume: " + DpsPerVolumeDict[dt]);
                        }
                    }

                    writer.WriteLine("Cost per shell: " + CostPerShell);
                    writer.WriteLine("Loader cost: " + LoaderCostBelt);
                    writer.WriteLine("Cooler cost: " + CoolerCostBelt);
                    writer.WriteLine("Charger cost: " + ChargerCostBelt);
                    writer.WriteLine("Fuel burned: " + FuelBurnedBelt);
                    writer.WriteLine("Engine cost: " + EngineCostBelt);
                    writer.WriteLine("Fuel access cost: " + FuelAccessCostBelt);
                    writer.WriteLine("Fuel storage cost: " + FuelStorageCostBelt);
                    writer.WriteLine("Recoil cost: " + RecoilCostBelt);
                    writer.WriteLine("Ammo used: " + ShellCostBelt);
                    writer.WriteLine("Ammo access cost: " + AmmoAccessCostBelt);
                    writer.WriteLine("Ammo storage cost: " + AmmoStorageCostBelt);
                    writer.WriteLine("Total cost: " + CostPerIntakeBelt);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine((DamageType)(int)dt + " DPS per cost: " + DpsPerCostDict[dt]);
                        }
                    }
                }
                else
                {
                    writer.WriteLine("Reload time (s): " + ReloadTime);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine((DamageType)(int)dt + " DPS: " + DpsDict[dt]);
                        }
                    }

                    writer.WriteLine("Loader volume: " + LoaderVolume);
                    writer.WriteLine("Cooler volume: " + CoolerVolume);
                    writer.WriteLine("Charger volume: " + ChargerVolume);
                    writer.WriteLine("Engine volume: " + EngineVolume);
                    writer.WriteLine("Fuel access volume: " + FuelAccessVolume);
                    writer.WriteLine("Fuel storage volume: " + FuelStorageVolume);
                    writer.WriteLine("Recoil volume: " + RecoilVolume);
                    writer.WriteLine("Ammo access volume: " + AmmoAccessVolume);
                    writer.WriteLine("Ammo storage volume: " + AmmoStorageVolume);
                    writer.WriteLine("Total volume: " + VolumePerIntake);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine((DamageType)(int)dt + " DPS per volume: " + DpsPerVolumeDict[dt]);
                        }
                    }

                    writer.WriteLine("Cost per shell: " + CostPerShell);
                    writer.WriteLine("Loader cost: " + LoaderCost);
                    writer.WriteLine("Cooler cost: " + CoolerCost);
                    writer.WriteLine("Charger cost: " + ChargerCost);
                    writer.WriteLine("Fuel burned: " + FuelBurned);
                    writer.WriteLine("Engine cost: " + EngineCost);
                    writer.WriteLine("Fuel access cost: " + FuelAccessCost);
                    writer.WriteLine("Fuel storage cost: " + FuelStorageCost);
                    writer.WriteLine("Recoil cost: " + RecoilCost);
                    writer.WriteLine("Ammo used: " + ShellCost);
                    writer.WriteLine("Ammo access cost: " + AmmoAccessCost);
                    writer.WriteLine("Ammo storage cost: " + AmmoStorageCost);
                    writer.WriteLine("Total cost: " + CostPerIntake);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine((DamageType)(int)dt + " DPS per cost: " + DpsPerCostDict[dt]);
                        }
                    }
                }
            }
            else
            {
                writer.WriteLine(Gauge);
                writer.WriteLine(TotalLength);
                writer.WriteLine(ProjectileLength);
                writer.WriteLine(ModuleCountTotal);

                if (showGP)
                {
                    writer.WriteLine(GPCasingCount);
                }
                if (showRG)
                {
                    writer.WriteLine(RGCasingCount);
                }

                foreach (int index in modsToShow)
                {
                    writer.WriteLine(BodyModuleCounts[index]);
                }
                writer.WriteLine(HeadModule.Name);


                if (showDraw)
                {
                    writer.WriteLine(RailDraw);
                }
                // Recoil = draw if no GP
                if (showGP)
                {
                    writer.WriteLine(TotalRecoil);
                }

                writer.WriteLine(Velocity);
                writer.WriteLine(EffectiveRange);
                writer.WriteLine(BarrelLengthForInaccuracy);
                writer.WriteLine(BarrelLengthForPropellant);


                foreach (DamageType dt in dtToShow.Keys)
                {
                    if (dtToShow[dt])
                    {
                        if (dt == DamageType.Kinetic)
                        {
                            writer.WriteLine(RawKD);
                            writer.WriteLine(ArmorPierce);
                            if (HeadModule == Module.HollowPoint || targetAC == 20f)
                            {
                                writer.WriteLine("1");
                            }
                            else if (HeadModule == Module.SabotHead)
                            {
                                writer.WriteLine(SabotAngleMultiplier);
                            }
                            else
                            {
                                writer.WriteLine(NonSabotAngleMultiplier);
                            }
                        }
                        if (dt == DamageType.Frag)
                        {
                            writer.WriteLine(FragCount);
                            writer.WriteLine(DamagePerFrag);
                        }
                        else if (dt == DamageType.FlaK)
                        {
                            writer.WriteLine(RawFlaK);
                            writer.WriteLine(FlaKExplosionRadius);
                        }
                        else if (dt == DamageType.HE)
                        {
                            writer.WriteLine(RawHE);
                            writer.WriteLine(HEExplosionRadius);
                        }
                        writer.WriteLine(DamageDict[dt]);
                    }
                }


                if (IsBelt)
                {
                    writer.WriteLine(ReloadTimeBelt);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine(DpsDict[dt]);
                        }
                    }

                    writer.WriteLine(LoaderVolumeBelt);
                    writer.WriteLine(CoolerVolumeBelt);
                    writer.WriteLine(ChargerVolumeBelt);
                    writer.WriteLine(EngineVolumeBelt);
                    writer.WriteLine(FuelAccessVolumeBelt);
                    writer.WriteLine(FuelStorageVolumeBelt);
                    writer.WriteLine(RecoilVolumeBelt);
                    writer.WriteLine(AmmoAccessVolumeBelt);
                    writer.WriteLine(AmmoStorageVolumeBelt);
                    writer.WriteLine(VolumePerIntakeBelt);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine(DpsPerVolumeDict[dt]);
                        }
                    }

                    writer.WriteLine(CostPerShell);
                    writer.WriteLine(LoaderCostBelt);
                    writer.WriteLine(CoolerCostBelt);
                    writer.WriteLine(ChargerCostBelt);
                    writer.WriteLine(FuelBurnedBelt);
                    writer.WriteLine(EngineCostBelt);
                    writer.WriteLine(FuelAccessCostBelt);
                    writer.WriteLine(FuelStorageCostBelt);
                    writer.WriteLine(RecoilCostBelt);
                    writer.WriteLine(ShellCostBelt);
                    writer.WriteLine(AmmoAccessCostBelt);
                    writer.WriteLine(AmmoStorageCostBelt);
                    writer.WriteLine(CostPerIntakeBelt);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine(DpsPerCostDict[dt]);
                        }
                    }
                }
                else
                {
                    writer.WriteLine(ReloadTime);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine(DpsDict[dt]);
                        }
                    }

                    writer.WriteLine(LoaderVolume);
                    writer.WriteLine(CoolerVolume);
                    writer.WriteLine(ChargerVolume);
                    writer.WriteLine(EngineVolume);
                    writer.WriteLine(FuelAccessVolume);
                    writer.WriteLine(FuelStorageVolume);
                    writer.WriteLine(RecoilVolume);
                    writer.WriteLine(AmmoAccessVolume);
                    writer.WriteLine(AmmoStorageVolume);
                    writer.WriteLine(VolumePerIntake);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine(DpsPerVolumeDict[dt]);
                        }
                    }

                    writer.WriteLine(CostPerShell);
                    writer.WriteLine(LoaderCost);
                    writer.WriteLine(CoolerCost);
                    writer.WriteLine(ChargerCost);
                    writer.WriteLine(FuelBurned);
                    writer.WriteLine(EngineCost);
                    writer.WriteLine(FuelAccessCost);
                    writer.WriteLine(FuelStorageCost);
                    writer.WriteLine(RecoilCost);
                    writer.WriteLine(ShellCost);
                    writer.WriteLine(AmmoAccessCost);
                    writer.WriteLine(AmmoStorageCost);
                    writer.WriteLine(CostPerIntake);
                    foreach (DamageType dt in dtToShow.Keys)
                    {
                        if (dtToShow[dt])
                        {
                            writer.WriteLine(DpsPerCostDict[dt]);
                        }
                    }
                }
            }
        }
    }
}
