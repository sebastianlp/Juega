using Microsoft.Xna.Framework;

namespace Juega.Graphics
{
    public class ImageEffect
    {
        protected Image image;
        public bool IsActive;

        public ImageEffect()
        {
            IsActive = false;
        }

        public virtual void LoadContent(ref Image Image)
        {
            image = Image;
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
