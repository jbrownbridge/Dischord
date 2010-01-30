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
        public const String MAP_FILE_5 = "maps/large1.map";
        public const String MAP_FILE_6 = "maps/large_sparse.map";

        GraphicsDeviceManager graphics;

        private Dictionary<String, Sprite> spriteSheets = new Dictionary<string,Sprite>();

        private Dictionary<String, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

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

        private KeyboardState oldstate;

        private SoundEffectInstance birdSong;
        private float nextBirdSong;
        private Random rand;

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
            rand = new Random();


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
            Texture2D smoke = Content.Load<Texture2D>("smoke");
            Texture2D fire = Content.Load<Texture2D>("fire");

            spriteSheets["Enemy"]    = new Sprite(enemy, 128, 128, 8);
            spriteSheets["Character"] = new Sprite(obstacle, 64, 64, 4);
            spriteSheets["Obstacle"] = new Sprite(obstacle, 64, 64, 4);
            spriteSheets["Smoke"] = new Sprite(smoke, 32, 32, 1);
            spriteSheets["Fire"] = new Sprite(fire, 32, 32, 1);
            
            sounds["Crackle"] = Content.Load<SoundEffect>("Sounds/crackle");
            sounds["Walk"] = Content.Load<SoundEffect>("Sounds/walk");
            sounds["Woods"] = Content.Load<SoundEffect>("Sounds/woods");

            birdSong = sounds["Woods"].CreateInstance();
            birdSong.Volume = 0.75f;
            nextBirdSong = (float)(sounds["Woods"].Duration.TotalMilliseconds) + rand.Next((int)sounds["Woods"].Duration.TotalMilliseconds);
            
            this.map = new Map(MAP_FILE_5);
            map.Update();

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

        protected virtual void HandleMovementInput(KeyboardState state) {            
            // Allows the game to exit
            if(state.IsKeyDown(Keys.Escape))
                this.Exit();

            int facing = 0;

            if(state.IsKeyDown(Keys.A)) {
                facing = 3;
                if(state.IsKeyDown(Keys.W))
                    ++facing;
                else if(state.IsKeyDown(Keys.S))
                    --facing;
            }
            else if(state.IsKeyDown(Keys.D)) {
                facing = 7;
                if(state.IsKeyDown(Keys.W))
                    --facing;
                else if(state.IsKeyDown(Keys.S))
                    ++facing;
            }
            else if(state.IsKeyDown(Keys.W)) {
                facing = 5;
            }
            else if(state.IsKeyDown(Keys.S)) {
                facing = 1;
            }

            characterControls.Direction = facing;

            if(state.IsKeyDown(Keys.Space))
                characterControls.Jump = true;
            else
                characterControls.Jump = false;

            if(state.IsKeyDown(Keys.Delete) && oldstate.IsKeyUp(Keys.Delete))
                map.Add(new Fire(new Point(64, 64), 6000f));

        }

        protected virtual void HandleActionInput(KeyboardState state) {

        }

        protected virtual void HandleMenuInput(KeyboardState state) {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            nextBirdSong -= gameTime.ElapsedGameTime.Milliseconds;
            if(nextBirdSong < 0) {
                birdSong.Play();
                nextBirdSong = (float)(sounds["Woods"].Duration.TotalMilliseconds * 2) + rand.Next((int)sounds["Woods"].Duration.TotalMilliseconds);
            }
            map.Update();
            //map.draw();
            foreach (Entity e in map.Entities) {
                if (e is Enemy)
                {
                    Direction d = ai.findPath(map, e.Position, e as Enemy);
                    (e as Enemy).move(d);
                    if (e.MapCell.Type != MapCell.MapCellType.floor)
                    {
                        switch (d)
                        {
                            case Direction.up:
                                d = Direction.down;
                                break;
                            case Direction.down:
                                d = Direction.up;
                                break;
                            case Direction.left:
                                d = Direction.right;
                                break;
                            case Direction.right:
                                d = Direction.left;
                                break;
                        }
                        (e as Enemy).move(d);
                        (e as Enemy).Wait = 50;
                    }
                }
            }
            map.Update(); // FIXME: Are 2 update calls really required?
            //map.draw();
            KeyboardState state = Keyboard.GetState();

            switch(controlMode) {
                case ControlMode.movement:
                    HandleMovementInput(state);
                    break;
                case ControlMode.action:
                    HandleActionInput(state);
                    break;
                case ControlMode.menu:
                    HandleMenuInput(state);
                    break;
            }

            foreach (Entity entity in Entities)
            {
                entity.Update(gameTime);
            }

            map.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
            oldstate = state;
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

        public SoundEffect GetSound(String soundName) {
            return sounds[soundName];
        }
    }
}
