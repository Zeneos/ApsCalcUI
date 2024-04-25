using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class VariableModuleItem(int index, string name)
    {
        public int Index { get; } = index;
        public string Name { get; } = name;


        /// <summary>
        /// Creates a list of all variable modules
        /// </summary>
        public static IEnumerable<VariableModuleItem> GenerateBodyList()
        {
            for (int index = 0; index < Module.AllModules.Length; index++)
            {
                if (Module.AllModules[index].CanBeVariable)
                {
                    VariableModuleItem varMod = new(index, Module.AllModules[index].Name);

                    yield return varMod;
                }
            }
        }
    }
}
