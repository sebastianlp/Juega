using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Juega.Graphics;
using System.Collections.Generic;

namespace Juega.Characters
{
    public class Skeleton
    {
        public Image Image;
        public Vector2 Velocity;
        public float MoveSpeed;
        public int AttackPower = 10;
        public bool Strike = false;
        public int Life = 50;
        public List<Shoot> Shoots = new List<Shoot>();
        public bool Hitted = false;
        public bool OutOfBounds = false;
        float elapsedTimeSinceLastShoot = 0;
        

        public Skeleton()
        {
            Image = new Image();
            Velocity = Vector2.Zero;
            Image.Path = "Characters/Skeleton";
            Image.Effects = "SpriteSheetEffect";
            MoveSpeed = 60;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.SpriteSheetEffect.CurrentFrame.Y = 10;
            Image.SpriteSheetEffect.CurrentFrame.X = 0;
        }

        public void UnloadContent()
        {
            Image.UnloadContent();

            foreach (Shoot shoot in Shoots)
                shoot.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;

            Image.SpriteSheetEffect.LimitX = 8; // Seteamos esto por lo mismo que el player.

            Velocity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            elapsedTimeSinceLastShoot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Cada un segundo disparamos
            if (elapsedTimeSinceLastShoot > 1)
            {
                Shoot shoot = new Shoot("Characters/Fireball", 120);
                Vector2 shootStartPosition = new Vector2(Image.Position.X + (Image.SpriteSheetEffect.FrameWidth / 4), Image.Position.Y + 20);
                shoot.LoadContent(shootStartPosition);
                Shoots.Add(shoot);
                elapsedTimeSinceLastShoot -= 1;
            }

            foreach (Shoot shoot in Shoots)
                shoot.Update(gameTime, "up");

            Image.Update(gameTime);

            Image.Position = Image.Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);

            foreach (Shoot shoot in Shoots)
                shoot.Draw(spriteBatch);
        }
    }
}
