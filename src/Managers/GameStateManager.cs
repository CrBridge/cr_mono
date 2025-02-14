using cr_mono.Core;
using cr_mono.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Managers
{
    internal partial class GameStateManager : Component
    {
        private MenuScene ms = new MenuScene();
        private GameScene gs = new GameScene();
        private SettingsScene ss = new SettingsScene();

        internal override void LoadContent(ContentManager content) {
            ms.LoadContent(content);
            gs.LoadContent(content);
            ss.LoadContent(content);
        }
        
        internal override void update(GameTime gameTime) {
            switch (Data.CurrentScene) {
                case Data.Scenes.Menu:
                    ms.update(gameTime);
                    break;
                case Data.Scenes.Game:
                    gs.update(gameTime);
                    break;
                case Data.Scenes.Settings:
                    ss.update(gameTime);
                    break;
            }
        }

        internal override void draw(SpriteBatch spriteBatch) {
            switch (Data.CurrentScene)
            {
                case Data.Scenes.Menu:
                    ms.draw(spriteBatch);
                    break;
                case Data.Scenes.Game:
                    gs.draw(spriteBatch);
                    break;
                case Data.Scenes.Settings:
                    ss.draw(spriteBatch);
                    break;
            }
        }
    }
}
