#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Engine.Logic.Actors;
using Engine.Graphics.Cameras;
using Engine.Utilities;
#endregion

namespace Engine.World
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 1.23.2011
    /// Description: 
    /// </summary>
    public sealed class GameWorld
    {
        #region Fields
        private static readonly GameWorld instance = new GameWorld();

        private bool enabled = false;
        private ContentManager content;

        public int level;
        public int width;
        public int height;
        public WorldTile[] layout;
        public Texture2D background;
        public Point start;
        public Point end;

        public VortexActor vortex;
        public HeroActor hero;
        private SpriteBatch spriteBatch;
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static GameWorld Instance { get { return instance; } }
        /// <summary>Is the world enabled</summary>
        public bool Enabled { get { return this.enabled; } set { this.enabled = value; } }

        /// <summary>HERO ACTOR</summary>
        public HeroActor Hero { get { return this.hero; } set { this.hero = value; } }

        public OnLevelEnd LevelEndHandler { get; set; }
        #endregion

        #region Constructors
        public GameWorld()
        {

        }
        #endregion

        #region Delegates
        public delegate void OnLevelEnd(GameWorld sender, int level);
        #endregion

        #region Public Methods
        public void Initialize(int level, OnLevelEnd levelEndHandler)
        {
            this.content = new ContentManager(DeviceManager.Instance.Content.ServiceProvider);
            this.spriteBatch = new SpriteBatch(DeviceManager.Instance.GraphicsDevice);
            this.LevelEndHandler = levelEndHandler;
            
            //Load Level
            LevelMap levelMap = WorldTypes.Levels[level];
            this.width = levelMap.Width;
            this.height = levelMap.Height;
            this.start = levelMap.Start;
            this.end = levelMap.End;
            this.level = levelMap.Level;
            this.layout = new WorldTile[this.width * this.height];
            for (int y = 0; y < this.height; ++y)
            {
                for (int x = 0; x < this.width; ++x)
                {
                    this.layout[x + y * this.width] = new WorldTile(levelMap.Layout[x + y * this.width], new Vector2(x * WorldTypes.TILE_SIZE, y * WorldTypes.TILE_SIZE));
                }
            }

            //Load Background
            if (this.level == 1)
            {
                this.background = content.Load<Texture2D>(@"Content\World\Backgrounds\Level1\RunJumpBackground");
            }
            else if (this.level == 2)
            {
                this.background = content.Load<Texture2D>(@"Content\World\Backgrounds\Level2\RunJumpBackground");
            }
            else
            {
                this.background = content.Load<Texture2D>(@"Content\World\Backgrounds\level3\ActionBackground");
            }

            //Where should our hero start?
            Vector2 startPoint = UtilityWorld.GridToWorld(this.start);
            this.hero = ActorFactory.Instance.CreateHero(startPoint, new Point(36, 110));

            //Where should our Vortex start?
            Vector2 endPoint = UtilityWorld.GridToWorld(this.end);
            this.vortex = ActorFactory.Instance.CreateVortex(endPoint);

            //Update camera to center hero
            Camera.Instance.Jump((int)startPoint.X, startPoint.Y - 175);

            enabled = true;
        }

        public void Update(GameTime gameTime)
        {
            //Update all actors if enabled and if game isn't paused
            if (!enabled || DeviceManager.Instance.Paused)
                return;

            Point heroTile = UtilityWorld.WorldToGrid(this.hero.GetPosition().Position);
            if (this.end.X == heroTile.X && this.end.Y == heroTile.Y)
            {
                //We have reached the end of all things
                this.LevelEndHandler(this, this.level);
            }
            
            this.hero.Update(gameTime);
            this.vortex.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (this.enabled)
            {
                if (level == 3)
                {
                    this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
                    this.spriteBatch.Draw(this.background, new Vector2(0, 0), Color.White);
                    this.spriteBatch.End();
                }

                //Render
                this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Camera.Instance.View);

                //Draw Background
                if (level == 1)
                {
                    this.spriteBatch.Draw(this.background, new Vector2(0, -475), Color.White);
                    this.spriteBatch.Draw(this.background, new Vector2(800, -475), Color.White);
                    this.spriteBatch.Draw(this.background, new Vector2(1600, -475), Color.White);
                }

                else if (level == 2)
                {
                    this.spriteBatch.Draw(this.background, new Vector2(0, -315), Color.White);
                    this.spriteBatch.Draw(this.background, new Vector2(800, -315), Color.White);
                    this.spriteBatch.Draw(this.background, new Vector2(1600, -315), Color.White);
                    this.spriteBatch.Draw(this.background, new Vector2(2400, -315), Color.White);
                    this.spriteBatch.Draw(this.background, new Vector2(3200, -315), Color.White);
                }


                //Draw Tiles
                for (int y = 0; y < this.height; ++y)
                {
                    for (int x = 0; x < this.width; ++x)
                    {
                        WorldTile tile = this.layout[x + y * this.width];
                        if (Camera.Instance.OnScreen(new Point((int)tile.Position.X, (int)tile.Position.Y)))
                            tile.Draw(this.spriteBatch, tile.Position);
                    }
                }

                //Draw the Vortex
                if (this.vortex != null)
                    this.vortex.Draw(gameTime, this.spriteBatch);

                //Draw the Hero
                if (this.hero != null)
                    this.hero.Draw(gameTime, this.spriteBatch);


                this.spriteBatch.End();
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
