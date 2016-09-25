using FlappyBirdMonoGame.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FlappyBirdMonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Bird bird;
        private Pipe pipe;
        private Background ground;
        private Background background;

        private List<IEntity> entityList;
        private bool escapePressed;
        private bool paused;

        private bool gameOver;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            entityList = new List<IEntity>();

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
            background.Speed = 20f;
            entityList.Add(background);

            var upperPipeTexture = Content.Load<Texture2D>(@"PipeGreen2");
            var lowerPipeTexture = Content.Load<Texture2D>(@"PipeGreen1");
            pipe = new Pipe(upperPipeTexture, lowerPipeTexture, 3, 80, 150f, -420, -200, 320, 80, 492);
            pipe.Speed = 100f;
            entityList.Add(pipe);

            var groundTexture = Content.Load<Texture2D>(@"Ground");
            ground = new Background(groundTexture, 0, 540, 480, 150);
            ground.Speed = 100f;
            entityList.Add(ground);

            var birdTexture = Content.Load<Texture2D>(@"Bird1");
            bird = new Bird(birdTexture, 120, 200, 60, 60);
            entityList.Add(bird);

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
                if (!paused)
                {
                    paused = true;
                    foreach (var entity in entityList)
                    {
                        entity.Pause();
                    }
                }
                else
                {
                    paused = false;
                    foreach (var entity in entityList)
                    {
                        entity.Resume();
                    }
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Escape) && escapePressed)
            {
                escapePressed = false;
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
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void GameOver()
        {
            gameOver = true;
            bird.SetGameOver(true);
            pipe.Pause();
            ground.Pause();
            background.Pause();

        }
    }
}
