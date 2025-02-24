using cr_mono.Core;
using cr_mono.Core.GameMath;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace cr_mono.Scenes
{
    internal class GameScene : Component
    {
        Texture2D tileSetTexture;

        private Dictionary<Vector2, int> baseLayer, topLayer;
        private Dictionary<Vector2, bool> navMap;
        private List<Rectangle> textureStore;

        private Camera camera;
        private Vector2 selectedTile;
        private Vector2 worldCentre;

        internal override void LoadContent(ContentManager content)
        {
            Data.RNG = new Random(3);
            camera = new Camera();
            (baseLayer, topLayer) = MapGenerator.JaggedLevel(50, Data.RNG);
            navMap = MapGenerator.GenerateNavMap(baseLayer, topLayer);
            textureStore = new() { 
                new Rectangle(0, 0, 32, 32),
                new Rectangle(32, 0, 32, 32),
                new Rectangle(64, 0, 32, 32)
            };
            tileSetTexture = content.Load<Texture2D>("Textures/tileset");

        }

        internal override void Update(GameTime gameTime)
        {
            if (Data.currentKeyboardState.IsKeyDown(Keys.Tab) && 
                !Data.previousKeyboardState.IsKeyDown(Keys.Tab))
            {
                Data.CurrentScene = Data.Scenes.Menu;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                Data.CurrentScene = Data.Scenes.Settings;
            }
            camera.Update(gameTime);
            selectedTile = Tile.PixelToIsometric(Mouse.GetState().Position.ToVector2(), camera);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            RenderLayer(spriteBatch, baseLayer, 0);
            RenderLayer(spriteBatch, topLayer, 1);
           
            spriteBatch.End();
        }

        internal void RenderLayer(
            SpriteBatch spritebatch, 
            Dictionary<Vector2, int> layer, 
            int layerNumber)
        {
            foreach (var item in layer) {
                Rectangle dst = Tile.IsometricToPixel(item.Key, camera, layerNumber);
                Rectangle src = textureStore[item.Value - 1];

                if (item.Key == selectedTile && camera.zoomIndex > 0) {
                    spritebatch.Draw(tileSetTexture, dst, src, Color.Silver);
                }
                else {
                    spritebatch.Draw(tileSetTexture, dst, src, Color.White);
                }
            }
        }
    }
}