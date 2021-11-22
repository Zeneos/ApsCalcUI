using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PenCalc;

namespace ApsCalcUI
{
    public struct TestParameters
    {
        public int BarrelCount;
        public int MinGauge;
        public int MaxGauge;
        public List<int> HeadIndices;
        public Module BaseModule;
        public float[] FixedModulecounts;
        public float MinModulecount;
        public int[] VariableModuleIndices;
        public float MaxGPCasingCount;
        public float MaxRGCasingCount;
        public float MinLength;
        public float MaxLength;
        public float MaxDraw;
        public float MaxRecoil;
        public float MinVelocity;
        public float MinEffectiverange;
        public List<float> TargetACList;
        public DamageType DamageType;
        public Scheme ArmorScheme;
        public int TestType;
        public bool Labels;
        public int TestInterval;
        public float StoragePerVolume;
        public float StoragePerCost;
        public float Ppm;
        public float Ppv;
        public float Ppc;
        public bool Fuel;
    }

    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ParameterInput());
        }
    }
}
