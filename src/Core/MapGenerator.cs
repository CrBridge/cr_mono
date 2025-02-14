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
        internal static Dictionary<Vector2, int> DiamondLevel(int size)
        {
            Dictionary<Vector2, int> map = [];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    map[new Vector2(x, y)] = 1;
                }
            }

            return map;
        }

        internal static Dictionary<Vector2, int> JaggedLevel(int size)
        {
            Dictionary<Vector2, int> map = new();
            int width = (size-2)/2, height = size;

            for (int x = 0; x < height; x++)
            {
                if (x != 0)
                    map[new Vector2(x, x)] = 1;
                map[new Vector2(x + 1, x)] = 1;
                map[new Vector2(x, x + 1)] = 1;
            }
 
            for (int i = 1; i <= width; i++)
            {
                Dictionary<Vector2, int> newTiles = [];
            
                foreach (var tile in map.Keys.ToList())
                {
                    Vector2 leftExpansion = new(tile.X - 1, tile.Y + 1);
                    Vector2 rightExpansion = new(tile.X + 1, tile.Y - 1);
            
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
