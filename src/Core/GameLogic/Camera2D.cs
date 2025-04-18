﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Core.GameLogic
{
    internal class Camera2D
    {
        internal readonly int[] zoomLevels = [8, 16, 32, 64];
        internal int zoomIndex = 2;
        
        internal Vector2 Position;

        internal Camera2D() {
            this.Position = new Vector2(
                Data.NativeWidth / 2 - (zoomLevels[zoomIndex] / 2), Data.NativeHeight / 2);
        }

        internal void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position.Y += 1 * zoomLevels[zoomIndex] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                Position.X += 2 * zoomLevels[zoomIndex] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y -= 1 * zoomLevels[zoomIndex] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X -= 2 * zoomLevels[zoomIndex] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.I) && !Data.PreviousKeyboardState.IsKeyDown(Keys.I))
            {
                if (zoomIndex < zoomLevels.Length - 1) {
                    AdjustZoom(zoomIndex + 1);
                }
            }
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.O) && !Data.PreviousKeyboardState.IsKeyDown(Keys.O))
            {
                if (zoomIndex > 0) {
                    AdjustZoom(zoomIndex - 1);
                }
            }
        }

        private void AdjustZoom(int newZoom) {
            Vector2 centre = new Vector2(
                Data.NativeWidth / 2, Data.NativeHeight / 2);
            Vector2 worldCentre = (centre - Position) / zoomLevels[zoomIndex];

            zoomIndex = newZoom;

            Position = centre - worldCentre * zoomLevels[zoomIndex];
            //Position = new Vector2(235.0f, 45.0f); //Rough centre position value
        }
    }
}
