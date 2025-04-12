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

        private readonly ContentManager content;

        internal event EventHandler OnNewGame;
        internal event EventHandler OnReturnToMenu;
        internal event EventHandler OnSettings;

        private readonly EventHandler newGameHandler;
        private readonly EventHandler settingsHandler;
        private readonly EventHandler returnToMenuHandler;

        internal GameStateManager(ContentManager content)
        {
            this.content = content;
            ms = new MenuScene();
            newGameHandler = (sender, e) => OnNewGame?.Invoke(this, EventArgs.Empty);
            settingsHandler = (sender, e) => OnSettings?.Invoke(this, EventArgs.Empty);
            returnToMenuHandler = (sender, e) => OnReturnToMenu?.Invoke(this, EventArgs.Empty);

            ms.NewGameRequested += newGameHandler;
            ms.SettingsRequested += settingsHandler;

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
            DecoupleMenuEvents();
            DecoupleSettingsEvents();
            gs = new GameScene();
            gs.MenuRequested += returnToMenuHandler;
            gs.LoadContent(content);
            Data.CurrentScene = Data.Scenes.Game;
        }

        private void HandleReturnToMenu(object sender, EventArgs e)
        {
            DecoupleGameEvents();
            DecoupleSettingsEvents();
            ms = new MenuScene();
            ms.NewGameRequested += newGameHandler;
            ms.SettingsRequested += settingsHandler;
            ms.LoadContent(content);
            Data.CurrentScene = Data.Scenes.Menu;
        }

        private void HandleGoToSettings(object sender, EventArgs e)
        {
            DecoupleMenuEvents();
            DecoupleGameEvents();
            ss = new SettingsScene();
            ss.MenuRequested += returnToMenuHandler;
            ss.LoadContent(content);
            Data.CurrentScene = Data.Scenes.Settings;
        }

        private void DecoupleMenuEvents() 
        {
            if (ms != null)
            {
                ms.NewGameRequested -= newGameHandler;
                ms.SettingsRequested -= settingsHandler;
                ms = null;
            }
        }

        private void DecoupleSettingsEvents() 
        {
            if (ss != null)
            {
                ss.MenuRequested -= returnToMenuHandler;
                ss = null;
            }
        }

        private void DecoupleGameEvents() 
        {
            if (gs != null)
            {
                gs.MenuRequested -= returnToMenuHandler;
                gs = null;
            }
        }
    }
}
