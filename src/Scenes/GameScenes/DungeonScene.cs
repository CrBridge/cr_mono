using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cr_mono.Core.GameLogic;
using cr_mono.Managers;
using cr_mono.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.src.Scenes.GameScenes
{
    internal class DungeonScene : IGameSubScene
    {
        private GameContext context;
        private GameSubSceneManager manager;

        internal DungeonScene(GameContext context, GameSubSceneManager manager, int structureId)
        {
            this.context = context;
            this.manager = manager;

            //
        }

        public void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
