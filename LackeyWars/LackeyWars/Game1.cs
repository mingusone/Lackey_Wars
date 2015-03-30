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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        public Random rnd { get; private set; }

        SpriteFont moneySpriteFont;
        public int playerMoney = 0;

        SpriteFont levelSpriteFont;
        public int level = 0;
        public bool levelSpawned = false;
        public int nextLevel = 1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            rnd = new Random();
            Content.RootDirectory = "Content";

        //======================================================
		// Width is map size
            graphics.PreferredBackBufferWidth = (GameInfo.mapSize.X * GameInfo.tileSize);
		// Height is the same with 2 empty square spacings on the bottom but must have more room at the top
		// We'll try to have it as 4 spacings at the top (to fit info like money and stuff) 
            graphics.PreferredBackBufferHeight = ((GameInfo.mapSize.Y * GameInfo.tileSize) + GameInfo.uiSpace);
		//======================================================

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            moneySpriteFont = Content.Load<SpriteFont>(@"Money");
            levelSpriteFont = Content.Load<SpriteFont>(@"Money");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Draw money count
            spriteBatch.DrawString(moneySpriteFont, "Money: " + playerMoney, new Vector2(10, 5), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
            // Draw level count
            spriteBatch.DrawString(levelSpriteFont, "Level: " + level, new Vector2(Window.ClientBounds.Width/2 - 70, 5), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
