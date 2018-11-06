using Juega.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juega.Meta
{
    public class Life
    {
        int life = 100;
        Image Image;
        public bool Hitted = false;
        public int Modifier = 1;

        public Life()
        {
            Image = new Image();
            Image.FontName = "Fonts/Score";
            Image.Text = "Vida: " + life.ToString();
            Image.Color = Color.Green;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.Position = new Vector2(480 - Image.Texture.Width * 2, 0);
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;

            Image.Update(gameTime);

            if (Hitted)
            {
                life -= 10 * Modifier;
                Image.Text = "Vida: " + life.ToString();
                Hitted = false;
                Modifier = 1;
            }

            if (life < 60) Image.Color = Color.Yellow;
            if (life < 30) Image.Color = Color.Red;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.IsActive = true;
            Image.Draw(spriteBatch, "Text");
        }
    }
}
