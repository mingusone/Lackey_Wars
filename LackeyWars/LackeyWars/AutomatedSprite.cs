using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LackeyWars
{
    class AutomatedSprite:Sprite
    {
        public int team;

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int team)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            this.team = team;
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, int team)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame) 
        {
            this.team = team;
        }

        public override Vector2 Direction
        {
            get { return speed; }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {
            base.Draw(gameTime, spriteBatch, layerDepth);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += Direction;
            base.Update(gameTime, clientBounds);
        }
    }
}
