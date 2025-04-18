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
        private Dictionary<Vector2, int> layer;
        private List<Rectangle> textureStore;
        private Camera2D camera;

        internal DungeonScene(GameContext context, GameSubSceneManager manager, int structureId)
        {
            this.context = context;
            this.manager = manager;

            // generate a small grid and get testing
            //layer = ProcGenHelpers.GenerateGrid(10);
            layer = DungeonGen.RandomWalk(Data.RNG, new Vector2(0, 0), 500);
            camera = new Camera2D();
            textureStore = [
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
            foreach (var item in layer)
            {
                Rectangle dst = Tile.IsometricToPixel(item.Key, camera, 0);
                Rectangle src = textureStore[item.Value];

                spriteBatch.Draw(dungeonTileSet, dst, src, Color.White);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
