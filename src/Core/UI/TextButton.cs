using System.Security.Cryptography;
using cr_mono.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.UI
{
    internal class TextButton : Button
    {
        protected SpriteFont font;
        protected string text;
        protected Vector2 textPos;
        protected Color textColor;

        internal TextButton(Rectangle dst, Rectangle src, string text, Color color) 
            : base(dst, src) 
        {
            this.text = text;
            this.font = ResourceManager.FontRegular;
            this.textColor = color;

            Vector2 textLength = font.MeasureString(text);
            this.textPos = new Vector2(
                dst.X + (dst.Width - textLength.X) / 2,
                dst.Y + (dst.Height - textLength.Y) / 2);
        }

        internal override void Draw(SpriteBatch spriteBatch) 
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(font, text, textPos, textColor);
        }
    }
}
