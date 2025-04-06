using System.Security.Cryptography;
using cr_mono.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.UI
{
    internal class TextButton : Button
    {
        protected string text;
        protected Vector2 textPos;
        protected Color textColor;
        protected static SpriteFont Font 
        {
            get { return ResourceManager.FontRegular; }
        }

        internal TextButton(Rectangle dst, Rectangle src, string text, Color color) 
            : base(dst, src) 
        {
            this.text = text;
            this.textColor = color;

            Vector2 textLength = Font.MeasureString(text);
            this.textPos = new Vector2(
                dst.X + (dst.Width - textLength.X) / 2,
                dst.Y + (dst.Height - textLength.Y) / 2);
        }

        internal override void Draw(SpriteBatch spriteBatch) 
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(Font, text, textPos, textColor);
        }
    }
}
