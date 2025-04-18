using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace cr_mono.Core.GameLogic
{
    internal static class DungeonGen
    {
        internal static Dictionary<Vector2, int> RandomWalk(RNG rng, Vector2 startPosition, int walkLength) 
        {
            // have to account for duplicate positions due to walker walking over itself
            // I think that depending on how I do it, i'll just be overwriting, which should be fine
            // need to sort
            Dictionary<Vector2, int> walk = [];
            walk[startPosition] = 0;
            Vector2 previousPosition = startPosition;

            var directions = ProcGenHelpers.GetCardinalDirections();

            for (int i = 0; i < walkLength; i++)
            {
                Vector2 direction = directions[rng.Next(0, directions.Count)];
                Vector2 newPosition = previousPosition + direction;
                walk[newPosition] = 0;
                previousPosition = newPosition;
            }

            walk = walk.OrderBy(pair => pair.Key.Y)
                    .ThenBy(pair => pair.Key.X)
                    .ToDictionary();

            return walk;
        }
    }
}
