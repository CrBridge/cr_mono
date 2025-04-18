using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cr_mono.Core;
using cr_mono.Core.GameLogic;
using cr_mono.Managers;
using cr_mono.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.src.Scenes.GameScenes
{
    internal class DungeonScene : IGameSubScene
    {
        private GameContext context;
        private GameSubSceneManager manager;

        private Texture2D dungeonTileSet;
        private List<Dictionary<Vector2, int>> layers;
        private List<Rectangle> textureStore;
        private Camera2D camera;

        internal DungeonScene(GameContext context, GameSubSceneManager manager, int structureId)
        {
            this.context = context;
            this.manager = manager;

            layers = DungeonGen.CreateWalledDungeon(Data.RNG, Vector2.Zero, 15, 40);
            camera = new Camera2D();
            textureStore = [
                new Rectangle(0, 0, 64, 64),
                new Rectangle(64, 0, 64, 64)
            ];
        }

        public void LoadContent(ContentManager content)
        {
            dungeonTileSet = content.Load<Texture2D>("Textures/DungeonSet");
        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            camera.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                RenderLayer(spriteBatch, layers[i], i);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        private void RenderLayer(
            SpriteBatch spriteBatch,
            Dictionary<Vector2, int> layer,
            int layerNumber) 
        {
            foreach (var item in layer) 
            {
                Rectangle dst = Tile.IsometricToPixel(item.Key, camera, layerNumber);
                Rectangle src = textureStore[item.Value];

                spriteBatch.Draw(dungeonTileSet, dst, src, Color.White);
            }
        }
    }
}
