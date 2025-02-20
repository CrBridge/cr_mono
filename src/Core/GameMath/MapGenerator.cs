using cr_mono.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace cr_mono.Core.GameMath
{
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

            // adding noise
            float[][] noise = Noise.GeneratePerlinNoise(2, size, size, 5);
            foreach (var tile in map) {
                if (noise[(int)tile.Key.X][(int)tile.Key.Y] >= 0.5) {
                    map[tile.Key] = 2;
                }
                if (noise[(int)tile.Key.X][(int)tile.Key.Y] <= 0.3)
                {
                    map[tile.Key] = 3;
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

            int minX = (int)map.Keys.Min(tile => tile.X);
            int minY = (int)map.Keys.Min(tile => tile.Y);

            int noiseSize = size * 2 - 1;
            float[][] noise = Noise.GeneratePerlinNoise(3, noiseSize, noiseSize, 6);
            foreach (var tile in map.Keys.ToList())
            {
                int noiseX = (int)tile.X - minX;
                int noiseY = (int)tile.Y - minY;

                if (noise[noiseX][noiseY] >= 0.5)
                {
                    map[tile] = 3;
                }
                if (noise[noiseX][noiseY] <= 0.3)
                {
                    map[tile] = 3;
                }
            }

            var sortedMap = map.OrderBy(pair => pair.Key.Y)
                                .ThenBy(pair => pair.Key.X)
                                .ToDictionary();

            return sortedMap;
        }
    }
}
