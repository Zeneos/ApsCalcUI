using PenCalc;

namespace ApsCalcUI
{
    class ArmorLayerItem
    {
        public ArmorLayerItem(string name, Layer layer)
        {
            Name = name;
            Layer = layer;
        }

        public string Name { get; }
        public Layer Layer { get; }
    }
}
