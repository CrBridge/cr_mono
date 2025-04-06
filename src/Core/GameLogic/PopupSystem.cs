using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cr_mono.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Core.GameLogic
{
    internal class PopupSystem
    {
        internal bool isActive;
        private TextBlock popupInfo;
        private List<InteractiveButton> popupButtons;

        internal PopupSystem() 
        {
            this.isActive = false;
        }

        internal void Trigger(Structure structure)
        {
            isActive = true;
            int x = (Data.NativeWidth - 192) / 2;
            int y = (Data.NativeHeight - 160) / 2;
            popupInfo = new TextBlock(new Rectangle(x, y, 192, 160), $"Structure id is {structure.id} do you enter?", 100);
            // challenge now is how I can position the buttons. Idea for now is just positioning them from the bottom
            //  with a constant value (will only have 2 buttons for now, so values can be pretty constant)
            //  If there is too much text, It will be obscured by the buttons. Solutions there can be either
            //  to make sure there isn't ever too much text, or coding scroll functionality. Thats a way off though
            popupButtons = [];
            Rectangle buttonSrc = new Rectangle(0, 0, 128, 32);
            string[] buttonTexts = ["Enter", "Leave"];
            int buttonX = (x + (x + 192 - 128)) / 2;
            int buttonY = (y + (y + 160 - 32)) / 2;
            for (int i = 0; i < buttonTexts.Length; i++) 
            {
                //todo! Buttons are too big. I should size them down. Issue is that InteractiveButton uses
                // the large font. So I'll have to alter interactiveButton code to support small text too
                Rectangle dst = new Rectangle(buttonX, buttonY + (48 * i), 128, 32);
                popupButtons.Add(new InteractiveButton(dst, buttonSrc, buttonTexts[i], Color.Black, Color.White, Color.DarkGreen));
            }
        }

        internal bool Pressed(int i, MouseState ms, Vector2 mousePos)
        {
            return popupButtons[i].Pressed(ms, mousePos);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            popupInfo.Draw(spriteBatch);
            foreach(var button in popupButtons)
            {
                button.Draw(spriteBatch);
            }
        }

        internal void Clear()
        {
            popupInfo = null;
            popupButtons = null;
            isActive = false;
        }
    }
}
