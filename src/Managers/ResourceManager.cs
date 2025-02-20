﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Managers
{
    public static class ResourceManager
    {
        public static SpriteFont FontRegular { get; private set; }
        public static Texture2D UI { get; private set; }

        public static void LoadContent(ContentManager content) {
            FontRegular = content.Load<SpriteFont>("Fonts/silkscreen_regular");
            UI = content.Load<Texture2D>("Textures/ui");
        }
    }
}
