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

        public Shoot()
        {
            Image = new Image();
            Image.Path = "Characters/Arrow";
            MoveSpeed = 100;
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

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;

            Velocity.Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

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