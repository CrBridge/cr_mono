using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cr_mono.Core;
using cr_mono.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Scenes
{
    internal class PauseScene
    {
        // todo! this works for now but its pretty ugly, mostly since the buttons dont take up much space so
        // most of the space is empty. Definitely getting changed later, putting this off because it's more of
        // a visual issue. The plan to fix it is probably having an actually stylised pause menu and I'd want
        // the scenes it leads to (inventory, log etc.) to be finished first
        private readonly Button background;
        private readonly List<InteractiveButton> buttons = [];
        internal bool isPaused;

        internal PauseScene()
        {
            Rectangle bgSrc = new Rectangle(128, 0, 192, 160);
            int x = (Data.NativeWidth - bgSrc.Width) / 2;
            int y = (Data.NativeHeight - bgSrc.Height) / 2;
            Rectangle bgDst = new Rectangle(x, y, 192, 160);

            background = new Button(bgDst, bgSrc);

            Rectangle buttonSrc = new Rectangle(0, 0, 128, 32);
            Rectangle[] buttonsDst = new Rectangle[3];
            string[] buttonTexts = ["Resume", "Settings", "Quit to Menu"];
            int buttonHeight = 16;
            int buttonWidth = 64;
            int spacing = 16;
            int totalHeight = (buttonHeight * buttonsDst.Length) + 
                (spacing * (buttonsDst.Length - 1));

            int startY = bgDst.Y + (bgDst.Height - totalHeight) / 2;

            for (int i = 0; i < buttonsDst.Length; i++)
            {
                int buttonX = bgDst.X + (bgDst.Width - buttonWidth) / 2;
                int buttonY = startY + (buttonHeight + spacing) * i;
                buttonsDst[i] = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                buttons.Add(new InteractiveButton(
                    buttonsDst[i], buttonSrc, buttonTexts[i], Color.Black, Color.White, Color.DarkGreen));
            }

            isPaused = false;
        }

        internal bool Pressed(int i, MouseState ms, Vector2 mousePos)
        {
            return buttons[i].Pressed(ms, mousePos);
        }

        internal void Draw(SpriteBatch spriteBatch) 
        {
            background.Draw(spriteBatch);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(spriteBatch);
            }
        }
    }
}
