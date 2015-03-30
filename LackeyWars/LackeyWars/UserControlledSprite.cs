using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LackeyWars
{
    class UserControlledSprite: Sprite
    {
        public int health;
        public int maxHealth = 3;
        public float rateOfFire = 0.5F;
        public int damage = 1;
        public WeaponReload gun;
        public bool dead = false;
        public Vector2 inputDirection;
        public int summonTimer = 100;
        public int bulletSize = 1;
        public Vector2 speedMod = Vector2.Zero;


        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            gun = new WeaponReload(rateOfFire);
            health = maxHealth;
        }

        public override Vector2 Direction
        {
            get
            {
                inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputDirection.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    inputDirection.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    inputDirection.Y += 1;

                // Temporary speed modifier for player
                if (speedMod != Vector2.Zero)
                    return inputDirection * speed * speedMod;
                else
                    return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            gun.update(gameTime);

            // Bounce things that hit the edge back to display bounce recognition
            if (position.Y < GameInfo.gameTopBoundary)
                position.Y = GameInfo.gameTopBoundary + 2;

            if (position.Y > GameInfo.gameBottomBoundary - frameSize.Y)
                position.Y = GameInfo.gameBottomBoundary - frameSize.Y - 2;

            if (position.X < GameInfo.gameLeftBoundary)
                position.X = GameInfo.gameLeftBoundary + 2;
            if (position.X > GameInfo.gameRightBoundary - frameSize.X)
                position.X = GameInfo.gameRightBoundary - frameSize.X - 2;

            position += Direction;
            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {
            if (inputDirection.X >= 0)
            {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.None, layerDepth);
            }
            else if (inputDirection.X < 0)
            {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.FlipHorizontally, layerDepth);
            }
        }

        public void Damage()
        {
            health--;
        }

        public bool isAlive()
        {
            if (health > 0)
                return true;
            else
                return false;
        }

        public void levelUp()
        {
            maxHealth++;
            health = maxHealth;
        }

    }
}
