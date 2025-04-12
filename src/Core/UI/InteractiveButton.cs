using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Core.UI
{
    internal class InteractiveButton : TextButton
    {
        private bool isSelected;
        private Color selectedButtonColor;
        private Color selectedTextColor;

        private Rectangle selectedDst;

        internal InteractiveButton(
            Rectangle dst,
            Rectangle src,
            string text,
            Color textColor,
            Color selectedTextColor,
            Color selectedBtnColor,
            bool isSmall = true)
            : base(dst, src, text, textColor, isSmall) 
        {
            this.selectedButtonColor = selectedBtnColor;
            this.selectedTextColor = selectedTextColor;

            int selectedWidth = (int)(dst.Width * 1.2f);
            int selectedHeight = (int)(dst.Height * 1.2f);

            this.selectedDst = new Rectangle(
                dst.Center.X - selectedWidth / 2,
                dst.Center.Y - selectedHeight / 2,
                selectedWidth,
                selectedHeight);
        }

        internal bool Pressed(MouseState ms, Vector2 mousePos)
        {
            Rectangle bounds = isSelected ? selectedDst : dst;

            if (bounds.Contains(mousePos))
            {
                isSelected = true;
                if (ms.LeftButton == ButtonState.Pressed) {
                    return true;
                }
                return false;
            }
            else
            {
                isSelected = false;
                return false;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (isSelected) 
            {
                spriteBatch.Draw(Texture, selectedDst, src, selectedButtonColor);
                spriteBatch.DrawString(Font, text, textPos, selectedTextColor);
            }
            else 
            {
                spriteBatch.Draw(Texture, dst, src, Color.White);
                spriteBatch.DrawString(Font, text, textPos, textColor);
            }
        }
    }
}
