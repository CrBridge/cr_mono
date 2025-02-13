using cr_mono.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace cr_mono.Scenes
{
    internal class GameScene : Component
    {
        Texture2D tileSetTexture;
        Texture2D unitsTexture;
        
        private Dictionary<Vector2, int> tilemap;
        private List<Rectangle> textureStore;

        private Camera camera;
        private Vector2 selectedTile;

        internal override void LoadContent(ContentManager content)
        {
            camera = new Camera();
            tilemap = MapGenerator.JaggedLevel(12);
            textureStore = new() { new Rectangle(0, 0, 32, 32) };
            tileSetTexture = content.Load<Texture2D>("tileset");
            unitsTexture = content.Load<Texture2D>("units");
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
            camera.Update(gameTime);
            selectedTile = Tile.PixelToIsometric(Mouse.GetState().Position.ToVector2(), camera);
        }

        internal override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            foreach (var item in tilemap) {
                Rectangle dst = Tile.IsometricToPixel(item.Key, camera);
                Rectangle src = textureStore[item.Value - 1];

                if (item.Key == selectedTile)
                {
                    spriteBatch.Draw(tileSetTexture, dst, src, Color.Silver);
                }
                else
                {
                    spriteBatch.Draw(tileSetTexture, dst, src, Color.White);
                }
            }
            Rectangle unitDst = Tile.IsometricToPixel(new Vector2(1 - 1, 1 - 1), camera);
            Rectangle unitSrc = new Rectangle(0, 0, 32, 32);
            spriteBatch.Draw(unitsTexture, unitDst, unitSrc, Color.White);
            
            spriteBatch.End();
        }
    }
}