using cr_mono.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace cr_mono.Scenes
{
    internal class SettingsScene : Component
    {
        internal event EventHandler MenuRequested;

        internal override void LoadContent(ContentManager content)
        {
            //
        }

        internal override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                MenuRequested?.Invoke(this, EventArgs.Empty);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                if (Data.IsFullScreen)
                {
                    Data.IsFullScreen = false;
                    Core.Game1.graphics.IsFullScreen = false;
                    Core.Game1.graphics.ApplyChanges();
                }
                else {
                    Data.IsFullScreen = true;
                    Core.Game1.graphics.IsFullScreen = true;
                    Core.Game1.graphics.ApplyChanges();
                }
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            //
        }
    }
}