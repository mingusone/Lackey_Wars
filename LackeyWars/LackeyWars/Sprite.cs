using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LackeyWars
{
    abstract class Sprite
    {
        public Texture2D textureImage;
        protected Point frameSize;
        public Point currentFrame;
        public Point sheetSize;
        public int collisionOffset;
        public int timeSinceLastFrame = 0;
        public int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 200;
        public Vector2 speed;
        public Vector2 position;
        public Vector2 spriteCenter;

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : this (textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Track sprite's center
            spriteCenter = new Vector2((int)position.X + (frameSize.X / 2), (int)position.Y + (frameSize.Y / 2));

            // Animoots
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
            }


        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.None, 0);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.None, layerDepth);
        }

        public abstract Vector2 Direction
        {
            get;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

        public float distanceBetween(Vector2 s)
        {
            float distance =(float)Math.Abs(Math.Pow(Math.Pow((s.X - position.X), 2) +Math.Pow((s.Y - position.Y), 2), (1/2)));

            return distance;
        }
    }
}
