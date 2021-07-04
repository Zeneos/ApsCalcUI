using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PenCalc
{
    public class Scheme
    {
        /// <summary>
        /// Stores the armor configuration and runs calculations for pendepth
        /// </summary>
        public Scheme() { }

        // List of layers
        public List<Layer> LayerList { get; set; } = new List<Layer>();

        // Maximum useful AC
        public float MaxAC { get; set; }


        /// <summary>
        /// Get user input for layer list
        /// </summary>
        public void GetLayerList()
        {
            while (true)
            {
                string input;
                int layerIndex;
                for (int i = 0; i < Layer.AllLayers.Length; i++)
                {
                    Console.WriteLine(i + " : " + Layer.AllLayers[i].Name);
                }
                Console.WriteLine("\nEnter a number to add a layer, or type 'done'.");
                input = Console.ReadLine();
                if (input == "done")
                {
                    break;
                }
                if (int.TryParse(input, out layerIndex))
                {
                    if (layerIndex < 0 || layerIndex > Layer.AllLayers.Length)
                    {
                        Console.WriteLine("\nLAYER INDEX RANGE ERROR: Enter an integer from 0 thru "
                            + Layer.AllLayers.Length
                            + ", or type 'done'.");
                    }
                    else
                    {
                        LayerList.Add(Layer.AllLayers[layerIndex]);
                        Console.WriteLine("\n" + Layer.AllLayers[layerIndex].Name + " added.\n");
                    }
                }
                else
                {
                    Console.WriteLine("\nLAYER INDEX PARSE ERROR: Enter an integer from 0 thru "
                        + Layer.AllLayers.Length
                        + ", or type 'done'.");
                }
            }
        }


        /// <summary>
        /// Calculates the AC of each layer, taking into account structural bonuses
        /// </summary>
        public void CalculateLayerAC()
        {
            // Add structural bonus, if applicable
            for (int layerIndex = 0; layerIndex < LayerList.Count - 1; layerIndex++)
            {
                if (LayerList[layerIndex].IsStruct && LayerList[layerIndex + 1].IsStruct)
                {
                    LayerList[layerIndex].AC = LayerList[layerIndex].RawAC + LayerList[layerIndex + 1].ACBonus;
                }
                else
                {
                    LayerList[layerIndex].AC = LayerList[layerIndex].RawAC;
                }

                // Update max useful AC
                MaxAC = Math.Max(MaxAC, LayerList[layerIndex].AC);
            }

            // Last layer is left at default
        }


        /// <summary>
        /// Retrieves the KD required to pen the armor at the given AP
        /// </summary>
        /// <param name="ap">AP of the incoming shell</param>
        /// <returns>Required KD to pen</returns>
        public float GetRequiredKD(float ap)
        {
            float requiredKD = 0;

            if (ap == 0)
            {
                requiredKD = float.MaxValue;
            }
            else
            {
                foreach (Layer layer in LayerList)
                {
                    float kdMultiplier = Math.Min(1, ap / layer.AC);
                    if (kdMultiplier >= 1)
                    {
                        requiredKD += layer.HP;
                    }
                    else
                    {
                        requiredKD += layer.HP / kdMultiplier;
                    }
                }
            }
            return requiredKD;
        }
    }
}
