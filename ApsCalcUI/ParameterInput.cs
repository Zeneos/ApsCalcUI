using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApsCalc;

namespace ApsCalcUI
{
    public partial class ParameterInput : Form
    {
        List<TestParameters> parameterList = new();
        int testsInQueue = 0;

        readonly Dictionary<int, int> gaugeHardCaps = new()
        {
            { 1, 500 },
            { 2, 250 },
            { 3, 225 },
            { 4, 200 },
            { 5, 175 },
            { 6, 150 }
        };
        int gaugeHardCap = 500;

        public ParameterInput()
        {
            InitializeComponent();
        }

        private void ParameterInput_Load(object sender, EventArgs e)
        {
            // Barrel count dropdown
            BarrelCountItem[] barrelCounts = new[]
            {
                new BarrelCountItem { ID = 1, Text = "1, 500 mm max" },
                new BarrelCountItem { ID = 2, Text = "2, 250 mm max" },
                new BarrelCountItem { ID = 3, Text = "3, 225 mm max" },
                new BarrelCountItem { ID = 4, Text = "4, 200 mm max" },
                new BarrelCountItem { ID = 5, Text = "5, 175 mm max" },
                new BarrelCountItem { ID = 6, Text = "6, 150 mm max" }
            };
            BarrelCountDD.DataSource = barrelCounts;
            BarrelCountDD.DisplayMember = "Text";
            BarrelCountDD.SelectedIndex = 0;

            // Head module checked list
            foreach (HeadModuleItem head in HeadModuleItem.GenerateHeadList())
            {
                HeadModulesCL.Items.Add(head, false);
            }
            HeadModulesCL.DisplayMember = "Name";

            // Variable module checked list
            foreach (VariableModuleItem varMod in VariableModuleItem.GenerateBodyList())
            {
                VariableModulesCL.Items.Add(varMod, false);
            }
            VariableModulesCL.DisplayMember = "Name";

            // Damage type dropdown
            DamageTypeItem[] damageTypes = new[]
            {
                new DamageTypeItem { ID = DamageType.Kinetic, Text = "Kinetic" },
                new DamageTypeItem { ID = DamageType.Emp, Text = "EMP" },
                new DamageTypeItem { ID = DamageType.FlaK, Text = "FlaK" },
                new DamageTypeItem { ID = DamageType.Frag, Text = "Frag" },
                new DamageTypeItem { ID = DamageType.HE, Text = "HE" },
                new DamageTypeItem { ID = DamageType.HEAT, Text = "HEAT" },
                new DamageTypeItem { ID = DamageType.Pendepth, Text = "Pendepth" },
                new DamageTypeItem { ID = DamageType.Disruptor, Text = "Disruptor" }
            };
            DamageTypeDD.DataSource = damageTypes;
            DamageTypeDD.DisplayMember = "Text";
            DamageTypeDD.SelectedIndex = 0;

            // Target AC checked list
            TargetACCL.Items.Add(new TargetACItem { ID = 8, Text = "8, wood" });
            TargetACCL.Items.Add(new TargetACItem { ID = 9.6f, Text = "9.6, stacked wood" });
            TargetACCL.Items.Add(new TargetACItem { ID = 20, Text = "20, munitions" });
            TargetACCL.Items.Add(new TargetACItem { ID = 35, Text = "35, alloy" });
            TargetACCL.Items.Add(new TargetACItem { ID = 40, Text = "40, metal" });
            TargetACCL.Items.Add(new TargetACItem { ID = 42, Text = "42, stacked alloy" });
            TargetACCL.Items.Add(new TargetACItem { ID = 48, Text = "48, stacked metal" });
            TargetACCL.Items.Add(new TargetACItem { ID = 60, Text = "60, heavy armour" });
            TargetACCL.Items.Add(new TargetACItem { ID = 72, Text = "72, stacked heavy armour" });
            TargetACCL.DisplayMember = "Text";

            // Armor layer dropdown
            for (int index = 0; index < PenCalc.Layer.AllLayers.Length; index++)
            {
                ArmorLayerDD.Items.Add(new ArmorLayerItem
                {
                    Name = PenCalc.Layer.AllLayers[index].Name,
                    Layer = PenCalc.Layer.AllLayers[index]
                });
            }
            ArmorLayerDD.DisplayMember = "Name";
            ArmorLayerDD.SelectedIndex = 0;

            // Armor scheme list
            ArmorLayerLB.DisplayMember = "Name";
        }

        /// <summary>
        /// Updates max module counts in each category according to length and module count limits
        /// </summary>
        private void UpdateModuleCounts()
        {
            decimal maxFixedCount = Math.Min(20, Math.Floor(MaxLengthUD.Value / MinGaugeUD.Value)) - 1; // always have a head
            if (!NoBaseRB.Checked)
            {
                maxFixedCount -= 1;
            }

            decimal maxCasingCount;            

            SolidBodyFixedUD.Maximum = maxFixedCount;
            SabotBodyFixedUD.Maximum = SolidBodyFixedUD.Maximum - SolidBodyFixedUD.Value;
            EmpBodyFixedUD.Maximum = SabotBodyFixedUD.Maximum - SabotBodyFixedUD.Value;
            FlaKBodyFixedUD.Maximum = EmpBodyFixedUD.Maximum - EmpBodyFixedUD.Value;
            FragBodyFixedUD.Maximum = FlaKBodyFixedUD.Maximum - FlaKBodyFixedUD.Value;
            HEBodyFixedUD.Maximum = FragBodyFixedUD.Maximum - FragBodyFixedUD.Value;
            FuseFixedUD.Maximum = HEBodyFixedUD.Maximum - HEBodyFixedUD.Value;
            FinFixedUD.Maximum = FuseFixedUD.Maximum - FuseFixedUD.Value;
            GravCompFixedUD.Maximum = FinFixedUD.Maximum - FinFixedUD.Value;

            maxCasingCount = GravCompFixedUD.Maximum - GravCompFixedUD.Value;
            MaxGPUD.Maximum = maxCasingCount;
            MaxRGUD.Maximum = maxCasingCount;
        }


        /// <summary>
        /// Set max gauge according to barrel count
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarrelCountDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            gaugeHardCap = gaugeHardCaps[((BarrelCountItem)BarrelCountDD.SelectedValue).ID];
            MinGaugeUD.Maximum = gaugeHardCap;
            MaxGaugeUD.Maximum = gaugeHardCap;
            UpdateModuleCounts();
        }

        /// <summary>
        /// Ensures max gauge is never smaller than min gauge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinGaugeUD_ValueChanged(object sender, EventArgs e)
        {
            MaxGaugeUD.Minimum = MinGaugeUD.Value;
            UpdateModuleCounts();
        }

        private void MaxGaugeUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void SolidBodyFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void SabotBodyFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void EmpBodyFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void FlaKBodyFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void FragBodyFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void HEBodyFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void FuseFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void FinFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void GravCompFixedUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void NoBaseRB_CheckedChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }


        /// <summary>
        /// Enables bore evacuator checkbox if GP is allowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxGPUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void MaxRGUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }

        private void MaxLengthUD_ValueChanged(object sender, EventArgs e)
        {
            UpdateModuleCounts();
        }


        /// <summary>
        /// Activates AC or armor selection panels depending on damage type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DamageTypeDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DamageTypeItem)DamageTypeDD.SelectedItem).ID == DamageType.Kinetic)
            {
                TargetACPanel.Enabled = true;
            }
            else
            {
                TargetACPanel.Enabled = false;
            }

            if (((DamageTypeItem)DamageTypeDD.SelectedItem).ID == DamageType.Pendepth)
            {
                TargetSchemePanel.Enabled = true;
            }
            else
            {
                TargetSchemePanel.Enabled = false;
            }
        }


        private void AddLayerButton_Click(object sender, EventArgs e)
        {
            if (ArmorLayerDD.SelectedItem != null)
            {
                ArmorLayerLB.BeginUpdate();
                ArmorLayerLB.Items.Add((ArmorLayerItem)ArmorLayerDD.SelectedItem);
                ArmorLayerLB.EndUpdate();
            }
        }

        private void RemoveLayerButton_Click(object sender, EventArgs e)
        {
            if (ArmorLayerLB.Items.Count > 0)
            {
                ArmorLayerLB.BeginUpdate();
                ArmorLayerLB.Items.Remove(ArmorLayerLB.Items[^1]);
                ArmorLayerLB.EndUpdate();
            }
        }

        /// <summary>
        /// Validates input and creates test parameters from current selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddParametersButton_Click(object sender, EventArgs e)
        {
            bool error = false;
            QueueErrorProvider.Clear();

            // For checking chemical types for pendepth
            List<Module> varModList = new();
            foreach (VariableModuleItem varModItem in VariableModulesCL.CheckedItems)
            {
                varModList.Add(Module.AllModules[varModItem.Index]);
            }

            if (HeadModulesCL.CheckedItems.Count == 0)
            {
                error = true;
                QueueErrorProvider.SetError(AddParametersButton, "Select at least one head");
            }
            else if (VariableModulesCL.CheckedItems.Count == 0)
            {
                error = true;
                QueueErrorProvider.SetError(AddParametersButton, "Select at least one variable module");
            }
            else if (((DamageTypeItem)DamageTypeDD.SelectedItem).ID == DamageType.Kinetic && TargetACCL.SelectedItems.Count == 0)
            {
                error = true;
                QueueErrorProvider.SetError(AddParametersButton, "Select at least one target AC");
            }
            else if (((DamageTypeItem)DamageTypeDD.SelectedItem).ID == DamageType.Pendepth
                && FlaKBodyFixedUD.Value == 0
                && FragBodyFixedUD.Value == 0
                && HEBodyFixedUD.Value == 0
                && !varModList.Contains(Module.FlaKBody)
                && !varModList.Contains(Module.FragBody)
                && !varModList.Contains(Module.HEBody))
            {
                error = true;
                QueueErrorProvider.SetError(AddParametersButton, "Add FlaK, Frag, and/or HE body as a fixed or variable module for pendepth");
            }
            else if (((DamageTypeItem)DamageTypeDD.SelectedItem).ID == DamageType.Pendepth && ArmorLayerLB.Items == null)
            {
                error = true;
                QueueErrorProvider.SetError(AddParametersButton, "Add at least one layer to the target armor scheme");
            }

            if (!error)
            {
                // Run button is off by default
                RunButton.Enabled = true;

                // Update "Tests in Queue" text
                testsInQueue += 1;
                TestsInQueueLabel.Text = "Tests in Queue: " + testsInQueue.ToString();

                TestParameters testParameters = new();
                testParameters.BarrelCount = ((BarrelCountItem)BarrelCountDD.SelectedItem).ID;
                testParameters.MinGauge = (int)MinGaugeUD.Value;
                testParameters.MaxGauge = (int)MaxGaugeUD.Value;

                List<int> headIndices = new();
                foreach (HeadModuleItem head in HeadModulesCL.CheckedItems)
                {
                    headIndices.Add(head.Index);
                }
                testParameters.HeadIndices = headIndices;

                if (BaseBleederRB.Checked)
                {
                    testParameters.BaseModule = Module.BaseBleeder;
                }
                else if (SupercavRB.Checked)
                {
                    testParameters.BaseModule = Module.Supercav;
                }
                else if (TracerRB.Checked)
                {
                    testParameters.BaseModule = Module.Tracer;
                }
                else if (GravRB.Checked)
                {
                    testParameters.BaseModule = Module.GravRam;
                }
                else
                {
                    testParameters.BaseModule = null;
                }

                float[] fixedModuleCounts = new float[]
                {
                            (float)SolidBodyFixedUD.Value,
                            (float)SabotBodyFixedUD.Value,
                            (float)EmpBodyFixedUD.Value,
                            (float)FlaKBodyFixedUD.Value,
                            (float)FragBodyFixedUD.Value,
                            (float)HEBodyFixedUD.Value,
                            (float)FuseFixedUD.Value,
                            (float)FinFixedUD.Value,
                            (float)GravCompFixedUD.Value
                };
                testParameters.FixedModulecounts = fixedModuleCounts;

                float minModuleCount;
                if (NoBaseRB.Checked)
                {
                    minModuleCount = 1;
                }
                else
                {
                    minModuleCount = 2;
                }
                minModuleCount += (float)fixedModuleCounts.Sum();
                testParameters.MinModulecount = minModuleCount;

                List<int> varModIndices = new();
                foreach (VariableModuleItem varMod in VariableModulesCL.CheckedItems)
                {
                    varModIndices.Add(varMod.Index);
                }
                // Array must have 9 items. Duplicates of item 0 will be ignored by ShellCalc
                while (varModIndices.Count < 9)
                {
                    varModIndices.Add(varModIndices[0]);
                }
                int[] variableModuleIndices = varModIndices.ToArray();
                testParameters.VariableModuleIndices = variableModuleIndices;

                testParameters.MaxGPCasingCount = (float)MaxGPUD.Value;
                testParameters.MaxRGCasingCount = (float)MaxRGUD.Value;
                testParameters.MaxLength = (float)MaxLengthUD.Value;
                testParameters.MaxDraw = (float)MaxDrawUD.Value;
                testParameters.MaxRecoil = (float)MaxRecoilUD.Value;
                testParameters.MinVelocity = (float)MinVelocityUD.Value;
                testParameters.MinEffectiverange = (float)MinRangeUD.Value;
                testParameters.DamageType = ((DamageTypeItem)DamageTypeDD.SelectedItem).ID;

                if (testParameters.DamageType == DamageType.HEAT)
                {
                    // Overwrite head list with shaped charge head
                    testParameters.HeadIndices.Clear();
                    int modIndex = 0;
                    foreach (Module head in Module.AllModules)
                    {
                        if (head == Module.ShapedChargeHead)
                        {
                            testParameters.HeadIndices.Add(modIndex);
                            break;
                        }
                        modIndex++;
                    }
                }
                else if (testParameters.DamageType == DamageType.Disruptor)
                {
                    // Overwrite head list with disruptor conduit
                    testParameters.HeadIndices.Clear();
                    int modIndex = 0;
                    foreach (Module head in Module.AllModules)
                    {
                        if (head == Module.Disruptor)
                        {
                            testParameters.HeadIndices.Add(modIndex);
                            break;
                        }
                        modIndex++;
                    }
                }

                List<float> targetACList = new();
                if (testParameters.DamageType == DamageType.Kinetic)
                {
                    foreach (TargetACItem ac in TargetACCL.CheckedItems)
                    {
                        targetACList.Add(ac.ID);
                    }
                    testParameters.TargetACList = targetACList;
                }

                PenCalc.Scheme targetArmorScheme = new();
                if (testParameters.DamageType == DamageType.Pendepth)
                {
                    foreach (ArmorLayerItem layerItem in ArmorLayerLB.Items)
                    {
                        targetArmorScheme.LayerList.Add(layerItem.Layer);
                    }
                    targetArmorScheme.CalculateLayerAC();
                }
                testParameters.ArmorScheme = targetArmorScheme;

                if (PerVolumeRB.Checked)
                {
                    testParameters.TestType = 0;
                }
                else
                {
                    testParameters.TestType = 1;
                }

                if (LabelsCB.Checked)
                {
                    testParameters.Labels = true;
                }
                else
                {
                    testParameters.Labels = false;
                }
                testParameters.WriteToFile = true;

                parameterList.Add(testParameters);
            }
        }

        /// <summary>
        /// Runs tests for all parameters in queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunButton_Click(object sender, EventArgs e)
        {
            bool error = false;

            if (parameterList.Count == 0)
            {
                error = true;
                RunErrorProvider.SetError(RunButton, "Add parameters to queue");
            }

            if (!error)
            {
                // Lock buttons
                TestsInQueueLabel.Text = "Tests Remaining: " + testsInQueue.ToString();
                AddParametersButton.Enabled = false;
                RunButton.Enabled = false;
                RunButton.Text = "Running...";

                foreach (TestParameters testParameters in parameterList)
                {
                    if (testParameters.DamageType == DamageType.Kinetic)
                    {
                        foreach (float ac in testParameters.TargetACList)
                        {
                            ConcurrentBag<Shell> shellBag = new();
                            Parallel.For(testParameters.MinGauge, testParameters.MaxGauge + 1, gauge =>
                            {
                                float gaugeFloat = gauge;
                                ShellCalc calcLocal = new(
                                    testParameters.BarrelCount,
                                    gauge,
                                    testParameters.HeadIndices,
                                    testParameters.BaseModule,
                                    testParameters.FixedModulecounts,
                                    testParameters.MinModulecount,
                                    testParameters.VariableModuleIndices,
                                    testParameters.MaxGPCasingCount,
                                    testParameters.MaxRGCasingCount,
                                    testParameters.MaxLength,
                                    testParameters.MaxDraw,
                                    testParameters.MaxRecoil,
                                    testParameters.MinVelocity,
                                    testParameters.MinEffectiverange,
                                    ac,
                                    testParameters.DamageType,
                                    testParameters.ArmorScheme,
                                    testParameters.TestType,
                                    testParameters.Labels,
                                    testParameters.WriteToFile
                                    );


                                calcLocal.ShellTest();
                                calcLocal.AddTopShellsToLocalList();

                                foreach (Shell topShellLocal in calcLocal.TopShellsLocal)
                                {
                                    shellBag.Add(topShellLocal);
                                }
                            });

                            ShellCalc calcFinal = new(
                                    testParameters.BarrelCount,
                                    0f, // Gauge does not matter for calcFinal because it is only running tests on pre-calculated shells
                                    testParameters.HeadIndices,
                                    testParameters.BaseModule,
                                    testParameters.FixedModulecounts,
                                    testParameters.MinModulecount,
                                    testParameters.VariableModuleIndices,
                                    testParameters.MaxGPCasingCount,
                                    testParameters.MaxRGCasingCount,
                                    testParameters.MaxLength,
                                    testParameters.MaxDraw,
                                    testParameters.MaxRecoil,
                                    testParameters.MinVelocity,
                                    testParameters.MinEffectiverange,
                                    ac,
                                    testParameters.DamageType,
                                    testParameters.ArmorScheme,
                                    testParameters.TestType,
                                    testParameters.Labels,
                                    testParameters.WriteToFile
                                );

                            calcFinal.FindTopShellsInList(shellBag);
                            calcFinal.AddTopShellsToDictionary();
                            calcFinal.WriteTopShells(testParameters.MinGauge, testParameters.MaxGauge);
                        }
                    }
                    else
                    {
                        ConcurrentBag<Shell> shellBag = new();
                        Parallel.For(testParameters.MinGauge, testParameters.MaxGauge + 1, gauge =>
                        {
                            float gaugeFloat = gauge;
                            ShellCalc calcLocal = new(
                                testParameters.BarrelCount,
                                gauge,
                                testParameters.HeadIndices,
                                testParameters.BaseModule,
                                testParameters.FixedModulecounts,
                                testParameters.MinModulecount,
                                testParameters.VariableModuleIndices,
                                testParameters.MaxGPCasingCount,
                                testParameters.MaxRGCasingCount,
                                testParameters.MaxLength,
                                testParameters.MaxDraw,
                                testParameters.MaxRecoil,
                                testParameters.MinVelocity,
                                testParameters.MinEffectiverange,
                                0, // Target AC does not matter for non-kinetic tests
                                testParameters.DamageType,
                                testParameters.ArmorScheme,
                                testParameters.TestType,
                                testParameters.Labels,
                                testParameters.WriteToFile
                                );

                            calcLocal.ShellTest();
                            calcLocal.AddTopShellsToLocalList();

                            foreach (Shell topShellLocal in calcLocal.TopShellsLocal)
                            {
                                shellBag.Add(topShellLocal);
                            }
                        });

                        ShellCalc calcFinal = new(
                                testParameters.BarrelCount,
                                0f, // Gauge does not matter for calcFinal because it is only running tests on pre-calculated shells
                                testParameters.HeadIndices,
                                testParameters.BaseModule,
                                testParameters.FixedModulecounts,
                                testParameters.MinModulecount,
                                testParameters.VariableModuleIndices,
                                testParameters.MaxGPCasingCount,
                                testParameters.MaxRGCasingCount,
                                testParameters.MaxLength,
                                testParameters.MaxDraw,
                                testParameters.MaxRecoil,
                                testParameters.MinVelocity,
                                testParameters.MinEffectiverange,
                                0, // Target AC does not matter for non-kinetic tests
                                testParameters.DamageType,
                                testParameters.ArmorScheme,
                                testParameters.TestType,
                                testParameters.Labels,
                                testParameters.WriteToFile
                            );

                        calcFinal.FindTopShellsInList(shellBag);
                        calcFinal.AddTopShellsToDictionary();
                        calcFinal.WriteTopShells(testParameters.MinGauge, testParameters.MaxGauge);
                    }

                    testsInQueue -= 1;
                    TestsInQueueLabel.Text = "Tests Remaining: " + testsInQueue.ToString();
                }

                // Unlock buttons and update queue count
                parameterList.Clear();
                testsInQueue = 0;
                TestsInQueueLabel.Text = "Tests in Queue: " + testsInQueue.ToString();

                AddParametersButton.Enabled = true;
                RunButton.Enabled = true;
                RunButton.Text = "Run Queued Tests";
            }
        }
    }
}
