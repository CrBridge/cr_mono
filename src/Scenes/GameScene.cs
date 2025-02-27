using cr_mono.Core;
using cr_mono.Core.GameMath;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using cr_mono.Managers;
using cr_mono.Core.GameLogic;

namespace cr_mono.Scenes
{
    internal class GameScene : Component
    {
        private Texture2D tileSetTexture, unitsTexture;

        private ArmyPlayer player;

        private Dictionary<Vector2, int> baseLayer, topLayer;
        // will be used when units are a thing
        private Dictionary<Vector2, bool> navMap;
        private List<Rectangle> textureStore;

        private Camera camera;
        private Vector2 selectedTile;
        // TODO: use worldCentre to lock cameraPosition when zoom out is max
        private Vector2 worldCentre;

        int visibleTiles;

        internal override void LoadContent(ContentManager content)
        {
            tileSetTexture = content.Load<Texture2D>("Textures/tileset");
            unitsTexture = content.Load<Texture2D>("Textures/units");

            Data.RNG = new Random(2);
            (baseLayer, topLayer) = WorldLogic.GenerateJaggedMap(50, Data.RNG);
            navMap = WorldLogic.GenerateNavMap(baseLayer, topLayer);
            textureStore = new() { 
                new Rectangle(0, 0, 32, 32),
                new Rectangle(32, 0, 32, 32),
                new Rectangle(64, 0, 32, 32)
            };
            player = new ArmyPlayer(navMap, Data.RNG, unitsTexture);
            camera = new Camera();

            visibleTiles = 0;
        }

        internal override void Update(GameTime gameTime)
        {
            if (Data.currentKeyboardState.IsKeyDown(Keys.Tab) && 
                !Data.previousKeyboardState.IsKeyDown(Keys.Tab))
            {
                Data.CurrentScene = Data.Scenes.Menu;
            }
            camera.Update(gameTime);
            selectedTile = Tile.PixelToIsometric(Mouse.GetState().Position.ToVector2(), camera);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            visibleTiles = 0;
            RenderLayer(spriteBatch, baseLayer, 0);
            RenderLayer(spriteBatch, topLayer, 1);

            player.RenderToMap(camera, selectedTile, spriteBatch);

            spriteBatch.DrawString(
                ResourceManager.FontRegular, 
                $"Tiles Drawn: {visibleTiles}",
                Vector2.Zero,
                Color.White);
           
            spriteBatch.End();
        }

        internal void RenderLayer(
            SpriteBatch spritebatch, 
            Dictionary<Vector2, int> layer, 
            int layerNumber)
        {
            Rectangle bounds = new Rectangle(0, 0, Data.NativeWidth, Data.NativeHeight);

            foreach (var item in layer) {
                Rectangle dst = Tile.IsometricToPixel(item.Key, camera, layerNumber);
                Rectangle src = textureStore[item.Value - 1];


                if (dst.Intersects(bounds)) {
                    if (item.Key == selectedTile && camera.zoomIndex > 0)
                    {
                        spritebatch.Draw(tileSetTexture, dst, src, Color.Silver);
                    }
                    else
                    {
                        spritebatch.Draw(tileSetTexture, dst, src, Color.White);
                    }
                    visibleTiles++;
                }
            }
        }
    }
}