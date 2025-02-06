using cr_mono.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Scenes
{
    internal class GameScene : Component
    {
        Texture2D texture;
        
        private Dictionary<Vector2, int> tilemap;
        private List<Rectangle> textureStore;

        internal override void LoadContent(ContentManager content)
        {
            tilemap = LoadMap();
            textureStore = new() { new Rectangle(0, 0, 32, 32) };
            texture = content.Load<Texture2D>("tileset");
        }

        internal override void update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1)) {
                Data.CurrentScene = Data.Scenes.Menu;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                Data.CurrentScene = Data.Scenes.Settings;
            }
        }

        internal override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            foreach (var item in tilemap) {
                Rectangle dst = new(
                    (int)((item.Key.X * 0.5 * 32) + (item.Key.Y * -0.5 * 32) + (Data.ScreenWidth/2 - 16)),
                    (int)((item.Key.X * 0.25 * 32) + (item.Key.Y * 0.25 * 32)) + (Data.ScreenHeight/3),
                    32, 32);
                Rectangle src = textureStore[item.Value - 1];
            
                spriteBatch.Draw(texture, dst, src, Color.White);
            }
            
            spriteBatch.End();
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
    }
}