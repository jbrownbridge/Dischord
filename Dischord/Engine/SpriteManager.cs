using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dischord.Engine
{
    public abstract class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected List<Sprite> spriteList;
        private AnimationManager animationManager;
        protected Boolean reset;

        protected SpriteBatch spriteBatch;
        protected Texture2D spriteImage;

        private string imagePath;
        private int columns;
        private int rows;
        private string behaviourPath;

        public SpriteManager(Game game, string imagePath, int columns, int rows, string behaviourPath) : base(game)
        {
            this.imagePath = imagePath;
            this.columns = columns;
            this.rows = rows;
            this.behaviourPath = behaviourPath;
            reset = false;
            spriteList = new List<Sprite>();
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            spriteImage = Game.Content.Load<Texture2D>(imagePath);
            animationManager = new AnimationManager(
                spriteImage, columns, rows, behaviourPath
            );
        }

        public IEnumerable<Sprite> Sprites
        {
            get
            {
                foreach (Sprite sprite in this.spriteList)
                    yield return sprite;
            }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public Texture2D SpriteImage
        {
            get { return spriteImage; }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        
        }

        public abstract override void Update(GameTime gameTime);

        public void AddSprite(Sprite sprite, int framesPerSecond)
        {
            sprite.SpriteManager = this;
            sprite.Animation = new Animation(animationManager, framesPerSecond);
            sprite.Initialize();
            spriteList.Add(sprite);
        }

        public void Reset() {
            reset = true;
        }
    }
}