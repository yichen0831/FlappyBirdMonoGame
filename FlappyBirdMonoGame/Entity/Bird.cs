using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBirdMonoGame.Entity
{
    class Bird : IEntity
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        protected Texture2D texture;
        public Rectangle destRect;

        private int animIndex;
        private Rectangle sourceRect;
        private bool flapping;
        private int spriteSize;
        private float velocity;
        private readonly float gravity = 320f;

        public float animInterval { get; set; }
        private float animTimeLeft;

        private bool paused;
        private bool gameOver;
        private float rotation;
        private Vector2 origin;

        public Bird(Texture2D texture, float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            this.texture = texture;
            destRect = new Rectangle((int)X, (int)Y, Width, Height);

            animIndex = 1;
            spriteSize = 20;

            velocity = 120f;
            animInterval = 0.06f;
            sourceRect = new Rectangle(0, 0, spriteSize, spriteSize);
            origin = new Vector2(0.5f, 0.5f);
        }

        public void Update(GameTime gameTime)
        {
            if (paused)
            {
                return;
            }

            if (!gameOver)
            {
                // Control is only available when game is not over
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !flapping)
                {
                    flapping = true;
                    Flap();
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.Space))
                {
                    flapping = false;
                }
            }
            else
            {
                rotation += 0.65f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (animIndex > 0)
            {
                animTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (animTimeLeft <= 0)
                {
                    animTimeLeft = animInterval;
                    animIndex++;

                    animIndex = animIndex % 3;
                }
            }

            sourceRect.X = animIndex * spriteSize;

            velocity += (float)gameTime.ElapsedGameTime.TotalSeconds * gravity;
            Y += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            destRect.Y = (int)Y;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destinationRectangle: destRect, sourceRectangle: sourceRect, color: Color.White, rotation: rotation, origin: origin);
        }

        private void Flap()
        {
            animIndex = 1;
            animTimeLeft = animInterval;
            velocity = -220f;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Resume()
        {
            paused = false;
        }

        public bool Intersect(Rectangle rectangle)
        {
            return destRect.Intersects(rectangle);
        }

        public void SetGameOver(bool gameOver)
        {
            this.gameOver = gameOver;
        }
    }
}
