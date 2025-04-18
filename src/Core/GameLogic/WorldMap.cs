using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace cr_mono.Core.GameLogic
{
    internal class WorldMap
    {
        internal Dictionary<Vector2, int> baseLayer, topLayer;
        internal Dictionary<Vector2, Structure> structures;
        internal Dictionary<Vector2, bool> navMap;
        internal List<Rectangle> textureStore;

        internal WorldMap(int size, RNG rng)
        {
            (baseLayer, topLayer) = MapGen.GenerateJaggedMap(size, rng);
            navMap = MapGen.GenerateNavMap(baseLayer, topLayer);
            structures = MapGen.AddStructures(navMap, topLayer, rng, 15);

            textureStore = [
                //new Rectangle(0, 0, 32, 32),
                //new Rectangle(32, 0, 32, 32),
                //new Rectangle(64, 0, 32, 32),
                //new Rectangle(96, 0, 32, 32),
                //new Rectangle(128, 0, 32, 32)
                new Rectangle(0, 0, 64, 64),
                new Rectangle(64, 0, 64, 64),
                new Rectangle(128, 0, 64, 64),
                new Rectangle(192, 0, 64, 64),
                new Rectangle(256, 0, 64, 64)
            ];
        }
    }
}
