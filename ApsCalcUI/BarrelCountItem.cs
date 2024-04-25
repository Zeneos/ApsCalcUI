using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class BarrelCountItem
    {
        public BarrelCountItem(int id, string text)
        {
            ID = id;
            Text = text;
        }
        public int ID { get; }
        public string Text { get; }
    }
}
