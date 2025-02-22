using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Core.GameMath
{
    public class Camera
    {
        public readonly int[] zoomLevels = [8, 16, 32, 64];
        public int zoomIndex = 2;

        public Vector2 Position;
        private Vector2 centreTile;

        public Camera() {
            this.Position = new Vector2(
                Data.NativeWidth / 2 - (zoomLevels[zoomIndex] / 2), Data.NativeHeight / 3);
            this.centreTile = Vector2.Zero;
        }

        public void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position.Y += zoomLevels[zoomIndex] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                Position.X += zoomLevels[zoomIndex] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y -= zoomLevels[zoomIndex] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X -= zoomLevels[zoomIndex] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Data.currentKeyboardState.IsKeyDown(Keys.I) && !Data.previousKeyboardState.IsKeyDown(Keys.I))
            {
                if (zoomIndex < zoomLevels.Length - 1) {
                    zoomIndex++;
                }
            }
            if (Data.currentKeyboardState.IsKeyDown(Keys.O) && !Data.previousKeyboardState.IsKeyDown(Keys.O))
            {
                if (zoomIndex > 0) {
                    zoomIndex--;
                }
            }
        }

        public Vector2 UpdateCentreTile() {
            Vector2 test = Vector2.Zero;
            return Tile.PixelToIsometric(test, this);
        }
    }
}
