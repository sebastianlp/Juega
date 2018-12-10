using Juega.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juega.Meta
{
    public class Life
    {
        public float PlayerLife = 100;
        Image Image;
        public bool Hitted = false;
        public float Modifier = 1.0f;

        public Life()
        {
            Image = new Image();
            Image.FontName = "Fonts/Score";
            Image.Text = "Vida: " + ((int)PlayerLife).ToString();
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
                PlayerLife -= 10.0f * Modifier;
                Image.Text = "Vida: " + ((int)PlayerLife).ToString();
                Hitted = false;
                Modifier = 1;
            }

            if (PlayerLife < 60) Image.Color = Color.Yellow;
            if (PlayerLife < 30) Image.Color = Color.Red;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch, "Text");
        }
    }
}
