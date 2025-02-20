using cr_mono.Core;
using cr_mono.Core.GameMath;
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
        //Texture2D unitsTexture;
        
        private Dictionary<Vector2, int> tilemap;
        private List<Rectangle> textureStore;

        private Camera camera;
        private Vector2 selectedTile;

        internal override void LoadContent(ContentManager content)
        {
            camera = new Camera();
            tilemap = MapGenerator.JaggedLevel(50);
            textureStore = new() { 
                new Rectangle(0, 0, 32, 32),
                new Rectangle(32, 0, 32, 32),
                new Rectangle(64, 0, 32, 32)
            };
            tileSetTexture = content.Load<Texture2D>("Textures/tileset");
            //unitsTexture = content.Load<Texture2D>("Textures/units");
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
                    if (item.Value == 3)
                    {
                        Vector2 mtn = new Vector2(item.Key.X - 1, item.Key.Y - 1);
                        Rectangle mtnDst = Tile.IsometricToPixel(mtn, camera);
                        Rectangle groundSrc = textureStore[0];
                        spriteBatch.Draw(tileSetTexture, dst, groundSrc, Color.Silver);
                        spriteBatch.Draw(tileSetTexture, mtnDst, src, Color.Silver);
                    }
                    else
                    {
                        spriteBatch.Draw(tileSetTexture, dst, src, Color.Silver);
                    }
                }
                else
                {
                    if (item.Value == 3)
                    {
                        Vector2 mtn = new Vector2(item.Key.X - 1, item.Key.Y - 1);
                        Rectangle mtnDst = Tile.IsometricToPixel(mtn, camera);
                        Rectangle groundSrc = textureStore[0];
                        spriteBatch.Draw(tileSetTexture, dst, groundSrc, Color.White);
                        spriteBatch.Draw(tileSetTexture, mtnDst, src, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(tileSetTexture, dst, src, Color.White);
                    }
                }
            }
            //Rectangle unitDst = Tile.IsometricToPixel(new Vector2(1 - 1, 1 - 1), camera);
            //Rectangle unitSrc = new Rectangle(0, 0, 32, 32);
            //spriteBatch.Draw(unitsTexture, unitDst, unitSrc, Color.White);
            
            spriteBatch.End();
        }
    }
}