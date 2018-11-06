using Juega.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Juega.Screens
{
    public class GameScreen
    {
        protected ContentManager contentManager;
        public Type Type;
        
        public GameScreen()
        {
            Type = GetType();
        }

        public virtual void LoadContent()
        {
            contentManager = new ContentManager(ScreenManager.Instance.ContentManager.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            contentManager.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
