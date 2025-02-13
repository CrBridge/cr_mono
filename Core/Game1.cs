using cr_mono.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Core
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameStateManager gsm;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = Data.ScreenWidth,
                PreferredBackBufferHeight = Data.ScreenHeight
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gsm = new GameStateManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gsm.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gsm.update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            gsm.draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
