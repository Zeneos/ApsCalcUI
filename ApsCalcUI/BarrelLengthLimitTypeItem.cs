using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class BarrelLengthLimitTypeItem(BarrelLengthLimit id, string text)
    {
        public BarrelLengthLimit ID { get; } = id;
        public string Text { get; } = text;
    }
}
