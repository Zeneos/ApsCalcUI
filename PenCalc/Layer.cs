using System;
using System.Collections.Generic;
using System.Text;

namespace PenCalc
{
    public class Layer
    {
        /// <summary>
        /// An armor layer.  Stats given are for 4 m beams.
        /// </summary>
        /// <param name="name">Material name</param>
        /// <param name="hp">Hit points</param>
        /// <param name="ac">Armor class</param>
        /// <param name="isStruct">True if the block is structural</param>
        public Layer (string name, float hp, float ac, bool isStruct)
        {
            Name = name;
            HP = hp;
            RawAC = ac;
            ACBonus = 0.2f * RawAC;
            AC = ac; // Actual AC defaults to unbonused
            IsStruct = isStruct;
        }
        public string Name { get; }
        public float HP { get; }
        public float RawAC { get; }
        public float ACBonus { get; }
        public float AC { get; set; }
        public bool IsStruct { get; }


        // Initialize all supported materials
        public static Layer Wood { get; } = new Layer("Wood", 864f, 8f, true);
        public static Layer Metal { get; } = new Layer("Metal", 1680f, 40f, true);
        public static Layer Alloy { get; } = new Layer("Alloy", 1440f, 35f, true);
        public static Layer Lead { get; } = new Layer("Lead", 1440f, 10f, true);
        public static Layer Stone { get; } = new Layer("Stone", 1200f, 16f, true);
        public static Layer Heavy { get; } = new Layer("Heavy Armour", 6000f, 60f, true);
        public static Layer Air { get; } = new Layer("Air", 0, 0, false);


        // List all supported materials
        public static Layer[] AllLayers { get; } =
        {
            Wood,
            Metal,
            Alloy,
            Lead,
            Stone,
            Heavy,
            Air
        };
    }
}
