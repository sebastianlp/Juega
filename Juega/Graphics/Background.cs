
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juega.Graphics
{
    public class Background
    {
        public Image Image;

        public Background()
        {
            Image = new Image();
            Image.Path = "Maps/Background";
        }

        public void LoadContent()
        {
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;
            Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
