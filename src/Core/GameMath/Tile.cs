using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.GameMath
{
    internal class Tile
    {
        // basic class for helper functions for now
        // This class may also include the enum for tile types (air, solid, half, stairs etc.)
        internal static Rectangle IsometricToPixel(Vector2 itemKey, Camera camera) {
            return new Rectangle(
                (int)((itemKey.X * 0.5 * 32) + (itemKey.Y * -0.5 * 32) + camera.Position.X),
                (int)((itemKey.X * 0.25 * 32) + (itemKey.Y * 0.25 * 32) + camera.Position.Y),
                32, 32);
        }

        internal static Vector2 PixelToIsometric(Vector2 mousePos, Camera camera) {
            // for now, tile is size 32, may have zoom levels (enum) later.
            int screenWidth = Data.IsFullScreen ?
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width : Data.ScreenWidth;
            mousePos = mousePos /
                (screenWidth / (float)Data.NativeWidth);
            
            float screenX = mousePos.X - camera.Position.X - 16;
            float screenY = mousePos.Y - camera.Position.Y - 8;

            int gridX = (int)Math.Round((screenY / 16) + (screenX / 32), 0);
            int gridY = (int)Math.Round((screenY / 16) - (screenX / 32), 0);

            return new Vector2(gridX, gridY);
        }
    }
}
