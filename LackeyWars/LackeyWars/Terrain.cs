using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LackeyWars
{
    class Terrain : Sprite
    {
        public int terrainID; // 1 = Plains, 2 = Mud, 3 = Cemetary, 4 = Buildings
        public bool unitCollide = false;
        public bool bulletCollide = false;

        public Terrain(Texture2D textureImage, Vector2 position, int terrainID)
            : base(textureImage, position, new Point(GameInfo.tileSize, GameInfo.tileSize), 0, new Point(0, 0), new Point(32, 32), Vector2.Zero)
        {
            this.terrainID = terrainID;
            if (terrainID == 4)
            {
                unitCollide = true;
                bulletCollide = true;
            }
            else if (terrainID == 3)
            {
                bulletCollide = true;
            }


        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(0, 0, frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0);
        }

        public override Vector2 Direction
        {
            get { return Vector2.Zero; }
        }

    }
}
