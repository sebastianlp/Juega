using Juega.Graphics;
using Juega.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juega.Screens
{
    public class PauseScreen : GameScreen
    {
        public Image Image;
        GameScreen gameScreen;

        public PauseScreen()
        {
            Image = new Image();
            Image.Path = "Screens/PauseScreen";
            Image.IsActive = true;
            Image.Alpha = 0.5f;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();
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
                ScreenManager.Instance.PreviousScreen();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
