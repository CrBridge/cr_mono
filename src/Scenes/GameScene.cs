using cr_mono.Core;
using cr_mono.Managers;
using cr_mono.Core.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace cr_mono.Scenes
{
    internal class GameScene : Component
    {
        private Texture2D tileSetTexture, unitsTexture;

        private ArmyPlayer player;

        private Dictionary<Vector2, int> baseLayer, topLayer;
        private Dictionary<Vector2, bool> navMap;
        private List<Rectangle> textureStore;

        private Camera camera;
        private Vector2 selectedTile;
        // TODO: use worldCentre to lock cameraPosition when zoom out is max
        //private Vector2 worldCentre;

        // var to debug number of draw calls
        int visibleTiles;

        private WorldTime worldTime;
        private int lastMoveMinute;

        internal override void LoadContent(ContentManager content)
        {
            tileSetTexture = content.Load<Texture2D>("Textures/tileset");
            unitsTexture = content.Load<Texture2D>("Textures/units");

            Data.RNG = new RNG(6);
            (baseLayer, topLayer) = WorldLogic.GenerateJaggedMap(50, Data.RNG);
            navMap = WorldLogic.GenerateNavMap(baseLayer, topLayer);
            WorldLogic.AddStructures(navMap, topLayer, Data.RNG, 15);
            textureStore = new() { 
                new Rectangle(0, 0, 32, 32),
                new Rectangle(32, 0, 32, 32),
                new Rectangle(64, 0, 32, 32),
                new Rectangle(96, 0, 32, 32),
                new Rectangle(128, 0, 32, 32)
            };
            player = new ArmyPlayer(navMap, Data.RNG, unitsTexture);
            camera = new Camera();

            visibleTiles = 0;

            worldTime = new WorldTime();
            lastMoveMinute = worldTime.minutes;
        }

        internal override void Update(GameTime gameTime)
        {
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.Tab) && 
                !Data.PreviousKeyboardState.IsKeyDown(Keys.Tab))
            {
                Data.CurrentScene = Data.Scenes.Menu;
            }
            camera.Update(gameTime);

            MouseState ms = Mouse.GetState();
            selectedTile = Tile.PixelToIsometric(ms.Position.ToVector2(), camera);
            if (ms.LeftButton == ButtonState.Pressed && navMap.ContainsKey(selectedTile))
            {
                List<Vector2> path = Pathfinding.FindPath(player.position, selectedTile, navMap);
                if (path != null)
                {
                    player.SetPath(path);
                }
            }

            worldTime.Update(gameTime);
            // update entities every x minutes
            int timeDifference = (worldTime.minutes - lastMoveMinute + 60) % 60;
            if (timeDifference >= 5)
            {
                player.UpdatePosition();
                lastMoveMinute = worldTime.minutes;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            visibleTiles = 0;
            RenderLayer(spriteBatch, baseLayer, 0);
            player.RenderToMap(camera, selectedTile, spriteBatch);
            RenderLayer(spriteBatch, topLayer, 1);


            spriteBatch.DrawString(
                ResourceManager.FontSmall, 
                $"Tiles Drawn: {visibleTiles}",
                Vector2.Zero,
                Color.White);

            spriteBatch.DrawString(
                ResourceManager.FontSmall,
                $"Minute: {worldTime.minutes} Hour: {worldTime.hours} Day: {worldTime.days}",
                new Vector2(0, 14),
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
                    // TODO! only do this for the top layer. its proccing on base too and making it look strange
                    if (layerNumber != 0 && item.Key == player.position)
                    {
                        //spritebatch.Draw(tileSetTexture, dst, src, new Color(Color.White, 0.8f));
                        spritebatch.Draw(tileSetTexture, dst, src, new Color(worldTime.skyColor, 0.8f));
                    }
                    else if (item.Key == selectedTile && camera.zoomIndex > 0)
                    {
                        //spritebatch.Draw(tileSetTexture, dst, src, Color.Silver);
                        spritebatch.Draw(tileSetTexture, dst, src, Color.Lerp(worldTime.skyColor, Color.Silver, 0.5f));
                    }
                    else
                    {
                        //spritebatch.Draw(tileSetTexture, dst, src, Color.White);
                        spritebatch.Draw(tileSetTexture, dst, src, worldTime.skyColor);
                    }
                    visibleTiles++;
                }
            }
        }
    }
}