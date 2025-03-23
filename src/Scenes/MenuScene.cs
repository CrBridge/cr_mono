using System;
using System.Collections.Generic;
using cr_mono.Core;
using cr_mono.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Scenes
{
    internal class MenuScene : Component
    {
        internal event EventHandler NewGameRequested;
        internal event EventHandler SettingsRequested;

        private readonly List<InteractiveButton> buttons = [];

        internal override void LoadContent(ContentManager content)
        {
            Rectangle buttonSrc = new Rectangle(0, 0, 128, 32);
            Rectangle[] buttonsDst = new Rectangle[3];
            string[] buttonTexts = ["New Game", "Settings", "Exit"];

            for (int i = 0; i < buttonsDst.Length; i++) {
                buttonsDst[i] = new Rectangle(
                    Data.NativeWidth / 2 - 64, Data.NativeHeight / 2 - 64 + (i * 48), 128, 32);
                buttons.Add(new InteractiveButton(
                    buttonsDst[i], buttonSrc, buttonTexts[i], Color.Black, Color.White, Color.DarkGreen));
            }
        }

        internal override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            int screenWidth = Data.IsFullScreen ? 
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width : Data.ScreenWidth;
            Vector2 scaledPos = ms.Position.ToVector2() /
                (screenWidth / (float)Data.NativeWidth);

            if (buttons[0].Pressed(ms, scaledPos)) {
                // worth noting, this event creates a new instance of GameScene when invoked
                // menu scene doesnt simply pause the game anymore
                NewGameRequested?.Invoke(this, EventArgs.Empty);
            }

            if (buttons[1].Pressed(ms, scaledPos))
            {
                //Data.CurrentScene = Data.Scenes.Settings;
                SettingsRequested?.Invoke(this, EventArgs.Empty);
            }

            if (buttons[2].Pressed(ms, scaledPos)) {
                Data.Exit = true;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var button in buttons) {
                button.Draw(spriteBatch);
            }
            
            spriteBatch.End();
        }
    }
}
