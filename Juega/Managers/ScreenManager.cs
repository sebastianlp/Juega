using Juega.Graphics;
using Juega.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Juega.Managers
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        public Vector2 Dimensions { private set; get; }
        public ContentManager ContentManager { private set; get; }
        GameScreen previousScreen, currentScreen, newScreen;
        public GraphicsDevice GraphicsDevice;
        public SpriteBatch SpriteBatch;
        public Image Image;
        public bool IsTransitioning { get; private set; }
        public bool IsLoadingPrevious { get; private set; }
        public bool IsLoadingPauseScreen { get; private set; }
        public int ViewportHeight
        {
            get
            {
                return GraphicsDevice.Viewport.Height;
            }
        }

        public int ViewportWidth
        {
            get
            {
                return GraphicsDevice.Viewport.Width;
            }
        }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();

                return instance;
            }
        }

        public ScreenManager()
        {
            Dimensions = new Vector2(480, 640);
            currentScreen = new PresentationScreen();
            Image = new Image();
            Image.Path = "FadeEffect/Image";
            Image.Effects = "FadeEffect";
            Image.Scale = new Vector2(480, 640);
        }

        public void LoadContent(ContentManager contentManager)
        {
            ContentManager = new ContentManager(contentManager.ServiceProvider, "Content");
            currentScreen.LoadContent();
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
            TransitionPauseScreen(gameTime);
            TransitionBack(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);

            if (IsTransitioning || IsLoadingPrevious)
                Image.Draw(spriteBatch);
        }

        public void ChangeScreens(string screenName)
        {
            previousScreen = currentScreen;
            newScreen = (GameScreen)Activator.CreateInstance(Type.GetType("Juega.Screens." + screenName));
            Image.IsActive = true;
            Image.Alpha = 0.0f;
            Image.FadeEffect.Increase = true;
            IsTransitioning = true;
        }

        public void PreviousScreen()
        {
            IsLoadingPrevious = true;
        }

        public void PauseScreen()
        {
            previousScreen = currentScreen;
            IsLoadingPauseScreen = true;
        }

        void Transition(GameTime gameTime)
        {
            if (IsTransitioning && !IsLoadingPrevious)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    currentScreen.LoadContent();
                } else if (Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }
            }
        }

        void TransitionBack(GameTime gameTime)
        {
            if (IsLoadingPrevious)
            {
                Image.Update(gameTime);
                currentScreen = previousScreen;
                IsLoadingPrevious = false;
            }
        }

        void TransitionPauseScreen(GameTime gameTime)
        {
            if (IsLoadingPauseScreen)
            {
                Image.Update(gameTime);
                currentScreen = (GameScreen)Activator.CreateInstance(Type.GetType("Juega.Screens.PauseScreen"));
                currentScreen.LoadContent();
                IsLoadingPrevious = false;
                IsTransitioning = false;
                IsLoadingPauseScreen = false;
            }
        }
    }
}
