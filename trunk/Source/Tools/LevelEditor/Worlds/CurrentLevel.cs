#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Graphics.Backgrounds;
using MonsterEscape.Graphics.Cameras;
using MonsterEscape.Graphics.UI;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Enemies;
using MonsterEscape.Logic.Actors.Misc;
using MonsterEscape.Logic.Actors.Monsters;
using MonsterEscape.Logic.AI;
using MonsterEscape.Logic.ClassComponents;
using MonsterEscape.Utility;

using Engine.World;
#endregion

namespace MonsterEscape.Worlds
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 5.22.2010
    /// Description: The game world
    /// </summary>
    public class CurrentLevel
    {
        #region Fields
        private static readonly CurrentLevel instance = new CurrentLevel();
        private string name;
        private int parTime;
        private int world;
        private int level;
        private int width;
        private int height;
        private Point eggPoint;
        private Point end;
        private TileType[][] map;
        private Background background;
        private EggActor egg;
        private List<SceneryTile> scenery = new List<SceneryTile>();
        private List<Actor> monsters = new List<Actor>();
        private List<Actor> enemies = new List<Actor>();
        private List<Actor> items = new List<Actor>();
        private List<Actor> misc = new List<Actor>();
        private SpriteBatch spriteBatch;
        private Texture2D solidTexture;
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static CurrentLevel Instance { get { return instance; } }
        /// <summary>Enabled when a level is loaded</summary>
        public bool Enabled { get; set; }
        /// <summary>Egg Starting Point</summary>
        public Point EggPoint { get { return this.eggPoint; } }
        /// <summary>Ending Point</summary>
        public Point End { get { return this.end; } }
        /// <summary>Level Name</summary>
        public string Name { get { return this.name; } }
        /// <summary>Par Time</summary>
        public int ParTime { get { return this.parTime; } }
        /// <summary>World Number</summary>
        public int World { get { return this.world; } }
        /// <summary>Level Number</summary>
        public int Level { get { return this.level; } }
        /// <summary>Level Width</summary>
        public int Width { get { return this.width; } set { this.width = value; } }
        /// <summary>Level Height</summary>
        public int Height { get { return this.height; } set { this.height = value; } }
        /// <summary>Scenery</summary>
        public List<SceneryTile> Scenery { get { return this.scenery; } }
        /// <summary>Monsters</summary>
        public List<Actor> Monsters { get { return this.monsters; } }
        /// <summary>Enemies</summary>
        public List<Actor> Enemies { get { return this.enemies; } }
        /// <summary>Items</summary>
        public List<Actor> Items { get { return this.items; } }
        /// <summary> The miscellaneous in the Level </summary>
        public List<Actor> Misc { get { return this.misc; } }
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        private CurrentLevel()
        {
            spriteBatch = new SpriteBatch(DeviceManager.Instance.GraphicsDevice);
           
            //Set Data needs an array, so we do this stupid dance
            Color[] final = new Color[1];
            final[0] = new Color(255, 255, 255, 120);
            solidTexture = new Texture2D(DeviceManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            solidTexture.SetData<Color>(final);
        }
        #endregion

        #region Public Methods
        public void Initialize(int world)
        {
            this.name = "";
            this.parTime = 60000;
            this.world = world;
            this.level = 0;
            this.eggPoint = Point.Zero;
            this.end = Point.Zero;

            if (width < 0 || height < 0)
                return;

            Vector2 startingTile = WorldUtilities.TileToWorld(this.eggPoint);
            //this.egg = ActorFactory.Instance.Create(ActorType.EGG) as EggActor;
            //this.egg.AddToWorld(startingTile);


            this.map = new TileType[this.width][];
            for (int i = 0; i < this.width; ++i)
                this.map[i] = new TileType[this.height];

            World currentWorld = TerrainKey.WorldTypes[this.world];
            for (int y = 0; y < this.height; ++y)
            {
                for (int x = 0; x < this.width; ++x)
                {
                    this.map[x][y] = new TileType(0, -1);//currentWorld.GetRandomSpire());
                }
            }

            //this.background = TerrainKey.Backgrounds[this.world];
            //this.background.Initialize();

            this.scenery.Clear();
            this.monsters.Clear();
            this.enemies.Clear();
            this.items.Clear();
            this.misc.Clear();

            //Recenter the camera to the new level
            Camera.Instance.Initialize();

            Enabled = true;
        }

        public void Initialize(int worldNumber, int levelNumber)
        {
            LevelMap level = WinphoneUtilities.DeserializeFromXML<LevelMap>("/Levels/World" + worldNumber + "/level" + levelNumber + ".xml");

            Initialize(level);
        }

        public void Initialize(LevelMap level)
        {
            this.name = level.Name;
            this.parTime = level.ParTime;
            this.world = level.World;
            this.level = level.Level;
            this.width = level.Width;
            this.height = level.Height;
            this.eggPoint = level.Egg;
            this.end = level.End;

            this.map = new TileType[this.width][];
            for (int i = 0; i < this.width; ++i)
                this.map[i] = new TileType[this.height];

            World currentWorld = TerrainKey.WorldTypes[this.world];
            for (int y = 0; y < this.height; ++y)
            {
                for (int x = 0; x < this.width; ++x)
                {
                    this.map[x][y] = new TileType(level.Layout[x + y * this.width], -1);// currentWorld.GetRandomSpire());
                }
            }
            
            //this.background = TerrainKey.Backgrounds[this.world];
            //this.background.Initialize();

            this.scenery.Clear();
            if (level.Scenery != null)
            {
                for (int i = 0; i < level.Scenery.Length; ++i)
                    this.scenery.Add(level.Scenery[i]);
            }

            Vector2 startingTile = WorldUtilities.TileToWorld(this.eggPoint);
            //this.egg = ActorFactory.Instance.Create(ActorType.EGG) as EggActor;
            //this.egg.AddToWorld(startingTile);

            this.monsters.Clear();
            if (level.Monsters != null)
            {
                foreach (EntityMarker entity in level.Monsters)
                {
                    startingTile = WorldUtilities.TileToWorld(entity.Position);
                    Actor actor = ActorFactory.Instance.Create(entity.Type);
                    actor.AddToWorld(startingTile);
                }
            }

            this.enemies.Clear();
            if (level.Enemies != null)
            {
                foreach (EntityMarkerEnemy entity in level.Enemies)
                {
                    startingTile = WorldUtilities.TileToWorld(entity.Position);
                    Enemy actor = ActorFactory.Instance.Create(entity.Type) as Enemy;
                    actor.AddToWorld(startingTile);
                    actor.Initialize(entity.AIConfiguration);
                }
            }

            this.items.Clear();
            if (level.Items != null)
            {
                foreach (EntityMarker entity in level.Items)
                {
                    startingTile = WorldUtilities.TileToWorld(entity.Position);
                    Actor actor = ActorFactory.Instance.Create(entity.Type);
                    actor.AddToWorld(startingTile);
                }
            }

            this.misc.Clear();

            //Recenter the camera to the new level
            Camera.Instance.Initialize();

            Enabled = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            //Update the Actors
            foreach (Actor actor in this.monsters)
                actor.Update(gameTime);
            foreach (Actor actor in this.enemies)
                actor.Update(gameTime);
            foreach (Actor actor in this.items)
                actor.Update(gameTime);
            foreach (Actor actor in this.misc)
                actor.Update(gameTime);

            //Sort the Actors and Scenery
            if (this.scenery != null)
                this.scenery.Sort(new SceneryComparer());
            if (this.monsters != null)
                this.monsters.Sort(new ActorComparer());
            if (this.enemies != null)
                this.enemies.Sort(new ActorComparer());
            if (this.items != null)
                this.items.Sort(new ActorComparer());
            if (this.misc != null)
                this.misc.Sort(new ActorComparer());

           // this.background.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (!Enabled || UIManager.Instance.MakingNewWorld || this.map == null)
                return;

            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Camera.Instance.View);
            
            //Background
           // this.background.Draw(gameTime, this.spriteBatch);

            //Render our actors and scenery from back to front flipping between the possible
            //lists based on who should be rendered next. The lists are sorted by this point.
            bool renderedScenery;
            bool renderedMonster;
            bool renderedEnemy;
            bool renderedItem;
            bool renderedMisc;
            bool renderedEgg = false;

            int sceneryI = 0;
            int monsterI = 0;
            int enemyI = 0;
            int itemI = 0;
            int miscI = 0;

            SceneryTile sceneryTile = null;
            Actor monster = null;
            Actor enemy = null;
            Actor item = null;
            Actor misc = null;
            
            //Are there any enemies, we'll go through them and render the water ones
            if (this.enemies.Count > 0)
                enemy = this.enemies[enemyI];

            Vector2 waterOffset = new Vector2(0, TerrainKey.TILE_SIZE);
            //Tiles
            for (int y = 0; y < this.height; ++y)
            {
                for (int x = 0; x < this.width; ++x)
                {
                    Vector2 pos = new Vector2(x * TerrainKey.TILE_SIZE, y * TerrainKey.TILE_SIZE);
                    WorldTile worldTile = new WorldTile( this.map[x][y].ID);
                    worldTile.Draw(this.spriteBatch, pos);

                    //Draw enemies that are water type which are on this tile
                    if (enemy != null && enemy.GetPathfind().Walker == WalkerType.Water)
                    {
                        Point tile = WorldUtilities.WorldToTile(enemy.GetPosition().Position);
                        if (tile.X >= x && tile.Y >= y)
                        {
                            enemy.Draw(gameTime, this.spriteBatch, waterOffset, false);

                            ++enemyI;
                            if (enemyI < this.enemies.Count)
                            {
                                enemy = this.enemies[enemyI];
                            }
                            else
                            {
                                enemy = null;
                            }
                        }
                    }
                    else
                    {
                        ++enemyI;
                        if (enemyI < this.enemies.Count)
                        {
                            enemy = this.enemies[enemyI];
                        }
                        else
                        {
                            enemy = null;
                        }
                    }
                }
            }

            //Draw Start and End
            Vector2 worldPos = WorldUtilities.TileToWorld(this.eggPoint);
            this.spriteBatch.Draw(solidTexture, new Rectangle((int)worldPos.X - TerrainKey.HALF_TILE_SIZE, (int)worldPos.Y - TerrainKey.HALF_TILE_SIZE, TerrainKey.TILE_SIZE, TerrainKey.TILE_SIZE), new Color(0,255,0, 0));

            worldPos = WorldUtilities.TileToWorld(this.end);
            this.spriteBatch.Draw(solidTexture, new Rectangle((int)worldPos.X - TerrainKey.HALF_TILE_SIZE, (int)worldPos.Y - TerrainKey.HALF_TILE_SIZE, TerrainKey.TILE_SIZE, TerrainKey.TILE_SIZE), new Color(255, 0, 0, 0));

            //Reset enemy variables
            enemyI = 0;
            enemy = null;

            //Are there any scenery tiles
            if (this.scenery.Count > 0)
                sceneryTile = this.scenery[sceneryI];
            //Are there any monsters
            if (this.monsters.Count > 0)
                monster = this.monsters[monsterI];
            //Are there any enemies
            if (this.enemies.Count > 0)
                enemy = this.enemies[enemyI];
            //Are there any items
            if (this.items.Count > 0)
                item = this.items[itemI];
            //Are there any misc
            if (this.misc.Count > 0)
                misc = this.misc[miscI];
            if (this.egg == null)
                renderedEgg = true;

            //Render all until both lists are exhausted
            float lowestPostion;
            while (sceneryTile != null || monster != null || enemy != null || item != null || misc != null || !renderedEgg )
            {
                lowestPostion = float.MaxValue;
                renderedScenery = false;
                renderedMonster = false;
                renderedEnemy = false;
                renderedItem = false;
                renderedMisc = false;

                //Determine furthest back object
                if (sceneryTile != null)
                {
                    if (lowestPostion > sceneryTile.Position.Y)
                    {
                        lowestPostion = sceneryTile.Position.Y;
                        renderedScenery = true;
                    }
                }

                if (monster != null)
                {
                    if (lowestPostion > monster.GetPosition().Position.Y)
                    {
                        lowestPostion = monster.GetPosition().Position.Y;
                        renderedMonster = true;
                        renderedScenery = false;
                    }
                }

                if (enemy != null)
                {
                    if (lowestPostion > enemy.GetPosition().Position.Y)
                    {
                        lowestPostion = enemy.GetPosition().Position.Y;
                        renderedEnemy = true;
                        renderedMonster = false;
                        renderedScenery = false;
                    }
                }

                if (item != null)
                {
                    if (lowestPostion > item.GetPosition().Position.Y)
                    {
                        lowestPostion = item.GetPosition().Position.Y;
                        renderedItem = true;
                        renderedEnemy = false;
                        renderedMonster = false;
                        renderedScenery = false;
                    }
                }

                if (misc != null)
                {
                    if (lowestPostion > misc.GetPosition().Position.Y)
                    {
                        lowestPostion = misc.GetPosition().Position.Y;
                        renderedMisc = true;
                        renderedItem = false;
                        renderedEnemy = false;
                        renderedMonster = false;
                        renderedScenery = false;
                    }
                }

                if (this.egg != null && !renderedEgg)
                {
                    if (lowestPostion > this.egg.GetPosition().Position.Y)
                    {
                        lowestPostion = this.egg.GetPosition().Position.Y;
                        renderedEgg = true;
                        renderedMisc = false;
                        renderedItem = false;
                        renderedEnemy = false;
                        renderedMonster = false;
                        renderedScenery = false;
                    }
                }

                //Render the object
                if (renderedScenery)
                {
                    sceneryTile.Draw(this.spriteBatch);

                    ++sceneryI;
                    if (sceneryI < this.scenery.Count)
                    {
                        sceneryTile = this.scenery[sceneryI];
                    }
                    else
                    {
                        sceneryTile = null;
                    }
                }
                else if (renderedMonster)
                {
                    monster.Draw(gameTime, this.spriteBatch, Vector2.Zero, false);

                    ++monsterI;
                    if (monsterI < this.monsters.Count)
                    {
                        monster = this.monsters[monsterI];
                    }
                    else
                    {
                        monster = null;
                    }
                }
                else if (renderedEnemy)
                {
                    //Only draw enemies that aren't water type, since we did those already
                    if (enemy.GetPathfind().Walker != WalkerType.Water)
                    {
                        enemy.Draw(gameTime, this.spriteBatch, Vector2.Zero, false);
                    }

                    ++enemyI;
                    if (enemyI < this.enemies.Count)
                    {
                        enemy = this.enemies[enemyI];
                    }
                    else
                    {
                        enemy = null;
                    }
                }
                else if (renderedItem)
                {
                    item.Draw(gameTime, this.spriteBatch, Vector2.Zero, true);

                    ++itemI;
                    if (itemI < this.items.Count)
                    {
                        item = this.items[itemI];
                    }
                    else
                    {
                        item = null;
                    }
                }
                else if (renderedMisc)
                {
                    misc.Draw(gameTime, this.spriteBatch, Vector2.Zero, false);

                    ++miscI;
                    if (miscI < this.misc.Count)
                    {
                        misc = this.misc[miscI];
                    }
                    else
                    {
                        misc = null;
                    }
                }
                else if (renderedEgg)
                {
                    this.egg.Draw(gameTime, this.spriteBatch, Vector2.Zero, true);
                }
            }
            this.spriteBatch.End();
        }

        public void SetTile(Point tile, int typeID)
        {
            World currentWorld = TerrainKey.WorldTypes[this.world];
            this.map[tile.X][tile.Y] = new TileType(typeID, -1);//currentWorld.GetRandomSpire());
        }

        public void SetScenery(Point lastMousePos, Point currenTile, int sceneryID)
        {
            Vector2 position = new Vector2(lastMousePos.X - Camera.Instance.View.Translation.X,
                                           lastMousePos.Y - Camera.Instance.View.Translation.Y);

            //Remove Scenery
            if (sceneryID == 0)
            {
                List<SceneryTile> reversedScenery = new List<SceneryTile>(this.scenery.ToArray());
                reversedScenery.Reverse();

                foreach (SceneryTile sceneryTile in reversedScenery)
                {
                    int halfWidth = sceneryTile.SceneryType.Frame.Width / 2;

                    //Remove the top scenery object at this location
                    if (position.X >= sceneryTile.Position.X - halfWidth && position.X <= sceneryTile.Position.X + halfWidth &&
                        position.Y >= sceneryTile.Position.Y - sceneryTile.SceneryType.Frame.Height && position.Y <= sceneryTile.Position.Y)
                    {
                        this.scenery.Remove(sceneryTile);
                        return;
                    }
                }
            }
            else //Add Scenery
            {
                this.scenery.Add(new SceneryTile(sceneryID, position));
                this.scenery.Sort(new SceneryComparer());
            }
        }

        public void SetObstacle(Point lastMousePos, Point currentTile, bool add)
        {
            Vector2 position = new Vector2(lastMousePos.X - Camera.Instance.View.Translation.X,
                               lastMousePos.Y - Camera.Instance.View.Translation.Y);

            //Remove 
            if (!add)
            {
                List<Actor> reversedList = new List<Actor>(this.items.ToArray());
                reversedList.Reverse();

                foreach (Actor actor in reversedList)
                {
                    int halfWidth = actor.GetSprite().GetFrame().Width / 2;
                    Vector2 pos = actor.GetPosition().Position;
                    //Remove the top actor object of this category from this location
                    if (position.X >= pos.X - halfWidth && position.X <= pos.X + halfWidth &&
                        position.Y >= pos.Y - actor.GetSprite().GetFrame().Height && position.Y <= pos.Y)
                    {
                        this.items.Remove(actor);
                        ActorFactory.Instance.RemoveActor(actor.ActorID);
                        return;
                    }
                }
            }
            else //Add 
            {
                Actor actor = ActorFactory.Instance.Create(ActorType.OBSTACLE);
                if (actor != null)
                {
                    //Make sure there isn't already an obstacle here
                    foreach (Actor item in this.items)
                    {
                        if (item.ActorType == ActorType.OBSTACLE)
                        {
                            Point tile = WorldUtilities.WorldToTile(item.GetPosition().Position);
                            if (tile == currentTile)
                                return;
                        }
                    }

                    actor.GetPosition().Position = WorldUtilities.TileToWorld(currentTile);
                    this.items.Add(actor);
                    this.items.Sort(new ActorComparer());
                }
            }
        }

        public void SetMonsterType(Point lastMousePos, Point currentTile, int monsterID)
        {
            Vector2 position = new Vector2(lastMousePos.X - Camera.Instance.View.Translation.X,
                                           lastMousePos.Y - Camera.Instance.View.Translation.Y);

            //Remove 
            if (monsterID < 0)
            {
                List<Actor> reversedList = new List<Actor>(this.monsters.ToArray());
                reversedList.Reverse();

                foreach (Actor actor in reversedList)
                {
                    int halfWidth = actor.GetSprite().GetFrame().Width / 2;
                    Vector2 pos = actor.GetPosition().Position;
                    //Remove the top actor object of this category from this location
                    if (position.X >= pos.X - halfWidth && position.X <= pos.X + halfWidth &&
                        position.Y >= pos.Y - actor.GetSprite().GetFrame().Height && position.Y <= pos.Y)
                    {
                        this.monsters.Remove(actor);
                        ActorFactory.Instance.RemoveActor(actor.ActorID);
                        return;
                    }
                }
            }
            else //Add 
            {
                Actor actor = ActorFactory.Instance.Create(ActorKey.MonsterEnum[monsterID]);
                if (actor != null)
                {
                    actor.GetPosition().Position = WorldUtilities.TileToWorld(currentTile);
                    this.monsters.Add(actor);
                    this.monsters.Sort(new ActorComparer());
                }
            }
        }

        public void SetEnemyType(Point lastMousePos, Point currentTile, int enemyID)
        {
            Vector2 position = new Vector2(lastMousePos.X - Camera.Instance.View.Translation.X,
                                           lastMousePos.Y - Camera.Instance.View.Translation.Y);

            //Remove 
            if (enemyID < 0)
            {
                List<Actor> reversedList = new List<Actor>(this.enemies.ToArray());
                reversedList.Reverse();

                foreach (Actor actor in reversedList)
                {
                    int halfWidth = actor.GetSprite().GetFrame().Width / 2;
                    Vector2 pos = actor.GetPosition().Position;
                    //Remove the top actor object of this category from this location
                    if (position.X >= pos.X - halfWidth && position.X <= pos.X + halfWidth &&
                        position.Y >= pos.Y - actor.GetSprite().GetFrame().Height && position.Y <= pos.Y)
                    {
                        this.enemies.Remove(actor);
                        ActorFactory.Instance.RemoveActor(actor.ActorID);
                        return;
                    }
                }
            }
            else //Add 
            {
                Actor actor = ActorFactory.Instance.Create(ActorKey.EnemyEnum[enemyID]);
                if (actor != null)
                {
                    actor.GetPosition().Position = WorldUtilities.TileToWorld(currentTile);
                    this.enemies.Add(actor);
                    this.enemies.Sort(new ActorComparer());
                }
            }
        }

        public void SetEnemyAIDir(Point lastMousePos, Point currentTile, PathDirection dir)
        {
            Vector2 position = new Vector2(lastMousePos.X - Camera.Instance.View.Translation.X,
                                           lastMousePos.Y - Camera.Instance.View.Translation.Y);

            List<Actor> reversedList = new List<Actor>(this.enemies.ToArray());
            reversedList.Reverse();

            foreach (Actor actor in reversedList)
            {
                int halfWidth = actor.GetSprite().GetFrame().Width / 2;
                Vector2 pos = actor.GetPosition().Position;
                //Remove the top actor object of this category from this location
                if (position.X >= pos.X - halfWidth && position.X <= pos.X + halfWidth &&
                    position.Y >= pos.Y - actor.GetSprite().GetFrame().Height && position.Y <= pos.Y)
                {
                    (actor as Enemy).AIConfig.DirectionPreference = dir;

                    return;
                }
            }
        }

        public void SetPowerUpType(Point lastMousePos, Point currentTile, int powerupID)
        {
            Vector2 position = new Vector2(lastMousePos.X - Camera.Instance.View.Translation.X,
                                           lastMousePos.Y - Camera.Instance.View.Translation.Y);

            //Remove 
            if (powerupID < 0)
            {
                List<Actor> reversedList = new List<Actor>(this.items.ToArray());
                reversedList.Reverse();

                foreach (Actor actor in reversedList)
                {
                    int halfWidth = actor.GetSprite().GetFrame().Width / 2;
                    Vector2 pos = actor.GetPosition().Position;
                    //Remove the top actor object of this category from this location
                    if (position.X >= pos.X - halfWidth && position.X <= pos.X + halfWidth &&
                        position.Y >= pos.Y - actor.GetSprite().GetFrame().Height && position.Y <= pos.Y)
                    {
                        this.items.Remove(actor);
                        ActorFactory.Instance.RemoveActor(actor.ActorID);
                        return;
                    }
                }
            }
            else //Add 
            {
                Actor actor = ActorFactory.Instance.Create(ActorKey.PowerUpEnum[powerupID]);
                if (actor != null)
                {
                    actor.GetPosition().Position = WorldUtilities.TileToWorld(currentTile);
                    this.items.Add(actor);
                    this.items.Sort(new ActorComparer());
                }
            }
        }

        public void SetStartPoint(Point tile)
        {
            if (GetTile(tile).ID != 0)
            {
                this.eggPoint = tile;
                this.egg.GetPosition().Position = WorldUtilities.TileToWorld(tile);
            }
        }
        
        public void SetEndPoint(Point tile)
        {
            if (GetTile(tile).ID != 0)
                this.end = tile;
        }

        public TileType GetTile(int x, int y)
        {
            return this.map[x][y];
        }

        public TileType GetTile(Point tile)
        {
            return this.map[tile.X][tile.Y];
        }

        public void ChangeWorldType(int worldType)
        {
            this.world = worldType;
            this.background = TerrainKey.Backgrounds[this.world];
            this.background.Initialize();

            //Go through the map and change the spire types
            World currentWorld = TerrainKey.WorldTypes[this.world];
            for (int y = 0; y < this.height; ++y)
            {
                for (int x = 0; x < this.width; ++x)
                {
                   // this.map[x][y].SpireTypeID = currentWorld.GetRandomSpire();
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sorts our scenery list in ascending positions
        /// </summary>
        private class SceneryComparer : IComparer<SceneryTile>
        {
            public int Compare(SceneryTile a, SceneryTile b)
            {
                //Simply look at the y value to see who is lower
                if (a.Position.Y > b.Position.Y)
                    return 1;
                else if (a.Position.Y < b.Position.Y)
                    return -1;

                if (a.Position.X > b.Position.X)
                    return 1;
                else if (a.Position.X < b.Position.X)
                    return -1;

                if (a.SceneryTypeID > b.SceneryTypeID)
                    return 1;
                else if (a.SceneryTypeID < b.SceneryTypeID)
                    return -1;

                //Exactly the same tile
                return 0;
            }
        }

        /// <summary>
        /// Sorts our actors in ascending positions
        /// </summary>
        private class ActorComparer : IComparer<Actor>
        {
            public int Compare(Actor a, Actor b)
            {
                PositionComponent aPosition = (a.GetComponent(ComponentType.POSITION) as PositionComponent);
                PositionComponent bPosition = (b.GetComponent(ComponentType.POSITION) as PositionComponent);
                if (aPosition == null || bPosition == null)
                {
                    throw new ArgumentNullException("Position Component");
                }

                //Simply look at the y value to see who is lower
                if (aPosition.Position.Y > bPosition.Position.Y)
                    return 1;
                else if (aPosition.Position.Y < bPosition.Position.Y)
                    return -1;

                if (aPosition.Position.X > bPosition.Position.X)
                    return 1;
                else if (aPosition.Position.X < bPosition.Position.X)
                    return -1;

                if (a.ActorType < b.ActorType)
                    return 1;
                else if (a.ActorType > b.ActorType)
                    return -1;

                if (a.ActorID.GetHashCode() > b.ActorID.GetHashCode())
                    return 1;
                else if (a.ActorID.GetHashCode() < b.ActorID.GetHashCode())
                    return -1;

                //Exactly the same tile
                return 0;
            }
        }
        #endregion
    }
}
