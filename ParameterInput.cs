using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApsCalc;

namespace ApsCalcUI
{
    public partial class ParameterInput : Form
    {
        Dictionary<int, int> gaugeHardCaps = new()
        {
            { 1, 500 },
            { 2, 250 },
            { 3, 225 },
            { 4, 200 },
            { 5, 175 },
            { 6, 150 }
        };
        int gaugeHardCap = 500;
        List<ArmorLayerItem> layerItemList = new();

        public ParameterInput()
        {
            InitializeComponent();
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
        }

        private void ParameterInput_Load(object sender, EventArgs e)
        {
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
                new DamageTypeItem { ID = DamageType.Pendepth, Text = "Pendepth" },
                new DamageTypeItem { ID = DamageType.Disruptor, Text = "Disruptor" }
            };
            DamageTypeDD.DataSource = damageTypes;
            DamageTypeDD.DisplayMember = "Text";

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

            // Armor scheme list
            ArmorLayerLB.DisplayMember = "Name";
        }

        /// <summary>
        /// Set max gauge text according to barrel count
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarrelCountDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            gaugeHardCap = gaugeHardCaps[((BarrelCountItem)BarrelCountDD.SelectedValue).ID];
            MaxGaugeTB.Text = MathF.Min(Convert.ToInt32(MaxGaugeTB.Text), gaugeHardCap).ToString();
        }

        /// <summary>
        /// Min gauge must be >= 18 and <= 500
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinGaugeTB_LostFocus(object sender, EventArgs e)
        {
            if (Int32.TryParse(MinGaugeTB.Text, out int minGaugeInput))
            {
                if (minGaugeInput < 18)
                {
                    minGaugeErrorProvider.SetError(MinGaugeTB, "Min gauge must be >= 18 mm");
                }
                else if (minGaugeInput > 500)
                {
                    minGaugeErrorProvider.SetError(MinGaugeTB, "Min gauge must be <= 500 mm");
                }
                else
                {
                    minGaugeErrorProvider.Clear();
                }
            }
        }

        /// <summary>
        /// Min gauge must be >= min gauge and <= 500
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxGaugeTB_LostFocus(object sender, EventArgs e)
        {
            if (Int32.TryParse(MinGaugeTB.Text, out int minGaugeInput) && Int32.TryParse(MaxGaugeTB.Text, out int maxGaugeInput))
            {
                if (maxGaugeInput < 18)
                {
                    maxGaugeErrorProvider.SetError(MaxGaugeTB, "Max gauge must be >= 18 mm");
                }
                else if (maxGaugeInput < minGaugeInput)
                {
                    maxGaugeErrorProvider.SetError(MaxGaugeTB, "Max gauge must be >= min gauge");
                }
                else if (maxGaugeInput > 500)
                {
                    maxGaugeErrorProvider.SetError(MaxGaugeTB, "Max gauge must be <= 500 mm");
                }
                else
                {
                    maxGaugeErrorProvider.Clear();
                }
            }
        }

        /// <summary>
        /// Enable bore evacuator checkbox if any gunpowder is allowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxGPCB_OnTextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Activate target AC or target armor configuration panels when needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DamageTypeDD_SelectionChangeCommitted(object sender, EventArgs e)
        {

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
    }
}
