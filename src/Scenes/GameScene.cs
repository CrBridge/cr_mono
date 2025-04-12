using System;
using System.Collections.Generic;
using cr_mono.Core;
using cr_mono.Core.GameLogic;
using cr_mono.Core.UI;
using cr_mono.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Scenes 
{
    internal class GameScene : Component, IDisposable
    {
        private GameContext context;
        private GameSubSceneManager subSceneManager;
        private PauseScene pauseMenu;

        internal event EventHandler MenuRequested;

        internal GameScene() 
        {
            context = new GameContext();
            subSceneManager = new GameSubSceneManager(context);
            pauseMenu = new PauseScene();
        }

        internal override void LoadContent(ContentManager content)
        {
            subSceneManager.LoadContent(content);
        }

        internal override void Update(GameTime gameTime)
        {
            bool escapePressed = (Data.CurrentKeyboardState.IsKeyDown(Keys.Escape) &&
                !Data.PreviousKeyboardState.IsKeyDown(Keys.Escape));

            if (Data.CurrentKeyboardState.IsKeyDown(Keys.Escape) && !Data.PreviousKeyboardState.IsKeyDown(Keys.Escape))
            {
                pauseMenu.isPaused = true;
            }

            MouseState ms = Mouse.GetState();
            int screenWidth = Data.IsFullScreen ?
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width : Data.ScreenWidth;
            Vector2 scaledPos = ms.Position.ToVector2() /
                (screenWidth / (float)Data.NativeWidth);

            if (pauseMenu.isPaused) 
            {
                if (pauseMenu.Pressed(0, ms, scaledPos)) { pauseMenu.isPaused = false; }
                // todo! add code so these 2 handle going to settings and main menu
                // also want a save button in the future
                // in the current implementation, going to settings will cause this scene to be emptied, which
                // I dont really want. I should either change the code so going from game to settings
                // doesnt null the game scene, or I could have a new settings subscene specific to game
                // which would have less functionality (e.g. maybe just audio and fullscreening etc.)
                //      I like this idea, because some future settings could be a real hassle/maybe undoable
                //      while already in game
                if (pauseMenu.Pressed(1, ms, scaledPos)) { }
                if (pauseMenu.Pressed(2, ms, scaledPos)) 
                {
                    MenuRequested?.Invoke(this, EventArgs.Empty);
                }
            }
            else { subSceneManager.Update(gameTime); }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            subSceneManager.Draw(spriteBatch);

            if (pauseMenu.isPaused)
                pauseMenu.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Dispose() => subSceneManager.Dispose();
    }
}