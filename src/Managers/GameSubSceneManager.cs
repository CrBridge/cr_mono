using System;
using cr_mono.Core.GameLogic;
using cr_mono.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Managers
{
    internal class GameSubSceneManager : IDisposable
    {
        private GameContext context;
        IGameSubScene currentScene;
        private ContentManager content;

        internal GameSubSceneManager(GameContext context) 
        {
            this.context = context;
            // init current scene with the starting scene class
            // current scene = new MapScene etc.
            currentScene = new MapScene(context, this);
        }

        internal void LoadContent(ContentManager content) 
        {
            this.content = content;
            currentScene.LoadContent(content);
        }

        internal void Update(GameTime gameTime) 
        {
            currentScene.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch) 
        {
            currentScene.Draw(spriteBatch);
        }

        internal void SwitchScene(IGameSubScene newScene)
        {
            Dispose();
            currentScene = newScene;
            newScene.LoadContent(content);
        }

        public void Dispose() => currentScene.Dispose();
    }
}
