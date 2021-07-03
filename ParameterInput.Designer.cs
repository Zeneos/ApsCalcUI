
namespace ApsCalcUI
{
    partial class ParameterInput
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BarrelCountDD = new System.Windows.Forms.ComboBox();
            this.HeadModulesLabel = new System.Windows.Forms.Label();
            this.SupercavRB = new System.Windows.Forms.RadioButton();
            this.GravRB = new System.Windows.Forms.RadioButton();
            this.MinGaugeTB = new System.Windows.Forms.TextBox();
            this.MaxGaugeTB = new System.Windows.Forms.TextBox();
            this.SolidBodyFixedTB = new System.Windows.Forms.TextBox();
            this.SabotBodyFixedTB = new System.Windows.Forms.TextBox();
            this.EmpBodyFixedTB = new System.Windows.Forms.TextBox();
            this.FlaKBodyFixedTB = new System.Windows.Forms.TextBox();
            this.FragBodyFixedTB = new System.Windows.Forms.TextBox();
            this.HEBodyFixedTB = new System.Windows.Forms.TextBox();
            this.FuseFixedTB = new System.Windows.Forms.TextBox();
            this.FinFixedTB = new System.Windows.Forms.TextBox();
            this.GravCompFixedTB = new System.Windows.Forms.TextBox();
            this.MaxGPTB = new System.Windows.Forms.TextBox();
            this.MaxRailDrawTB = new System.Windows.Forms.TextBox();
            this.MaxRecoilTB = new System.Windows.Forms.TextBox();
            this.BoreEvacuatorCB = new System.Windows.Forms.CheckBox();
            this.MinRangeTB = new System.Windows.Forms.TextBox();
            this.DamageTypeDD = new System.Windows.Forms.ComboBox();
            this.PerCostRB = new System.Windows.Forms.RadioButton();
            this.PerVolumeRB = new System.Windows.Forms.RadioButton();
            this.ArmorLayerDD = new System.Windows.Forms.ComboBox();
            this.AddLayerButton = new System.Windows.Forms.Button();
            this.RemoveLayerButton = new System.Windows.Forms.Button();
            this.BarrelCountLabel = new System.Windows.Forms.Label();
            this.MinGaugeLabel = new System.Windows.Forms.Label();
            this.MaxGaugeLabel = new System.Windows.Forms.Label();
            this.HeadModulesCL = new System.Windows.Forms.CheckedListBox();
            this.BasePanel = new System.Windows.Forms.Panel();
            this.BaseModulesLabel = new System.Windows.Forms.Label();
            this.NoBaseRB = new System.Windows.Forms.RadioButton();
            this.TracerRB = new System.Windows.Forms.RadioButton();
            this.BaseBleederRB = new System.Windows.Forms.RadioButton();
            this.FixedModulesPanel = new System.Windows.Forms.Panel();
            this.GravCompFixedLabel = new System.Windows.Forms.Label();
            this.FinLabel = new System.Windows.Forms.Label();
            this.FuseFixedLabel = new System.Windows.Forms.Label();
            this.HEBodyFixedLabel = new System.Windows.Forms.Label();
            this.FragBodyFixedLabel = new System.Windows.Forms.Label();
            this.FlaKBodyFixedLabel = new System.Windows.Forms.Label();
            this.EmpBodyFixedLabel = new System.Windows.Forms.Label();
            this.SabotBodyFixedLabel = new System.Windows.Forms.Label();
            this.SolidBodyFixedLabel = new System.Windows.Forms.Label();
            this.FixedModulesLabel = new System.Windows.Forms.Label();
            this.VariableModulesPanel = new System.Windows.Forms.Panel();
            this.VariableModulesCL = new System.Windows.Forms.CheckedListBox();
            this.VariableModulesLabel = new System.Windows.Forms.Label();
            this.MaxGPLabel = new System.Windows.Forms.Label();
            this.MaxRGTB = new System.Windows.Forms.TextBox();
            this.MaxRGLabel = new System.Windows.Forms.Label();
            this.MaxDrawLabel = new System.Windows.Forms.Label();
            this.MaxRecoilLabel = new System.Windows.Forms.Label();
            this.MaxLengthTB = new System.Windows.Forms.TextBox();
            this.MaxLengthLabel = new System.Windows.Forms.Label();
            this.MinVelocityTB = new System.Windows.Forms.TextBox();
            this.MinVelocityLabel = new System.Windows.Forms.Label();
            this.MinRangeLabel = new System.Windows.Forms.Label();
            this.DamageTypeLabel = new System.Windows.Forms.Label();
            this.TestTypePanel = new System.Windows.Forms.Panel();
            this.PerLabel = new System.Windows.Forms.Label();
            this.TargetACPanel = new System.Windows.Forms.Panel();
            this.TargetACCL = new System.Windows.Forms.CheckedListBox();
            this.TargetACLabel = new System.Windows.Forms.Label();
            this.GaugeLabel = new System.Windows.Forms.Label();
            this.TargetSchemePanel = new System.Windows.Forms.Panel();
            this.ArmorLayerLB = new System.Windows.Forms.ListBox();
            this.TargetSchemeLabel = new System.Windows.Forms.Label();
            this.minGaugeErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.maxGaugeErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.BasePanel.SuspendLayout();
            this.FixedModulesPanel.SuspendLayout();
            this.VariableModulesPanel.SuspendLayout();
            this.TestTypePanel.SuspendLayout();
            this.TargetACPanel.SuspendLayout();
            this.TargetSchemePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minGaugeErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxGaugeErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // BarrelCountDD
            // 
            this.BarrelCountDD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BarrelCountDD.FormattingEnabled = true;
            this.BarrelCountDD.Location = new System.Drawing.Point(12, 31);
            this.BarrelCountDD.Name = "BarrelCountDD";
            this.BarrelCountDD.Size = new System.Drawing.Size(110, 23);
            this.BarrelCountDD.TabIndex = 1;
            this.ToolTip.SetToolTip(this.BarrelCountDD, "Number of barrels on the firing piece.\nMultiple barrels increase cooling effectiv" +
        "eness, but restrict max gauge.\nBarrels : Max gauge\n1 : 500\n2 : 250\n3 : 225\n4 : 2" +
        "00\n5 : 175\n6 : 150");
            this.BarrelCountDD.SelectedIndexChanged += new System.EventHandler(this.BarrelCountDD_SelectedIndexChanged);
            // 
            // HeadModulesLabel
            // 
            this.HeadModulesLabel.Location = new System.Drawing.Point(12, 78);
            this.HeadModulesLabel.Name = "HeadModulesLabel";
            this.HeadModulesLabel.Size = new System.Drawing.Size(205, 15);
            this.HeadModulesLabel.TabIndex = 7;
            this.HeadModulesLabel.Text = "Head Modules";
            this.HeadModulesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTip.SetToolTip(this.HeadModulesLabel, "Module at front of shell. A complete test will be run for each selected module.");
            // 
            // SupercavRB
            // 
            this.SupercavRB.AutoSize = true;
            this.SupercavRB.Location = new System.Drawing.Point(112, 50);
            this.SupercavRB.Name = "SupercavRB";
            this.SupercavRB.Size = new System.Drawing.Size(73, 19);
            this.SupercavRB.TabIndex = 1;
            this.SupercavRB.Text = "Supercav";
            this.ToolTip.SetToolTip(this.SupercavRB, "Supercavitation Base");
            this.SupercavRB.UseVisualStyleBackColor = true;
            // 
            // GravRB
            // 
            this.GravRB.AutoSize = true;
            this.GravRB.Location = new System.Drawing.Point(112, 72);
            this.GravRB.Name = "GravRB";
            this.GravRB.Size = new System.Drawing.Size(76, 19);
            this.GravRB.TabIndex = 3;
            this.GravRB.Text = "Grav Ram";
            this.ToolTip.SetToolTip(this.GravRB, "Graviton Ram");
            this.GravRB.UseVisualStyleBackColor = true;
            // 
            // MinGaugeTB
            // 
            this.MinGaugeTB.Location = new System.Drawing.Point(152, 43);
            this.MinGaugeTB.Name = "MinGaugeTB";
            this.MinGaugeTB.Size = new System.Drawing.Size(25, 23);
            this.MinGaugeTB.TabIndex = 4;
            this.MinGaugeTB.Text = "18";
            this.MinGaugeTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.MinGaugeTB, "Smallest gauge to test, in mm.\nMinimum 18.");
            this.MinGaugeTB.TextChanged += new System.EventHandler(this.MinGaugeTB_LostFocus);
            // 
            // MaxGaugeTB
            // 
            this.MaxGaugeTB.Location = new System.Drawing.Point(192, 43);
            this.MaxGaugeTB.Name = "MaxGaugeTB";
            this.MaxGaugeTB.Size = new System.Drawing.Size(25, 23);
            this.MaxGaugeTB.TabIndex = 6;
            this.MaxGaugeTB.Text = "500";
            this.MaxGaugeTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.MaxGaugeTB, "Largest gauge to test, in mm.\nMaximum 500.");
            // 
            // SolidBodyFixedTB
            // 
            this.SolidBodyFixedTB.Location = new System.Drawing.Point(125, 28);
            this.SolidBodyFixedTB.Name = "SolidBodyFixedTB";
            this.SolidBodyFixedTB.Size = new System.Drawing.Size(25, 23);
            this.SolidBodyFixedTB.TabIndex = 2;
            this.SolidBodyFixedTB.Text = "0";
            this.SolidBodyFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.SolidBodyFixedTB, "Minimum solid bodies to include in every shell");
            // 
            // SabotBodyFixedTB
            // 
            this.SabotBodyFixedTB.Location = new System.Drawing.Point(125, 53);
            this.SabotBodyFixedTB.Name = "SabotBodyFixedTB";
            this.SabotBodyFixedTB.Size = new System.Drawing.Size(25, 23);
            this.SabotBodyFixedTB.TabIndex = 4;
            this.SabotBodyFixedTB.Text = "0";
            this.SabotBodyFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.SabotBodyFixedTB, "Minimum sabot bodies to include in every shell");
            // 
            // EmpBodyFixedTB
            // 
            this.EmpBodyFixedTB.Location = new System.Drawing.Point(125, 78);
            this.EmpBodyFixedTB.Name = "EmpBodyFixedTB";
            this.EmpBodyFixedTB.Size = new System.Drawing.Size(25, 23);
            this.EmpBodyFixedTB.TabIndex = 6;
            this.EmpBodyFixedTB.Text = "0";
            this.EmpBodyFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.EmpBodyFixedTB, "Minimum EMP bodies to include in every shell");
            // 
            // FlaKBodyFixedTB
            // 
            this.FlaKBodyFixedTB.Location = new System.Drawing.Point(125, 103);
            this.FlaKBodyFixedTB.Name = "FlaKBodyFixedTB";
            this.FlaKBodyFixedTB.Size = new System.Drawing.Size(25, 23);
            this.FlaKBodyFixedTB.TabIndex = 13;
            this.FlaKBodyFixedTB.Text = "0";
            this.FlaKBodyFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.FlaKBodyFixedTB, "Minimum FlaK bodies to include in every shell");
            // 
            // FragBodyFixedTB
            // 
            this.FragBodyFixedTB.Location = new System.Drawing.Point(125, 128);
            this.FragBodyFixedTB.Name = "FragBodyFixedTB";
            this.FragBodyFixedTB.Size = new System.Drawing.Size(25, 23);
            this.FragBodyFixedTB.TabIndex = 15;
            this.FragBodyFixedTB.Text = "0";
            this.FragBodyFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.FragBodyFixedTB, "Minimum Frag bodies to include in every shell");
            // 
            // HEBodyFixedTB
            // 
            this.HEBodyFixedTB.Location = new System.Drawing.Point(125, 153);
            this.HEBodyFixedTB.Name = "HEBodyFixedTB";
            this.HEBodyFixedTB.Size = new System.Drawing.Size(25, 23);
            this.HEBodyFixedTB.TabIndex = 17;
            this.HEBodyFixedTB.Text = "0";
            this.HEBodyFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.HEBodyFixedTB, "Minimum HE bodies to include in every shell");
            // 
            // FuseFixedTB
            // 
            this.FuseFixedTB.Location = new System.Drawing.Point(125, 178);
            this.FuseFixedTB.Name = "FuseFixedTB";
            this.FuseFixedTB.Size = new System.Drawing.Size(25, 23);
            this.FuseFixedTB.TabIndex = 19;
            this.FuseFixedTB.Text = "0";
            this.FuseFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.FuseFixedTB, "Minimum fuses to include in every shell. Includes Emergency ejection defuse");
            // 
            // FinFixedTB
            // 
            this.FinFixedTB.Location = new System.Drawing.Point(125, 203);
            this.FinFixedTB.Name = "FinFixedTB";
            this.FinFixedTB.Size = new System.Drawing.Size(25, 23);
            this.FinFixedTB.TabIndex = 21;
            this.FinFixedTB.Text = "0";
            this.FinFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.FinFixedTB, "Minimum Stabilizer fin bodies to include in every shell");
            // 
            // GravCompFixedTB
            // 
            this.GravCompFixedTB.Location = new System.Drawing.Point(125, 228);
            this.GravCompFixedTB.Name = "GravCompFixedTB";
            this.GravCompFixedTB.Size = new System.Drawing.Size(25, 23);
            this.GravCompFixedTB.TabIndex = 23;
            this.GravCompFixedTB.Text = "0";
            this.GravCompFixedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.GravCompFixedTB, "Minimum Gravity compensators to include in every shell");
            // 
            // MaxGPTB
            // 
            this.MaxGPTB.Location = new System.Drawing.Point(560, 35);
            this.MaxGPTB.Name = "MaxGPTB";
            this.MaxGPTB.Size = new System.Drawing.Size(50, 23);
            this.MaxGPTB.TabIndex = 14;
            this.MaxGPTB.Text = "0";
            this.MaxGPTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.MaxGPTB, "Up to two decimal places");
            // 
            // MaxRailDrawTB
            // 
            this.MaxRailDrawTB.Location = new System.Drawing.Point(560, 85);
            this.MaxRailDrawTB.Name = "MaxRailDrawTB";
            this.MaxRailDrawTB.Size = new System.Drawing.Size(50, 23);
            this.MaxRailDrawTB.TabIndex = 18;
            this.MaxRailDrawTB.Text = "0";
            this.MaxRailDrawTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.MaxRailDrawTB, "Max 200,000");
            // 
            // MaxRecoilTB
            // 
            this.MaxRecoilTB.Location = new System.Drawing.Point(560, 110);
            this.MaxRecoilTB.Name = "MaxRecoilTB";
            this.MaxRecoilTB.Size = new System.Drawing.Size(50, 23);
            this.MaxRecoilTB.TabIndex = 20;
            this.MaxRecoilTB.Text = "250000";
            this.MaxRecoilTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.MaxRecoilTB, "Max total recoil. Includes gunpowder and railgun recoil");
            // 
            // BoreEvacuatorCB
            // 
            this.BoreEvacuatorCB.AutoSize = true;
            this.BoreEvacuatorCB.Checked = true;
            this.BoreEvacuatorCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BoreEvacuatorCB.Enabled = false;
            this.BoreEvacuatorCB.Location = new System.Drawing.Point(617, 38);
            this.BoreEvacuatorCB.Name = "BoreEvacuatorCB";
            this.BoreEvacuatorCB.Size = new System.Drawing.Size(127, 19);
            this.BoreEvacuatorCB.TabIndex = 21;
            this.BoreEvacuatorCB.Text = "Use Bore Evacuator";
            this.ToolTip.SetToolTip(this.BoreEvacuatorCB, "Bore evacuator is a barrel attachment which improves cooling");
            this.BoreEvacuatorCB.UseVisualStyleBackColor = true;
            // 
            // MinRangeTB
            // 
            this.MinRangeTB.Location = new System.Drawing.Point(741, 110);
            this.MinRangeTB.Name = "MinRangeTB";
            this.MinRangeTB.Size = new System.Drawing.Size(50, 23);
            this.MinRangeTB.TabIndex = 27;
            this.MinRangeTB.Text = "0";
            this.MinRangeTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.MinRangeTB, "Minimum effective range");
            // 
            // DamageTypeDD
            // 
            this.DamageTypeDD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DamageTypeDD.FormattingEnabled = true;
            this.DamageTypeDD.Location = new System.Drawing.Point(504, 164);
            this.DamageTypeDD.Name = "DamageTypeDD";
            this.DamageTypeDD.Size = new System.Drawing.Size(121, 23);
            this.DamageTypeDD.TabIndex = 28;
            this.ToolTip.SetToolTip(this.DamageTypeDD, "Damage type to optimize");
            // 
            // PerCostRB
            // 
            this.PerCostRB.AutoSize = true;
            this.PerCostRB.Location = new System.Drawing.Point(4, 26);
            this.PerCostRB.Name = "PerCostRB";
            this.PerCostRB.Size = new System.Drawing.Size(49, 19);
            this.PerCostRB.TabIndex = 1;
            this.PerCostRB.Text = "Cost";
            this.ToolTip.SetToolTip(this.PerCostRB, "Optimize DPS per cost");
            this.PerCostRB.UseVisualStyleBackColor = true;
            // 
            // PerVolumeRB
            // 
            this.PerVolumeRB.AutoSize = true;
            this.PerVolumeRB.Checked = true;
            this.PerVolumeRB.Location = new System.Drawing.Point(4, 3);
            this.PerVolumeRB.Name = "PerVolumeRB";
            this.PerVolumeRB.Size = new System.Drawing.Size(65, 19);
            this.PerVolumeRB.TabIndex = 0;
            this.PerVolumeRB.TabStop = true;
            this.PerVolumeRB.Text = "Volume";
            this.ToolTip.SetToolTip(this.PerVolumeRB, "Optimize DPS per volume");
            this.PerVolumeRB.UseVisualStyleBackColor = true;
            // 
            // ArmorLayerDD
            // 
            this.ArmorLayerDD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ArmorLayerDD.FormattingEnabled = true;
            this.ArmorLayerDD.Location = new System.Drawing.Point(6, 33);
            this.ArmorLayerDD.Name = "ArmorLayerDD";
            this.ArmorLayerDD.Size = new System.Drawing.Size(121, 23);
            this.ArmorLayerDD.TabIndex = 2;
            this.ToolTip.SetToolTip(this.ArmorLayerDD, "Select an armor type to add to target armor scheme.\r\nAdd outermost layer first");
            // 
            // AddLayerButton
            // 
            this.AddLayerButton.Location = new System.Drawing.Point(6, 58);
            this.AddLayerButton.Name = "AddLayerButton";
            this.AddLayerButton.Size = new System.Drawing.Size(50, 23);
            this.AddLayerButton.TabIndex = 3;
            this.AddLayerButton.Text = "Add";
            this.ToolTip.SetToolTip(this.AddLayerButton, "Add selected layer to list");
            this.AddLayerButton.UseVisualStyleBackColor = true;
            this.AddLayerButton.Click += new System.EventHandler(this.AddLayerButton_Click);
            // 
            // RemoveLayerButton
            // 
            this.RemoveLayerButton.Location = new System.Drawing.Point(77, 58);
            this.RemoveLayerButton.Name = "RemoveLayerButton";
            this.RemoveLayerButton.Size = new System.Drawing.Size(50, 23);
            this.RemoveLayerButton.TabIndex = 4;
            this.RemoveLayerButton.Text = "Del";
            this.ToolTip.SetToolTip(this.RemoveLayerButton, "Remove most recently-added layer");
            this.RemoveLayerButton.UseVisualStyleBackColor = true;
            this.RemoveLayerButton.Click += new System.EventHandler(this.RemoveLayerButton_Click);
            // 
            // BarrelCountLabel
            // 
            this.BarrelCountLabel.AutoSize = true;
            this.BarrelCountLabel.Location = new System.Drawing.Point(10, 13);
            this.BarrelCountLabel.Name = "BarrelCountLabel";
            this.BarrelCountLabel.Size = new System.Drawing.Size(73, 15);
            this.BarrelCountLabel.TabIndex = 0;
            this.BarrelCountLabel.Text = "Barrel Count";
            // 
            // MinGaugeLabel
            // 
            this.MinGaugeLabel.AutoSize = true;
            this.MinGaugeLabel.Location = new System.Drawing.Point(150, 25);
            this.MinGaugeLabel.Name = "MinGaugeLabel";
            this.MinGaugeLabel.Size = new System.Drawing.Size(28, 15);
            this.MinGaugeLabel.TabIndex = 3;
            this.MinGaugeLabel.Text = "Min";
            // 
            // MaxGaugeLabel
            // 
            this.MaxGaugeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxGaugeLabel.AutoSize = true;
            this.MaxGaugeLabel.Location = new System.Drawing.Point(190, 25);
            this.MaxGaugeLabel.Name = "MaxGaugeLabel";
            this.MaxGaugeLabel.Size = new System.Drawing.Size(30, 15);
            this.MaxGaugeLabel.TabIndex = 5;
            this.MaxGaugeLabel.Text = "Max";
            // 
            // HeadModulesCL
            // 
            this.HeadModulesCL.FormattingEnabled = true;
            this.HeadModulesCL.Location = new System.Drawing.Point(13, 97);
            this.HeadModulesCL.Name = "HeadModulesCL";
            this.HeadModulesCL.Size = new System.Drawing.Size(205, 364);
            this.HeadModulesCL.TabIndex = 8;
            // 
            // BasePanel
            // 
            this.BasePanel.Controls.Add(this.BaseModulesLabel);
            this.BasePanel.Controls.Add(this.NoBaseRB);
            this.BasePanel.Controls.Add(this.GravRB);
            this.BasePanel.Controls.Add(this.TracerRB);
            this.BasePanel.Controls.Add(this.SupercavRB);
            this.BasePanel.Controls.Add(this.BaseBleederRB);
            this.BasePanel.Location = new System.Drawing.Point(12, 469);
            this.BasePanel.Name = "BasePanel";
            this.BasePanel.Size = new System.Drawing.Size(206, 97);
            this.BasePanel.TabIndex = 10;
            // 
            // BaseModulesLabel
            // 
            this.BaseModulesLabel.Location = new System.Drawing.Point(0, 0);
            this.BaseModulesLabel.Name = "BaseModulesLabel";
            this.BaseModulesLabel.Size = new System.Drawing.Size(206, 27);
            this.BaseModulesLabel.TabIndex = 5;
            this.BaseModulesLabel.Text = "Base Modules";
            this.BaseModulesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NoBaseRB
            // 
            this.NoBaseRB.AutoSize = true;
            this.NoBaseRB.Location = new System.Drawing.Point(51, 28);
            this.NoBaseRB.Name = "NoBaseRB";
            this.NoBaseRB.Size = new System.Drawing.Size(108, 19);
            this.NoBaseRB.TabIndex = 4;
            this.NoBaseRB.Text = "No Special Base";
            this.NoBaseRB.UseVisualStyleBackColor = true;
            // 
            // TracerRB
            // 
            this.TracerRB.AutoSize = true;
            this.TracerRB.Location = new System.Drawing.Point(14, 72);
            this.TracerRB.Name = "TracerRB";
            this.TracerRB.Size = new System.Drawing.Size(93, 19);
            this.TracerRB.TabIndex = 2;
            this.TracerRB.Text = "Visible Tracer";
            this.TracerRB.UseVisualStyleBackColor = true;
            // 
            // BaseBleederRB
            // 
            this.BaseBleederRB.AutoSize = true;
            this.BaseBleederRB.Checked = true;
            this.BaseBleederRB.Location = new System.Drawing.Point(14, 51);
            this.BaseBleederRB.Name = "BaseBleederRB";
            this.BaseBleederRB.Size = new System.Drawing.Size(91, 19);
            this.BaseBleederRB.TabIndex = 0;
            this.BaseBleederRB.TabStop = true;
            this.BaseBleederRB.Text = "Base Bleeder";
            this.BaseBleederRB.UseVisualStyleBackColor = true;
            // 
            // FixedModulesPanel
            // 
            this.FixedModulesPanel.Controls.Add(this.GravCompFixedTB);
            this.FixedModulesPanel.Controls.Add(this.GravCompFixedLabel);
            this.FixedModulesPanel.Controls.Add(this.FinFixedTB);
            this.FixedModulesPanel.Controls.Add(this.FinLabel);
            this.FixedModulesPanel.Controls.Add(this.FuseFixedTB);
            this.FixedModulesPanel.Controls.Add(this.FuseFixedLabel);
            this.FixedModulesPanel.Controls.Add(this.HEBodyFixedTB);
            this.FixedModulesPanel.Controls.Add(this.HEBodyFixedLabel);
            this.FixedModulesPanel.Controls.Add(this.FragBodyFixedTB);
            this.FixedModulesPanel.Controls.Add(this.FragBodyFixedLabel);
            this.FixedModulesPanel.Controls.Add(this.FlaKBodyFixedTB);
            this.FixedModulesPanel.Controls.Add(this.EmpBodyFixedTB);
            this.FixedModulesPanel.Controls.Add(this.FlaKBodyFixedLabel);
            this.FixedModulesPanel.Controls.Add(this.EmpBodyFixedLabel);
            this.FixedModulesPanel.Controls.Add(this.SabotBodyFixedTB);
            this.FixedModulesPanel.Controls.Add(this.SabotBodyFixedLabel);
            this.FixedModulesPanel.Controls.Add(this.SolidBodyFixedTB);
            this.FixedModulesPanel.Controls.Add(this.SolidBodyFixedLabel);
            this.FixedModulesPanel.Controls.Add(this.FixedModulesLabel);
            this.FixedModulesPanel.Location = new System.Drawing.Point(255, 12);
            this.FixedModulesPanel.Name = "FixedModulesPanel";
            this.FixedModulesPanel.Size = new System.Drawing.Size(154, 257);
            this.FixedModulesPanel.TabIndex = 11;
            // 
            // GravCompFixedLabel
            // 
            this.GravCompFixedLabel.AutoSize = true;
            this.GravCompFixedLabel.Location = new System.Drawing.Point(4, 231);
            this.GravCompFixedLabel.Name = "GravCompFixedLabel";
            this.GravCompFixedLabel.Size = new System.Drawing.Size(119, 15);
            this.GravCompFixedLabel.TabIndex = 22;
            this.GravCompFixedLabel.Text = "Gravity Compensator";
            // 
            // FinLabel
            // 
            this.FinLabel.AutoSize = true;
            this.FinLabel.Location = new System.Drawing.Point(4, 206);
            this.FinLabel.Name = "FinLabel";
            this.FinLabel.Size = new System.Drawing.Size(73, 15);
            this.FinLabel.TabIndex = 20;
            this.FinLabel.Text = "Stabilizer Fin";
            // 
            // FuseFixedLabel
            // 
            this.FuseFixedLabel.AutoSize = true;
            this.FuseFixedLabel.Location = new System.Drawing.Point(4, 181);
            this.FuseFixedLabel.Name = "FuseFixedLabel";
            this.FuseFixedLabel.Size = new System.Drawing.Size(31, 15);
            this.FuseFixedLabel.TabIndex = 18;
            this.FuseFixedLabel.Text = "Fuse";
            // 
            // HEBodyFixedLabel
            // 
            this.HEBodyFixedLabel.AutoSize = true;
            this.HEBodyFixedLabel.Location = new System.Drawing.Point(4, 156);
            this.HEBodyFixedLabel.Name = "HEBodyFixedLabel";
            this.HEBodyFixedLabel.Size = new System.Drawing.Size(52, 15);
            this.HEBodyFixedLabel.TabIndex = 16;
            this.HEBodyFixedLabel.Text = "HE Body";
            // 
            // FragBodyFixedLabel
            // 
            this.FragBodyFixedLabel.AutoSize = true;
            this.FragBodyFixedLabel.Location = new System.Drawing.Point(4, 131);
            this.FragBodyFixedLabel.Name = "FragBodyFixedLabel";
            this.FragBodyFixedLabel.Size = new System.Drawing.Size(60, 15);
            this.FragBodyFixedLabel.TabIndex = 14;
            this.FragBodyFixedLabel.Text = "Frag Body";
            // 
            // FlaKBodyFixedLabel
            // 
            this.FlaKBodyFixedLabel.AutoSize = true;
            this.FlaKBodyFixedLabel.Location = new System.Drawing.Point(4, 106);
            this.FlaKBodyFixedLabel.Name = "FlaKBodyFixedLabel";
            this.FlaKBodyFixedLabel.Size = new System.Drawing.Size(59, 15);
            this.FlaKBodyFixedLabel.TabIndex = 12;
            this.FlaKBodyFixedLabel.Text = "FlaK Body";
            // 
            // EmpBodyFixedLabel
            // 
            this.EmpBodyFixedLabel.AutoSize = true;
            this.EmpBodyFixedLabel.Location = new System.Drawing.Point(4, 81);
            this.EmpBodyFixedLabel.Name = "EmpBodyFixedLabel";
            this.EmpBodyFixedLabel.Size = new System.Drawing.Size(61, 15);
            this.EmpBodyFixedLabel.TabIndex = 5;
            this.EmpBodyFixedLabel.Text = "EMP Body";
            // 
            // SabotBodyFixedLabel
            // 
            this.SabotBodyFixedLabel.AutoSize = true;
            this.SabotBodyFixedLabel.Location = new System.Drawing.Point(4, 56);
            this.SabotBodyFixedLabel.Name = "SabotBodyFixedLabel";
            this.SabotBodyFixedLabel.Size = new System.Drawing.Size(67, 15);
            this.SabotBodyFixedLabel.TabIndex = 3;
            this.SabotBodyFixedLabel.Text = "Sabot Body";
            // 
            // SolidBodyFixedLabel
            // 
            this.SolidBodyFixedLabel.AutoSize = true;
            this.SolidBodyFixedLabel.Location = new System.Drawing.Point(4, 31);
            this.SolidBodyFixedLabel.Name = "SolidBodyFixedLabel";
            this.SolidBodyFixedLabel.Size = new System.Drawing.Size(63, 15);
            this.SolidBodyFixedLabel.TabIndex = 1;
            this.SolidBodyFixedLabel.Text = "Solid Body";
            // 
            // FixedModulesLabel
            // 
            this.FixedModulesLabel.Location = new System.Drawing.Point(0, 0);
            this.FixedModulesLabel.Name = "FixedModulesLabel";
            this.FixedModulesLabel.Size = new System.Drawing.Size(154, 27);
            this.FixedModulesLabel.TabIndex = 0;
            this.FixedModulesLabel.Text = "Fixed Modules";
            this.FixedModulesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VariableModulesPanel
            // 
            this.VariableModulesPanel.Controls.Add(this.VariableModulesCL);
            this.VariableModulesPanel.Controls.Add(this.VariableModulesLabel);
            this.VariableModulesPanel.Location = new System.Drawing.Point(255, 276);
            this.VariableModulesPanel.Name = "VariableModulesPanel";
            this.VariableModulesPanel.Size = new System.Drawing.Size(154, 207);
            this.VariableModulesPanel.TabIndex = 12;
            // 
            // VariableModulesCL
            // 
            this.VariableModulesCL.FormattingEnabled = true;
            this.VariableModulesCL.Location = new System.Drawing.Point(4, 31);
            this.VariableModulesCL.Name = "VariableModulesCL";
            this.VariableModulesCL.Size = new System.Drawing.Size(147, 166);
            this.VariableModulesCL.TabIndex = 1;
            // 
            // VariableModulesLabel
            // 
            this.VariableModulesLabel.Location = new System.Drawing.Point(0, 0);
            this.VariableModulesLabel.Name = "VariableModulesLabel";
            this.VariableModulesLabel.Size = new System.Drawing.Size(154, 27);
            this.VariableModulesLabel.TabIndex = 0;
            this.VariableModulesLabel.Text = "Variable Modules";
            this.VariableModulesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MaxGPLabel
            // 
            this.MaxGPLabel.AutoSize = true;
            this.MaxGPLabel.Location = new System.Drawing.Point(446, 38);
            this.MaxGPLabel.Name = "MaxGPLabel";
            this.MaxGPLabel.Size = new System.Drawing.Size(92, 15);
            this.MaxGPLabel.TabIndex = 13;
            this.MaxGPLabel.Text = "Max GP Casings";
            // 
            // MaxRGTB
            // 
            this.MaxRGTB.Location = new System.Drawing.Point(560, 60);
            this.MaxRGTB.Name = "MaxRGTB";
            this.MaxRGTB.Size = new System.Drawing.Size(50, 23);
            this.MaxRGTB.TabIndex = 16;
            this.MaxRGTB.Text = "0";
            this.MaxRGTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MaxRGLabel
            // 
            this.MaxRGLabel.AutoSize = true;
            this.MaxRGLabel.Location = new System.Drawing.Point(446, 63);
            this.MaxRGLabel.Name = "MaxRGLabel";
            this.MaxRGLabel.Size = new System.Drawing.Size(92, 15);
            this.MaxRGLabel.TabIndex = 15;
            this.MaxRGLabel.Text = "Max RG Casings";
            // 
            // MaxDrawLabel
            // 
            this.MaxDrawLabel.AutoSize = true;
            this.MaxDrawLabel.Location = new System.Drawing.Point(446, 88);
            this.MaxDrawLabel.Name = "MaxDrawLabel";
            this.MaxDrawLabel.Size = new System.Drawing.Size(82, 15);
            this.MaxDrawLabel.TabIndex = 17;
            this.MaxDrawLabel.Text = "Max Rail Draw";
            // 
            // MaxRecoilLabel
            // 
            this.MaxRecoilLabel.AutoSize = true;
            this.MaxRecoilLabel.Location = new System.Drawing.Point(446, 113);
            this.MaxRecoilLabel.Name = "MaxRecoilLabel";
            this.MaxRecoilLabel.Size = new System.Drawing.Size(65, 15);
            this.MaxRecoilLabel.TabIndex = 19;
            this.MaxRecoilLabel.Text = "Max Recoil";
            // 
            // MaxLengthTB
            // 
            this.MaxLengthTB.Location = new System.Drawing.Point(741, 60);
            this.MaxLengthTB.Name = "MaxLengthTB";
            this.MaxLengthTB.Size = new System.Drawing.Size(50, 23);
            this.MaxLengthTB.TabIndex = 23;
            this.MaxLengthTB.Text = "8000";
            this.MaxLengthTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MaxLengthLabel
            // 
            this.MaxLengthLabel.AutoSize = true;
            this.MaxLengthLabel.Location = new System.Drawing.Point(627, 63);
            this.MaxLengthLabel.Name = "MaxLengthLabel";
            this.MaxLengthLabel.Size = new System.Drawing.Size(103, 15);
            this.MaxLengthLabel.TabIndex = 22;
            this.MaxLengthLabel.Text = "Max Length (mm)";
            // 
            // MinVelocityTB
            // 
            this.MinVelocityTB.Location = new System.Drawing.Point(741, 85);
            this.MinVelocityTB.Name = "MinVelocityTB";
            this.MinVelocityTB.Size = new System.Drawing.Size(50, 23);
            this.MinVelocityTB.TabIndex = 25;
            this.MinVelocityTB.Text = "0";
            this.MinVelocityTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MinVelocityLabel
            // 
            this.MinVelocityLabel.AutoSize = true;
            this.MinVelocityLabel.Location = new System.Drawing.Point(627, 88);
            this.MinVelocityLabel.Name = "MinVelocityLabel";
            this.MinVelocityLabel.Size = new System.Drawing.Size(104, 15);
            this.MinVelocityLabel.TabIndex = 24;
            this.MinVelocityLabel.Text = "Min Velocity (m/s)";
            // 
            // MinRangeLabel
            // 
            this.MinRangeLabel.AutoSize = true;
            this.MinRangeLabel.Location = new System.Drawing.Point(627, 113);
            this.MinRangeLabel.Name = "MinRangeLabel";
            this.MinRangeLabel.Size = new System.Drawing.Size(106, 15);
            this.MinRangeLabel.TabIndex = 26;
            this.MinRangeLabel.Text = "Min Eff. Range (m)";
            // 
            // DamageTypeLabel
            // 
            this.DamageTypeLabel.AutoSize = true;
            this.DamageTypeLabel.Location = new System.Drawing.Point(446, 167);
            this.DamageTypeLabel.Name = "DamageTypeLabel";
            this.DamageTypeLabel.Size = new System.Drawing.Size(55, 15);
            this.DamageTypeLabel.TabIndex = 29;
            this.DamageTypeLabel.Text = "Optimize";
            // 
            // TestTypePanel
            // 
            this.TestTypePanel.Controls.Add(this.PerCostRB);
            this.TestTypePanel.Controls.Add(this.PerVolumeRB);
            this.TestTypePanel.Location = new System.Drawing.Point(682, 153);
            this.TestTypePanel.Name = "TestTypePanel";
            this.TestTypePanel.Size = new System.Drawing.Size(106, 48);
            this.TestTypePanel.TabIndex = 30;
            // 
            // PerLabel
            // 
            this.PerLabel.AutoSize = true;
            this.PerLabel.Location = new System.Drawing.Point(629, 167);
            this.PerLabel.Name = "PerLabel";
            this.PerLabel.Size = new System.Drawing.Size(48, 15);
            this.PerLabel.TabIndex = 31;
            this.PerLabel.Text = "DPS per";
            // 
            // TargetACPanel
            // 
            this.TargetACPanel.Controls.Add(this.TargetACCL);
            this.TargetACPanel.Controls.Add(this.TargetACLabel);
            this.TargetACPanel.Location = new System.Drawing.Point(446, 205);
            this.TargetACPanel.Name = "TargetACPanel";
            this.TargetACPanel.Size = new System.Drawing.Size(190, 195);
            this.TargetACPanel.TabIndex = 32;
            // 
            // TargetACCL
            // 
            this.TargetACCL.FormattingEnabled = true;
            this.TargetACCL.Location = new System.Drawing.Point(0, 25);
            this.TargetACCL.Name = "TargetACCL";
            this.TargetACCL.Size = new System.Drawing.Size(190, 166);
            this.TargetACCL.TabIndex = 1;
            // 
            // TargetACLabel
            // 
            this.TargetACLabel.Location = new System.Drawing.Point(0, 0);
            this.TargetACLabel.Name = "TargetACLabel";
            this.TargetACLabel.Size = new System.Drawing.Size(190, 23);
            this.TargetACLabel.TabIndex = 0;
            this.TargetACLabel.Text = "Target AC (for Kinetic)";
            this.TargetACLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GaugeLabel
            // 
            this.GaugeLabel.AutoSize = true;
            this.GaugeLabel.Location = new System.Drawing.Point(148, 7);
            this.GaugeLabel.Name = "GaugeLabel";
            this.GaugeLabel.Size = new System.Drawing.Size(74, 15);
            this.GaugeLabel.TabIndex = 33;
            this.GaugeLabel.Text = "Gauge (mm)";
            // 
            // TargetSchemePanel
            // 
            this.TargetSchemePanel.Controls.Add(this.RemoveLayerButton);
            this.TargetSchemePanel.Controls.Add(this.AddLayerButton);
            this.TargetSchemePanel.Controls.Add(this.ArmorLayerDD);
            this.TargetSchemePanel.Controls.Add(this.ArmorLayerLB);
            this.TargetSchemePanel.Controls.Add(this.TargetSchemeLabel);
            this.TargetSchemePanel.Location = new System.Drawing.Point(655, 207);
            this.TargetSchemePanel.Name = "TargetSchemePanel";
            this.TargetSchemePanel.Size = new System.Drawing.Size(133, 193);
            this.TargetSchemePanel.TabIndex = 34;
            // 
            // ArmorLayerLB
            // 
            this.ArmorLayerLB.FormattingEnabled = true;
            this.ArmorLayerLB.ItemHeight = 15;
            this.ArmorLayerLB.Location = new System.Drawing.Point(3, 84);
            this.ArmorLayerLB.Name = "ArmorLayerLB";
            this.ArmorLayerLB.Size = new System.Drawing.Size(127, 94);
            this.ArmorLayerLB.TabIndex = 1;
            // 
            // TargetSchemeLabel
            // 
            this.TargetSchemeLabel.Location = new System.Drawing.Point(0, 0);
            this.TargetSchemeLabel.Name = "TargetSchemeLabel";
            this.TargetSchemeLabel.Size = new System.Drawing.Size(133, 31);
            this.TargetSchemeLabel.TabIndex = 0;
            this.TargetSchemeLabel.Text = "Target Armor Scheme\r\n(for Pendepth)";
            this.TargetSchemeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // minGaugeErrorProvider
            // 
            this.minGaugeErrorProvider.ContainerControl = this;
            // 
            // maxGaugeErrorProvider
            // 
            this.maxGaugeErrorProvider.ContainerControl = this;
            // 
            // ParameterInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 583);
            this.Controls.Add(this.TargetSchemePanel);
            this.Controls.Add(this.GaugeLabel);
            this.Controls.Add(this.TargetACPanel);
            this.Controls.Add(this.PerLabel);
            this.Controls.Add(this.TestTypePanel);
            this.Controls.Add(this.DamageTypeLabel);
            this.Controls.Add(this.DamageTypeDD);
            this.Controls.Add(this.MinRangeTB);
            this.Controls.Add(this.MinRangeLabel);
            this.Controls.Add(this.MinVelocityTB);
            this.Controls.Add(this.MinVelocityLabel);
            this.Controls.Add(this.MaxLengthTB);
            this.Controls.Add(this.MaxLengthLabel);
            this.Controls.Add(this.BoreEvacuatorCB);
            this.Controls.Add(this.MaxRecoilTB);
            this.Controls.Add(this.MaxRecoilLabel);
            this.Controls.Add(this.MaxRailDrawTB);
            this.Controls.Add(this.MaxDrawLabel);
            this.Controls.Add(this.MaxRGTB);
            this.Controls.Add(this.MaxRGLabel);
            this.Controls.Add(this.MaxGPTB);
            this.Controls.Add(this.MaxGPLabel);
            this.Controls.Add(this.VariableModulesPanel);
            this.Controls.Add(this.FixedModulesPanel);
            this.Controls.Add(this.BasePanel);
            this.Controls.Add(this.HeadModulesLabel);
            this.Controls.Add(this.HeadModulesCL);
            this.Controls.Add(this.BarrelCountDD);
            this.Controls.Add(this.BarrelCountLabel);
            this.Controls.Add(this.MaxGaugeLabel);
            this.Controls.Add(this.MinGaugeLabel);
            this.Controls.Add(this.MaxGaugeTB);
            this.Controls.Add(this.MinGaugeTB);
            this.Name = "ParameterInput";
            this.Text = "Parameter Input";
            this.Load += new System.EventHandler(this.ParameterInput_Load);
            this.BasePanel.ResumeLayout(false);
            this.BasePanel.PerformLayout();
            this.FixedModulesPanel.ResumeLayout(false);
            this.FixedModulesPanel.PerformLayout();
            this.VariableModulesPanel.ResumeLayout(false);
            this.TestTypePanel.ResumeLayout(false);
            this.TestTypePanel.PerformLayout();
            this.TargetACPanel.ResumeLayout(false);
            this.TargetSchemePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minGaugeErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxGaugeErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Label MinGaugeLabel;
        private System.Windows.Forms.Label MaxGaugeLabel;
        private System.Windows.Forms.TextBox MinGaugeTB;
        private System.Windows.Forms.TextBox MaxGaugeTB;
        private System.Windows.Forms.Label BarrelCountLabel;
        private System.Windows.Forms.ComboBox BarrelCountDD;
        private System.Windows.Forms.CheckedListBox HeadModulesCL;
        private System.Windows.Forms.Label HeadModulesLabel;
        private System.Windows.Forms.Panel BasePanel;
        private System.Windows.Forms.RadioButton BaseBleederRB;
        private System.Windows.Forms.RadioButton GravRB;
        private System.Windows.Forms.RadioButton TracerRB;
        private System.Windows.Forms.RadioButton SupercavRB;
        private System.Windows.Forms.RadioButton NoBaseRB;
        private System.Windows.Forms.Panel FixedModulesPanel;
        private System.Windows.Forms.Label SolidBodyFixedLabel;
        private System.Windows.Forms.Label FixedModulesLabel;
        private System.Windows.Forms.TextBox GravCompFixedTB;
        private System.Windows.Forms.Label GravCompFixedLabel;
        private System.Windows.Forms.TextBox FinFixedTB;
        private System.Windows.Forms.Label FinLabel;
        private System.Windows.Forms.TextBox FuseFixedTB;
        private System.Windows.Forms.Label FuseFixedLabel;
        private System.Windows.Forms.TextBox HEBodyFixedTB;
        private System.Windows.Forms.Label HEBodyFixedLabel;
        private System.Windows.Forms.TextBox FragBodyFixedTB;
        private System.Windows.Forms.Label FragBodyFixedLabel;
        private System.Windows.Forms.TextBox FlaKBodyFixedTB;
        private System.Windows.Forms.TextBox EmpBodyFixedTB;
        private System.Windows.Forms.Label FlaKBodyFixedLabel;
        private System.Windows.Forms.Label EmpBodyFixedLabel;
        private System.Windows.Forms.TextBox SabotBodyFixedTB;
        private System.Windows.Forms.Label SabotBodyFixedLabel;
        private System.Windows.Forms.TextBox SolidBodyFixedTB;
        private System.Windows.Forms.Panel VariableModulesPanel;
        private System.Windows.Forms.CheckedListBox VariableModulesCL;
        private System.Windows.Forms.Label VariableModulesLabel;
        private System.Windows.Forms.Label BaseModulesLabel;
        private System.Windows.Forms.Label MaxGPLabel;
        private System.Windows.Forms.TextBox MaxGPTB;
        private System.Windows.Forms.TextBox MaxRGTB;
        private System.Windows.Forms.Label MaxRGLabel;
        private System.Windows.Forms.TextBox MaxRailDrawTB;
        private System.Windows.Forms.Label MaxDrawLabel;
        private System.Windows.Forms.TextBox MaxRecoilTB;
        private System.Windows.Forms.Label MaxRecoilLabel;
        private System.Windows.Forms.CheckBox BoreEvacuatorCB;
        private System.Windows.Forms.TextBox MaxLengthTB;
        private System.Windows.Forms.Label MaxLengthLabel;
        private System.Windows.Forms.TextBox MinVelocityTB;
        private System.Windows.Forms.Label MinVelocityLabel;
        private System.Windows.Forms.TextBox MinRangeTB;
        private System.Windows.Forms.Label MinRangeLabel;
        private System.Windows.Forms.Label DamageTypeLabel;
        private System.Windows.Forms.Panel TestTypePanel;
        private System.Windows.Forms.RadioButton PerCostRB;
        private System.Windows.Forms.RadioButton PerVolumeRB;
        private System.Windows.Forms.Label PerLabel;
        private System.Windows.Forms.Panel TargetACPanel;
        private System.Windows.Forms.CheckedListBox TargetACCL;
        private System.Windows.Forms.Label TargetACLabel;
        private System.Windows.Forms.Label GaugeLabel;
        private System.Windows.Forms.ComboBox DamageTypeDD;
        private System.Windows.Forms.Panel TargetSchemePanel;
        private System.Windows.Forms.Label TargetSchemeLabel;
        private System.Windows.Forms.ListBox ArmorLayerLB;
        private System.Windows.Forms.ComboBox ArmorLayerDD;
        private System.Windows.Forms.Button RemoveLayerButton;
        private System.Windows.Forms.Button AddLayerButton;
        private System.Windows.Forms.ErrorProvider minGaugeErrorProvider;
        private System.Windows.Forms.ErrorProvider maxGaugeErrorProvider;
    }
}

