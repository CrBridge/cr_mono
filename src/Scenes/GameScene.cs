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

        //private ArmyPlayer player;

        private Dictionary<Vector2, int> baseLayer, topLayer;
        private Dictionary<Vector2, bool> navMap;
        private List<Rectangle> textureStore;

        private Camera camera;
        private Vector2 selectedTile;
        // TODO: use worldCentre to lock cameraPosition when zoom out is max
        //private Vector2 worldCentre;

        // var to debug number of draw calls
        int visibleTiles;

        //private List<Vector2> pathfindingTiles;
        private WorldTime worldTime;

        internal override void LoadContent(ContentManager content)
        {
            tileSetTexture = content.Load<Texture2D>("Textures/tileset");
            unitsTexture = content.Load<Texture2D>("Textures/units");

            Data.RNG = new RNG(6);
            (baseLayer, topLayer) = WorldLogic.GenerateJaggedMap(50, Data.RNG);
            navMap = WorldLogic.GenerateNavMap(baseLayer, topLayer);
            textureStore = new() { 
                new Rectangle(0, 0, 32, 32),
                new Rectangle(32, 0, 32, 32),
                new Rectangle(64, 0, 32, 32),
                new Rectangle(96, 0, 32, 32)
            };
            //player = new ArmyPlayer(navMap, Data.RNG, unitsTexture);
            camera = new Camera();

            visibleTiles = 0;
            
            //pathfindingTiles = new List<Vector2>();

            worldTime = new WorldTime();
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
            //if (ms.LeftButton == ButtonState.Pressed && camera.zoomIndex > 0) {
            //if (ms.LeftButton == ButtonState.Pressed && camera.zoomIndex > 0 && navMap.ContainsKey(selectedTile))
            //{
                //player.MovePlayer(selectedTile, navMap);
                //pathfindingTiles = Pathfinding.FindPath(player.position, selectedTile, navMap);
            //}

            if (Data.CurrentKeyboardState.IsKeyDown(Keys.Space) &&
                !Data.PreviousKeyboardState.IsKeyDown(Keys.Space))
            {
                //refresh world to test world gen
                (baseLayer, topLayer) = WorldLogic.GenerateJaggedMap(50, Data.RNG);
                navMap = WorldLogic.GenerateNavMap(baseLayer, topLayer);

            }

            worldTime.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            visibleTiles = 0;
            RenderLayer(spriteBatch, baseLayer, 0);
            //player.RenderToMap(camera, selectedTile, spriteBatch);
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
                    if (item.Key == selectedTile && camera.zoomIndex > 0)
                    //if (pathfindingTiles != null && pathfindingTiles.Contains(item.Key) && camera.zoomIndex > 0)
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