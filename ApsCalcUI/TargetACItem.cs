using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class TargetACItem(float id, string text)
    {
        public float ID { get; } = id;
        public string Text { get; } = text;
    }
}
