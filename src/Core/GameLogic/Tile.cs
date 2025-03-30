using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.GameLogic
{
    internal enum TileType 
    {
        GRASS,
        WATER,
        MOUNTAIN,
        FOREST,
        TOWER
    }

    internal static class Tile
    {
        internal static Rectangle IsometricToPixel(Vector2 itemKey, Camera2D camera, int layer) {
            int zoom = camera.zoomLevels[camera.zoomIndex];
            int x = (int)itemKey.X - layer;
            int y = (int)itemKey.Y - layer;

            return new Rectangle(
                (int)Math.Round((x * 0.5 * zoom) + (y * -0.5 * zoom) + camera.Position.X),
                (int)Math.Round((x * 0.25 * zoom) + (y * 0.25 * zoom) + camera.Position.Y),
                zoom, zoom);
        }

        internal static Vector2 PixelToIsometric(Vector2 mousePos, Camera2D camera) {
            int screenWidth = Data.IsFullScreen ?
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width : Data.ScreenWidth;
            mousePos = mousePos /
                (screenWidth / (float)Data.NativeWidth);

            int zoom = camera.zoomLevels[camera.zoomIndex];
            
            float screenX = mousePos.X - camera.Position.X - (zoom / 2);
            float screenY = mousePos.Y - camera.Position.Y - (zoom / 4);

            int gridX = (int)Math.Round((screenY / (zoom / 2)) + (screenX / zoom), 0);
            int gridY = (int)Math.Round((screenY / (zoom / 2)) - (screenX / zoom), 0);

            return new Vector2(gridX, gridY);
        }
    }
}
