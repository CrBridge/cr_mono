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
        bool isSelected = false;
        Rectangle buttonSrc;
        Rectangle buttonDst;

        internal override void LoadContent(ContentManager content)
        {
            buttonTexture = content.Load<Texture2D>("ui");
            buttonDst = new Rectangle(Data.NativeWidth / 2 - 128, Data.NativeHeight / 2 - 32, 256, 64);
            buttonSrc = new Rectangle(0, 0, 128, 32);
        }

        internal override void update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            Vector2 scaledPos = ms.Position.ToVector2() / (Data.ScreenWidth / Data.NativeWidth);

            if (buttonDst.Contains(scaledPos))
            {
                isSelected = true;
                if (ms.LeftButton == ButtonState.Pressed) {
                    Data.CurrentScene = Data.Scenes.Game;
                }
            }
            else {
                isSelected = false;
            }
        }

        internal override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (isSelected)
            {
                spriteBatch.Draw(buttonTexture, buttonDst, buttonSrc, Color.DarkGreen);
            }
            else {
                spriteBatch.Draw(buttonTexture, buttonDst, buttonSrc, Color.White);
            }
            spriteBatch.End();
        }
    }
}
