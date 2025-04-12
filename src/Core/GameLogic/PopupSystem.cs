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
        internal int structureId;

        internal PopupSystem() 
        {
            this.isActive = false;
        }

        internal void Trigger(Structure structure)
        {
            isActive = true;
            structureId = structure.id;
            int x = (Data.NativeWidth - 192) / 2;
            int y = (Data.NativeHeight - 160) / 2;
            popupInfo = new TextBlock(new Rectangle(x, y, 192, 160), $"Structure id is {structure.id} do you enter?", 100);
            popupButtons = [];
            Rectangle buttonSrc = new Rectangle(0, 0, 128, 32);
            string[] buttonTexts = ["Enter", "Leave"];
            int buttonX = (x + (x + 192 - 64)) / 2;
            int buttonY = 32 + (y + (y + 160 - 16)) / 2;
            for (int i = 0; i < buttonTexts.Length; i++) 
            {
                Rectangle dst = new Rectangle(buttonX, buttonY + (24 * i), 64, 16);
                popupButtons.Add(new InteractiveButton(
                    dst, buttonSrc, buttonTexts[i], Color.Black, Color.White, Color.DarkGreen));
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
