using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.GameLogic
{
    internal class WorldMap
    {
        //internal Dictionary<Vector2, int> baseLayer, topLayer;
        internal List<Dictionary<Vector2, int>> layers;
        internal Dictionary<Vector2, Structure> structures;
        internal Dictionary<Vector2, bool> navMap;
        internal List<Rectangle> textureStore;

        internal WorldMap(int size, RNG rng)
        {
            layers = MapGen.GenerateJaggedMap(size, rng);
            navMap = MapGen.GenerateNavMap(layers[0], layers[1]);
            structures = MapGen.AddStructures(navMap, layers[1], rng, 15);

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

        // beyond what is in this class, it would need access
        //  to the camera, playerPos, texture2D and WorldTime
        //  worldTime one is interesting, that is unique to 
        //  the map scene, so I'm still thinking on how to handle that

        // In some scenes, I might not need access to player position if
        // I know that I wouldnt need the code to alpha blend
        //  e.g. the player will never be obstructed
        // even then, I imagine a camera, player and texture
        // will be so universal to all the scenes using this that
        // I could probably have some kind of SceneInfo struct/class
        // that stores these things.

        // i'll come back to this when im more certain what the end product
        // of the different scenes will be like
        private void RenderLayer(
        SpriteBatch spritebatch,
        Dictionary<Vector2, int> layer,
        int layerNumber)
        { }
        }
}
