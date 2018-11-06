using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Juega.Graphics;

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
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;

            Image.SpriteSheetEffect.LimitX = 8; // Seteamos esto por lo mismo que el player.

            Velocity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Image.Update(gameTime);

            Image.Position = Image.Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
