using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class BarrelCountItem(int id, string text)
    {
        public int ID { get; } = id;
        public string Text { get; } = text;
    }
}
