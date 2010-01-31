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
using Dischord.Engine;

namespace Dischord
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private RandyManager randyManager;
        private MobileManager mobileManager;
        private TileSet tileSet;
        public Engine.Map tileMap;
        private EntityManager eManager;

        public EntityManager EManager {
            get {
                return eManager;
            }
        }


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
                return this.eManager.Entities;
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

        private Randy character;

        private KeyboardState oldstate;

        private SoundEffectInstance birdSong;
        private float nextBirdSong;
        private Random rand;

        private List<HudItem> hud;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 640;
            graphics.PreferredBackBufferWidth = 640;
            Content.RootDirectory = "Content";

            mobileManager = new MobileManager(this, @"Sprites\randy", 8, 3, @"..\..\..\Content\Behaviours\randy.xml");
            this.Components.Add(mobileManager);

            randyManager = new RandyManager(this, @"Sprites\randy", 8, 3, @"..\..\..\Content\Behaviours\randy.xml");
            this.Components.Add(randyManager);
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rand = new Random();
            hud = new List<HudItem>();
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);

            base.Initialize();

            tileSet = new TileSet(Content.Load<Texture2D>(@"Map\tiles"), 1280 / 32, 1600 / 32);
            tileMap = new Engine.Map(@"..\..\..\Content\Map\level1.xml", true);

            eManager = new EntityManager(tileMap.Rows, tileMap.Columns);

            character = new Randy(tileMap.PlayerSpawnPoint, 220, Facing.Right, tileMap, tileSet);

            randyManager.AddSprite(character, 12);
            foreach (Vector2 spawn in tileMap.EnemySpawnPoints)
            {
                mobileManager.AddSprite(new Baddie(spawn, 220, Facing.Right, tileMap, tileSet), 12);
            }

            //this.map = new Map(MAP_FILE_1);
            //map.Update();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {                  
            
            Texture2D enemy = Content.Load<Texture2D>("enemy");
            Texture2D obstacle = Content.Load<Texture2D>("obstacle");
            Texture2D smoke = Content.Load<Texture2D>("smoke");
            Texture2D fire = Content.Load<Texture2D>("fire");
            Texture2D gluetrap = Content.Load<Texture2D>("gluetrap");
            Texture2D immobilized = Content.Load<Texture2D>("immobilized");
            Texture2D wall = Content.Load<Texture2D>("wall");

            Texture2D hudleft = Content.Load<Texture2D>("HUD/hud-left");
            Texture2D hudtop = Content.Load<Texture2D>("HUD/hud-top");
            Texture2D hudright = Content.Load<Texture2D>("HUD/hud-right");
            Texture2D hudbottom = Content.Load<Texture2D>("HUD/hud-bottom");

            hud.Add(new HudItem(hudleft, new Point(0, (Window.ClientBounds.Height - hudleft.Height) / 2)));
            hud.Add(new HudItem(hudright, new Point(Window.ClientBounds.Width - hudright.Width, (Window.ClientBounds.Height - hudright.Height) / 2)));
            hud.Add(new HudItem(hudtop, new Point((Window.ClientBounds.Width - hudtop.Width) / 2, 0)));
            hud.Add(new HudItem(hudbottom, new Point((Window.ClientBounds.Width - hudtop.Width) / 2, Window.ClientBounds.Height - hudbottom.Height)));

            spriteSheets["Enemy"]    = new Sprite(enemy, 128, 128, 8);
            spriteSheets["Character"] = new Sprite(obstacle, 64, 64, 4);
            spriteSheets["Obstacle"] = new Sprite(obstacle, 64, 64, 4);
            spriteSheets["Smoke"] = new Sprite(smoke, 32, 32, 1,0.6f);
            spriteSheets["Fire"] = new Sprite(fire, 32, 32, 1);
            spriteSheets["GlueTrap"] = new Sprite(gluetrap, 32, 32, 1);
            spriteSheets["Immobilized"] = new Sprite(immobilized, 64, 64, 4);
            spriteSheets["Wall"] = new Sprite(wall, 32, 32, 1);
            
            sounds["Crackle"] = Content.Load<SoundEffect>("Sounds/crackle");
            sounds["Walk"] = Content.Load<SoundEffect>("Sounds/steps3");
            sounds["Bird"] = Content.Load<SoundEffect>("Sounds/bird");
            sounds["Lyre"] = Content.Load<SoundEffect>("Sounds/lyre2");
            sounds["GlueTrap"] = Content.Load<SoundEffect>("Sounds/squish");

            birdSong = sounds["Bird"].CreateInstance();
            birdSong.Volume = 0.75f;
            nextBirdSong = (float)(sounds["Bird"].Duration.TotalMilliseconds) + rand.Next((int)sounds["Bird"].Duration.TotalMilliseconds);

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

            if(state.IsKeyDown(Keys.Up) && state.IsKeyUp(Keys.Down))
                character.SetDirection(Mobile.Direction.Up);
            else if(state.IsKeyUp(Keys.Up) && state.IsKeyDown(Keys.Down))
                character.SetDirection(Mobile.Direction.Down);
            else if(state.IsKeyDown(Keys.Left) && state.IsKeyUp(Keys.Right))
                character.SetDirection(Mobile.Direction.Left);
            else if(state.IsKeyUp(Keys.Left) && state.IsKeyDown(Keys.Right))
                character.SetDirection(Mobile.Direction.Right);
            else
                character.SetDirection(Mobile.Direction.Stand);

            if(state.IsKeyDown(Keys.LeftAlt) && oldstate.IsKeyUp(Keys.LeftAlt))
                character.useFire();

            if(state.IsKeyDown(Keys.LeftControl) && oldstate.IsKeyUp(Keys.LeftControl))
                character.useGlue();

            if(state.IsKeyDown(Keys.Space) && oldstate.IsKeyUp(Keys.Space))
                character.useLyre();

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
                nextBirdSong = (float)(sounds["Bird"].Duration.TotalMilliseconds * 1.5) + rand.Next((int)sounds["Bird"].Duration.TotalMilliseconds);
            }
            //map.draw();
            /*
             * AI Code
             */

            /*foreach (Entity e in eManager.Entities) {
                if (e is Enemy)
                {
                    Direction d = ai.findPath(map, e.Position, e as Enemy);
                    (e as Enemy).move(d);
                    //if (e.Cell.Type != MapCell.MapCellType.floor)
                    //{
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
                    //}
                }
            }*/

            //map.Update(); // FIXME: Are 2 update calls really required?
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
            eManager.Update();
            //map.Update();
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
            //GraphicsDevice.Viewport.X = Math.Min(0,randyManager.Sprites.First().Position.X - GraphicsDevice.Viewport.Width/2);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
            foreach (Entity entity in Entities)
            {
                entity.Draw(gameTime);
            }
            foreach (HudItem h in hud)
            {
                spriteBatch.Draw(h.Texture, new Rectangle(h.Position.X, h.Position.Y, h.Texture.Width, h.Texture.Height), Color.White);
            }

            // TODO: Add your drawing code here

            // TODO: Add your drawing code here
            spriteBatch.GraphicsDevice.RenderState.DepthBufferEnable = true;
            spriteBatch.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            spriteBatch.GraphicsDevice.RenderState.DepthBufferFunction = CompareFunction.GreaterEqual;
            spriteBatch.GraphicsDevice.RenderState.AlphaTestEnable = true;

            //this draws the map
            tileMap.Draw(spriteBatch, tileSet, randyManager.Sprites.First().Position);
            base.Draw(gameTime);
            spriteBatch.End();
        }

        private static Game game;

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

        public SoundEffect GetSound(String soundName) {
            return sounds[soundName];
        }

        public TileSet GetTileSet() {
            return tileSet;
        }

        public Vector2 CharacterPosition() {
            return character.Position;
        }

        public Vector2 RenderPosition() {
            return character.RenderPos;
        }

    }
}
