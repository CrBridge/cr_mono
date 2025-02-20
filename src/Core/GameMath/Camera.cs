using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Core.GameMath
{
    public class Camera
    {
        public Vector2 Position;

        public Camera() {
            // the 16 here is half of the tile size, so may need to account for that
            this.Position = new Vector2(Data.NativeWidth / 2 - 16, Data.NativeHeight / 3);
        }

        public void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position.Y += 32 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                Position.X += 32 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y -= 32 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X -= 32 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
