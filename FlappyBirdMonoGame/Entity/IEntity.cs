﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBirdMonoGame.Entity
{
    interface IEntity
    {
        void Pause();
        void Resume();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        bool Intersect(Rectangle rectangle);
    }
}
