using cr_mono.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cr_mono.Core.GameMath
{
    internal class WorldLogic
    {
        internal static (Dictionary<Vector2, int>, Dictionary<Vector2, int>) GenerateDiamondMap(
            int size,
            Random rng)
        {
            Dictionary<Vector2, int> baseLayer = [];
            Dictionary<Vector2, int> topLayer = [];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    baseLayer[new Vector2(x, y)] = 1;
                }
            }

            // adding noise
            float[][] noise = Noise.GeneratePerlinNoise(rng, size, size, 5);
            foreach (var tile in baseLayer) {
                if (noise[(int)tile.Key.X][(int)tile.Key.Y] >= 0.5) {
                    baseLayer[tile.Key] = 2;
                }
                if (noise[(int)tile.Key.X][(int)tile.Key.Y] <= 0.3)
                {
                    topLayer[tile.Key] = 3;
                }
            }

            return (baseLayer, topLayer);
        }

        internal static (Dictionary<Vector2, int>, Dictionary<Vector2, int>) GenerateJaggedMap(
            int size, 
            Random rng)
        {
            Dictionary<Vector2, int> baseLayer = new();
            Dictionary<Vector2, int> topLayer = new();
            int width = (size-2)/2, height = size;

            for (int x = 0; x < height; x++)
            {
                if (x != 0)
                    baseLayer[new Vector2(x, x)] = 1;
                baseLayer[new Vector2(x + 1, x)] = 1;
                baseLayer[new Vector2(x, x + 1)] = 1;
            }
 
            for (int i = 1; i <= width; i++)
            {
                Dictionary<Vector2, int> newTiles = [];
            
                foreach (var tile in baseLayer.Keys.ToList())
                {
                    Vector2 leftExpansion = new(tile.X - 1, tile.Y + 1);
                    Vector2 rightExpansion = new(tile.X + 1, tile.Y - 1);
            
                    if (!baseLayer.ContainsKey(leftExpansion))
                        newTiles[leftExpansion] = 1;
            
                    if (!baseLayer.ContainsKey(rightExpansion))
                        newTiles[rightExpansion] = 1;
                }
            
                foreach (var newTile in newTiles)
                {
                    baseLayer[newTile.Key] = newTile.Value;
                }
            }

            int minX = (int)baseLayer.Keys.Min(tile => tile.X);
            int minY = (int)baseLayer.Keys.Min(tile => tile.Y);
            int maxX = (int)baseLayer.Keys.Max(tile => tile.X);
            int maxY = (int)baseLayer.Keys.Max(tile => tile.Y);

            int centreX = (minX + maxX) / 2;
            int centreY = (minY + maxY) / 2;
            Vector2 centre = new Vector2(centreX, centreY);

            int noiseSize = size * 2 - 1;
            float[][] noise = Noise.GeneratePerlinNoise(rng, noiseSize, noiseSize, 6);
            foreach (var tile in baseLayer.Keys.ToList())
            {
                int noiseX = (int)tile.X - minX;
                int noiseY = (int)tile.Y - minY;

                if (noise[noiseX][noiseY] >= 0.5)
                {
                    baseLayer[tile] = 2;
                }
                if (noise[noiseX][noiseY] <= 0.3)
                {
                    topLayer[tile] = 3;
                }
            }

            var sortedBase = baseLayer.OrderBy(pair => pair.Key.Y)
                                .ThenBy(pair => pair.Key.X)
                                .ToDictionary();
            var sortedTop = topLayer.OrderBy(pair => pair.Key.Y)
                                .ThenBy(pair => pair.Key.X)
                                .ToDictionary();

            return (sortedBase, sortedTop);
        }

        internal static Dictionary<Vector2, bool> GenerateNavMap(
            Dictionary<Vector2, int> baseLayer, 
            Dictionary<Vector2, int> topLayer)
        {
            Dictionary<Vector2, bool> navMap = new();

            foreach (var tile in baseLayer) {
                if (tile.Value == 1 && !topLayer.ContainsKey(tile.Key))
                {
                    navMap[tile.Key] = true;
                }
                else {
                    navMap[tile.Key] = false;
                }
            }

            return navMap;
        }

        internal static Vector2 GetRandomMapPos(
            Dictionary<Vector2, bool> navMap,
            Random rng) {
            List<Vector2> keys = navMap
                .Where(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();

            Vector2 position = keys[rng.Next(keys.Count)];

            return position;
        }
    }
}
