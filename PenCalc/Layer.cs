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
        /// <param name="givesACBonus">True if the block is structural</param>
        /// <param name="angleHPMultiplier">Multiply layer HP by this value to get required KD due to angle</param>
        public Layer (string name, float hp, float ac, bool givesACBonus, float angleHPMultiplier)
        {
            Name = name;
            HP = hp;
            RawAC = ac;
            ACBonus = 0.2f * RawAC;
            AC = ac; // Actual AC defaults to unbonused
            GivesACBonus = givesACBonus;
            AngleHPMultiplier = angleHPMultiplier;
        }
        public string Name { get; }
        public float HP { get; }
        public float RawAC { get; }
        public float ACBonus { get; }
        public float AC { get; set; }
        public bool GivesACBonus { get; }
        public float AngleHPMultiplier { get; }


        // Angle multipliers for slopes and wedges. Steep means closer to perpendicular (less damage reduction)
        const float Slope1Multiplier = 1.414214f;
        const float Slope2SteepMultiplier = 1.118034f;
        const float Slope2ShallowMultiplier = 2.236068f;
        const float Slope3SteepMultiplier = 1.054093f;
        const float Slope3ShallowMultiplier = 3.162278f;
        const float Slope4SteepMultiplier = 1.030776f;
        const float Slope4ShallowMultiplier = 4.123106f;

        const float Wedge1SteepMultiplier = 1.118034f;
        const float Wedge1ShallowMultiplier = 2.236068f;
        const float Wedge2SteepMultiplier = 1.030776f;
        const float Wedge2ShallowMultiplier = 4.123106f;
        const float Wedge3SteepMultiplier = 1.013794f;
        const float Wedge3ShallowMultiplier = 6.082763f;
        const float Wedge4SteepMultiplier = 1.007782f;
        const float Wedge4ShallowMultiplier = 8.062258f;

        // Initialize all supported layers
        public static Layer Air { get; } = new Layer("Air", 0, 0, false, 1f);
        public static Layer AlloyBeam { get; } = new Layer("Alloy 4m Beam", 1440f, 35f, true, 1f);
        public static Layer HeavyBeam { get; } = new Layer("HA 4m Beam", 6000f, 60f, true, 1f);
        public static Layer MetalBeam { get; } = new Layer("Metal 4m Beam", 1680f, 40f, true, 1f);
        public static Layer StoneBeam { get; } = new Layer("Stone 4m Beam", 1200f, 16f, true, 1f);
        public static Layer WoodBeam { get; } = new Layer("Wood 4m Beam", 864f, 8f, true, 1f);

        public static Layer AlloyBeamSlope { get; } = new Layer("Alloy Beam Slope", 720f, 35f, false, Slope1Multiplier);
        public static Layer HeavyBeamSlope { get; } = new Layer("HA Beam Slope", 3000f, 60f, false, Slope1Multiplier);
        public static Layer MetalBeamSlope { get; } = new Layer("Metal Beam Slope", 840f, 40f, false, Slope1Multiplier);
        public static Layer StoneBeamSlope { get; } = new Layer("Stone Beam Slope", 600f, 16f, false, Slope1Multiplier);
        public static Layer WoodBeamSlope { get; } = new Layer("Wood Beam Slope", 432f, 8f, false, Slope1Multiplier);

        public static Layer Alloy2mSlopeSteep { get; } = new Layer("Alloy 2m Slope (steep)", 330f, 35f, false, Slope2SteepMultiplier);
        public static Layer Heavy2mSlopeSteep { get; } = new Layer("HA 2m Slope (steep)", 1375f, 60f, false, Slope2SteepMultiplier);
        public static Layer Metal2mSlopeSteep { get; } = new Layer("Metal 2m Slope (steep)", 385f, 40f, false, Slope2SteepMultiplier);
        public static Layer Stone2mSlopeSteep { get; } = new Layer("Stone 2m Slope (steep)", 275f, 16f, false, Slope2SteepMultiplier);
        public static Layer Wood2mSlopeSteep { get; } = new Layer("Wood 2m Slope (steep)", 198f, 8f, false, Slope2SteepMultiplier);

        public static Layer Alloy2mSlopeShallow { get; } = new Layer("Alloy 2m Slope (shallow)", 330f, 35f, false, Slope2ShallowMultiplier);
        public static Layer Heavy2mSlopeShallow { get; } = new Layer("HA 2m Slope (shallow)", 1375f, 60f, false, Slope2ShallowMultiplier);
        public static Layer Metal2mSlopeShallow { get; } = new Layer("Metal 2m Slope (shallow)", 385f, 40f, false, Slope2ShallowMultiplier);
        public static Layer Stone2mSlopeShallow { get; } = new Layer("Stone 2m Slope (shallow)", 275f, 16f, false, Slope2ShallowMultiplier);
        public static Layer Wood2mSlopeShallow { get; } = new Layer("Wood 2m Slope (shallow)", 198f, 8f, false, Slope2ShallowMultiplier);

        public static Layer Alloy3mSlopeSteep { get; } = new Layer("Alloy 3m Slope (steep)", 517.5f, 35f, false, Slope3SteepMultiplier);
        public static Layer Heavy3mSlopeSteep { get; } = new Layer("HA 3m Slope (steep)", 2156.2f, 60f, false, Slope3SteepMultiplier);
        public static Layer Metal3mSlopeSteep { get; } = new Layer("Metal 3m Slope (steep)", 603.8f, 40f, false, Slope3SteepMultiplier);
        public static Layer Stone3mSlopeSteep { get; } = new Layer("Stone 3m Slope (steep)", 431.2f, 16f, false, Slope3SteepMultiplier);
        public static Layer Wood3mSlopeSteep { get; } = new Layer("Wood 3m Slope (steep)", 310.5f, 8f, false, Slope3SteepMultiplier);

        public static Layer Alloy3mSlopeShallow { get; } = new Layer("Alloy 3m Slope (shallow)", 517.5f, 35f, false, Slope3ShallowMultiplier);
        public static Layer Heavy3mSlopeShallow { get; } = new Layer("HA 3m Slope (shallow)", 2156.2f, 60f, false, Slope3ShallowMultiplier);
        public static Layer Metal3mSlopeShallow { get; } = new Layer("Metal 3m Slope (shallow)", 603.8f, 40f, false, Slope3ShallowMultiplier);
        public static Layer Stone3mSlopeShallow { get; } = new Layer("Stone 3m Slope (shallow)", 431.2f, 16f, false, Slope3ShallowMultiplier);
        public static Layer Wood3mSlopeShallow { get; } = new Layer("Wood 3m Slope (shallow)", 310.5f, 8f, false, Slope3ShallowMultiplier);

        public static Layer Alloy4mSlopeSteep { get; } = new Layer("Alloy 4m Slope (steep)", 720f, 35f, false, Slope4SteepMultiplier);
        public static Layer Heavy4mSlopeSteep { get; } = new Layer("HA 4m Slope (steep)", 3000f, 60f, false, Slope4SteepMultiplier);
        public static Layer Metal4mSlopeSteep { get; } = new Layer("Metal 4m Slope (steep)", 840f, 40f, false, Slope4SteepMultiplier);
        public static Layer Stone4mSlopeSteep { get; } = new Layer("Stone 4m Slope (steep)", 600f, 16f, false, Slope4SteepMultiplier);
        public static Layer Wood4mSlopeSteep { get; } = new Layer("Wood 4m Slope (steep)", 432f, 8f, false, Slope4SteepMultiplier);

        public static Layer Alloy4mSlopeShallow { get; } = new Layer("Alloy 4m Slope (shallow)", 720f, 35f, false, Slope4ShallowMultiplier);
        public static Layer Heavy4mSlopeShallow { get; } = new Layer("HA 4m Slope (shallow)", 3000f, 60f, false, Slope4ShallowMultiplier);
        public static Layer Metal4mSlopeShallow { get; } = new Layer("Metal 4m Slope (shallow)", 840f, 40f, false, Slope4ShallowMultiplier);
        public static Layer Stone4mSlopeShallow { get; } = new Layer("Stone 4m Slope (shallow)", 600f, 16f, false, Slope4ShallowMultiplier);
        public static Layer Wood4mSlopeShallow { get; } = new Layer("Wood 4m Slope (shallow)", 432f, 8f, false, Slope4ShallowMultiplier);

        public static Layer AlloyWedgeSteep { get; } = new Layer("Alloy Wedge (steep)", 150f, 35f, false, Wedge1SteepMultiplier);
        public static Layer HeavyWedgeSteep { get; } = new Layer("HA Wedge (steep)", 625f, 60f, false, Wedge1SteepMultiplier);
        public static Layer MetalWedgeSteep { get; } = new Layer("Metal Wedge (steep)", 175f, 40f, false, Wedge1SteepMultiplier);
        public static Layer StoneWedgeSteep { get; } = new Layer("Stone Wedge (steep)", 125f, 16f, false, Wedge1SteepMultiplier);
        public static Layer WoodWedgeSteep { get; } = new Layer("Wood Wedge (steep)", 90f, 8f, false, Wedge1SteepMultiplier);

        public static Layer AlloyWedgeShallow { get; } = new Layer("Alloy Wedge (shallow)", 150f, 35f, false, Wedge1ShallowMultiplier);
        public static Layer HeavyWedgeShallow { get; } = new Layer("HA Wedge (shallow)", 625f, 60f, false, Wedge1ShallowMultiplier);
        public static Layer MetalWedgeShallow { get; } = new Layer("Metal Wedge (shallow)", 175f, 40f, false, Wedge1ShallowMultiplier);
        public static Layer StoneWedgeShallow { get; } = new Layer("Stone Wedge (shallow)", 125f, 16f, false, Wedge1ShallowMultiplier);
        public static Layer WoodWedgeShallow { get; } = new Layer("Wood Wedge (shallow)", 90f, 8f, false, Wedge1ShallowMultiplier);

        public static Layer Alloy2mWedgeSteep { get; } = new Layer("Alloy 2m Wedge (steep)", 330f, 35f, false, Wedge2SteepMultiplier);
        public static Layer Heavy2mWedgeSteep { get; } = new Layer("HA 2m Wedge (steep)", 1375f, 60f, false, Wedge2SteepMultiplier);
        public static Layer Metal2mWedgeSteep { get; } = new Layer("Metal 2m Wedge (steep)", 385f, 40f, false, Wedge2SteepMultiplier);
        public static Layer Stone2mWedgeSteep { get; } = new Layer("Stone 2m Wedge (steep)", 275f, 16f, false, Wedge2SteepMultiplier);
        public static Layer Wood2mWedgeSteep { get; } = new Layer("Wood 2m Wedge (steep)", 198f, 8f, false, Wedge2SteepMultiplier);

        public static Layer Alloy2mWedgeShallow { get; } = new Layer("Alloy 2m Wedge (shallow)", 330f, 35f, false, Wedge2ShallowMultiplier);
        public static Layer Heavy2mWedgeShallow { get; } = new Layer("HA 2m Wedge (shallow)", 1375f, 60f, false, Wedge2ShallowMultiplier);
        public static Layer Metal2mWedgeShallow { get; } = new Layer("Metal 2m Wedge (shallow)", 385f, 40f, false, Wedge2ShallowMultiplier);
        public static Layer Stone2mWedgeShallow { get; } = new Layer("Stone 2m Wedge (shallow)", 275f, 16f, false, Wedge2ShallowMultiplier);
        public static Layer Wood2mWedgeShallow { get; } = new Layer("Wood 2m Wedge (shallow)", 198f, 8f, false, Wedge2ShallowMultiplier);

        public static Layer Alloy3mWedgeSteep { get; } = new Layer("Alloy 3m Wedge (steep)", 517.5f, 35f, false, Wedge3SteepMultiplier);
        public static Layer Heavy3mWedgeSteep { get; } = new Layer("HA 3m Wedge (steep)", 2156.2f, 60f, false, Wedge3SteepMultiplier);
        public static Layer Metal3mWedgeSteep { get; } = new Layer("Metal 3m Wedge (steep)", 603.8f, 40f, false, Wedge3SteepMultiplier);
        public static Layer Stone3mWedgeSteep { get; } = new Layer("Stone 3m Wedge (steep)", 431.2f, 16f, false, Wedge3SteepMultiplier);
        public static Layer Wood3mWedgeSteep { get; } = new Layer("Wood 3m Wedge (steep)", 310.5f, 8f, false, Wedge3SteepMultiplier);

        public static Layer Alloy3mWedgeShallow { get; } = new Layer("Alloy 3m Wedge (shallow)", 517.5f, 35f, false, Wedge3ShallowMultiplier);
        public static Layer Heavy3mWedgeShallow { get; } = new Layer("HA 3m Wedge (shallow)", 2156.2f, 60f, false, Wedge3ShallowMultiplier);
        public static Layer Metal3mWedgeShallow { get; } = new Layer("Metal 3m Wedge (shallow)", 603.8f, 40f, false, Wedge3ShallowMultiplier);
        public static Layer Stone3mWedgeShallow { get; } = new Layer("Stone 3m Wedge (shallow)", 431.2f, 16f, false, Wedge3ShallowMultiplier);
        public static Layer Wood3mWedgeShallow { get; } = new Layer("Wood 3m Wedge (shallow)", 310.5f, 8f, false, Wedge3ShallowMultiplier);

        public static Layer Alloy4mWedgeSteep { get; } = new Layer("Alloy 4m Wedge (steep)", 720f, 35f, false, Wedge4SteepMultiplier);
        public static Layer Heavy4mWedgeSteep { get; } = new Layer("HA 4m Wedge (steep)", 3000f, 60f, false, Wedge4SteepMultiplier);
        public static Layer Metal4mWedgeSteep { get; } = new Layer("Metal 4m Wedge (steep)", 840f, 40f, false, Wedge4SteepMultiplier);
        public static Layer Stone4mWedgeSteep { get; } = new Layer("Stone 4m Wedge (steep)", 600f, 16f, false, Wedge4SteepMultiplier);
        public static Layer Wood4mWedgeSteep { get; } = new Layer("Wood 4m Wedge (steep)", 432f, 8f, false, Wedge4SteepMultiplier);

        public static Layer Alloy4mWedgeShallow { get; } = new Layer("Alloy 4m Wedge (shallow)", 720f, 35f, false, Wedge4ShallowMultiplier);
        public static Layer Heavy4mWedgeShallow { get; } = new Layer("HA 4m Wedge (shallow)", 3000f, 60f, false, Wedge4ShallowMultiplier);
        public static Layer Metal4mWedgeShallow { get; } = new Layer("Metal 4m Wedge (shallow)", 840f, 40f, false, Wedge4ShallowMultiplier);
        public static Layer Stone4mWedgeShallow { get; } = new Layer("Stone 4m Wedge (shallow)", 600f, 16f, false, Wedge4ShallowMultiplier);
        public static Layer Wood4mWedgeShallow { get; } = new Layer("Wood 4m Wedge (shallow)", 432f, 8f, false, Wedge4ShallowMultiplier);


        // List all supported layers
        public static Layer[] AllLayers { get; } =
        {
            Air,
            AlloyBeam,
            AlloyBeamSlope,
            Alloy2mSlopeSteep,
            Alloy2mSlopeShallow,
            Alloy3mSlopeSteep,
            Alloy3mSlopeShallow,
            Alloy4mSlopeSteep,
            Alloy4mSlopeShallow,
            AlloyWedgeSteep,
            AlloyWedgeShallow,
            Alloy2mWedgeSteep,
            Alloy2mWedgeShallow,
            Alloy3mWedgeSteep,
            Alloy3mWedgeShallow,
            Alloy4mWedgeSteep,
            Alloy4mWedgeShallow,
            HeavyBeam,
            HeavyBeamSlope,
            Heavy2mSlopeSteep,
            Heavy2mSlopeShallow,
            Heavy3mSlopeSteep,
            Heavy3mSlopeShallow,
            Heavy4mSlopeSteep,
            Heavy4mSlopeShallow,
            HeavyWedgeSteep,
            HeavyWedgeShallow,
            Heavy2mWedgeSteep,
            Heavy2mWedgeShallow,
            Heavy3mSlopeSteep,
            Heavy3mSlopeShallow,
            Heavy4mWedgeSteep,
            Heavy4mWedgeShallow,
            MetalBeam,
            MetalBeamSlope,
            Metal2mSlopeSteep,
            Metal2mSlopeShallow,
            Metal3mSlopeSteep,
            Metal3mSlopeShallow,
            Metal4mSlopeSteep,
            Metal4mSlopeShallow,
            MetalWedgeSteep,
            MetalWedgeShallow,
            Metal2mWedgeSteep,
            Metal2mWedgeShallow,
            Metal3mWedgeSteep,
            Metal3mWedgeShallow,
            Metal4mWedgeSteep,
            Metal4mWedgeShallow,
            StoneBeam,
            StoneBeamSlope,
            Stone2mSlopeSteep,
            Stone2mSlopeShallow,
            Stone3mSlopeSteep,
            Stone3mSlopeShallow,
            Stone4mSlopeSteep,
            Stone4mSlopeShallow,
            StoneWedgeSteep,
            StoneWedgeShallow,
            Stone2mWedgeSteep,
            Stone2mWedgeShallow,
            Stone3mWedgeSteep,
            Stone3mWedgeShallow,
            Stone4mWedgeSteep,
            Stone4mWedgeShallow,
            WoodBeam,
            WoodBeamSlope,
            Wood2mSlopeSteep,
            Wood2mSlopeShallow,
            Wood3mSlopeSteep,
            Wood3mSlopeShallow,
            Wood4mSlopeSteep,
            Wood4mSlopeShallow,
            WoodWedgeSteep,
            WoodWedgeShallow,
            Wood2mWedgeSteep,
            Wood2mWedgeShallow,
            Wood3mWedgeSteep,
            Wood3mWedgeShallow,
            Wood4mWedgeSteep,
            Wood4mWedgeShallow,
        };
    }
}
