using Juega.Graphics;
using Juega.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juega.Screens
{
    public class PresentationScreen : GameScreen
    {
        public Image Image;

        public PresentationScreen()
        {
            Image = new Image();
            Image.Path = "Screens/PresentationScreen";
            Image.IsActive = true;
            Image.Alpha = 0.5f;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();
            Image.FadeEffect.FadeSpeed = 0.7f;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Enter, Keys.Space))
            {
                ScreenManager.Instance.ChangeScreens("GamePlayScreen");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Image.Draw(spriteBatch);
        }
    }
}
