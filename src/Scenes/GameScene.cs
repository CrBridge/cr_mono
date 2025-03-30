﻿using cr_mono.Core;
using cr_mono.Managers;
using cr_mono.Core.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace cr_mono.Scenes
{
    internal class GameScene : Component
    {
        internal event EventHandler MenuRequested;

        private Texture2D tileSetTexture, unitsTexture;

        private ArmyPlayer player;
        private WorldMap world;

        private Camera2D camera;
        private Vector2 selectedTile;

        private WorldTime worldTime;
        private int lastMoveMinute;

        internal override void LoadContent(ContentManager content)
        {
            tileSetTexture = content.Load<Texture2D>("Textures/tileset");
            unitsTexture = content.Load<Texture2D>("Textures/units");

            Data.RNG = new RNG(6);
            world = new WorldMap(50, Data.RNG);
            player = new ArmyPlayer(world.navMap, Data.RNG, unitsTexture);
            player.PlayerMovesPos += OnPlayerMovedTile;
            camera = new Camera2D();

            worldTime = new WorldTime();
            lastMoveMinute = worldTime.minutes;
        }

        internal override void Update(GameTime gameTime)
        {
            if (Data.CurrentKeyboardState.IsKeyDown(Keys.Tab) &&
                !Data.PreviousKeyboardState.IsKeyDown(Keys.Tab))
            {
                //Data.CurrentScene = Data.Scenes.Menu;
                MenuRequested?.Invoke(this, EventArgs.Empty);
            }
            camera.Update(gameTime);

            MouseState ms = Mouse.GetState();
            selectedTile = Tile.PixelToIsometric(ms.Position.ToVector2(), camera);
            if (ms.LeftButton == ButtonState.Pressed && world.navMap.ContainsKey(selectedTile))
            {
                List<Vector2> path = Pathfinding.FindPath(player.position, selectedTile, world.navMap);
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
                player.UpdatePosition(world.topLayer);
                lastMoveMinute = worldTime.minutes;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            RenderLayer(spriteBatch, world.baseLayer, 0);
            player.RenderToMap(camera, selectedTile, spriteBatch);
            RenderLayer(spriteBatch, world.topLayer, 1);

            spriteBatch.DrawString(
                ResourceManager.FontSmall,
                $"Minute: {worldTime.minutes} Hour: {worldTime.hours} Day: {worldTime.days}",
                new Vector2(0, 7),
                Color.White);

            spriteBatch.End();
        }

        private void RenderLayer(
            SpriteBatch spritebatch,
            Dictionary<Vector2, TileType> layer,
            int layerNumber)
        {
            Rectangle bounds = new Rectangle(0, 0, Data.NativeWidth, Data.NativeHeight);

            foreach (var item in layer)
            {
                Rectangle dst = Tile.IsometricToPixel(item.Key, camera, layerNumber);
                Rectangle src = world.textureStore[(int)item.Value];

                if (dst.Intersects(bounds))
                {
                    if (layerNumber != 0 && item.Key == player.position)
                    {
                        spritebatch.Draw(tileSetTexture, dst, src, new Color(worldTime.skyColor, 0.8f));
                    }
                    else if (item.Key == selectedTile && camera.zoomIndex > 0)
                    {
                        //todo! this looks bad, not sure if its the lerp threshold
                        // thats causing that, or silver just doesn't work for all, in which
                        // case i'll need a better way of highlighting a tile
                        spritebatch.Draw(tileSetTexture, dst, src, Color.Lerp(worldTime.skyColor, Color.Silver, 0.5f));
                    }
                    else
                    {
                        spritebatch.Draw(tileSetTexture, dst, src, worldTime.skyColor);
                    }
                }
            }
        }

        private void OnPlayerMovedTile(object sender, EventArgs e) 
        {
            TileType playerTile = 0;

            if (world.topLayer.TryGetValue(player.position, out TileType value)) 
            {
                playerTile = value;
            }

            // do some events based on what tile the player has just moved to
            // e.g. player moved to a dungeon tile, trigger something that lets them
            // enter or leave, same with other structure tiles.
            // Or if tile is forest or empty (e.g. no structure), roll a chance for
            // a random encounter
            // if other units exist on the map. Trigger here too
            //   -This one would be trickier since those units are also moving around
        }
    }
}
