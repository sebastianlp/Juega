using Juega.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Juega.Graphics
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRectangle;
        public bool IsActive;
        public Texture2D Texture;
        public Color Color;
        Vector2 origin;
        ContentManager contentManager;
        RenderTarget2D renderTarget2D;
        SpriteFont font;
        Dictionary<string, ImageEffect> effectList;
        public string Effects;
        public FadeEffect FadeEffect;
        public SpriteSheetEffect SpriteSheetEffect;

        public Rectangle Rectangle
        {
            get
            {
                int width = Effects != string.Empty ? SpriteSheetEffect.FrameWidth : Texture.Width;
                int heigh = Effects != string.Empty ? SpriteSheetEffect.FrameHeight : Texture.Height;
                return new Rectangle((int)Position.X, (int)Position.Y, width, heigh);
            }
        }

        public Image()
        {
            Path = Text = Effects = string.Empty;
            FontName = "Fonts/Arial";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRectangle = Rectangle.Empty;
            effectList = new Dictionary<string, ImageEffect>();
            Color = Color.White;
        }

        public void LoadContent()
        {
            contentManager = new ContentManager(ScreenManager.Instance.ContentManager.ServiceProvider, "Content");

            if (Path != string.Empty)
                Texture = contentManager.Load<Texture2D>(Path);

            font = contentManager.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
                dimensions.X += Texture.Width;
            else
                dimensions.X += font.MeasureString(Text).X;

            if (Texture != null)
                dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            else
                dimensions.Y = font.MeasureString(Text).Y;

            if (SourceRectangle == Rectangle.Empty)
                SourceRectangle = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            renderTarget2D = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget2D);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();
            Texture = renderTarget2D;
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            // Load Effects
            SetEffect(ref SpriteSheetEffect);
            SetEffect(ref FadeEffect);

            if (Effects != string.Empty)
            {
                string[] splitted = Effects.Split(':');

                foreach (string item in splitted)
                    ActivateEffect(item);
            }
        }

        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            // Vamos a recorrer, activar y ejecutar el update de todos los effectos cargados para la imagen
            foreach (var effect in effectList)
                if (effect.Value.IsActive)
                    effect.Value.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, string textureType = "Image")
        {
            origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);

            if (textureType == "Image")
                spriteBatch.Draw(Texture, Position + origin, SourceRectangle, Color * Alpha, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
            else if (textureType == "Text")
                spriteBatch.DrawString(font, Text, Position + origin, Color);
        }

        void SetEffect<T>(ref T effect)
        {
            if (effect == null)
                effect = (T)Activator.CreateInstance(typeof(T));
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            effectList.Add(effect.GetType().ToString().Replace("Juega.Graphics.", ""), (effect as ImageEffect));
        }

        public void ActivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = true;
                var obj = this;
                effectList[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = false;
                effectList[effect].UnloadContent();
            }
        }

        public void StoreEffects()
        {
            Effects = string.Empty;

            foreach(var effect in effectList)
            {
                if (effect.Value.IsActive)
                    Effects += effect.Key + ":";
            }

            // Borramos el ultimo :
            if (Effects != string.Empty)
                Effects.Remove(Effects.Length - 1);
        }

        public void RestoreEffects()
        {
            foreach (var effect in effectList)
                DeactivateEffect(effect.Key);

            string[] splitted = Effects.Split(':');

            foreach (string effect in splitted)
                ActivateEffect(effect);
        }
    }
}
