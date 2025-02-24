using cr_mono.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.UI
{
    internal class Button
    {
        protected Rectangle dst, src;
        protected Texture2D texture;

        internal Button(Rectangle dst, Rectangle src)
        {
            this.dst = dst;
            this.src = src;
            this.texture = ResourceManager.UI;
        }

        internal virtual void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(texture, dst, src, Color.White);
        }
    }
}
