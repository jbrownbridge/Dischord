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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Dischord
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        enum ControlMode {
            menu,
            movement,
            action,
        };

        public const int TILE_HEIGHT = 32;
        public const int TILE_WIDTH  = 32;
        public const String MAP_FILE_1 = "maps/simple1.map";
        public const String MAP_FILE_2 = "maps/simple2.map";
        public const String MAP_FILE_3 = "maps/simple3.map";
        public const String MAP_FILE_4 = "maps/simple4.map";

        GraphicsDeviceManager graphics;

        private Dictionary<String, Sprite> spriteSheets = new Dictionary<string,Sprite>();

        public IEnumerable<Entity> Entities
        {
            get
            {
                return this.Map.Entities;
            }
        }


        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
        }
        private SpriteBatch spriteBatch;

        private ControlMode controlMode;

        private Controls characterControls;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            controlMode = ControlMode.movement;
            characterControls = new Controls();


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
                       
            Texture2D enemy = Content.Load<Texture2D>("enemy");
            Texture2D obstacle = Content.Load<Texture2D>("obstacle");

            spriteSheets["Enemy"]    = new Sprite(enemy, 128, 128, 8);
            spriteSheets["Character"] = new Sprite(enemy, 128, 128, 8);
            spriteSheets["Obstacle"] = new Sprite(obstacle, 64, 64, 4);

            map = new Map(MAP_FILE_1);

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

        protected virtual void HandleMovementInput() {
            KeyboardState state = Keyboard.GetState();
            
            // Allows the game to exit
            if(state.IsKeyDown(Keys.Escape))
                this.Exit();
            
            int horizonal = 0, vertical = 0;
            
            if(state.IsKeyDown(Keys.W))
                ++vertical;

            if(state.IsKeyDown(Keys.S))
                --vertical;

            if(state.IsKeyDown(Keys.D))
                ++horizonal;

            if(state.IsKeyDown(Keys.A))
                --horizonal;

            characterControls.Direction = (horizonal * 3) + vertical;

            if(state.IsKeyDown(Keys.Space))
                characterControls.Jump = true;
            else
                characterControls.Jump = false;
        }

        protected virtual void HandleActionInput() {

        }

        protected virtual void HandleMenuInput() {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch(controlMode) {
                case ControlMode.movement:
                    HandleMovementInput();
                    break;
                case ControlMode.action:
                    HandleActionInput();
                    break;
                case ControlMode.menu:
                    HandleMenuInput();
                    break;
            }

            foreach (Entity entity in Entities)
            {
                entity.Update(gameTime);
            }
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

            spriteBatch.Begin();
            foreach (Entity entity in Entities)
            {
                entity.Draw(gameTime);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private static Game game;

        public Map Map
        {
            get
            {
                return this.map;
            }
        }
        private Map map;

        public static Game GetInstance()
        {
            if (game == null)
            {
                game = new Game();
            }
            return game;
        }

        public Sprite GetSprite(String spriteName)
        {
            return spriteSheets[spriteName];
        }

        public Controls GetCharacterControls() {
            return characterControls;
        }
    }
}
