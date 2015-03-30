using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LackeyWars
{
    class WeaponReload
    {
        public bool canFire;
        public double rateOfFire;
        public int millisecondsSinceLastShot;
        public bool AIrandomReloadOn = false;
        public static Random r = new Random();

        public WeaponReload(double rateOfFire)
        {
            canFire = true;
            this.rateOfFire = rateOfFire;
            millisecondsSinceLastShot = 0;

        }

        public void update(GameTime gameTime)
        {
            millisecondsSinceLastShot += gameTime.ElapsedGameTime.Milliseconds;
            if (millisecondsSinceLastShot > (rateOfFire * 1000))
            {
                canFire = true;
                if (!AIrandomReloadOn)
                {
                    millisecondsSinceLastShot = 0;
                }
                else
                {
                     millisecondsSinceLastShot = r.Next(-500, 500);
                }
            }
        }

    }
}
