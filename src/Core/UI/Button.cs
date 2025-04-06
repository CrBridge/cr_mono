using cr_mono.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.UI
{
    internal class Button
    {
        protected Rectangle dst, src;
        protected static Texture2D Texture 
        {
            get { return ResourceManager.UI; }
        }

        internal Button(Rectangle dst, Rectangle src)
        {
            this.dst = dst;
            this.src = src;
        }

        internal virtual void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(Texture, dst, src, Color.White);
        }
    }
}
