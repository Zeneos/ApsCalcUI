using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsCalcUI
{
    class TargetACItem
    {
        public TargetACItem(float id, string text)
        {
            ID = id;
            Text = text;
        }
        public float ID { get; }
        public string Text { get; }
    }
}
