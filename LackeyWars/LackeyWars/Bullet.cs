using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LackeyWars
{
    class Bullet:AutomatedSprite
    {
        public bool dead = false;
        public int damage;
        public Bullet(Texture2D textureImage, Vector2 position, Point frameSize, Vector2 speed, int team, int dmg)
            : base(textureImage, position, frameSize, 0,  Point.Zero,  new Point(1,1),  speed,  team)
        {
            damage = dmg;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (position.Y < GameInfo.gameTopBoundary ||
                position.Y > GameInfo.gameBottomBoundary ||
                position.X < GameInfo.gameLeftBoundary ||
                position.X > GameInfo.gameRightBoundary)
            {
                dead = true;
            }

            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }



    }
}
