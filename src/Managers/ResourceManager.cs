﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Managers
{
    public static class ResourceManager
    {
        // purpose of this class is to hold properties for loaded content
        // that will be regularly used across most scenes. That way I dont
        // have to unload and load them needlessly when transitioning
        public static SpriteFont FontRegular { get; private set; }
        public static SpriteFont FontSmall { get; private set; }
        public static Texture2D UI { get; private set; }

        public static void LoadContent(ContentManager content) {
            FontRegular = content.Load<SpriteFont>("Fonts/font_regular");
            FontSmall = content.Load<SpriteFont>("Fonts/font_small");
            UI = content.Load<Texture2D>("Textures/ui");
        }
    }
}
