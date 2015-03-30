using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LackeyWars
{

    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region All sprite Texture2d declarations
        Texture2D ArtIdle;
        Texture2D ArtMov;
        Texture2D BArtIdle;
        Texture2D BArtMov;
        Texture2D InfIdle;
        Texture2D InfMov;
        Texture2D BInfIdle;
        Texture2D BInfMov;
        Texture2D TankIdle;
        Texture2D TankMov;
        Texture2D BTankIdle;
        Texture2D BTankMov;
        Texture2D MechIdle;
        Texture2D MechMov;
        Texture2D BMechIdle;
        Texture2D BMechMov;
        Texture2D Bullet;
        Texture2D BBullet;
        Texture2D Cannon;
        Texture2D BCannon;
        Texture2D Plains;
        Texture2D Cemetary;
        Texture2D Mud;
        Texture2D Building;
        Texture2D PlayerIdle;
        Texture2D PlayerMov;
        #endregion

        SpriteBatch spriteBatch;
        UserControlledSprite player;
        int millisecondsSinceLastPlayerSummon = 0;
        SpriteFont healthFont;
        List<Mob> mobList = new List<Mob>();
        List<Terrain> terrainList = new List<Terrain>();
        List<Bullet> redBullets = new List<Bullet>();
        List<Bullet> blueBullets = new List<Bullet>();

        SoundEffect pew;
        SoundEffect pop;
        SoundEffect bgm;
        SoundEffectInstance bmgInstance;

        public SpriteManager(Game game)
            : base(game)
        {
            #region Load all sprites into texture2d's
            ArtIdle = game.Content.Load<Texture2D>(@"Sprites\Units\ArtIdle");
            ArtMov = game.Content.Load<Texture2D>(@"Sprites\Units\ArtMov");
            BArtIdle = game.Content.Load<Texture2D>(@"Sprites\Units\BArtIdle");
            BArtMov = game.Content.Load<Texture2D>(@"Sprites\Units\BArtMov");
            InfIdle = game.Content.Load<Texture2D>(@"Sprites\Units\InfIdle");
            InfMov = game.Content.Load<Texture2D>(@"Sprites\Units\InfMov");
            BInfIdle = game.Content.Load<Texture2D>(@"Sprites\Units\BInfIdle");
            BInfMov = game.Content.Load<Texture2D>(@"Sprites\Units\BInfMov");
            TankIdle = game.Content.Load<Texture2D>(@"Sprites\Units\TankIdle");
            TankMov = game.Content.Load<Texture2D>(@"Sprites\Units\TankMov");
            BTankIdle = game.Content.Load<Texture2D>(@"Sprites\Units\BTankIdle");
            BTankMov = game.Content.Load<Texture2D>(@"Sprites\Units\BTankMov");
            MechIdle = game.Content.Load<Texture2D>(@"Sprites\Units\MechIdle");
            MechMov = game.Content.Load<Texture2D>(@"Sprites\Units\MechMov");
            BMechIdle = game.Content.Load<Texture2D>(@"Sprites\Units\BMechIdle");
            BMechMov = game.Content.Load<Texture2D>(@"Sprites\Units\BMechMov");
            Bullet = game.Content.Load<Texture2D>(@"Sprites\Bullets\Bullet");
            BBullet = game.Content.Load<Texture2D>(@"Sprites\Bullets\BBullet");
            Cannon = game.Content.Load<Texture2D>(@"Sprites\Bullets\Cannon");
            BCannon = game.Content.Load<Texture2D>(@"Sprites\Bullets\BCannon");
            Plains = game.Content.Load<Texture2D>(@"Sprites\Terrain\Plains");
            Mud = game.Content.Load<Texture2D>(@"Sprites\Terrain\Mud");
            Cemetary = game.Content.Load<Texture2D>(@"Sprites\Terrain\Cemetary");
            Building = game.Content.Load<Texture2D>(@"Sprites\Terrain\Building");
            PlayerIdle = game.Content.Load<Texture2D>(@"Sprites\Units\PlayerIdle");
            PlayerMov = game.Content.Load<Texture2D>(@"Sprites\Units\PlayerMov");
            #endregion
        }

        public override void Initialize()
        {
            player = new UserControlledSprite(PlayerIdle,
                GameInfo.gameMiddle, new Point(32, 32), 10, Point.Zero,
                new Point(4, 1), new Vector2(1.5F, 1.5F));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            healthFont = Game.Content.Load<SpriteFont>(@"Money");

            #region load all terrain

            for (int z = 0; z < GameInfo.mapSize.Y; z++) // For every row
            {
                for (int i = 0; i < GameInfo.mapSize.X; i++) // And each element in that row
                {
                    // ... draw the random terrain

                    int tID; // temporary terrainID
                    int t = ((Game1)(Game)).rnd.Next(1, 101); // Rolling to see what terrain we get

                    Vector2 terrainSpawn = new Vector2(GameInfo.origin.X + (i * GameInfo.tileSize),
                        GameInfo.origin.Y + (z * 32));
                    if (t <= 3) // Chance of Buildings
                    {
                        tID = 4;

                        terrainList.Add(new Terrain(Building, terrainSpawn, tID));
                    }
                    else if (t <= 20) // Chance of Mud (minus chance of mountains)
                    {
                        tID = 2;
                        terrainList.Add(new Terrain(Mud, terrainSpawn, tID));
                    }
                    else if (t <= 23) // Chance of Cemetary (minus chance of mountains)
                    {
                        tID = 3;
                        terrainList.Add(new Terrain(Cemetary, terrainSpawn, tID));
                    }
                    else if (t <= 100) // Plains last
                    {
                        tID = 1;
                        terrainList.Add(new Terrain(Plains, terrainSpawn, tID));
                    }
                }
            }

            #endregion
            pew = Game.Content.Load<SoundEffect>(@"Sounds\pew");
            pop = Game.Content.Load<SoundEffect>(@"Sounds\pop");
            bgm = Game.Content.Load<SoundEffect>(@"Sounds\annoying BGM");
            bmgInstance = bgm.CreateInstance();
            bmgInstance.IsLooped = true;
            bmgInstance.Volume = 0.25F;
            bmgInstance.Play();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (player.isAlive())
                player.Update(gameTime, Game.Window.ClientBounds);

            #region Game Level and Spawning Enemies

            int remainingHostiles = 0;
            foreach (Mob s in mobList)
            {
                if (s.team != 1)
                    remainingHostiles++;
            }



            if (mobList.Count == 0 || remainingHostiles == 0) // If there are no mobs on the screen or no hostile mobs
            {
                ((Game1)(Game)).level++; // Increase level
                ((Game1)(Game)).levelSpawned = false; // Restarts level spawning procedure
            }

            if (((Game1)(Game)).levelSpawned == false) // Level has ended, but bonuses here
            {
                #region spawning foes based on level
                int level = ((Game1)(Game)).level;
                SpawnPointTracker s = new SpawnPointTracker(); // Tracker for rotating spawn point

                int infSpawn = 4 + level;
                Point SS = new Point(4,1); // universal sheet size, just placed here for convinence

                for (int i = 0; i < infSpawn; i++)
                {
                    mobList.Add( new Mob(BInfMov, s.next(), SS, 2, 1));
                }
                int mechSpawn = level / 3;
                for (int i = 0; i < mechSpawn; i++)
                {
                    mobList.Add(new Mob(BMechMov, s.next(), SS, 2, 2));
                }
                int tankSpawn = level / 2;
                for (int i = 0; i < tankSpawn; i++)
                {
                    mobList.Add(new Mob(BTankMov, s.next(), SS, 2, 3));
                }
                int artSpawn = level / 4;
                for (int i = 0; i < artSpawn; i++)
                {
                    mobList.Add(new Mob(BArtMov, s.next(), SS, 2, 4));
                }
                #endregion

                GameInfo.computerBulletSpeed += 1.01F;
                player.levelUp(); // Get 1 health per level
                foreach (Mob redSummon in mobList)
                {
                    if (redSummon.team == 1)
                        redSummon.LevelUp();
                }

                ((Game1)(Game)).levelSpawned = true;
            }

            #endregion

            #region Player idle/run animation switching
            if (player.Direction == Vector2.Zero && player.textureImage != InfIdle)
            {
                player.textureImage = PlayerIdle;
            }
            else if (player.Direction != Vector2.Zero && player.textureImage != InfMov)
            {
                player.textureImage = PlayerMov;
            }

            #endregion

            #region CHEATING
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                player.health = 999;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                ((Game1)(Game)).playerMoney += 100;
            }
            #endregion

            #region playerShooting
            Point levelOneBullet = new Point(11, 11);

            if (player.bulletSize == 1)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (player.gun.canFire == true)
                    {
                        redBullets.Add(new Bullet(Bullet, player.spriteCenter, levelOneBullet, new Vector2(GameInfo.playerBulletSpeed * 0, GameInfo.playerBulletSpeed * -1), 1, player.damage));
                        player.gun.canFire = false;
                        pew.Play(0.5F, 0, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    if (player.gun.canFire == true)
                    {
                        redBullets.Add(new Bullet(Bullet, player.spriteCenter, levelOneBullet, new Vector2(GameInfo.playerBulletSpeed * -1, GameInfo.playerBulletSpeed * 0), 1, player.damage));
                        player.gun.canFire = false;
                        pew.Play(0.5F, 0, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (player.gun.canFire == true)
                    {
                        redBullets.Add(new Bullet(Bullet, player.spriteCenter, levelOneBullet, new Vector2(GameInfo.playerBulletSpeed * 0, GameInfo.playerBulletSpeed * 1), 1, player.damage));
                        player.gun.canFire = false;
                        pew.Play(0.5F, 0, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    if (player.gun.canFire == true)
                    {
                        redBullets.Add(new Bullet(Bullet, player.spriteCenter, levelOneBullet, new Vector2(GameInfo.playerBulletSpeed * 1, GameInfo.playerBulletSpeed * 0), 1, player.damage));
                        player.gun.canFire = false;
                        pew.Play(0.5F, 0, 0);
                    }
                }

            }
            #endregion

            #region Player Spawning Minions

            millisecondsSinceLastPlayerSummon += gameTime.ElapsedGameTime.Milliseconds;

            if (Keyboard.GetState().IsKeyDown(Keys.D1) && millisecondsSinceLastPlayerSummon > player.summonTimer)
            {
                if (((Game1)(Game)).playerMoney >= GameInfo.infCost)
                {
                    mobList.Add(new Mob(InfMov, player.position, new Point(4,1), 1, 1));
                    ((Game1)(Game)).playerMoney -= GameInfo.infCost;
                    millisecondsSinceLastPlayerSummon = 0;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2) && millisecondsSinceLastPlayerSummon > player.summonTimer)
            {
                if (((Game1)(Game)).playerMoney >= GameInfo.mechCost)
                {
                    mobList.Add(new Mob(MechMov, player.position, new Point(4,1), 1, 2));
                    ((Game1)(Game)).playerMoney -= GameInfo.mechCost;
                    millisecondsSinceLastPlayerSummon = 0;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3) && millisecondsSinceLastPlayerSummon > player.summonTimer)
            {
                if (((Game1)(Game)).playerMoney >= GameInfo.tankCost)
                {
                    mobList.Add(new Mob(TankMov, player.position, new Point(4,1), 1, 3));
                    ((Game1)(Game)).playerMoney -= GameInfo.tankCost;
                    millisecondsSinceLastPlayerSummon = 0;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4) && millisecondsSinceLastPlayerSummon > player.summonTimer)
            {
                if (((Game1)(Game)).playerMoney >= GameInfo.artCost)
                {
                    mobList.Add(new Mob(ArtMov, player.position, new Point(4, 1), 1, 4));
                    ((Game1)(Game)).playerMoney -= GameInfo.artCost;
                    millisecondsSinceLastPlayerSummon = 0;
                }
            }
            
            #endregion

            #region Death Check and Money given to player
            if (player.health <= 0) // Checks to see if player is alive
            {
                player.dead = true;
            }

            List<Mob> dead = new List<Mob>(); // Temporary list of dead mob, should delete itself as it falls out of scope otherwise we got a huge problem of a list that just keeps on getting bigger
            foreach (Mob s in mobList)
            {
                if (s.health <= 0)
                    dead.Add(s); // If mob has no health, add it to dead list
            }
            foreach (Mob d in dead)
            {
                if (d.team != 1)
                {
                    // add money to player based on mob's loot
                    ((Game1)(Game)).playerMoney += d.loot;
                }
                mobList.Remove(d); // for every mob in dead list, remove it from game
            }

            List<Bullet> deadBullets = new List<Bullet>();
            foreach (Bullet b in redBullets)
            {
                if (b.dead == true)
                    deadBullets.Add(b);
            }
            foreach (Bullet d in deadBullets)
            {
                redBullets.Remove(d);
            }

            List<Bullet> blueDeadBullets = new List<Bullet>();
            foreach (Bullet b in blueBullets)
            {
                if (b.dead == true)
                    blueDeadBullets.Add(b);
            }
            foreach (Bullet d in blueDeadBullets)
            {
                blueBullets.Remove(d);
            }
            #endregion

            #region bullet updates and terrain collision



            foreach (Bullet b in redBullets)
            {
                foreach (Terrain t in terrainList)
                {
                    if (t.bulletCollide == true)
                    {
                        if (t.collisionRect.Intersects(b.collisionRect))
                        {
                            b.dead = true;
                        }
                    }
                }
                b.Update(gameTime, Game.Window.ClientBounds);
            }

            foreach (Bullet b in blueBullets)
            {
                foreach (Terrain t in terrainList)
                {
                    if (t.bulletCollide == true)
                    {
                        if (t.collisionRect.Intersects(b.collisionRect))
                        {
                            b.dead = true;
                        }
                    }
                }
                b.Update(gameTime, Game.Window.ClientBounds);
            }

            #endregion

            // WERID BUG WHEN TWO OBJECTS ARE COLLIDING AND THEY HIT A THIRD ONE, THE ORIGINAL TWO OBJECTS GO FLYING 
            // IN OPPOSITE DIRECTION
            #region Player and enemy bullet collisions
            foreach (Bullet b in blueBullets)
            {
                if (b.collisionRect.Intersects(player.collisionRect))
                {
                    b.dead = true;
                    player.Damage();
                    pop.Play();
                }
            }
            #endregion
            #region player terrain collision
            foreach (Terrain t in terrainList)
            {
                if (t.unitCollide== true)
                {
                    if (player.collisionRect.Intersects(t.collisionRect))
                    {
                        if (t.position.X < player.position.X)
                            player.position.X += 2;
                        else if (t.position.X > player.position.X)
                            player.position.X -= 2;

                        if (t.position.Y < player.position.Y)
                            player.position.Y += 2;
                        else if (t.position.Y > player.position.Y)
                            player.position.Y -= 2;
                    }
                }
            }
            #endregion
            #region Mob updates and mob collisions
            foreach (Mob s in mobList)
            {
                #region targetting

                s.targetNearestEnemy(mobList, player); // Tell AI to target

                if (s.assassin && (s.team != 1))
                { s.target = player; }

                #endregion
                #region AI shooting
                if (s.gun.canFire == true && s.target != null)
                {
                    if (s.team == 1)
                    {
                        if(s.bulletSize == 1)
                        {
                            redBullets.Add(new Bullet(Bullet, s.spriteCenter, s.bulletSizePoint,
                                s.bulletAim(s.target) * GameInfo.computerBulletSpeed, s.team, s.damage));
                        }
                        else if (s.bulletSize == 2)
                        {
                            redBullets.Add(new Bullet(Cannon, s.spriteCenter, s.bulletSizePoint,
                                s.bulletAim(s.target) * GameInfo.computerBulletSpeed, s.team, s.damage));
                        }
                        s.gun.canFire = false;
                    }
                    else if (s.team == 2)
                    {
                        if (s.bulletSize == 1)
                        {
                            blueBullets.Add(new Bullet(BBullet, s.spriteCenter, s.bulletSizePoint, s.bulletAim(s.target) * GameInfo.computerBulletSpeed, s.team, s.damage));
                        }
                        if (s.bulletSize == 2)
                        {
                            blueBullets.Add(new Bullet(BCannon, s.spriteCenter, s.bulletSizePoint, s.bulletAim(s.target) * GameInfo.computerBulletSpeed, s.team, s.damage));
                        }
                        s.gun.canFire = false;

                    }
                    pew.Play(0.1F, -1.0F, 0);
                }
                #endregion
                #region Bullet Collision

                foreach (Bullet b in redBullets)
                {
                    if (s.collisionRect.Intersects(b.collisionRect) && s.team != 1)
                    {
                        s.Damage(s.damage);
                        b.dead = true;
                        pop.Play(0.1F, 0, 0);
                    }
                }

                foreach (Bullet b in blueBullets)
                {
                    if (s.collisionRect.Intersects(b.collisionRect) && s.team != 2)
                    {
                        s.Damage(b.damage);
                        b.dead = true;
                        pop.Play(0.1F, 0, 0);
                    }
                }
                #endregion
                #region Player Collision
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    // P moves, S still
                    if ((player.Direction.X != 0 || player.Direction.Y != 0) && (s.Direction.X == 0 && s.Direction.Y == 0))
                    {
                        player.position.X = (player.position.X + (-2 * player.Direction.X + 1)); // negative means moving backward
                        player.position.Y = (player.position.Y + (-2 * player.Direction.Y + 1));
                        /* The +1 at the end is so that when a collision occurs, continuing to move in that direction 
                         * slides you tangent. This is to prevent those annoying "super friction surface contact" moments
                         * that noob 2d games have. */
                        s.position.X = (s.position.X + (2 * player.Direction.X));
                        s.position.Y = (s.position.Y + (2 * player.Direction.Y));
                    }
                    // P still, S moves
                    else if ((player.Direction.X == 0 && player.Direction.Y == 0) && (s.direction.X != 0 || s.direction.Y != 0))
                    {
                        player.position.X = (player.position.X + (2 * s.direction.X)); // Positive means moving forward of something's vector
                        player.position.Y = (player.position.Y + (2 * s.direction.Y));
                        s.position.X = (s.position.X + (-2 * s.direction.X+1));
                        s.position.Y = (s.position.Y + (-2 * s.direction.Y+1));
                    }
                    // P moves, S moves
                    else if ((player.Direction.X != 0 || player.Direction.Y != 0) && (s.direction.X != 0 || s.direction.Y != 0))
                    {
                        if (s.position.X < player.position.X)
                        {
                            s.position.X--;
                            player.position.X++;
                        }
                        else if (s.position.X > player.position.X)
                        {
                            s.position.X++;
                            player.position.X--;
                        }
                        if (s.position.Y < player.position.Y)
                        {
                            s.position.Y--;
                            player.position.Y++;
                        }
                        else if (s.position.Y > player.position.Y)
                        {
                            s.position.Y++;
                            player.position.Y--;
                        }
                    }

                }
                # endregion
                #region COMP to TERRAIN Collision
                foreach (Terrain t in terrainList)
                {
                    if(t.unitCollide == true && t.collisionRect.Intersects(s.collisionRect))
                    {
                        if (t.position.X < s.position.X)
                            s.position.X += 1.5F;
                        else if (t.position.X > s.position.X)
                            s.position.X -= 1.5F;

                        if (t.position.Y < s.position.Y)
                            s.position.Y += 1.5F;
                        else if (t.position.Y > s.position.Y)
                            s.position.Y -= 1.5F;
                    }
                }
                #endregion
                # region COMP to COMP Collision
                foreach (Mob a in mobList)
                {

                    if (a == s)
                        { continue; } // Do not compare sprite "a" with itself
                    if (a.collisionRect.Intersects(s.collisionRect))
                    {
                        // A moves, S still
                        if ((a.direction.X != 0 || a.direction.Y != 0) && (s.direction.X == 0 && s.direction.Y == 0))
                        {
                            a.position.X = (a.position.X + (-2 * a.direction.X + 1)); // negative means moving backward
                            a.position.Y = (a.position.Y + (-2 * a.direction.Y + 1));
                            /* The +1 at the end is so that when a collision occurs, continuing to move in that direction 
                             * slides you tangent. This is to prevent those annoying "super friction surface contact" moments
                             * that noob 2d games have. */
                            s.position.X = (s.position.X + (2 * a.direction.X));
                            s.position.Y = (s.position.Y + (2 * a.direction.Y));
                        }
                        // A still, S moves
                        else if ((a.direction.X == 0 && a.direction.Y == 0) && (s.direction.X != 0 || s.direction.Y != 0))
                        {
                            a.position.X = (a.position.X + (2 * s.direction.X)); // Positive means moving forward of something's vector
                            a.position.Y = (a.position.Y + (2 * s.direction.Y));
                            s.position.X = (s.position.X + (-2 * s.direction.X + 1));
                            s.position.Y = (s.position.Y + (-2 * s.direction.Y + 1));
                        }
                        // A moves, S moves
                        else if ((a.direction.X != 0 || a.direction.Y != 0) && (s.direction.X != 0 || s.direction.Y != 0))
                        {
                            if (s.position.X < a.position.X)
                            {
                                s.position.X--;
                                a.position.X++;
                            }
                            else if (s.position.X > a.position.X)
                            {
                                s.position.X++;
                                a.position.X--;
                            }
                            if (s.position.Y < a.position.Y)
                            {
                                s.position.Y--;
                                a.position.Y++;
                            }
                            else if (s.position.Y > a.position.Y)
                            {
                                s.position.Y++;
                                a.position.Y--;
                            }
                        }

                        a.Update(gameTime, Game.Window.ClientBounds); // If collision happens, update "a"
                    }
                }
                #endregion
                s.Update(gameTime, Game.Window.ClientBounds);
            }
            #endregion


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            // Draw all Terrain
            foreach (Sprite s in terrainList)
            {
                s.Draw(gameTime, spriteBatch, 0.5F);
            }

            // Draw all mobs
            foreach (Sprite s in mobList)
            {
                s.Draw(gameTime, spriteBatch, 0.31F);
            }
            
            // Draw Player
            player.Draw(gameTime, spriteBatch, 0.3F);

            // Draw Red Bullets
            foreach (Sprite s in redBullets)
            {
                s.Draw(gameTime, spriteBatch, 0.11F);
            }

            // Draw Blue Bullets
            foreach (Sprite s in blueBullets)
            {
                s.Draw(gameTime, spriteBatch, 0.1F);
            }

            spriteBatch.DrawString(healthFont, "Health: " + player.health + "/" + player.maxHealth, new Vector2((Game.Window.ClientBounds.Width / 5)*4, 5),
                Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}
