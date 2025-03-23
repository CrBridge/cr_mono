using cr_mono.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace cr_mono.Scenes
{
    internal class GameScene : Component
    {
        enum Scenes
        {
            WorldMap,
            Dungeon,
            Village,
            Debug3D
        }

        internal event EventHandler MenuRequested;

        private Scenes scene;
        WorldScene ws;

        private void HandleReturnToMenu(object sender, EventArgs e)
        {
            MenuRequested?.Invoke(this, EventArgs.Empty);
        }

        internal override void LoadContent(ContentManager content)
        {
            scene = Scenes.WorldMap;
            ws = new WorldScene();
            ws.MenuRequested += HandleReturnToMenu;
            ws.LoadContent(content);
        }

        internal override void Update(GameTime gameTime)
        {
            switch (scene)
            {
                case Scenes.WorldMap:
                    ws.Update(gameTime);
                    break;
                case Scenes.Dungeon:
                    break;
                case Scenes.Village:
                    break;
                case Scenes.Debug3D:
                    break;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            switch (scene)
            {
                case Scenes.WorldMap:
                    ws.Draw(spriteBatch);
                    break;
                case Scenes.Dungeon:
                    break;
                case Scenes.Village:
                    break;
                case Scenes.Debug3D:
                    break;
            }
        }
    }
}