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
        private InteractiveButton button;

        internal override void LoadContent(ContentManager content)
        {
            Rectangle buttonDst = new Rectangle(Data.NativeWidth / 2 - 64, Data.NativeHeight / 2 - 16, 128, 32);
            Rectangle buttonSrc = new Rectangle(0, 0, 128, 32);

            button = new InteractiveButton(
                buttonDst, buttonSrc, "Start Game", Color.Black, Color.White, Color.DarkGreen);
        }

        internal override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            int screenWidth = Data.IsFullScreen ? 
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width : Data.ScreenWidth;
            Vector2 scaledPos = ms.Position.ToVector2() /
                (screenWidth / (float)Data.NativeWidth);

            if (button.Pressed(ms, scaledPos)) {
                Data.CurrentScene = Data.Scenes.Game;
            }

            if (Data.currentKeyboardState.IsKeyDown(Keys.Escape)) {
                Data.Exit = true;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            button.Draw(spriteBatch);
            
            spriteBatch.End();
        }
    }
}
