using Juega.Graphics;
using Juega.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Juega.Characters
{
    public class Player
    {
        public Image Image;
        public Vector2 Velocity;
        public float MoveSpeed;
        public List<Shoot> Shoots = new List<Shoot>();
        Vector2 shootStartPosition;

        public Player()
        {
            Image = new Image();
            Velocity = Vector2.Zero;
            Image.Path = "Characters/Player";
            Image.Effects = "SpriteSheetEffect";
            MoveSpeed = 100;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.Position.X = (ScreenManager.Instance.ViewportWidth - Image.SpriteSheetEffect.FrameWidth) / 2;
            Image.Position.Y = ScreenManager.Instance.ViewportHeight - 64;
            Image.SpriteSheetEffect.CurrentFrame.Y = 8;
            Image.SpriteSheetEffect.CurrentFrame.X = 0;
            shootStartPosition = new Vector2(Image.Position.X + (Image.SpriteSheetEffect.FrameWidth / 4), Image.Position.Y - 20);
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
            
            foreach(Shoot shoot in Shoots)
                shoot.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Image.IsActive = true;
            Image.SpriteSheetEffect.LimitX = 8; // Para Movimiento es 8 el limite de sprites que hay en la imagen (lo instanceamos con el maximo que es mayor)

            if (InputManager.Instance.KeyDown(Keys.Right))
            {
                Velocity.X = elapsed;
                Image.SpriteSheetEffect.CurrentFrame.Y = 11;
            }
            else if (InputManager.Instance.KeyDown(Keys.Left))
            {
                Velocity.X = -elapsed;
                Image.SpriteSheetEffect.CurrentFrame.Y = 9;
            }
            else
            {
                Velocity.X = 0;
                Image.SpriteSheetEffect.CurrentFrame.Y = 8; // Lo hago mirar para arriba
                Image.SpriteSheetEffect.LimitX = 1; // Lo seteo en uno asi no cambia frames y deja el 0 clavado.
            }

            if (InputManager.Instance.KeyPressed(Keys.Space))
            {
                Shoot shoot = new Shoot("Characters/Arrow", 200);
                shoot.LoadContent(shootStartPosition);
                Shoots.Add(shoot);
            }

            Vector2 newPosition = Image.Position += Velocity;

            if (newPosition.X + Image.SpriteSheetEffect.FrameWidth >= ScreenManager.Instance.ViewportWidth)
                newPosition.X = ScreenManager.Instance.ViewportWidth - Image.SpriteSheetEffect.FrameWidth;

            if (newPosition.X < 0)
                newPosition.X = 0;

            foreach (Shoot shoot in Shoots)
                shoot.Update(gameTime);

            Image.Update(gameTime);
            shootStartPosition = new Vector2(newPosition.X + (Image.SpriteSheetEffect.FrameWidth / 4), newPosition.Y - 20);
            Image.Position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
            
            foreach(Shoot shoot in Shoots)
                shoot.Draw(spriteBatch);
        }
    }
}
