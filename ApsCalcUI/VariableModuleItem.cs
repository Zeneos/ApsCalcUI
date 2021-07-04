using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class VariableModuleItem
    {
        public int Index { get; set; }
        public string Name { get; set; }


        /// <summary>
        /// Creates a list of all body modules
        /// </summary>
        public static IEnumerable<VariableModuleItem> GenerateBodyList()
        {
            for (int index = 0; index < ApsCalc.Module.AllModules.Length; index++)
            {
                if (ApsCalc.Module.AllModules[index].ModulePosition == ApsCalc.Module.Position.Middle)
                {
                    VariableModuleItem varMod = new()
                    {
                        Index = index,
                        Name = ApsCalc.Module.AllModules[index].Name
                    };

                    yield return varMod;
                }
            }
        }
    }
}
