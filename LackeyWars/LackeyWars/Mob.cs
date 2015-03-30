using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LackeyWars
{
    class Mob:AutomatedSprite
    {
        #region mob variables
        public int unitID; // 1 = Infantry, 2 = Mech, 3 = Tank, 4 = Artillery
        public float health;
        public float maxHealth;
        public float originalHealth;
        public float maxSpeed;
        public float maxLeveledUpSpeed;
        public float rateOfFire;
        public float maxLeveledUpRateOfFire;
        public int damage = 1;
        public int loot = 0;
        public int bulletSize = 1;
        public Point bulletSizePoint;
        public WeaponReload gun;
        public Sprite target = null;
        public int movPattern;
        public bool assassin = false; // Assassins will go only after player instead of closest enemy
        // 1 = Follow Player, 2 = Bounce around
        public Vector2 direction;
        public Vector2 speedMod = Vector2.Zero;
        Random random = new Random();
        #endregion

        public Mob(Texture2D textureImage, Vector2 position,Point sheetSize, int team, int unitID): base(textureImage, position, new Point(32,32), 10, Point.Zero, sheetSize, Vector2.Zero, team)
        {
            this.unitID = unitID;

            #region Unit stats
            if (team != 999) // Reserved for future team stats
            {
                if (unitID == 1) // Infantry
                {
                    health = 3;
                    rateOfFire = 2F;
                    damage = 1;
                    bulletSize = 1;
                    movPattern = 2;
                    maxSpeed = 0.5F;
                    speed = new Vector2(maxSpeed, maxSpeed);
                    loot = 100;
                }
                else if (unitID == 2) // Mech
                {
                    health = 5;
                    rateOfFire = 2.5F;
                    damage = 1;
                    bulletSize = 2;
                    movPattern = 2;
                    maxSpeed = 0.4F;
                    speed = new Vector2(maxSpeed, maxSpeed);
                    loot = 150;
                    assassin = true;
                }
                else if (unitID == 3) // Tank
                {
                    health = 7;
                    rateOfFire = 4F;
                    damage = 1;
                    bulletSize = 1;
                    movPattern = 2;
                    maxSpeed = 0.8F;
                    speed = new Vector2(maxSpeed, maxSpeed);
                    loot = 300;
                }
                else if (unitID == 4) // Artillery
                {
                    health = 4;
                    rateOfFire = 1F;
                    damage = 1;
                    bulletSize = 2;
                    movPattern = 3;
                    maxSpeed = 0.3F;
                    speed = new Vector2(maxSpeed, maxSpeed);
                    loot = 300;
                }

                this.maxHealth = health;
                originalHealth = health;
                this.maxLeveledUpSpeed = maxSpeed * 3;
                maxLeveledUpRateOfFire = rateOfFire * 0.5F;
            }
            #endregion
            #region bullet size stuff
            if (bulletSize == 1)
            {
                bulletSizePoint = new Point(11, 11);
            }
            if (bulletSize == 2)
            {
                bulletSizePoint = new Point(17, 17);
            }
            if (bulletSize == 3)
            {
                bulletSizePoint = new Point(25, 25);
            }
            #endregion
            gun = new WeaponReload(rateOfFire);
            gun.AIrandomReloadOn = true;
        }
        
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            #region Mov Pattern 1
            // AI reverse direction when hitting the edge of the screen
            if (movPattern == 1)
            {
                if ((position.X < GameInfo.gameLeftBoundary && speed.X < 0) || ((position.X > GameInfo.gameRightBoundary - frameSize.X) && speed.X > 0))
                    speed.X *= -1;
                if ((position.Y < GameInfo.gameTopBoundary && speed.Y < 0) || ((position.Y > GameInfo.gameBottomBoundary - frameSize.Y) && speed.Y > 0))
                    speed.Y *= -1;

                // Don't go past edges
                if (position.X < GameInfo.gameLeftBoundary)
                    position.X += 3;
                else if (position.X > GameInfo.gameRightBoundary - frameSize.X)
                    position.X -= 3;

                if (position.Y < GameInfo.gameTopBoundary)
                    position.Y += 3;
                else if (position.Y > GameInfo.gameBottomBoundary - frameSize.Y)
                    position.Y -= 3;
            }
            #endregion
            #region Mov Pattern 2
            // AI Follow player and valid target
            if (movPattern == 2 && target != null)
            {
                // Change direction to follow player

                    if (target.position.X < position.X)
                        direction.X = -1;
                    else if (target.position.X > position.X)
                        direction.X = +1;

                    if (target.position.Y < position.Y)
                        direction.Y = -1;
                    else if (target.position.Y > position.Y)
                        direction.Y = +1;

                // Don't go past edges
                    if (position.X < GameInfo.gameLeftBoundary)
                        position.X += 3;
                    else if (position.X > GameInfo.gameRightBoundary - frameSize.X)
                        position.X -= 3;

                    if (position.Y < GameInfo.gameTopBoundary)
                        position.Y += 3;
                    else if (position.Y > GameInfo.gameBottomBoundary - frameSize.Y)
                        position.Y -= 3;
            }
            #endregion
            #region Mov Pattern 3
            // AI evade player and target
            if (movPattern == 3 && target != null)
            {
                // Change direction to evade player

                if (target.position.X < position.X)
                    direction.X = +1;
                else if (target.position.X > position.X)
                    direction.X = -1;

                if (target.position.Y < position.Y)
                    direction.Y = +1;
                else if (target.position.Y > position.Y)
                    direction.Y = -1;

                // Don't go past edges
                if (position.X < GameInfo.gameLeftBoundary)
                    position.X += 3;
                else if (position.X > GameInfo.gameRightBoundary - frameSize.X)
                    position.X -= 3;

                if (position.Y < GameInfo.gameTopBoundary)
                    position.Y += 3;
                else if (position.Y > GameInfo.gameBottomBoundary - frameSize.Y)
                    position.Y -= 3;
            }
            #endregion
            gun.update(gameTime);
            base.Update(gameTime, clientBounds);
        }

        public void LevelUp()
        {
            maxHealth += (originalHealth * 0.1F); // Heal to full, increase max life by 10%
            health = maxHealth;

            if (speed.X < maxLeveledUpSpeed) 
                speed = speed * 1.05F;
            if (rateOfFire < maxLeveledUpRateOfFire)
                rateOfFire *= 0.99F;
        }

        public override Vector2 Direction
        {
            get { return speed * direction; }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {
            if (direction.X >= 0)
            {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.None, layerDepth);
            }
            else if (direction.X < 0)
            {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.FlipHorizontally, layerDepth);
            }
        }

        public void Damage(int dmg)
        {
            health = health - dmg;
        }

        public Vector2 whereIsTarget()
        {
            Vector2 compassDirection = new Vector2();
            if (target == null)
                return Vector2.Zero;

            compassDirection = target.position - position;
            compassDirection.Normalize();
            return compassDirection;

            
        }

        public Vector2 whereIsPlayer(Sprite player)
        {
            Vector2 compassDirection = new Vector2();
            compassDirection = player.position - position;
            compassDirection.Normalize();

            return compassDirection;
        }

        public Vector2 bulletAim(Sprite player)
        {
            if (target != null)
            {
                Vector2 Aim = target.position - position;
                Aim.X += target.Direction.X * random.Next(-50,50);
                Aim.Y += target.Direction.Y * random.Next(-50, 50);
                Aim.Normalize();
                return Aim;
            }
            else
            {
                return whereIsPlayer(player);
            }
            
        }

        public bool isThereAnEnemy(List<Mob> list)
        {
            foreach (Mob s in list)
            {
                if (s.team != team) // When something not on Mob's own team is found, return true
                    return true;
            }
            return false; // Otherwise return false
        }

        public void targetNearestEnemy(List<Mob> list, Sprite Player)
        {
            if (isThereAnEnemy(list)) // Check if there are enemies
            {
                Sprite closest;
                if (team != 1)
                    closest = Player;
                else
                    closest = null;

                foreach (Mob current in list)
                {
                    if (closest == null && current.team != team) // First run, if s is not friendly
                    {
                        closest = current;
                    }
                    else if (current.team != team)
                    {
                        float distanceToClosest = Math.Abs(Vector2.Distance(position, closest.position));
                        float distanceToCurrent = Math.Abs(Vector2.Distance(position, current.position));

                        if (distanceToCurrent < distanceToClosest)
                        { closest = current; }
                    }
                }
                target = closest;
            }
            else
            {
                target = Player;
            }
        }

    }
}
