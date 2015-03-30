using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LackeyWars
{


    class SpawnPointTracker
    {
        Vector2 North, East, South, West;
        int current;

        public SpawnPointTracker()
        {
            North = new Vector2(((float)GameInfo.gameRightBoundary / 2), ((float)GameInfo.gameTopBoundary)); // = 1
            East = new Vector2(((float)GameInfo.gameLeftBoundary), ((float)GameInfo.gameBottomBoundary / 2)); // = 2
            South = new Vector2(((float)GameInfo.gameRightBoundary), ((float)GameInfo.gameBottomBoundary / 2)); // = 3
            West = new Vector2(((float)GameInfo.gameRightBoundary / 2), ((float)GameInfo.gameBottomBoundary)); // = 4
            current = 1;
        }


        public Vector2 next()
        {
            switch(current)
            {
                case 1:
                    current++;
                    return North;
                case 2:
                    current++;
                    return East;
                case 3:
                    current++;
                    return South;
                case 4:
                    current = 1;
                    return West;
                default:
                    current = 1;
                    return North;
            }
        }

    }
}
