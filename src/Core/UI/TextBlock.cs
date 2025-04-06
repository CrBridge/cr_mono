using cr_mono.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.UI
{
    internal class TextBlock
    {
        private TextArea text;
        private Button background;
        private Vector2 textOffset;

        internal TextBlock(Rectangle dst, string text, int textAreaWidth) 
        {
            this.background = new Button(dst, new Rectangle(128, 0, 192, 160));
            this.text = new TextArea(text, textAreaWidth);

            // calculate the distance between button left hand side and desired start of text
            Vector2 textOffsetFromButton = new Vector2((dst.Width - textAreaWidth) / 2, 18);
            Vector2 buttonCorner = new Vector2(dst.X, dst.Y);
            // gives an offset from the topleft screen corner (e.g. can be used as regular vec2)
            this.textOffset = buttonCorner + textOffsetFromButton;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            text.Draw(spriteBatch, textOffset);
        }
    }
}
