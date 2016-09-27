using FlappyBirdMonoGame.Entity;
using FlappyBirdMonoGame.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FlappyBirdMonoGame
{
    public enum GameStatus
    {
        Instruction,
        GetReady,
        Play,
        GameOver,
        Pause,
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Bird bird { get; private set; }
        private Pipe pipe;
        private Background ground;
        private Background background;

        private Hud hud;

        private List<IEntity> entityList;
        private bool escapePressed;

        private float gameRestartTimeLeft;

        public GameStatus Status { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            entityList = new List<IEntity>();
            hud = new Hud();

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 640;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Status = GameStatus.Instruction;
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

            // TODO: use this.Content to load your game content here
            var backgroundTexure = Content.Load<Texture2D>(@"Background1");
            background = new Background(backgroundTexure, 0, -160, 480, 853);
            background.SetSpeed(20f);
            entityList.Add(background);

            var upperPipeTexture = Content.Load<Texture2D>(@"PipeGreen2");
            var lowerPipeTexture = Content.Load<Texture2D>(@"PipeGreen1");
            pipe = new Pipe(upperPipeTexture, lowerPipeTexture, 3, 80, 150f, -420, -200, 320, 80, 492);
            pipe.SetSpeed(100f);
            entityList.Add(pipe);

            var groundTexture = Content.Load<Texture2D>(@"Ground");
            ground = new Background(groundTexture, 0, 540, 480, 150);
            ground.SetSpeed(100f);
            entityList.Add(ground);

            var birdTexture = Content.Load<Texture2D>(@"Bird1");
            bird = new Bird(this, birdTexture, 120, 240, 60, 40);
            entityList.Add(bird);

            Texture2D[] nTextures = new Texture2D[10];
            for (int i = 0; i < 10; i++)
            {
                nTextures[i] = Content.Load<Texture2D>(i.ToString());
            }

            var pauseIconTexture = Content.Load<Texture2D>(@"Pause");
            var getReadyTexture = Content.Load<Texture2D>(@"GetReady");
            var gameOverTexture = Content.Load<Texture2D>(@"GameOver");
            var instructionTexture = Content.Load<Texture2D>(@"Instruction");

            hud.setTextures(this, nTextures, pauseIconTexture, gameOverTexture, getReadyTexture, instructionTexture);
            pipe.AddScore += hud.AddScore;
            Reset();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !escapePressed)
            {
                escapePressed = true;
                if (Status == GameStatus.Play)
                {
                    SetPause(true);
                }
                else if (Status == GameStatus.Pause)
                {
                    SetPause(false);
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Escape) && escapePressed)
            {
                escapePressed = false;
            }

            if (Status == GameStatus.Instruction && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                StartGame();
            }

            foreach (var entity in entityList)
            {
                entity.Update(gameTime);
            }

            // Check if game is over
            if (pipe.Intersect(bird.destRect) || ground.Intersect(bird.destRect))
            {
                GameOver();
            }

            if (Status == GameStatus.GameOver)
            {
                // Count down and restart the game
                gameRestartTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (gameRestartTimeLeft <= 0)
                {
                    Reset();
                }
            }
            else if (Status == GameStatus.Play)
            {

            }

            hud.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (var entity in entityList)
            {
                entity.Draw(spriteBatch);
            }
            hud.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void GameOver()
        {
            Status = GameStatus.GameOver;
            bird.SetGameOver(true);
            pipe.Pause();
            ground.Pause();
            background.Pause();
            gameRestartTimeLeft = 1f;
        }

        private void StartGame()
        {
            Status = GameStatus.Play;
            bird.ShouldDraw = true;
            pipe.ShouldDraw = true;

            // Game starts after get ready count down
            foreach (var entity in entityList)
            {
                entity.Resume();
            }
        }

        private void SetPause(bool pause)
        {
            Status = pause ? GameStatus.Pause : GameStatus.Play;
            foreach (var entity in entityList)
            {
                if (pause)
                {
                    entity.Pause();
                }
                else
                {
                    entity.Resume();
                }
            }
        }

        private void Reset()
        {
            Status = GameStatus.Instruction;
            foreach (var entity in entityList)
            {
                entity.Pause();
            }
            bird.Reset();
            pipe.Reset();
            bird.ShouldDraw = false;
            pipe.ShouldDraw = false;

            hud.ResetScore();
        }
    }
}
