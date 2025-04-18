using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace cr_mono.Core.GameLogic
{
    internal static class DungeonGen
    {
        internal static List<Dictionary<Vector2, int>> CreateWalledDungeon(
            RNG rng, Vector2 startPosition, int walkLength, int iterations) 
        {
            HashSet<Vector2> floor = IterativeWalk(rng, startPosition, walkLength, iterations);
            List<Vector2> directions = ProcGenHelpers.GetCardinalDirections();

            HashSet<Vector2> walls = [];

            foreach (Vector2 tile in floor)
            {
                foreach (Vector2 direction in directions)
                {
                    Vector2 neighbor = tile + direction;
                    if (!floor.Contains(neighbor))
                    {
                        walls.Add(neighbor);
                    }
                }
            }

            Dictionary<Vector2, int> baseLayer = floor.ToDictionary(key => key, value => 0)
                .Concat(walls.ToDictionary(key => key, value => 1))
                .OrderBy(pair => pair.Key.Y)
                .ThenBy(pair => pair.Key.X)
                .ToDictionary();
            Dictionary<Vector2, int> topLayer = walls.ToDictionary(key => key, value => 1)
                .OrderBy(pair => pair.Key.Y)
                .ThenBy(pair => pair.Key.X)
                .ToDictionary();

            return new List<Dictionary<Vector2, int>> { baseLayer, topLayer };
        }

        private static HashSet<Vector2> IterativeWalk(RNG rng, Vector2 startPosition, int walkLength, int iterations)
        {
            Vector2 currentPosition = startPosition;
            HashSet<Vector2> hashmap = [];
        
            for (int i = 0; i < iterations; i++)
            {
                HashSet<Vector2> walk = RandomWalk(rng, currentPosition, walkLength);
                hashmap.UnionWith(walk);
                currentPosition = hashmap.ElementAt(rng.Next(0, hashmap.Count));
            }

            return hashmap;
        }

        private static HashSet<Vector2> RandomWalk(RNG rng, Vector2 startPosition, int walkLength) 
        {
            // we use hashsets here because we need to append many of these walks together, which will contain
            // some duplicate keys. The code to manually add 2 dictionaries while avoiding duplicate keys would be
            // slower, and its easier to just initially work with hashmaps, then convert to dict at the end
            HashSet<Vector2> walk = [];
            walk.Add(startPosition);
            Vector2 previousPosition = startPosition;

            List<Vector2> directions = ProcGenHelpers.GetCardinalDirections();

            for (int i = 0; i < walkLength; i++)
            {
                Vector2 direction = directions[rng.Next(0, directions.Count)];
                Vector2 newPosition = previousPosition + direction;
                walk.Add(newPosition);
                previousPosition = newPosition;
            }

            return walk;
        }
    }
}
