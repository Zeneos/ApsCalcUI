using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class BarrelLengthLimitTypeItem
    {
        public BarrelLengthLimitTypeItem(BarrelLengthLimit id, string text)
        {
            ID = id;
            Text = text;
        }
        public BarrelLengthLimit ID { get; }
        public string Text { get; }
    }
}
