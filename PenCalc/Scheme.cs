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
                if (int.TryParse(input, out int layerIndex))
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
        /// Calculates AC of each layer, taking into account structural bonuses
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
        /// Retrieves KD required to pen armor at given AP
        /// </summary>
        /// <param name="ap">AP of incoming shell</param>
        /// <returns>Required KD to pen</returns>
        public float GetRequiredKD(float ap)
        {
            float requiredKD = 0;

            if (LayerList.Count > 0)
            {
                foreach (Layer layer in LayerList)
                {
                    float kdMultiplier = Math.Min(1, ap / layer.AC);
                    if (kdMultiplier == 1)
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


        /// <summary>
        /// Calculates thump damage required to destroy all armor at given AP
        /// </summary>
        /// <param name="ap">AP of incoming shell</param>
        /// <returns>Required thump damage to destroy entire scheme</returns>
        public float GetRequiredThump(float ap)
        {
            float requiredTD = 0;

            if (LayerList.Count > 0)
            {
                foreach (Layer layer in LayerList)
                {
                    float tdMultiplier = Math.Min(1, ap / layer.RawAC);
                    if (tdMultiplier == 1)
                    {
                        requiredTD += layer.HP;
                    }
                    else
                    {
                        requiredTD += layer.HP / tdMultiplier;
                    }
                }
            }
            return requiredTD;
        }
    }
}
