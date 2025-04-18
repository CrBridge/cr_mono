using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace cr_mono.Core.GameLogic
{
    // class for helper functions related to procedural generation
    // contains algorithms and such that I'll want to reuse
    // across the different generator classes
    internal class ProcGenHelpers
    {
        internal static List<Vector2> GetCardinalDirections() 
        {
            return new List<Vector2> 
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, 1),
                new Vector2(0, -1)
            };
        }

        internal static Dictionary<Vector2, int> GenerateGrid(int size)
        {
            Dictionary<Vector2, int> layer = [];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    layer[new Vector2(x, y)] = 0;
                }
            }

            return layer;
        }

        internal static Vector2 GetRandomGridPos(
            Dictionary<Vector2, int> navMap,
            RNG rng)
        {
            List<Vector2> keys = navMap
                .Select(kvp => kvp.Key)
                .ToList();

            Vector2 position = keys[rng.Next(0, keys.Count)];

            return position;
        }
    }
}
