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
                GaugeCoefficient = MathF.Pow(Gauge * Gauge * Gauge / 125000000f, 0.6f);
            }
        }
        public float GaugeCoefficient { get; set; } // Expensive to calculate and used in several formulae

        public bool IsBelt;

        // Keep counts of body modules.
        public float[] BodyModuleCounts { get; set; } = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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

        // Overall modifiers
        public float OverallVelocityModifier { get; set; }
        public float OverallKineticDamageModifier { get; set; }
        public float OverallArmorPierceModifier { get; set; }
        public float OverallChemModifier { get; set; }


        // Power
        public float GPRecoil { get; set; }
        public float MaxDraw { get; set; }
        public float RailDraw { get; set; }
        public float TotalRecoil { get; set; }
        public float Velocity { get; set; }

        // Reload
        public float ReloadTime { get; set; }
        public float ReloadTimeBelt { get; set; } // Beltfed Loader
        public float UptimeBelt { get; set; }
        public int BarrelCount { get; set; }
        public float CooldownTime { get; set; }

        // Effective range
        public float EffectiveRange { get; set; }


        // Damage
        public float KineticDamage { get; set; }
        public float ArmorPierce { get; set; }
        public float EffectiveKineticDamage { get; set; }

        public Dictionary<DamageType, float> DamageDict = new()
        {
            { DamageType.Kinetic, 0 },
            { DamageType.Emp, 0 },
            { DamageType.FlaK, 0 },
            { DamageType.Frag, 0 },
            { DamageType.HE, 0 },
            { DamageType.HEAT, 0 },
            { DamageType.Pendepth, 0 },
            { DamageType.Disruptor, 0 }
        };

        public Dictionary<DamageType, float> DpsDict = new()
        {
            { DamageType.Kinetic, 0 },
            { DamageType.Emp, 0 },
            { DamageType.FlaK, 0 },
            { DamageType.Frag, 0 },
            { DamageType.HE, 0 },
            { DamageType.HEAT, 0 },
            { DamageType.Pendepth, 0 },
            { DamageType.Disruptor, 0 }
        };

        public Dictionary<DamageType, float> DpsPerVolumeDict = new()
        {
            { DamageType.Kinetic, 0 },
            { DamageType.Emp, 0 },
            { DamageType.FlaK, 0 },
            { DamageType.Frag, 0 },
            { DamageType.HE, 0 },
            { DamageType.HEAT, 0 },
            { DamageType.Pendepth, 0 },
            { DamageType.Disruptor, 0 }
        };

        public Dictionary<DamageType, float> DpsPerCostDict = new()
        {
            { DamageType.Kinetic, 0 },
            { DamageType.Emp, 0 },
            { DamageType.FlaK, 0 },
            { DamageType.Frag, 0 },
            { DamageType.HE, 0 },
            { DamageType.HEAT, 0 },
            { DamageType.Pendepth, 0 },
            { DamageType.Disruptor, 0 }
        };


        // Volume
        public float LoaderVolume { get; set; }
        public float LoaderVolumeBelt { get; } = 4f; // loader, clip, 2 intakes
        public float RecoilVolume { get; set; }
        public float RecoilVolumeBelt { get; set; }
        public float ChargerVolume { get; set; }
        public float ChargerVolumeBelt { get; set; }
        public float CoolerVolume { get; set; }
        public float CoolerVolumeBelt { get; set; }
        public float AmmoCrateVolume { get; set; }
        public float AmmoCrateVolumeBelt { get; set; }
        public float MatStorageVolume { get; set; }
        public float MatStorageVolumeBelt { get; set; }
        public float VolumePerIntake { get; set; }
        public float VolumePerIntakeBelt { get; set; }


        // Cost
        public float LoaderCost { get; set; }
        public float LoaderCostBelt { get; } = 860f; // loader, clip, 2 intakes
        public float RecoilCost { get; set; }
        public float RecoilCostBelt { get; set; }
        public float ChargerCost { get; set; }
        public float ChargerCostBelt { get; set; }
        public float CoolerCost { get; set; }
        public float CoolerCostBelt { get; set; }
        public float CostPerShell { get; set; } // Material cost for one shell
        public float ShellCost { get; set; } // Material cost for all shells
        public float ShellCostBelt { get; set; }
        public float AmmoCrateCost { get; set; }
        public float AmmoCrateCostBelt { get; set; }
        public float MatStorageCost { get; set; }
        public float MatStorageCostBelt { get; set; }
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
            LoaderVolume += 1f; // Always have an intake
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
        /// Calculates marginal volume per intake of rail chargers
        /// </summary>
        public void CalculateChargerVolumeAndCost()
        {
            if (RailDraw > 0)
            {
                ChargerVolume = RailDraw / (ReloadTime * 200f); // Chargers are 200 Energy per second
                ChargerCost = ChargerVolume * 400f; // Chargers cost 400 per metre

                if (TotalLength <= 1000f)
                {
                    ChargerVolumeBelt = RailDraw / (ReloadTimeBelt * 200f);
                    ChargerCostBelt = ChargerVolumeBelt * 400f;
                }
                else
                {
                    ChargerVolumeBelt = 0;
                    ChargerCostBelt = 0;
                }
            }
            else
            {
                ChargerVolume = 0;
                ChargerCost = 0;
                ChargerVolumeBelt = 0;
                ChargerCostBelt = 0;
            }
        }


        /// <summary>
        /// Calculates all volumes and costs dependent on testing interval
        /// </summary>
        /// <param name="testInterval">Test interval in minutes</param>
        public void CalculateVariableVolumesAndCosts(int testInterval)
        {
            int testIntervalSeconds = testInterval * 60;
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
            AmmoCrateVolume = shellCostPerMinute / 50f;
            AmmoCrateCost = shellCostPerMinute / 5f;

            if (TotalLength <= 1000f)
            {
                float shellCostPerMinuteBelt = CostPerShell / ReloadTimeBelt * UptimeBelt * 60f;
                AmmoCrateVolumeBelt = shellCostPerMinuteBelt / 50f;
                AmmoCrateCostBelt = shellCostPerMinuteBelt / 5f;
            }

            // Calculate volume and cost of material storage
            // Generic crates: 1/500 m^3 and 1/250 material cost per material
            MatStorageVolume = ShellCost / 500f;
            MatStorageCost = ShellCost / 250f;

            if (TotalLength <= 1000f)
            {
                MatStorageVolumeBelt = ShellCostBelt / 500f;
                MatStorageCostBelt = ShellCostBelt / 250f;
            }
        }

        /// <summary>
        /// Calculates volume used by shell, including intake, loader, cooling, recoil absorbers, and rail chargers
        /// </summary>
        public void CalculateVolumeAndCostPerIntake()
        {

            VolumePerIntake = LoaderVolume + RecoilVolume + CoolerVolume + ChargerVolume + AmmoCrateVolume + MatStorageVolume;
            CostPerIntake = LoaderCost + RecoilCost + CoolerCost + ChargerCost + ShellCost + AmmoCrateCost + MatStorageCost;

            if (TotalLength <= 1000f)
            {
                VolumePerIntakeBelt = LoaderVolumeBelt + RecoilVolumeBelt + CoolerVolumeBelt + ChargerVolumeBelt + AmmoCrateVolumeBelt + MatStorageVolumeBelt;
                CostPerIntakeBelt = LoaderCostBelt + RecoilCostBelt + CoolerCostBelt + ChargerCostBelt + ShellCostBelt + AmmoCrateCostBelt + MatStorageCostBelt;
            }
        }


        /// <summary>
        /// Calculates reload time; also calculates beltfed reload time for shells 1000 mm or shorter
        /// </summary>
        public void CalculateReloadTime()
        {
            ReloadTime = MathF.Pow(Gauge * Gauge * Gauge / 125000000f, 0.45f)
                * (2f + EffectiveProjectileModuleCount + 0.25f * (RGCasingCount + GPCasingCount))
                * 17.5f;
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
                DamageDict[DamageType.Kinetic] =
                    GaugeCoefficient
                    * EffectiveProjectileModuleCount
                    * Velocity
                    * OverallKineticDamageModifier
                    * 3.5f;
            }
            else
            {
                DamageDict[DamageType.Kinetic] =
                    MathF.Pow(500 / MathF.Max(Gauge, 100f), 0.15f)
                    * GaugeCoefficient
                    * EffectiveProjectileModuleCount
                    * Velocity
                    * OverallKineticDamageModifier
                    * 3.5f;
            }
            KineticDamage = DamageDict[DamageType.Kinetic];
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
            float empBodies = BodyModuleCounts[2];

            if (HeadModule == Module.EmpHead || HeadModule == Module.EmpBody || HeadModule == Module.Disruptor)
            {
                empBodies++;
            }
            DamageDict[DamageType.Emp] = GaugeCoefficient * empBodies * OverallChemModifier * 1500;
        }

        /// <summary>
        /// Calculates EMP damage. Used by shield reduction
        /// </summary>
        void CalculateFlaKDamage()
        {
            float flaKBodies = BodyModuleCounts[3];

            if (HeadModule == Module.FlaKHead || HeadModule == Module.FlaKBody)
            {
                flaKBodies++;
            }
            DamageDict[DamageType.FlaK] = MathF.Pow(GaugeCoefficient * flaKBodies * 204 / 300, 0.9f) * OverallChemModifier * 3000;
        }

        /// <summary>
        /// Calculates damage from Frag, although EMP scales the same way
        /// </summary>
        void CalculateFragDamage()
        {
            float fragBodies = BodyModuleCounts[4];

            if (HeadModule == Module.FragHead || HeadModule == Module.FragBody)
            {
                fragBodies++;
            }
            DamageDict[DamageType.Frag] = GaugeCoefficient * fragBodies * OverallChemModifier * 60000;
        }

        /// <summary>
        /// Calculates damage from HE 
        /// </summary>
        void CalculateHEDamage()
        {
            float heBodies = BodyModuleCounts[5];

            if (HeadModule == Module.ShapedChargeHead)
            {
                heBodies += 0.2f;
            }
            else if (HeadModule == Module.HEHead || HeadModule == Module.HEBody)
            {
                heBodies++;
            }
            DamageDict[DamageType.HE] = MathF.Pow(GaugeCoefficient * heBodies * 24 / 30, 0.9f) * OverallChemModifier * 3000;
        }

        /// <summary>
        /// Calculates damage from HEAT, assuming special factor of 1 for all HE bodies and a penetration metric of 0.5
        /// HESH damage scales same as HEAT, so optimal configurations work for both types
        /// </summary>
        void CalculateHeatDamage()
        {
            if (HeadModule == Module.ShapedChargeHead)
            {
                float heBodies = BodyModuleCounts[5];
                // Special heads count as HE body with special factor of 0.8
                DamageDict[DamageType.HE] = MathF.Pow(GaugeCoefficient * 0.2f * 24 / 30, 0.9f) * OverallChemModifier * 3000;
                DamageDict[DamageType.HEAT] = GaugeCoefficient * (heBodies + 0.8f) * 60000f * 0.35f / MathF.Sqrt(0.5f) * (2 + MathF.Sqrt(30)) / 16f;
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
                DamageDict[DamageType.Disruptor] = DamageDict[DamageType.Emp] * 0.75f / 1500;
                DamageDict[DamageType.Disruptor] = MathF.Min(DamageDict[DamageType.Disruptor], 1f);
            }
            else
            {
                DamageDict[DamageType.Disruptor] = 0;
            }
        }

        /// <summary>
        /// Calculates weighted average of FlaK, Frag, and HE damage based on number of each warhead type
        /// </summary>
        void CalculatePendepthDamage()
        {
            // Calculate each damage type with and without damage-specific multipliers for equal comparison
            float flaKBodies = BodyModuleCounts[4];
            float flakDamageEquivalent = 0;
            if (flaKBodies > 0)
            {
                flakDamageEquivalent = MathF.Pow(GaugeCoefficient * flaKBodies * 204 / 300, 0.9f) * OverallChemModifier;
                DamageDict[DamageType.FlaK] = flakDamageEquivalent * 3000;
            }

            float fragBodies = BodyModuleCounts[4];
            float fragDamageEquivalent = 0;
            if (fragBodies > 0)
            {
                fragDamageEquivalent = GaugeCoefficient * fragBodies * OverallChemModifier;
                DamageDict[DamageType.Frag] = fragDamageEquivalent * 60000;
            }

            float heBodies = BodyModuleCounts[5];
            float heDamageEquivalent = 0;
            if (heBodies > 0)
            {
                heDamageEquivalent = MathF.Pow(GaugeCoefficient * heBodies * 24 / 30, 0.9f) * OverallChemModifier;
                DamageDict[DamageType.HE] = heDamageEquivalent * 3000;
            }

            // Weighted average of damage equivalents * 1 000 000 for easier reading
            DamageDict[DamageType.Pendepth] =
                (flakDamageEquivalent * flaKBodies + fragDamageEquivalent * fragBodies + heDamageEquivalent * heBodies)
                / (flaKBodies + fragBodies + heBodies)
                * 1000000;
        }


        /// <summary>
        /// Calculates applied kinetic damage for a given target armor class
        /// </summary>
        public void CalculateKineticDps(float targetAC)
        {
            CalculateKineticDamage();
            CalculateAP();

            EffectiveKineticDamage = KineticDamage * MathF.Min(1, ArmorPierce / targetAC);
            DamageDict[DamageType.Kinetic] = EffectiveKineticDamage;
            DpsDict[DamageType.Kinetic] = EffectiveKineticDamage / ReloadTime;

            DpsPerVolumeDict[DamageType.Kinetic] = DpsDict[DamageType.Kinetic] / VolumePerIntake;
            DpsPerCostDict[DamageType.Kinetic] = DpsDict[DamageType.Kinetic] / CostPerIntake;
        }


        public void CalculateKineticDpsBelt(float targetAC)
        {
            if (TotalLength <= 1000f)
            {
                CalculateKineticDamage();
                CalculateAP();

                EffectiveKineticDamage = KineticDamage * MathF.Min(1, ArmorPierce / targetAC);
                DamageDict[DamageType.Kinetic] = EffectiveKineticDamage;
                DpsDict[DamageType.Kinetic] = EffectiveKineticDamage / ReloadTimeBelt * UptimeBelt;

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
        public void CalculateEmpDps()
        {
            DpsDict[DamageType.Emp] = DamageDict[DamageType.Emp] / ReloadTime;
            DpsPerVolumeDict[DamageType.Emp] = DpsDict[DamageType.Emp] / VolumePerIntake;
            DpsPerCostDict[DamageType.Emp] = DpsDict[DamageType.Emp] / CostPerIntake;
        }

        /// <summary>
        /// Calculates EMP damage per second for beltfed loaders
        /// </summary>
        public void CalculateEmpDpsBelt()
        {
            if (TotalLength <= 1000)
            {
                DpsDict[DamageType.Emp] = DamageDict[DamageType.Emp] / ReloadTimeBelt * UptimeBelt;
                DpsPerVolumeDict[DamageType.Emp] = DpsDict[DamageType.Emp] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.Emp] = DpsDict[DamageType.Emp] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.Emp] = 0;
                DpsPerVolumeDict[DamageType.Emp] = 0;
                DpsPerCostDict[DamageType.Emp] = 0;
            }
        }

        /// <summary>
        /// Calculates FlaK damage per second
        /// </summary>
        public void CalculateFlaKDps()
        {
            DpsDict[DamageType.FlaK] = DamageDict[DamageType.FlaK] / ReloadTime;
            DpsPerVolumeDict[DamageType.FlaK] = DpsDict[DamageType.FlaK] / VolumePerIntake;
            DpsPerCostDict[DamageType.FlaK] = DpsDict[DamageType.FlaK] / CostPerIntake;
        }

        /// <summary>
        /// Calculates FlaK damage per second for beltfed loaders
        /// </summary>
        public void CalculateFlaKDpsBelt()
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
        public void CalculateFragDps()
        {
            DpsDict[DamageType.Frag] = DamageDict[DamageType.Frag] / ReloadTime;
            DpsPerVolumeDict[DamageType.Frag] = DpsDict[DamageType.Frag] / VolumePerIntake;
            DpsPerCostDict[DamageType.Frag] = DpsDict[DamageType.Frag] / CostPerIntake;
        }

        /// <summary>
        /// Calculates Frag damage per second for beltfed loaders
        /// </summary>
        public void CalculateFragDpsBelt()
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
        public void CalculateHEDps()
        {
            DpsDict[DamageType.HE] = DamageDict[DamageType.HE] / ReloadTime;
            DpsPerVolumeDict[DamageType.HE] = DpsDict[DamageType.HE] / VolumePerIntake;
            DpsPerCostDict[DamageType.HE] = DpsDict[DamageType.HE] / CostPerIntake;
        }

        /// <summary>
        /// Calculates HE damage per second for beltfed loaders
        /// </summary>
        public void CalculateHEDpsBelt()
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
        public void CalculateHeatDps()
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
        public void CalculateHeatDpsBelt()
        {
            if (HeadModule == Module.ShapedChargeHead)
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
        public void CalculateShieldRps()
        {
            CalculateEmpDps();

            DpsDict[DamageType.Disruptor] = DamageDict[DamageType.Disruptor] / ReloadTime;

            DpsPerVolumeDict[DamageType.Disruptor] = DpsDict[DamageType.Disruptor] / VolumePerIntake;
            DpsPerCostDict[DamageType.Disruptor] = DpsDict[DamageType.Disruptor] / CostPerIntake;
        }

        public void CalculateShieldRpsBelt()
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
        /// Calculate chemical damage of a shell if it is capable of penetrating given armor scheme
        /// </summary>
        public void CalculatePendepthDps(Scheme targetArmorScheme)
        {
            CalculateKineticDamage();
            CalculateAP();

            if (KineticDamage >= targetArmorScheme.GetRequiredKD(ArmorPierce))
            {
                DpsDict[DamageType.Pendepth] = DamageDict[DamageType.Pendepth] / ReloadTime;

                DpsPerVolumeDict[DamageType.Pendepth] = DpsDict[DamageType.Pendepth] / VolumePerIntake;
                DpsPerCostDict[DamageType.Pendepth] = DpsDict[DamageType.Pendepth] / CostPerIntake;
            }
            else
            {
                DpsDict[DamageType.Pendepth] = 0;

                DpsPerVolumeDict[DamageType.Pendepth] = 0;
                DpsPerCostDict[DamageType.Pendepth] = 0;
            }
        }


        public void CalculatePendepthDpsBelt(Scheme targetArmorScheme)
        {
            if (TotalLength <= 1000 && KineticDamage >= targetArmorScheme.GetRequiredKD(ArmorPierce))
            {
                CalculateKineticDamage();
                CalculateAP();

                DpsDict[DamageType.Pendepth] = DamageDict[DamageType.Pendepth] / ReloadTimeBelt * UptimeBelt;

                DpsPerVolumeDict[DamageType.Pendepth] = DpsDict[DamageType.Pendepth] / VolumePerIntakeBelt;
                DpsPerCostDict[DamageType.Pendepth] = DpsDict[DamageType.Pendepth] / CostPerIntakeBelt;
            }
            else
            {
                DpsDict[DamageType.Pendepth] = 0;

                DpsPerVolumeDict[DamageType.Pendepth] = 0;
                DpsPerCostDict[DamageType.Pendepth] = 0;
            }
        }


        /// <summary>
        /// Calculate damage modifier according to current DamageType
        /// </summary>
        public void CalculateDamageModifierByType(DamageType dt)
        {
            if (dt == DamageType.Kinetic)
            {
                CalculateKDModifier();
                CalculateAPModifier();
            }
            else if (dt == DamageType.Pendepth)
            {
                CalculateKDModifier();
                CalculateAPModifier();
                CalculateChemModifier();
            }
            else
            {
                CalculateChemModifier();
            }
        }


        /// <summary>
        /// Calculates damage according to current damageType
        /// </summary>
        public void CalculateDamageByType(DamageType dt)
        {
            if (dt == DamageType.Kinetic)
            {
                CalculateKineticDamage();
                CalculateAP();
            }
            else if (dt == DamageType.Emp)
            {
                CalculateEmpDamage();
            }
            else if (dt == DamageType.FlaK)
            {
                CalculateFlaKDamage();
            }
            else if (dt == DamageType.Frag)
            {
                CalculateFragDamage();
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
            else if (dt == DamageType.Pendepth)
            {
                CalculateKineticDamage();
                CalculateAP();
                CalculatePendepthDamage();
            }
        }


        public void CalculateDpsByType(DamageType dt, float targetAC, Scheme targetArmorScheme, int testInterval)
        {
            CalculateRecoil();
            CalculateChargerVolumeAndCost();
            CalculateRecoilVolumeAndCost();
            CalculateVariableVolumesAndCosts(testInterval);
            CalculateVolumeAndCostPerIntake();

            if (dt == DamageType.Kinetic)
            {
                CalculateAP();
                CalculateKineticDps(targetAC);
            }
            else if (dt == DamageType.Emp)
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
            else if (dt == DamageType.Pendepth)
            {
                CalculatePendepthDps(targetArmorScheme);
            }
        }

        public void CalculateDpsByTypeBelt(DamageType dt, float targetAC, Scheme targetArmorScheme)
        {
            CalculateRecoil();
            CalculateChargerVolumeAndCost();
            CalculateRecoilVolumeAndCost();
            CalculateVolumeAndCostPerIntake();

            if (dt == DamageType.Kinetic)
            {
                CalculateAP();
                CalculateKineticDpsBelt(targetAC);
            }
            else if (dt == DamageType.Emp)
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
            else if (dt == DamageType.Pendepth)
            {
                CalculatePendepthDpsBelt(targetArmorScheme);
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
            List<int> modsToShow)
        {
            if (labels)
            {
                writer.WriteLine("Gauge (mm): " + Gauge);
                writer.WriteLine("Total length (mm): " + TotalLength);
                writer.WriteLine("Length without casings: " + ProjectileLength);
                writer.WriteLine("Total modules: " + ModuleCountTotal);

                if (showGP)
                {
                    writer.WriteLine("GP Casing: " + GPCasingCount);
                }
                if (showRG)
                {
                    writer.WriteLine("RG Casing: " + RGCasingCount);
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

                if (dtToShow[DamageType.Kinetic])
                {
                    writer.WriteLine("Raw KD: " + KineticDamage);
                    writer.WriteLine("AP: " + ArmorPierce);
                }
                foreach (DamageType dt in dtToShow.Keys)
                {
                    if (dtToShow[dt])
                    {
                        writer.WriteLine((DamageType)(int)dt + " damage: " + DamageDict[dt]);
                    }
                }


                if (IsBelt)
                {
                    writer.WriteLine("Reload time: " + ReloadTimeBelt);
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
                    writer.WriteLine("Recoil volume: " + RecoilVolumeBelt);
                    writer.WriteLine("Ammo volume: " + AmmoCrateVolumeBelt);
                    writer.WriteLine("Storage volume: " + MatStorageVolumeBelt);
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
                    writer.WriteLine("Recoil cost: " + RecoilCostBelt);
                    writer.WriteLine("Shell cost: " + ShellCostBelt);
                    writer.WriteLine("Ammo cost: " + AmmoCrateCostBelt);
                    writer.WriteLine("Storage cost: " + MatStorageCostBelt);
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
                    writer.WriteLine("Reload time: " + ReloadTime);
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
                    writer.WriteLine("Recoil volume: " + RecoilVolume);
                    writer.WriteLine("Ammo volume: " + AmmoCrateVolume);
                    writer.WriteLine("Storage volume: " + MatStorageVolume);
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
                    writer.WriteLine("Recoil cost: " + RecoilCost);
                    writer.WriteLine("Shell cost: " + ShellCost);
                    writer.WriteLine("Ammo cost: " + AmmoCrateCost);
                    writer.WriteLine("Storage cost: " + MatStorageCost);
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

                if (dtToShow[DamageType.Kinetic])
                {
                    writer.WriteLine(KineticDamage);
                    writer.WriteLine(ArmorPierce);
                }
                foreach (DamageType dt in dtToShow.Keys)
                {
                    if (dtToShow[dt])
                    {
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
                    writer.WriteLine(RecoilVolumeBelt);
                    writer.WriteLine(AmmoCrateVolumeBelt);
                    writer.WriteLine(MatStorageVolumeBelt);
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
                    writer.WriteLine(RecoilCostBelt);
                    writer.WriteLine(ShellCostBelt);
                    writer.WriteLine(AmmoCrateCostBelt);
                    writer.WriteLine(MatStorageCostBelt);
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
                    writer.WriteLine(RecoilVolume);
                    writer.WriteLine(AmmoCrateVolume);
                    writer.WriteLine(MatStorageVolume);
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
                    writer.WriteLine(RecoilCost);
                    writer.WriteLine(ShellCost);
                    writer.WriteLine(AmmoCrateCost);
                    writer.WriteLine(MatStorageCost);
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
