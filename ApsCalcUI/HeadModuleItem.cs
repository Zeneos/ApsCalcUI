using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class HeadModuleItem
    {
        public int Index { get; set; }
        public string Name { get; set; }


        /// <summary>
        /// Creates a list of all modules which can be used as heads
        /// </summary>
        public static IEnumerable<HeadModuleItem> GenerateHeadList()
        {
            for (int index = 0; index < ApsCalc.Module.AllModules.Length; index++)
            {
                if (ApsCalc.Module.AllModules[index].ModulePosition == ApsCalc.Module.Position.Head
                    || ApsCalc.Module.AllModules[index].ModulePosition == ApsCalc.Module.Position.Middle)
                {
                    HeadModuleItem head = new()
                    {
                        Index = index,
                        Name = ApsCalc.Module.AllModules[index].Name
                    };

                    yield return head;
                }
            }
        }
    }
}
