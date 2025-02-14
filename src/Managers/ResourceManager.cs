using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Managers
{
    public static class ResourceManager
    {
        public static SpriteFont FontRegular { get; private set; }

        public static void LoadContent(ContentManager content) {
            FontRegular = content.Load<SpriteFont>("Fonts/silkscreen_regular");
        }
    }
}
