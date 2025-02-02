using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono
{
    public class Game1 : Game
    {
        const int WIDTH = 720;
        const int HEIGHT = 480;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont font;
        Texture2D texture;

        private Dictionary<Vector2, int> tilemap;
        private List<Rectangle> textureStore;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = WIDTH,
                PreferredBackBufferHeight = HEIGHT
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private Dictionary<Vector2, int> LoadMap() {
            Dictionary<Vector2, int> map = new();

            for (int y = 0; y < 10; y++) {
                for (int x = 0; x < 10; x++) {
                    map[new Vector2(x, y)] = 1;
                }
            }

            return map;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            font = Content.Load<SpriteFont>("fonts/main_font");

            tilemap = LoadMap();
            textureStore = new() { new Rectangle(0, 0, 32, 32) };
            texture = Content.Load<Texture2D>("tileset");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkMagenta);

            // TODO: Add your drawing code here
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var item in tilemap) {
                Rectangle dst = new(
                    (int)((item.Key.X * 0.5 * 32) + (item.Key.Y * -0.5 * 32) + (WIDTH/2 - 16)),
                    (int)((item.Key.X * 0.25 * 32) + (item.Key.Y * 0.25 * 32)) + (HEIGHT/3),
                    32, 32);
                Rectangle src = textureStore[item.Value - 1];

                spriteBatch.Draw(texture, dst, src, Color.White);
            }

            spriteBatch.DrawString(font, "Game Text Example...", Vector2.Zero, Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
