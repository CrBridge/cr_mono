using Microsoft.Xna.Framework;
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

        private Queue<Vector2> path;
        private Vector2? targetTile;

        internal ArmyPlayer(
            Dictionary<Vector2, bool> navMap, 
            RNG rng,
            Texture2D texture) 
        {
            this.texture = texture;
            this.textureSrc = new Rectangle(0, 0, 32, 32);
            this.armySize = 1;
            this.position = WorldLogic.GetRandomMapPos(navMap, rng);

            this.path = new Queue<Vector2>();
            this.targetTile = null;
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

        internal void SetPath(List<Vector2> newPath)
        {
            path = new Queue<Vector2>(newPath);
            targetTile = path.Count > 0 ? path.Dequeue() : null;
        }

        internal void UpdatePosition() 
        {
            if (targetTile.HasValue) 
            {
                position = targetTile.Value;
                targetTile = path.Count > 0 ? path.Dequeue() : null;
            }
        }
    }
}
