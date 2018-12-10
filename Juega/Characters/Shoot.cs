using Juega.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Juega.Characters
{
    public class Shoot
    {
        public Image Image;
        public Vector2 Velocity;
        public float MoveSpeed;
        public bool Impacted = false;

        public Shoot(string imagePath, int moveSpeed)
        {
            Image = new Image();
            Image.Path = imagePath;
            MoveSpeed = moveSpeed;
        }

        public void LoadContent(Vector2 shootStartPosition)
        {
            Image.LoadContent();
            Image.Position = shootStartPosition;
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime, string direction = "down")
        {
            Image.IsActive = true;

            if (direction == "down")
                Velocity.Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                Velocity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Image.Update(gameTime);

            Image.Position = Image.Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Image.IsActive)
                Image.Draw(spriteBatch);
        }
    }
}