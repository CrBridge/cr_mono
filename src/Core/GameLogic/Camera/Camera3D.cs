using System;
using cr_mono.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.src.Core.GameLogic
{
    internal class Camera3D
    {
        private Vector3 cameraPosition;

        private Vector3 forwards;
        private Vector3 right;
        private Vector3 up;

        private float yaw;
        private float pitch;
        Vector2 lastMousePos;
        bool firstMouseMove;

        private readonly float speed;
        private readonly float sensitivity;

        internal Matrix viewMatrix;
        internal Matrix projectionMatrix;

        internal Camera3D() 
        {
            cameraPosition = new Vector3(0.0f, 0.0f, -8.0f);

            yaw = -MathHelper.PiOver2;
            pitch = 0.0f;

            speed = 4.0f;
            sensitivity = 0.06f;
            firstMouseMove = true;

            UpdateVectors();

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(60.0f),
                (float)Data.NativeWidth / (float)Data.NativeHeight,
                1.0f, 1000.0f);
        }

        internal void Update(GameTime gameTime) 
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 moveDirection = Vector3.Zero;

            if (Data.CurrentKeyboardState.IsKeyDown(Keys.W)) 
            {
                moveDirection += forwards;
            }
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.S))
            {
                moveDirection -= forwards;
            }
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.A))
            {
                moveDirection -= right;
            }
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.D))
            {
                moveDirection += right;
            }
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.Space))
            {
                moveDirection += Vector3.Up;
            }
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                moveDirection -= Vector3.Up;
            }

            if (moveDirection != Vector3.Zero)
            {
                cameraPosition += moveDirection * speed * dt;
            }

            int centreX = Data.NativeWidth / 2;
            int centreY = Data.NativeHeight / 2;

            if (firstMouseMove)
            {
                Mouse.SetPosition(centreX, centreY);
                lastMousePos = new Vector2(centreX, centreY);
                firstMouseMove = false;
                return;
            }
            Vector2 mouseDelta = Data.CurrentMouseState.Position.ToVector2() - lastMousePos;
            Mouse.SetPosition(centreX, centreY);
            lastMousePos = new Vector2(centreX, centreY);
            float deltaX = mouseDelta.X * sensitivity;
            float deltaY = mouseDelta.Y * sensitivity;
            if (deltaX != 0 || deltaY != 0)
            {
                Rotate(deltaX, deltaY);
            }

            viewMatrix = View();
        }

        private Matrix View() 
        {
            Vector3 target = cameraPosition + forwards;
            return Matrix.CreateLookAt(cameraPosition, target, up);
        }

        private void UpdateVectors() 
        {
            forwards = Vector3.Normalize(new Vector3(
                (float)(Math.Cos(yaw) * Math.Cos(pitch)),
                (float)(Math.Sin(pitch)),
                (float)(Math.Sin(yaw) * Math.Cos(pitch))));

            right = Vector3.Normalize(Vector3.Cross(forwards, Vector3.Up));
            up = Vector3.Normalize(Vector3.Cross(right, forwards));
        }

        private void Rotate(float deltaX, float deltaY)
        {
            yaw += deltaX * sensitivity;
            pitch -= deltaY * sensitivity;

            pitch = MathHelper.Clamp(
                pitch, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);

            UpdateVectors();
        }
    }
}
