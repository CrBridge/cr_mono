using System;
using cr_mono.Core;
using cr_mono.src.Core.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.src.Scenes
{
    internal class Debug3dScene : Component
    {
        internal event EventHandler MenuRequested;

        Camera3D camera;

        Matrix worldMatrix;
        Model model;

        internal override void LoadContent(ContentManager content)
        {
            worldMatrix = Matrix.CreateWorld(
                Vector3.Zero, Vector3.Forward, Vector3.Up);
            camera = new Camera3D();

            model = content.Load<Model>("Models/DebugCube/cube");
        }

        internal override void Update(GameTime gameTime)
        {
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.Tab) &&
                !Data.PreviousKeyboardState.IsKeyDown(Keys.Tab))
            {
                MenuRequested?.Invoke(this, EventArgs.Empty);
            }

            camera.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            foreach (ModelMesh mesh in model.Meshes) 
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                    effect.Projection = camera.projectionMatrix;
                    effect.View = camera.viewMatrix;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
