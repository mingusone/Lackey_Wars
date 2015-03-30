using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LackeyWars
{
    static class GameInfo
    {
        public static Point mapSize = new Point(30, 15);

        public static int tileSize = 32;
        public static int uiSpace = 32;
        public static Point origin = new Point(0, uiSpace);
        public static int gameTopBoundary = origin.Y;
        public static int gameBottomBoundary = origin.Y + (mapSize.Y * tileSize);
        public static int gameLeftBoundary = origin.X;
        public static int gameRightBoundary = origin.X + (mapSize.X * tileSize);
        static int gameMiddleX = (mapSize.X * tileSize)/2;
        static int gameMiddleY = (mapSize.Y * tileSize + uiSpace)/2;
        public static Vector2 gameMiddle = new Vector2(gameMiddleX, gameMiddleY);

        public static int infCost = 150;
        public static int mechCost = 225;
        public static int tankCost = 450;
        public static int artCost = 450;
        public static float computerBulletSpeed = 1.0F;
        public static float playerBulletSpeed = 3.5F;
    }
}
