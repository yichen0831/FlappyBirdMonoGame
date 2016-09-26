using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBirdMonoGame.GUI
{
    class Hud
    {
        private Game1 game;

        private Texture2D[] numberTextures;
        private Texture2D pauseIconTexture;
        private Texture2D gameOverTexture;
        private Texture2D getReadyTexture;
        private Texture2D instuctionTexture;

        public void setTextures(Game1 game, Texture2D[] numberTextures, Texture2D pauseIconTexture, Texture2D gameOverTexture, Texture2D getReadyTexture, Texture2D instuctionTexture)
        {
            this.game = game;
            this.numberTextures = numberTextures;
            this.pauseIconTexture = pauseIconTexture;
            this.gameOverTexture = gameOverTexture;
            this.getReadyTexture = getReadyTexture;
            this.instuctionTexture = instuctionTexture;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (game.Status)
            {
                case GameStatus.Instruction:
                    spriteBatch.Draw(instuctionTexture, new Rectangle(180, 200, 120, 110), Color.White);
                    break;
                case GameStatus.GetReady:
                case GameStatus.Play:
                    break;
                case GameStatus.GameOver:
                    spriteBatch.Draw(gameOverTexture, new Rectangle(165, 240, 150, 42), Color.White);
                    break;
                case GameStatus.Pause:
                    spriteBatch.Draw(pauseIconTexture, new Rectangle(game.bird.destRect.X + 40, game.bird.destRect.Y - 20, 20, 20), Color.White);
                    break;
            }
        }
    }
}
