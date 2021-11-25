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
            for (int index = 0; index < Module.AllModules.Length; index++)
            {
                if (Module.AllModules[index].ModulePosition == Module.Position.Head
                    || (Module.AllModules[index].ModulePosition == Module.Position.Middle && Module.AllModules[index].CanBeVariable))
                {
                    HeadModuleItem head = new()
                    {
                        Index = index,
                        Name = Module.AllModules[index].Name
                    };

                    yield return head;
                }
            }
        }
    }
}
