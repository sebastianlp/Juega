using Juega.Graphics;
using Juega.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juega.Screens
{
    public class GameOverScreen : GameScreen
    {
        public Image Image;

        public GameOverScreen()
        {
            Image = new Image();
            Image.Path = "Screens/GameOverScreen";
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

            if (InputManager.Instance.KeyPressed(Keys.R))
                ScreenManager.Instance.ChangeScreens("GamePlayScreen");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Image.Draw(spriteBatch);
        }
    }
}
