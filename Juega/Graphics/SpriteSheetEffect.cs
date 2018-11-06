using Microsoft.Xna.Framework;

namespace Juega.Graphics
{
    public class SpriteSheetEffect : ImageEffect
    {
        public int FrameCounter;
        public int SwitchFrame;
        public Vector2 CurrentFrame;
        public Vector2 AmountOfFrames;
        public int LimitX;
        public int FrameWidth
        {
            get
            {
                if (image.Texture != null)
                    return image.Texture.Width / (int)AmountOfFrames.X;

                return 0;
            }
        }
        public int FrameHeight
        {
            get
            {
                if (image.Texture != null)
                    return image.Texture.Height / (int)AmountOfFrames.Y;

                return 0;
            }
        }

        public SpriteSheetEffect()
        {
            AmountOfFrames = new Vector2(13, 21);
            CurrentFrame = new Vector2(3, 0);
            SwitchFrame = 100;
            FrameCounter = 0;
            LimitX = (int)AmountOfFrames.X;
        }

        public override void LoadContent(ref Image Image)
        {
            base.LoadContent(ref Image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (image.IsActive)
            {
                FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (FrameCounter >= SwitchFrame)
                {
                    FrameCounter = 0;

                    if (CurrentFrame.X < LimitX)
                        CurrentFrame.X++;
                    else
                        CurrentFrame.X = 1;

                    if (CurrentFrame.X * FrameCounter >= image.Texture.Width)
                        CurrentFrame.X = 1;
                }
            }
            else
                CurrentFrame.X = 0;

            image.SourceRectangle = new Rectangle(
                (int)CurrentFrame.X * FrameWidth, 
                (int)CurrentFrame.Y * FrameHeight, 
                FrameWidth, 
                FrameHeight);
        }
    }
}
