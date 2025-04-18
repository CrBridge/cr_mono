using cr_mono.Core;
using cr_mono.Managers;
using cr_mono.Core.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using cr_mono.Scenes;
using cr_mono.src.Scenes.GameScenes;

namespace cr_mono.Scenes
{
    //TODO! this is due to the changing architecture
    // I'll have to manage the difference between coming here from a new game
    //      (only for now, I plan on having a menu inbetween new game and here for some player editing)
    // and coming here from a different game scene.
    // or will I? The point is that it has to generate everything that could change
    // (positions, things on map, the map itself) using the context which has to store all that

    internal class MapScene : IGameSubScene
    {
        private GameContext context;
        private GameSubSceneManager manager;

        private Texture2D tileSetTexture, unitsTexture;
        private ArmyPlayer player;
        private WorldMap world;
        private Camera2D camera;
        private Vector2 selectedTile;
        private WorldTime worldTime;
        private int lastMoveMinute;
        private PopupSystem popupSystem;

        internal MapScene(GameContext context, GameSubSceneManager manager)
        {
            this.context = context;
            this.manager = manager;
        }

        public void LoadContent(ContentManager content)
        {
            //tileSetTexture = content.Load<Texture2D>("Textures/tileset");
            tileSetTexture = content.Load<Texture2D>("Textures/tilesetLarge");
            unitsTexture = content.Load<Texture2D>("Textures/units");

            Data.RNG = new RNG(6);
            world = new WorldMap(50, Data.RNG);
            player = new ArmyPlayer(world.navMap, Data.RNG, unitsTexture);
            player.PlayerMovesPos += OnPlayerMovedTile;
            camera = new Camera2D();

            worldTime = new WorldTime();
            lastMoveMinute = worldTime.minutes;

            popupSystem = new PopupSystem();

        }

        public void Update(GameTime gameTime)
        {
            //todo! I hate having to do this for multiple things, (menu, tile class etc.)
            // I should mouse state and scaled pos as a global, then just update it every
            // frame in the Game1 class
            MouseState ms = Mouse.GetState();
            int screenWidth = Data.IsFullScreen ?
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width : Data.ScreenWidth;
            Vector2 scaledPos = ms.Position.ToVector2() /
                (screenWidth / (float)Data.NativeWidth);

            if (popupSystem.isActive)
            {
                if (popupSystem.Pressed(0, ms, scaledPos))
                {
                    //todo! when more scenes are created (e.g. village) I need to handle that as well
                    DungeonScene dungeon = new DungeonScene(context, manager, popupSystem.structureId);
                    manager.SwitchScene(dungeon);
                }
                if (popupSystem.Pressed(1, ms, scaledPos))
                {
                    popupSystem.Clear();
                }
            }
            else
            {
                camera.Update(gameTime);

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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            RenderLayer(spriteBatch, world.baseLayer, 0);
            player.RenderToMap(camera, selectedTile, spriteBatch);
            RenderLayer(spriteBatch, world.topLayer, 1);

            //todo! I dont like how the text constantly changes length due to spacing
            // either refactor to use 3 different DrawStrings, or format it so
            // the value "5" will display as "05"
            spriteBatch.DrawString(
                ResourceManager.FontSmall,
                $"Minute: {worldTime.minutes} Hour: {worldTime.hours} Day: {worldTime.days}",
                new Vector2(5, 7),
                Color.White);

            if (popupSystem.isActive)
            {
                popupSystem.Draw(spriteBatch);
            }
        }

        //todo! move this out of the mapScene so It can be reused across scenes
        //  uses alot of class members right now, but some can probably go in GameContext
        //  eventually. Each isometric scene could also hold a class of scene specific info,
        //  such as the camera and textureStore
        //  although this function uses worldTime to change color, which wouldnt be used
        //  elsewhere, gotta think about that
        private void RenderLayer(
        SpriteBatch spritebatch,
        Dictionary<Vector2, int> layer,
        int layerNumber)
        {
            Rectangle bounds = new Rectangle(0, 0, Data.NativeWidth, Data.NativeHeight);

            foreach (var item in layer)
            {
                Rectangle dst = Tile.IsometricToPixel(item.Key, camera, layerNumber);
                Rectangle src = world.textureStore[item.Value];

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

                        // play here could be a custom shader/effect for selectedTile
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
            int playerTile = 0;

            if (world.topLayer.TryGetValue(player.position, out int value))
            {
                playerTile = value;
            }

            if (value != 0 && value != 3)
            {
                // value is structure, trigger popup
                popupSystem.Trigger(world.structures[player.position]);
            }
        }

        public void Dispose()
        {
            // unload textures (because idk if thats done auto when nulling)
            // fairly certain i need to unload manually
        }
    }
}
