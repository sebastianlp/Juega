using Juega.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juega.Meta
{
    public class Score
    {
        float score = 0;
        public Image Image;
        public bool EnemyHitted = false;


        public Score()
        {
            Image = new Image();
            Image.FontName = "Fonts/Score";
            Image.Text = "Puntaje: " + score.ToString();
            Image.Color = Color.Blue;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.Position = Vector2.One;
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;

            Image.Update(gameTime);

            if (EnemyHitted)
            {
                score += 1;
                Image.Text = "Puntaje: " + score.ToString();
                EnemyHitted = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch, "Text");
        }
    }
}
