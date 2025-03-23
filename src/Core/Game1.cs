using cr_mono.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Core
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        private RenderTarget2D renderTarget;
        private SpriteBatch spriteBatch;
        
        private GameStateManager gsm;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = Data.ScreenWidth,
                PreferredBackBufferHeight = Data.ScreenHeight,
                HardwareModeSwitch = false,
                IsFullScreen = Data.IsFullScreen
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gsm = new GameStateManager(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            renderTarget = new RenderTarget2D(GraphicsDevice, Data.NativeWidth, Data.NativeHeight);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ResourceManager.LoadContent(Content);
            gsm.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            Data.PreviousKeyboardState = Data.CurrentKeyboardState;
            Data.CurrentKeyboardState = Keyboard.GetState();

            if (Data.Exit == true) {
                Exit();
            }

            gsm.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetRenderTarget(renderTarget);
            gsm.Draw(spriteBatch);

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
