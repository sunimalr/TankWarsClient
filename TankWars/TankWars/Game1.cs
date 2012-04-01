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

namespace TankWars
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D gamePieces;
        Texture2D background;
        Texture2D bullet;
        Texture2D explosion;

        SpriteFont Pericles20;
        SpriteFont Pericles18;

        Vector2 gameBoardDisplayOrigin = new Vector2(0, 0);   //The starting coordinates of the game board
        Vector2 namePosition = new Vector2(1056, 596);
        Vector2 serverIPPosition = new Vector2(971, 510);
        Vector2 clientIPPosition = new Vector2(956, 536);
        Vector2 playerNamePosition = new Vector2(825, 200);
        Vector2 score = new Vector2(98, 0);
        Vector2 coins = new Vector2(192, 0);
        Vector2 health = new Vector2(290, 0);
        Vector2 depth = new Vector2(0, 0);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //this.graphics.IsFullScreen = true;
            //this.Window.AllowUserResizing = true;
            //this.Window.ClientSizeChanged +=new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        //void Window_ClientSizeChanged(object sender, EventArgs e)
        //{
        //    // Make changes to handle the new window size.

        //    graphics.PreferredBackBufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
        //    graphics.PreferredBackBufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        //}


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true; //mouse is active inside the game window
            graphics.PreferredBackBufferWidth = 1200;   //define the size of the game window
            graphics.PreferredBackBufferHeight = 800;

            //graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;   //define the size of the game window
            //graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();



  
            GameManager.getGameManager.sendMessage("JOIN#");
            GameManager.getGameManager.recieveMessage();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gamePieces = Content.Load<Texture2D>(@"Textures\spriteSheet");
            background = Content.Load<Texture2D>(@"Textures\background");
            bullet = Content.Load<Texture2D>(@"Textures\bullet");
            explosion = Content.Load<Texture2D>(@"Textures\explosion");

            Pericles20 = Content.Load<SpriteFont>(@"Fonts\Pericles");
            Pericles18 = Content.Load<SpriteFont>(@"Fonts\Pericles18");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //GameManager.getGameManager.updateGame();
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend);

            spriteBatch.Draw(background,new Rectangle(0, 0,this.Window.ClientBounds.Width,this.Window.ClientBounds.Height),Color.White);

            for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                {
                    int pixelX = (int)gameBoardDisplayOrigin.X + (x * GamePiece.PieceWidth);
                    int pixelY = (int)gameBoardDisplayOrigin.Y + (y * GamePiece.PieceHeight);

                    spriteBatch.Draw(gamePieces, new Rectangle(pixelX,pixelY,GamePiece.PieceWidth,GamePiece.PieceHeight),GameBoard.getGameBoard.GetSourceRect(x, y),Color.White);
                    

                }

            foreach (Point p in GameManager.getGameManager.bulletLocs)
            {
                int pixelX = (int)gameBoardDisplayOrigin.X + (p.X * GamePiece.PieceWidth);
                int pixelY = (int)gameBoardDisplayOrigin.Y + (p.Y * GamePiece.PieceHeight);

                spriteBatch.Draw(bullet, new Rectangle(pixelX, pixelY, GamePiece.PieceWidth, GamePiece.PieceHeight),Color.White);
            }

            foreach (Point r in GameManager.getGameManager.impactLocs)
            {
                int pixelX = (int)gameBoardDisplayOrigin.X + (r.X * GamePiece.PieceWidth);
                int pixelY = (int)gameBoardDisplayOrigin.Y + (r.Y * GamePiece.PieceHeight);

                spriteBatch.Draw(explosion, new Rectangle(pixelX, pixelY, GamePiece.PieceWidth, GamePiece.PieceHeight), Color.White);
            }
            if (GameManager.playerArray != null)
            {
                spriteBatch.DrawString(Pericles20, GameManager.getGameManager.myPlayerName, namePosition, Color.OliveDrab);
                spriteBatch.DrawString(Pericles20, GameManager.getGameManager.serverIP, serverIPPosition, Color.OliveDrab);
                spriteBatch.DrawString(Pericles20, GameManager.getGameManager.cliientIP, clientIPPosition, Color.OliveDrab);

                foreach (bean.Player player in GameManager.playerArray)
                {
                    spriteBatch.DrawString(Pericles20, player.Name, playerNamePosition + depth, Color.OliveDrab);
                    spriteBatch.DrawString(Pericles20, player.Points.ToString(), playerNamePosition + score + depth, Color.OliveDrab);
                    spriteBatch.DrawString(Pericles20, player.Coins.ToString(), playerNamePosition + coins + depth, Color.OliveDrab);
                    spriteBatch.DrawString(Pericles20, player.Health.ToString() + "%", playerNamePosition + health + depth, Color.OliveDrab);

                    depth.X += 0;
                    depth.Y += 35;
                }
                depth.X = 0; depth.Y = 0;

            }

            //spriteBatch.DrawString(Pericles20, "player name", namePosition, Color.OliveDrab);
            //spriteBatch.DrawString(Pericles18, "127.0.0.1", serverIPPosition, Color.OliveDrab);
            //spriteBatch.DrawString(Pericles18, "127.0.0.1", clientIPPosition, Color.OliveDrab);

            //spriteBatch.DrawString(Pericles20, "P01", playerNamePosition, Color.OliveDrab);
            //spriteBatch.DrawString(Pericles20, "1000", playerNamePosition+score, Color.OliveDrab);
            //spriteBatch.DrawString(Pericles20, "2000", playerNamePosition + coins, Color.OliveDrab);
            //spriteBatch.DrawString(Pericles20, "100"+"%", playerNamePosition + health, Color.OliveDrab);

            //Vector2 test = new Vector2(0, 35);

            //spriteBatch.DrawString(Pericles20, "P02", playerNamePosition + test, Color.OliveDrab);
            //spriteBatch.DrawString(Pericles20, "1500", playerNamePosition + score + test, Color.OliveDrab);
            //spriteBatch.DrawString(Pericles20, "3000", playerNamePosition + coins + test, Color.OliveDrab);
            //spriteBatch.DrawString(Pericles20, "110" + "%", playerNamePosition + health + test, Color.OliveDrab);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
