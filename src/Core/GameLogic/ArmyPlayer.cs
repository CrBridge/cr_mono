using Microsoft.Xna.Framework;
using cr_mono.Core.GameMath;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.GameLogic
{
    internal class ArmyPlayer : ArmyUnit
    {
        private Texture2D texture;
        private Rectangle textureSrc;
        private int armySize;
        internal Vector2 position;

        internal ArmyPlayer(
            Dictionary<Vector2, bool> navMap, 
            Random rng,
            Texture2D texture) 
        {
            this.texture = texture;
            this.textureSrc = new Rectangle(0, 0, 32, 32);
            this.armySize = 1;
            this.position = WorldLogic.GetRandomMapPos(navMap, rng);
        }

        internal void RenderToMap(
            Camera camera, 
            Vector2 selectedTile, 
            SpriteBatch spriteBatch) 
        {
            Rectangle dst = Tile.IsometricToPixel(position, camera, 1);

            if (position == selectedTile && camera.zoomIndex > 0) {
                spriteBatch.Draw(texture, dst, textureSrc, Color.Silver);
            }
            else {
                spriteBatch.Draw(texture, dst, textureSrc, Color.White);
            }
        }

        internal void MovePlayer(
            Vector2 selectedTile,
            Dictionary<Vector2, bool> navMap)
        {
            if (navMap.TryGetValue(selectedTile, out bool value) && value) {
                position = selectedTile;
            }
        }
    }
}
