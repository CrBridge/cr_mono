﻿using cr_mono.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cr_mono.Core.GameLogic
{
    internal static class MapGen
    {
        internal static List<Dictionary<Vector2, int>> GenerateDiamondMap(
            int size,
            RNG rng)
        {
            Dictionary<Vector2, int> baseLayer = [];
            Dictionary<Vector2, int> topLayer = [];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    baseLayer[new Vector2(x, y)] = 0;
                }
            }

            // adding noise
            float[][] noise = Noise.GeneratePerlinNoise(rng, size, size, 5);
            foreach (var tile in baseLayer) {
                if (noise[(int)tile.Key.X][(int)tile.Key.Y] >= 0.5) {
                    baseLayer[tile.Key] = 1;
                }
                if (noise[(int)tile.Key.X][(int)tile.Key.Y] <= 0.3)
                {
                    topLayer[tile.Key] = 2;
                }
            }

            return new List<Dictionary<Vector2, int>> { baseLayer, topLayer };
        }

        internal static List<Dictionary<Vector2, int>> GenerateJaggedMap(
            int size, 
            RNG rng)
        {
            Dictionary<Vector2, int> baseLayer = new();
            Dictionary<Vector2, int> topLayer = new();
            int width = (size-2)/2, height = size;

            for (int x = 0; x < height; x++)
            {
                if (x != 0)
                    baseLayer[new Vector2(x, x)] = 0;
                baseLayer[new Vector2(x + 1, x)] = 0;
                baseLayer[new Vector2(x, x + 1)] = 0;
            }
 
            for (int i = 1; i <= width; i++)
            {
                Dictionary<Vector2, int> newTiles = [];
            
                foreach (var tile in baseLayer.Keys.ToList())
                {
                    Vector2 leftExpansion = new(tile.X - 1, tile.Y + 1);
                    Vector2 rightExpansion = new(tile.X + 1, tile.Y - 1);
            
                    if (!baseLayer.ContainsKey(leftExpansion))
                        newTiles[leftExpansion] = 0;
            
                    if (!baseLayer.ContainsKey(rightExpansion))
                        newTiles[rightExpansion] = 0;
                }
            
                foreach (var newTile in newTiles)
                {
                    baseLayer[newTile.Key] = newTile.Value;
                }
            }

            int minX = (int)baseLayer.Keys.Min(tile => tile.X);
            int minY = (int)baseLayer.Keys.Min(tile => tile.Y);

            int noiseSize = size * 2 - 1;
            // noisemap for ocean and mountain generation
            float[][] heightNoise = Noise.GeneratePerlinNoise(rng, noiseSize, noiseSize, 6);
            foreach (var tile in baseLayer.Keys.ToList())
            {
                int noiseX = (int)tile.X - minX;
                int noiseY = (int)tile.Y - minY;

                if (heightNoise[noiseX][noiseY] >= 0.6)
                {
                    baseLayer[tile] = 1;
                }
                if (heightNoise[noiseX][noiseY] <= 0.3)
                {
                    topLayer[tile] = 2;
                }
            }

            RefineBaseLayer(baseLayer, 10, 0, 1);
            RefineBaseLayer(baseLayer, 10, 1, 0);

            // noisemap for forest generation, lower octaves as it can look a little more random
            float[][] treeNoise = Noise.GeneratePerlinNoise(rng, noiseSize, noiseSize, 4);
            foreach (var tile in baseLayer.Keys.ToList())
            {
                int noiseX = (int)tile.X - minX;
                int noiseY = (int)tile.Y - minY;

                if (treeNoise[noiseX][noiseY] <= 0.3 && baseLayer[tile] != 1 && !topLayer.ContainsKey(tile))
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

            return new List<Dictionary<Vector2, int>> { sortedBase, sortedTop };
        }

        internal static Dictionary<Vector2, bool> GenerateNavMap(
            Dictionary<Vector2, int> baseLayer, 
            Dictionary<Vector2, int> topLayer)
        {
            Dictionary<Vector2, bool> navMap = new();

            foreach (var tile in baseLayer) {
                if (tile.Value == 0 && !(topLayer.TryGetValue(tile.Key, out int value) && value == 2))
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
            RNG rng) {
            List<Vector2> keys = navMap
                .Where(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();

            Vector2 position = keys[rng.Next(0, keys.Count)];

            return position;
        }

        // given a position on the map, returns a list
        // of all surrounding tiles of the same value
        internal static List<Vector2> FloodFillTiles(
            Dictionary<Vector2, int> layer,
            Vector2 startPosition,
            HashSet<Vector2> visited)
        {
            int startValue = layer[startPosition];
            List<Vector2> tiles = new();

            if (!layer.ContainsKey(startPosition))
            {
                return tiles;
            }

            Queue<Vector2> queue = new();
            queue.Enqueue(startPosition);
            tiles.Add(startPosition);
            visited.Add(startPosition);

            Vector2[] directions = {
                new Vector2(1, 0), new Vector2(-1, 0),
                new Vector2(0, 1), new Vector2(0, -1)
            };

            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();

                foreach (var direction in directions)
                {
                    Vector2 neighbor = current + direction;

                    if (layer.TryGetValue(neighbor, out int value) && value == startValue &&
                        !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        tiles.Add(neighbor);
                    }
                }
            }

            return tiles;
        }

        // function to iron out noisy sections of the base layer
        // e.g. small, enclosed areas of land or water
        // effectively removes small islands and lakes that would mostly cause issue
        internal static void RefineBaseLayer(
            Dictionary<Vector2, int> layer,
            int threshold,
            int target,
            int replacement) 
        {
            HashSet<Vector2> visited = new();

            foreach (var tile in layer.Keys.ToList())
            {
                if (!visited.Contains(tile) && layer[tile] == target)
                {
                    List<Vector2> floodTiles = FloodFillTiles(layer, tile, visited);

                    if (floodTiles.Count < threshold)
                    {
                        foreach (var pos in floodTiles)
                        {
                            layer[pos] = replacement;
                        }
                    }
                }
            }
        }

        // I guess I should do something like 10 towers, 10 caves, 10 settlements or something
        //  I could just run this 3 times, adding an argument for TileType,
        //  but if I do that, i gotta make sure It wont overwrite tiles if it picks
        //  a tile that already contains a structure
        //  also gotta think about other stuff. e.g. For cave tiles, should they have to be
        //  by mountains? If a settlement spawns by the water, should I add a dock/boat tile?
        internal static Dictionary<Vector2, Structure> AddStructures(
            Dictionary<Vector2, bool> navMap,
            Dictionary<Vector2, int> topLayer,
            RNG rng,
            int amount) 
        {
            Dictionary<Vector2, Structure> structures = [];
            List<Vector2> usedPositions = [];

            int i = 0, j = 0;
            while (i < amount && j < amount * 2)
            {
                Vector2 structurePos = GetRandomMapPos(navMap, rng);
                if (!usedPositions.Contains(structurePos)) 
                {
                    topLayer[structurePos] = 4;
                    structures[structurePos] = new Structure(i);
                    usedPositions.Add(structurePos);
                    i++;
                }
                // safeguard, in the event of horrible rng, this makes sure
                // it gives up after alot of goes of it
                j++;
            }

            return structures;
        }
    }
}
