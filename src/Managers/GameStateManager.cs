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
        
        internal override void Update(GameTime gameTime) {
            switch (Data.CurrentScene) {
                case Data.Scenes.Menu:
                    ms.Update(gameTime);
                    break;
                case Data.Scenes.Game:
                    gs.Update(gameTime);
                    break;
                case Data.Scenes.Settings:
                    ss.Update(gameTime);
                    break;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            switch (Data.CurrentScene)
            {
                case Data.Scenes.Menu:
                    ms.Draw(spriteBatch);
                    break;
                case Data.Scenes.Game:
                    gs.Draw(spriteBatch);
                    break;
                case Data.Scenes.Settings:
                    ss.Draw(spriteBatch);
                    break;
            }
        }
    }
}
