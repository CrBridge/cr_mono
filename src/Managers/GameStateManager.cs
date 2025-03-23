using System;
using cr_mono.Core;
using cr_mono.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Managers
{
    internal partial class GameStateManager : Component
    {
        private MenuScene ms;
        private GameScene gs;
        private SettingsScene ss;

        private ContentManager content;

        internal event EventHandler OnNewGame;
        internal event EventHandler OnReturnToMenu;
        internal event EventHandler OnSettings;

        internal GameStateManager(ContentManager content)
        {
            this.content = content;
            ms = new MenuScene();
            ms.NewGameRequested += (sender, e) => OnNewGame?.Invoke(this, EventArgs.Empty);

            OnNewGame += HandleNewGame;
            OnReturnToMenu += HandleReturnToMenu;
            OnSettings += HandleGoToSettings;
        }

        internal override void LoadContent(ContentManager content) {
            ms.LoadContent(content);
        }
        
        internal override void Update(GameTime gameTime) {
            switch (Data.CurrentScene) {
                case Data.Scenes.Menu:
                    ms?.Update(gameTime);
                    break;
                case Data.Scenes.Game:
                    gs?.Update(gameTime);
                    break;
                case Data.Scenes.Settings:
                    ss?.Update(gameTime);
                    break;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            switch (Data.CurrentScene)
            {
                case Data.Scenes.Menu:
                    ms?.Draw(spriteBatch);
                    break;
                case Data.Scenes.Game:
                    gs?.Draw(spriteBatch);
                    break;
                case Data.Scenes.Settings:
                    ss?.Draw(spriteBatch);
                    break;
            }
        }

        // At some point I will want functionality to load a game as well as start new,
        // I could handle this through another seperate event, or maybe using EventArgs
        private void HandleNewGame(object sender, EventArgs e)
        {
            ms = null;
            ss = null;
            gs = new GameScene();
            gs.MenuRequested += (sender, e) => OnReturnToMenu?.Invoke(this, EventArgs.Empty);
            gs.LoadContent(content);
            Data.CurrentScene = Data.Scenes.Game;
        }

        private void HandleReturnToMenu(object sender, EventArgs e)
        {
            gs = null;
            ss = null;
            ms = new MenuScene();
            ms.NewGameRequested += (sender, e) => OnNewGame?.Invoke(this, EventArgs.Empty);
            ms.SettingsRequested += (sender, e) => OnSettings?.Invoke(this, EventArgs.Empty);
            ms.LoadContent(content);
            Data.CurrentScene = Data.Scenes.Menu;
        }

        private void HandleGoToSettings(object sender, EventArgs e)
        {
            ms = null;
            gs = null;
            ss = new SettingsScene();
            ss.MenuRequested += (sender, e) => OnReturnToMenu?.Invoke(this, EventArgs.Empty);
            ss.LoadContent(content);
            Data.CurrentScene = Data.Scenes.Settings;
        }
    }
}
