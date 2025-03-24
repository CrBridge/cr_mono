using cr_mono.Core;
using cr_mono.src.Scenes;
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
        Debug3dScene debug;

        internal override void LoadContent(ContentManager content)
        {
            scene = Scenes.Debug3D;
            //ws = new WorldScene();
            //ws.MenuRequested += HandleReturnToMenu;
            //ws.LoadContent(content);
            debug = new Debug3dScene();
            debug.MenuRequested += HandleReturnToMenu;
            debug.LoadContent(content);
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
                    debug.Update(gameTime);
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
                    debug.Draw(spriteBatch);
                    break;
            }
        }

        private void HandleReturnToMenu(object sender, EventArgs e)
        {
            MenuRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}