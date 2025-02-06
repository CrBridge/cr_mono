using cr_mono.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cr_mono.Scenes
{
    internal class MenuScene : Component
    {
        internal override void LoadContent(ContentManager content)
        {
            //
        }

        internal override void update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                Data.CurrentScene = Data.Scenes.Game;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                Data.CurrentScene = Data.Scenes.Settings;
            }
        }

        internal override void draw(SpriteBatch spriteBatch)
        {
            //
        }
    }
}
