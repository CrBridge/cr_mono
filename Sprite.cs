using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono
{
    internal class Sprite
    {
        public Sprite(Texture2D texture, Vector2 position) {
            this.texture = texture;
            this.position = position;
        }

        public virtual void Update() {
            position.X = (position.X + 1) % 500;
        }

        public Rectangle Rect {
            get {
                return new Rectangle((int)position.X, (int)position.Y, 64, 64);
            }
        }
        public Texture2D texture;
        public Vector2 position;
    }
}
