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

        // idea is that I have this, so that I can load scenes after runtime, using
        // events & delegates or some such so it looks nice
        private ContentManager content;
        internal event EventHandler OnNewGame;

        internal GameStateManager(ContentManager content)
        {
            this.content = content;
            ms = new MenuScene();
            ss = new SettingsScene();

            ms.NewGameRequested += (sender, e) => OnNewGame?.Invoke(this, EventArgs.Empty);//
            OnNewGame += HandleNewGame;
        }

        internal override void LoadContent(ContentManager content) {
            // with this new setup for creating the gameScene only when needed,
            // It might be a good call to have it like that for all
            // since this will just be loaded at all times even when not needed
            // Its ok for now since there is so little in here, it is kinda
            // unnessacary
            ms.LoadContent(content);
            ss.LoadContent(content);
        }
        
        internal override void Update(GameTime gameTime) {
            switch (Data.CurrentScene) {
                case Data.Scenes.Menu:
                    ms.Update(gameTime);
                    break;
                case Data.Scenes.Game:
                    gs?.Update(gameTime);
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
                    gs?.Draw(spriteBatch);
                    break;
                case Data.Scenes.Settings:
                    ss.Draw(spriteBatch);
                    break;
            }
        }

        // This will always create a new scene. This means that entering game scene
        // from the menu scene will always start a fresh game. I could have another event
        // for game loading, or maybe I can use EventArgs

        // to clarify, once I go from the Game to menu, the game is still in memory and
        // loaded even if it cannot be reaccessed in that state, I should have some way to
        // unload it when I return to menu (delete keyword to delete scene, + another event?)
        private void HandleNewGame(object sneder, EventArgs e)
        {
            gs = new GameScene();
            gs.LoadContent(content);
            Data.CurrentScene = Data.Scenes.Game;
        }
    }
}
