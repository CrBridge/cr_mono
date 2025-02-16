using cr_mono.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Scenes
{
    internal class MenuScene : Component
    {
        Texture2D buttonTexture;
        private int selectedButton = -1;
        Rectangle buttonSrc;
        Rectangle buttonDst;
        Rectangle buttonSelectedDst;

        internal override void LoadContent(ContentManager content)
        {
            buttonTexture = content.Load<Texture2D>("Textures/ui");
            buttonDst = new Rectangle(Data.NativeWidth / 2 - 64, Data.NativeHeight / 2 - 16, 128, 32);
            buttonSelectedDst = new Rectangle(Data.NativeWidth / 2 - 72, Data.NativeHeight / 2 - 18, 144, 36);
            buttonSrc = new Rectangle(0, 0, 128, 32);
        }

        internal override void update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            int screenWidth = Data.IsFullScreen ? 
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width : Data.ScreenWidth;
            Vector2 scaledPos = ms.Position.ToVector2() /
                (screenWidth / (float)Data.NativeWidth);

            if (buttonDst.Contains(scaledPos))
            {
                selectedButton = 0;
                if (ms.LeftButton == ButtonState.Pressed) {
                    Data.CurrentScene = Data.Scenes.Game;
                }
            }
            else {
                selectedButton = -1;
            }
        }

        internal override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (selectedButton == 0)
            {
                spriteBatch.Draw(buttonTexture, buttonSelectedDst, buttonSrc, Color.DarkGreen);
            }
            else {
                spriteBatch.Draw(buttonTexture, buttonDst, buttonSrc, Color.White);
            }
            spriteBatch.End();
        }
    }
}
