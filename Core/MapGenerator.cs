using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace cr_mono.Core
{
    // functions for generating the tilemap Dictionaries, and maybe some other stuff too
    // To just make cool shapes, I just need to look into/think about the math.
    // If I wanted structures on the map, that might need to be its own thing,
    //  could maybe use wave function collapse
    // This could also be where I create the texture store as some levels may be using
    //  different sprites etc.
    internal class MapGenerator
    {
        internal static Dictionary<Vector2, int> LoadTestMap()
        {
            Dictionary<Vector2, int> map = [];

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    map[new Vector2(x, y)] = 1;
                }
            }

            return map;
        }

        // temp function just for testing out different map shapes. The iterating here
        //  pretty bad and would scale horribly, but maps will stay real small so maybe Ok.
        internal static Dictionary<Vector2, int> WiderMap()
        {
            Dictionary<Vector2, int> map = new();
            int width = 4, height = 4;
            int extraWidth = 6;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[new Vector2(x, y)] = 1;
                }
            }

            for (int i = 1; i <= extraWidth; i++)
            {
                Dictionary<Vector2, int> newTiles = [];

                foreach (var tile in map.Keys.ToList())
                {
                    Vector2 leftExpansion = new(tile.X - i, tile.Y + i);
                    Vector2 rightExpansion = new(tile.X + i, tile.Y - i);

                    if (!map.ContainsKey(leftExpansion))
                        newTiles[leftExpansion] = 1;

                    if (!map.ContainsKey(rightExpansion))
                        newTiles[rightExpansion] = 1;
                }

                foreach (var newTile in newTiles)
                {
                    map[newTile.Key] = newTile.Value;
                }
            }

            var sortedMap = map.OrderBy(pair => pair.Key.Y)
                                .ThenBy(pair => pair.Key.X)
                                .ToDictionary();

            return sortedMap;
        }
    }
}
