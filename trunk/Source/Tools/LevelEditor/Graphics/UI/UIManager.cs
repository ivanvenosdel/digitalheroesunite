#region Using Statements
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonsterEscape.Graphics.Cameras;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Enemies;
using MonsterEscape.Logic.Actors.Items;
using MonsterEscape.Logic.Actors.Monsters;
using MonsterEscape.Logic.AI;
using MonsterEscape.Logic.Input;
using MonsterEscape.Logic.Puzzle;
using MonsterEscape.Worlds;
using MonsterEscape.Utility;

using Engine.World;
#endregion

namespace MonsterEscape.Graphics.UI
{
    #region Enums
    /// <summary>Types of Edit Mode</summary>
    public enum EditModeType
    {
        WORLD = 0,
        SCENERY,
        MONSTERS,
        ENEMIES,
        OTHER,
        PUZZLE,
        End
    }

    /// <summary>Types of SubEdit Mode, Depending on Edit Mode</summary>
    public enum SubEditModeType
    {
        NONE = 0,
        STARTPOINT,
        ENDPOINT,
        AI_LEFT,
        AI_RIGHT,
        AI_DOWN,
        AI_UP,
        OBSTACLE,
        End
    }

    /// <summary>Font Types</summary>
    public enum FontType
    {
        /// <summary>Consolas 16</summary>
        DEFAULT = 0
    };
    #endregion

    public class UIManager
    {
        #region Fields
        private static readonly UIManager instance = new UIManager();

        private SpriteBatch spriteBatch;
        private Texture2D selectorHud;
        private Texture2D worldHud;
        private Texture2D sceneryHud;
        private Texture2D monsterHud;
        private Texture2D enemyHud;
        private Texture2D otherHud;
        private Texture2D newWorld;
        private Texture2D puzzlePieceMenu;
        private Texture2D[] puzzlePieces;
        private Texture2D puzzleSelector;
        private Texture2D[] arrows;
        private Texture2D spitter;
        private Texture2D sleeper;
        private Texture2D box;
        private Texture2D directional;
        private Texture2D random;
        private List<SpriteFont> fonts = new List<SpriteFont>();
        private Grid grid;

        private bool drawAI = true;
        private bool drawGrid = true;
        private bool makingNewWorld = true;
        private EditModeType editMode = EditModeType.WORLD;
        private SubEditModeType subeditMode = SubEditModeType.NONE;
        private int currentTileType;
        private int currentSceneryType;
        private int currentWorldType;
        private int currentMonsterType;
        private int currentEnemyType;
        private int currentPowerUpType;
        private List<PuzzlePieceTypes> puzzlePieceTypeList = new List<PuzzlePieceTypes>();
        private List<PuzzlePieceRotation> puzzlePieceRotationList = new List<PuzzlePieceRotation>();
        private int puzzlePieceSelection = 0;
        #endregion

        #region Properties
        /// <summary>Singelton</summary>
        public static UIManager Instance { get { return instance; } }
        /// <summary>Obtains a font</summary>
        public SpriteFont getFont(FontType fnt) { return this.fonts[(int)fnt]; }
        /// <summary>Flag indicating if the user is making a new world</summary>
        public bool MakingNewWorld { get { return this.makingNewWorld; } }
        /// <summary>Draw AI Indicators Flag</summary>
        public bool DrawAI { get { return this.drawAI; } }
        #endregion

        #region Constructors
        protected UIManager() { }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            this.grid = new Grid();
            this.currentTileType = 1;
            this.currentSceneryType = 1;
            this.currentWorldType = 1;

            InputManager.Instance.OnKeyEvent += new KeyEvent(this.OnKeyEvent);
            InputManager.Instance.OnMouseEvent += new MouseEvent(this.OnMouseEvent);
        }

        public void Update(GameTime gameTime)
        {
            FramesPerSecond.Update(gameTime);
        }
        
        public void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(DeviceManager.Instance.GraphicsDevice);
            this.fonts.Add(DeviceManager.Instance.ContentManager.Load<SpriteFont>(@"UI/Fonts/debugfont")); //FontType.DEFAULT

            this.selectorHud = DeviceManager.Instance.ContentManager.Load<Texture2D>("selectorhud");
            this.worldHud = DeviceManager.Instance.ContentManager.Load<Texture2D>("worldhud");
            this.sceneryHud = DeviceManager.Instance.ContentManager.Load<Texture2D>("sceneryhud");
            this.monsterHud = DeviceManager.Instance.ContentManager.Load<Texture2D>("monsterhud");
            this.enemyHud = DeviceManager.Instance.ContentManager.Load<Texture2D>("enemyhud");
            this.otherHud = DeviceManager.Instance.ContentManager.Load<Texture2D>("otherhud");
            this.newWorld = DeviceManager.Instance.ContentManager.Load<Texture2D>("newWorld");
            this.puzzlePieceMenu = DeviceManager.Instance.ContentManager.Load<Texture2D>("puzzlePieces");

            this.arrows = new Texture2D[5];
            //0 PathDirection.Unknown - unused
            this.arrows[(int)PathDirection.Up] = DeviceManager.Instance.ContentManager.Load<Texture2D>("up");
            this.arrows[(int)PathDirection.Down] = DeviceManager.Instance.ContentManager.Load<Texture2D>("down");
            this.arrows[(int)PathDirection.Left] = DeviceManager.Instance.ContentManager.Load<Texture2D>("left");
            this.arrows[(int)PathDirection.Right] = DeviceManager.Instance.ContentManager.Load<Texture2D>("right");

            this.spitter = DeviceManager.Instance.ContentManager.Load<Texture2D>("spitter");
            this.sleeper = DeviceManager.Instance.ContentManager.Load<Texture2D>("sleeper");
            this.box = DeviceManager.Instance.ContentManager.Load<Texture2D>("box");
            this.directional = DeviceManager.Instance.ContentManager.Load<Texture2D>("directional");
            this.random = DeviceManager.Instance.ContentManager.Load<Texture2D>("random");

            this.puzzlePieces = new Texture2D[5];
            this.puzzlePieces[(int)PuzzlePieceTypes.STRAIGHT] = DeviceManager.Instance.ContentManager.Load<Texture2D>("puzzle_straight");
            this.puzzlePieces[(int)PuzzlePieceTypes.STRAIGHT_FRONT] = DeviceManager.Instance.ContentManager.Load<Texture2D>("puzzle_front");
            this.puzzlePieces[(int)PuzzlePieceTypes.STRAIGHT_MID] = DeviceManager.Instance.ContentManager.Load<Texture2D>("puzzle_mid");
            this.puzzlePieces[(int)PuzzlePieceTypes.STRAIGHT_BACK] = DeviceManager.Instance.ContentManager.Load<Texture2D>("puzzle_back");
            this.puzzlePieces[(int)PuzzlePieceTypes.L] = DeviceManager.Instance.ContentManager.Load<Texture2D>("puzzle_l");

            this.puzzleSelector = DeviceManager.Instance.ContentManager.Load<Texture2D>("pieceSelector");
        }

        public void Draw(GameTime gameTime)
        {
            SpriteFont font = getFont(FontType.DEFAULT);

            //Draw Grid
            if (drawGrid)
                this.grid.Draw();

            this.spriteBatch.Begin();

            //Draw Selector
            this.spriteBatch.Draw(this.selectorHud, new Vector2(2, 2), Color.White);

            //Draw Hud
            Vector2 scale;
            switch(this.editMode)
            {
                case EditModeType.WORLD:
                    {
                        this.spriteBatch.Draw(this.worldHud, new Vector2(0, 480), Color.White);
                        //World Type ID
                        this.spriteBatch.DrawString(font, "" + this.currentWorldType, new Vector2(290, 486), Color.White);
                        //Tile Type ID
                        this.spriteBatch.DrawString(font, "" + this.currentTileType, new Vector2(440, 486), Color.White);
                        WorldTile tile = new WorldTile(this.currentTileType);
                        tile.Draw(this.spriteBatch, new Vector2(575, 483), new Vector2(0.55f,0.55f));
                    }
                    break;
                case EditModeType.SCENERY:
                    {
                        this.spriteBatch.Draw(this.sceneryHud, new Vector2(0, 480), Color.White);
                        //Scenery Type ID
                        this.spriteBatch.DrawString(font, "" + this.currentSceneryType, new Vector2(340, 486), Color.White);
                        SceneryTile tile = new SceneryTile(this.currentSceneryType, new Vector2(485, 520));
                        scale = FitImage(tile.SceneryType.Frame.Width, tile.SceneryType.Frame.Height, 40);
                        tile.Draw(this.spriteBatch, scale);
                    }
                    break;
                case EditModeType.MONSTERS:
                    {
                        this.spriteBatch.Draw(this.monsterHud, new Vector2(0, 480), Color.White);
                        //Monster Type ID
                        this.spriteBatch.DrawString(font, "" + this.currentMonsterType, new Vector2(340, 486), Color.White);
                        Actor actor = ActorFactory.Instance.Create(ActorKey.MonsterEnum[this.currentMonsterType]);
                        if (actor != null)
                        {
                            scale = FitImage(actor.GetSprite().GetFrame().Width, actor.GetSprite().GetFrame().Height, 40);
                            actor.GetSprite().Draw(gameTime, this.spriteBatch, new Vector2(485, 520), false, scale);
                            ActorFactory.Instance.RemoveActor(actor.ActorID);
                        }
                    }
                    break;
                case EditModeType.ENEMIES:
                    {
                        this.spriteBatch.Draw(this.enemyHud, new Vector2(0, 480), Color.White);
                        //Enemy Type ID
                        this.spriteBatch.DrawString(font, "" + this.currentEnemyType, new Vector2(340, 486), Color.White);
                        Actor actor = ActorFactory.Instance.Create(ActorKey.EnemyEnum[this.currentEnemyType]);
                        if (actor != null)
                        {
                            scale = FitImage(actor.GetSprite().GetFrame().Width, actor.GetSprite().GetFrame().Height, 40);
                            actor.GetSprite().Draw(gameTime, this.spriteBatch, new Vector2(485, 520), false, scale);
                            ActorFactory.Instance.RemoveActor(actor.ActorID);
                        }
                    }
                    break;
                case EditModeType.OTHER:
                    {
                        this.spriteBatch.Draw(this.otherHud, new Vector2(0, 480), Color.White);
                        //Powerup Type ID
                        this.spriteBatch.DrawString(font, "" + this.currentPowerUpType, new Vector2(340, 486), Color.White);
                        Actor actor = ActorFactory.Instance.Create(ActorKey.PowerUpEnum[this.currentPowerUpType]);
                        if (actor != null)
                        {
                            scale = FitImage(actor.GetSprite().GetFrame().Width, actor.GetSprite().GetFrame().Height, 40);
                            actor.GetSprite().Draw(gameTime, this.spriteBatch, new Vector2(485, 520), false, scale);
                            ActorFactory.Instance.RemoveActor(actor.ActorID);
                        }
                    }
                    break;
                case EditModeType.PUZZLE:
                    {
                        this.spriteBatch.Draw(this.puzzlePieceMenu, new Vector2(0, 0), Color.White);

                        //Draw pieces
                        const int PiecesPerRow = 18;
                        for (int i = 0; i < this.puzzlePieceTypeList.Count; ++i)
                        {
                            PuzzlePieceTypes type = this.puzzlePieceTypeList[i];
                            Texture2D piece = this.puzzlePieces[(int)type];

                            PuzzlePieceRotation rotation = this.puzzlePieceRotationList[i];
                            float rot = (float)(MathHelper.PiOver2 * (int)rotation);

                            int x = (i % PiecesPerRow) * 35 + 40;
                            int y = (i / PiecesPerRow) * 35 + 40;
                            this.spriteBatch.Draw(piece, new Rectangle(x, y, 32, 32), null, Color.White, rot, new Vector2(16,16), SpriteEffects.None, 0f );
                        }

                        //Selection over hud
                        int xx = (this.puzzlePieceSelection % PiecesPerRow) * 35 + 40 - 16;
                        int yy = (this.puzzlePieceSelection / PiecesPerRow) * 35 + 40 - 16;
                        this.spriteBatch.Draw(this.puzzleSelector, new Rectangle(xx, yy, 32, 32), Color.White);
                    }
                    break;
            };

            //Draw New World
            if (makingNewWorld)
            {
                this.spriteBatch.Draw(this.newWorld, Vector2.Zero, Color.White);

                //Draw Text
                this.spriteBatch.DrawString(font, "" + CurrentLevel.Instance.Width, new Vector2(400, 138), Color.White);
                this.spriteBatch.DrawString(font, "" + CurrentLevel.Instance.Height, new Vector2(400, 225), Color.White);
                this.spriteBatch.DrawString(font, "" + this.currentWorldType, new Vector2(400, 316), Color.White);
            }

            //Draw Debug Text
            //this.spriteBatch.DrawString(font, "Mode: " + this.editMode.ToString(), new Vector2(5, 45), Color.White);
            //this.spriteBatch.DrawString(font, InputManager.Instance.lastMousePos.ToString(), new Vector2(5, 65), Color.White);
            //this.spriteBatch.DrawString(font, InputManager.Instance.CurrentTile.ToString(), new Vector2(5, 85), Color.White);
            this.spriteBatch.End();

            //If drawAi is toggled, draw the AI indicators in world space
            if (this.drawAI && this.editMode != EditModeType.PUZZLE)
            {
                 this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.Instance.View);
                foreach (Actor enemy in CurrentLevel.Instance.Enemies)
                {
                    int heightOffset = enemy.GetSprite().GetFrame().Height / 2;

                    this.spriteBatch.Draw(this.arrows[(int)((enemy as Enemy).AIConfig.DirectionPreference)], enemy.GetPosition().Position + new Vector2(-this.arrows[(int)((enemy as Enemy).AIConfig.DirectionPreference)].Width /2, -heightOffset), Color.White);

                    if ((enemy as Enemy).AIConfig.Behavior == AIBehavior.SLEEPER)
                        this.spriteBatch.Draw(this.sleeper, enemy.GetPosition().Position + new Vector2(-16, -32 - heightOffset), Color.White);
                    else if ((enemy as Enemy).AIConfig.Behavior == AIBehavior.SPITTER)
                        this.spriteBatch.Draw(this.spitter, enemy.GetPosition().Position + new Vector2(16, -32 - heightOffset), Color.White);

                    if ((enemy as Enemy).AIConfig.Direction == AIDirection.BOX)
                        this.spriteBatch.Draw(this.box, enemy.GetPosition().Position + new Vector2(16, -32 - heightOffset), Color.White);
                    else if ((enemy as Enemy).AIConfig.Direction == AIDirection.DIRECTIONAL)
                        this.spriteBatch.Draw(this.directional, enemy.GetPosition().Position + new Vector2(16, -32 - heightOffset), Color.White);
                    else if ((enemy as Enemy).AIConfig.Direction == AIDirection.RANDOM)
                        this.spriteBatch.Draw(this.random, enemy.GetPosition().Position + new Vector2(16, -32 - heightOffset), Color.White);
                }
                this.spriteBatch.End();
            }
        }

        private Vector2 FitImage(float width, float height, float box)
        {
            float originalWidth = width;
            float originalHeight = height;

            float ratio;
            if (width > height)
            {
                ratio = height / width;

                width = box;
                height = width * ratio;
            }
            else
            {
                ratio = width / height;

                height = box;
                width = height * ratio;
            }

            return new Vector2(width / originalWidth, height / originalHeight);
        }

        private void RotateSelectedPuzzlePiece()
        {
            if (this.puzzlePieceSelection < 0)
                return;

            if (this.puzzlePieceRotationList[this.puzzlePieceSelection] == (PuzzlePieceRotation)3)
                this.puzzlePieceRotationList[this.puzzlePieceSelection] = PuzzlePieceRotation.DEGREES_0;
            else
                this.puzzlePieceRotationList[this.puzzlePieceSelection] += 1;
        }

        private void DeleteSelectedPuzzlePiece()
        {
            if (this.puzzlePieceSelection < 0 || this.puzzlePieceSelection > this.puzzlePieceRotationList.Count - 1)
                return;

            this.puzzlePieceRotationList.RemoveAt(this.puzzlePieceSelection);
            this.puzzlePieceTypeList.RemoveAt(this.puzzlePieceSelection);
            this.puzzlePieceSelection--;
            if (this.puzzlePieceSelection < 0)
                this.puzzlePieceSelection = 0;
        }

        Microsoft.Xna.Framework.Input.Keys keyDown;
        bool shiftDown = false;
        /// <summary>Handles key events from InputManager</summary>
        /// <param name="keyboardState">Keyboard State</param>
        public void OnKeyEvent(KeyboardState keyboardState)
        {
            //Shifter
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
            {
                if (keyDown != Microsoft.Xna.Framework.Input.Keys.LeftShift)
                {
                    keyDown = Microsoft.Xna.Framework.Input.Keys.LeftShift;

                    this.shiftDown = true;
                }
            }
            else if (keyDown == Microsoft.Xna.Framework.Input.Keys.LeftShift)
            {
                this.shiftDown = false;
                keyDown = Microsoft.Xna.Framework.Input.Keys.None;
            }

            if (makingNewWorld)
                return;

            //Toggle Mode
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab))
            {
                if (keyDown != Microsoft.Xna.Framework.Input.Keys.Tab)
                {
                    keyDown = Microsoft.Xna.Framework.Input.Keys.Tab;

                    this.editMode += 1;
                    if (this.editMode >= EditModeType.End)
                        this.editMode = EditModeType.WORLD;
                }
            }
            else if (keyDown == Microsoft.Xna.Framework.Input.Keys.Tab)
            {
                keyDown = Microsoft.Xna.Framework.Input.Keys.None;
            }
        }

        Microsoft.Xna.Framework.Input.ButtonState lastLeftState;
        Microsoft.Xna.Framework.Input.ButtonState lastRightState;
        /// <summary>Handles mouse events from InputManager</summary>
        public void OnMouseEvent(ButtonList mouseButtons, Vector2 mouseDelta, float scrollDelta, Point lastMousePos)
        {
            int scalar = this.shiftDown ? 10 : 1;

            if (makingNewWorld)
            {
                if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                }
                else if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Released && lastLeftState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Released;

                    //Width Down Arrow
                    if (lastMousePos.X >= 491 && lastMousePos.Y >= 139 &&
                        lastMousePos.X <= 522 && lastMousePos.Y <= 170)
                    {
                        CurrentLevel.Instance.Width -= 1 * scalar;
                        if (CurrentLevel.Instance.Width < 0)
                            CurrentLevel.Instance.Width = 0;
                    }
                    //Width Up Arrow
                    else if (lastMousePos.X >= 528 && lastMousePos.Y >= 138 &&
                        lastMousePos.X <= 559 && lastMousePos.Y <= 169)
                    {
                        CurrentLevel.Instance.Width += 1 * scalar;
                        if (CurrentLevel.Instance.Width > Int16.MaxValue)
                            CurrentLevel.Instance.Width = Int16.MaxValue;
                    }
                    //Height Down Arrow
                    else if (lastMousePos.X >= 493 && lastMousePos.Y >= 227 &&
                        lastMousePos.X <= 524 && lastMousePos.Y <= 256)
                    {
                        CurrentLevel.Instance.Height -= 1 * scalar;
                        if (CurrentLevel.Instance.Height < 0)
                            CurrentLevel.Instance.Height = 0;
                    }
                    //Height Up Arrow
                    else if (lastMousePos.X >= 528 && lastMousePos.Y >= 227 &&
                        lastMousePos.X <= 558 && lastMousePos.Y <= 254)
                    {
                        CurrentLevel.Instance.Height += 1 * scalar;
                        if (CurrentLevel.Instance.Height > Int16.MaxValue)
                            CurrentLevel.Instance.Height = Int16.MaxValue;
                    }
                    //World Down Arrow
                    else if (lastMousePos.X >= 491 && lastMousePos.Y >= 315 &&
                        lastMousePos.X <= 524 && lastMousePos.Y <= 346)
                    {
                        --currentWorldType;
                        if (currentWorldType < 0)
                            currentWorldType = TerrainKey.WorldTypes.Count-1;
                    }
                    //World Up Arrow
                    else if (lastMousePos.X >= 530 && lastMousePos.Y >= 315 &&
                        lastMousePos.X <= 558 && lastMousePos.Y <= 345)
                    {
                        ++currentWorldType;
                        if (currentWorldType > TerrainKey.WorldTypes.Count-1)
                            currentWorldType = 0;
                    }
                    //OK
                    else if (lastMousePos.X >= 335 && lastMousePos.Y >= 420 &&
                        lastMousePos.X <= 377 && lastMousePos.Y <= 443)
                    {
                        this.makingNewWorld = false;

                        CurrentLevel.Instance.Initialize(this.currentWorldType);
                    }
                    //Cancel
                    else if (lastMousePos.X >= 402 && lastMousePos.Y >= 420 &&
                        lastMousePos.X <= 471 && lastMousePos.Y <= 444)
                    {
                        this.makingNewWorld = false;
                    }
                }
            }
            else if (this.editMode == EditModeType.PUZZLE)
            {
                if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                }
                else if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Released && lastLeftState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Released;

                    //Okay
                    if (lastMousePos.X >= 390 && lastMousePos.Y >= 492 &&
                        lastMousePos.X <= 430 && lastMousePos.Y <= 514)
                    {
                        this.editMode = EditModeType.WORLD;
                    }
                    //Straight
                    else if (lastMousePos.X >= 37 && lastMousePos.Y >= 434 &&
                        lastMousePos.X <= 92 && lastMousePos.Y <= 486)
                    {
                        this.puzzlePieceTypeList.Add(PuzzlePieceTypes.STRAIGHT);
                        this.puzzlePieceRotationList.Add(PuzzlePieceRotation.DEGREES_0);
                    }
                    //Front
                    else if (lastMousePos.X >= 183 && lastMousePos.Y >= 434 &&
                        lastMousePos.X <= 244 && lastMousePos.Y <= 486)
                    {
                        this.puzzlePieceTypeList.Add(PuzzlePieceTypes.STRAIGHT_FRONT);
                        this.puzzlePieceRotationList.Add(PuzzlePieceRotation.DEGREES_0);
                    }
                    //Middle
                    else if (lastMousePos.X >= 306 && lastMousePos.Y >= 434 &&
                        lastMousePos.X <= 367 && lastMousePos.Y <= 486)
                    {
                        this.puzzlePieceTypeList.Add(PuzzlePieceTypes.STRAIGHT_MID);
                        this.puzzlePieceRotationList.Add(PuzzlePieceRotation.DEGREES_0);
                    }
                    //Back
                    else if (lastMousePos.X >= 451 && lastMousePos.Y >= 434 &&
                        lastMousePos.X <= 517 && lastMousePos.Y <= 486)
                    {
                        this.puzzlePieceTypeList.Add(PuzzlePieceTypes.STRAIGHT_BACK);
                        this.puzzlePieceRotationList.Add(PuzzlePieceRotation.DEGREES_0);
                    }
                    //L
                    else if (lastMousePos.X >= 589 && lastMousePos.Y >= 434 &&
                        lastMousePos.X <= 649 && lastMousePos.Y <= 486)
                    {
                        this.puzzlePieceTypeList.Add(PuzzlePieceTypes.L);
                        this.puzzlePieceRotationList.Add(PuzzlePieceRotation.DEGREES_0);
                    }
                    //Rotate
                    else if (lastMousePos.X >= 677 && lastMousePos.Y >= 434 &&
                        lastMousePos.X <= 727 && lastMousePos.Y <= 486)
                    {
                        RotateSelectedPuzzlePiece();
                    }
                    //Delete
                    else if (lastMousePos.X >= 729 && lastMousePos.Y >= 434 &&
                        lastMousePos.X <= 787 && lastMousePos.Y <= 486)
                    {
                        DeleteSelectedPuzzlePiece();
                    }
                    else //Determine which puzzle piece they may be over out of the list of puzzle pieces
                    {
                        const int PiecesPerRow = 20;
                        for (int i = 0; i < this.puzzlePieceTypeList.Count; ++i)
                        {
                            PuzzlePieceTypes type = this.puzzlePieceTypeList[i];
                            Texture2D piece = this.puzzlePieces[(int)type];

                            PuzzlePieceRotation rotation = this.puzzlePieceRotationList[i];
                            float rot = (float)(Math.PI * (int)rotation);

                            int x = (i % PiecesPerRow) * 35 + 30;
                            int y = (i / PiecesPerRow) * 35 + 30;

                            if (lastMousePos.X >= x - 16 && lastMousePos.Y >= y - 16 &&
                                lastMousePos.X <= x + 16 && lastMousePos.Y <= y + 16)
                            {
                                this.puzzlePieceSelection = i;
                                break;
                            }
                        }
                    }
                }
            }
            //Over the HUD
            else if (lastMousePos.Y > 480)
            {
                if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                }
                else if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Released && lastLeftState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Released;

                    //New
                    if (lastMousePos.X >= 7 && lastMousePos.Y >= 486 &&
                        lastMousePos.X <= 59 && lastMousePos.Y <= 510)
                    {
                        makingNewWorld = true;

                        this.puzzlePieceTypeList.Clear();
                        this.puzzlePieceRotationList.Clear();
                    }
                    //Save
                    else if (lastMousePos.X >= 69 && lastMousePos.Y >= 486 &&
                            lastMousePos.X <= 131 && lastMousePos.Y <= 510)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                        saveFileDialog.Filter = "XML File (*.xml)|*.xml|All Files (*.*)|*.*";
                        saveFileDialog.FilterIndex = 1;

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            LevelMap saveMap = new LevelMap();

                            saveMap.Name = CurrentLevel.Instance.Name;

                            saveMap.World = this.currentWorldType;
                            saveMap.Level = 0;

                            saveMap.ParTime = CurrentLevel.Instance.ParTime;

                            saveMap.Egg = CurrentLevel.Instance.EggPoint;
                            saveMap.End = CurrentLevel.Instance.End;

                            saveMap.Width = CurrentLevel.Instance.Width;
                            saveMap.Height = CurrentLevel.Instance.Height;

                            int[] map = new int[CurrentLevel.Instance.Width * CurrentLevel.Instance.Height];
                            for (int y = 0; y < CurrentLevel.Instance.Height; ++y)
                            {
                                for (int x = 0; x < CurrentLevel.Instance.Width; ++x)
                                {
                                    map[x + y * CurrentLevel.Instance.Width] = CurrentLevel.Instance.GetTile(x, y).ID;
                                }
                            }
                            saveMap.Layout = map;

                            SceneryTile[] scenery = new SceneryTile[CurrentLevel.Instance.Scenery.Count];
                            for (int i = 0; i < CurrentLevel.Instance.Scenery.Count; ++i)
                                scenery[i] = CurrentLevel.Instance.Scenery[i];
                            saveMap.Scenery = scenery;

                            EntityMarkerEnemy[] entityEnemyList = new EntityMarkerEnemy[CurrentLevel.Instance.Enemies.Count];
                            for (int i = 0; i < CurrentLevel.Instance.Enemies.Count; ++i)
                                entityEnemyList[i] = new EntityMarkerEnemy(CurrentLevel.Instance.Enemies[i].ActorType, WorldUtilities.WorldToTile(CurrentLevel.Instance.Enemies[i].GetPosition().Position), (CurrentLevel.Instance.Enemies[i] as Enemy).AIConfig);
                            saveMap.Enemies = entityEnemyList;

                            EntityMarker[] entityList = new EntityMarker[CurrentLevel.Instance.Monsters.Count];
                            for (int i = 0; i < CurrentLevel.Instance.Monsters.Count; ++i)
                                entityList[i] = new EntityMarker(CurrentLevel.Instance.Monsters[i].ActorType, WorldUtilities.WorldToTile(CurrentLevel.Instance.Monsters[i].GetPosition().Position));
                            saveMap.Monsters = entityList;

                            entityList = new EntityMarker[CurrentLevel.Instance.Items.Count];
                            for (int i = 0; i < CurrentLevel.Instance.Items.Count; ++i)
                                entityList[i] = new EntityMarker(CurrentLevel.Instance.Items[i].ActorType, WorldUtilities.WorldToTile(CurrentLevel.Instance.Items[i].GetPosition().Position));
                            saveMap.Items = entityList;

                            PuzzlePiece[] pieces = new PuzzlePiece[this.puzzlePieceTypeList.Count];
                            for (int i = 0; i < this.puzzlePieceTypeList.Count; ++i)
                            {
                                pieces[i] = new PuzzlePiece();
                                pieces[i].Type = this.puzzlePieceTypeList[i];
                                pieces[i].Rotation = this.puzzlePieceRotationList[i];
                            }
                            saveMap.PuzzlePieces = pieces;

                            WinphoneUtilities.SaveSerializeToXML<LevelMap>(saveMap, saveFileDialog.FileName);
                        }
                    }
                    //Load
                    else if (lastMousePos.X >= 145 && lastMousePos.Y >= 486 &&
                            lastMousePos.X <= 201 && lastMousePos.Y <= 510)
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                        openFileDialog.Filter = "XML File (*.xml)|*.xml|All Files (*.*)|*.*";
                        openFileDialog.FilterIndex = 1;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            LevelMap level = WinphoneUtilities.OpenDeserializeFromXML<LevelMap>(openFileDialog.FileName);
                            CurrentLevel.Instance.Initialize(level);

                            //this.puzzlePieceTypeList.Clear();
                            //this.puzzlePieceRotationList.Clear();
                            //PuzzlePiece[] pieces = level.PuzzlePieces;
                            //for (int i = 0; i < pieces.Length; ++i)
                            //{
                            //    PuzzlePiece piece = pieces[i];
                            //    this.puzzlePieceTypeList.Add(piece.Type);
                            //    this.puzzlePieceRotationList.Add(piece.Rotation);
                            //}
                        }
                    }

                    if (this.editMode == EditModeType.WORLD)
                    {
                        //World Arrow Down
                        if (lastMousePos.X >= 313 && lastMousePos.Y >= 484 &&
                                lastMousePos.X <= 345 && lastMousePos.Y <= 511)
                        {
                            --this.currentWorldType;
                            if (this.currentWorldType < 0)
                                this.currentWorldType = TerrainKey.WorldTypes.Count - 1;
                            CurrentLevel.Instance.ChangeWorldType(this.currentWorldType);
                        }
                        //World Arrow Up
                        else if (lastMousePos.X >= 350 && lastMousePos.Y >= 483 &&
                                lastMousePos.X <= 383 && lastMousePos.Y <= 514)
                        {
                            ++this.currentWorldType;
                            if (this.currentWorldType > TerrainKey.WorldTypes.Count - 1)
                                this.currentWorldType = 0;
                            CurrentLevel.Instance.ChangeWorldType(this.currentWorldType);
                        }
                        //Tile Arrow Down
                        else if (lastMousePos.X >= 486 && lastMousePos.Y >= 483 &&
                                lastMousePos.X <= 520 && lastMousePos.Y <= 512)
                        {
                            currentTileType -= 1 * scalar;
                            if (currentTileType < 1)
                                currentTileType = TerrainKey.TileTypes.Count - 1;
                        }
                        //Tile Arrow Up
                        else if (lastMousePos.X >= 524 && lastMousePos.Y >= 483 &&
                                lastMousePos.X <= 559 && lastMousePos.Y <= 512)
                        {
                            currentTileType += 1 * scalar;
                            if (currentTileType > TerrainKey.TileTypes.Count - 1)
                                currentTileType = 1;
                        }
                    }
                    else if (this.editMode == EditModeType.SCENERY)
                    {
                        //Arrow Down
                        if (lastMousePos.X >= 380 && lastMousePos.Y >= 484 &&
                                lastMousePos.X <= 412 && lastMousePos.Y <= 514)
                        {
                            currentSceneryType -= 1 * scalar;
                            if (currentSceneryType < 1)
                                currentSceneryType = TerrainKey.SceneryTypes.Count;
                            this.subeditMode = SubEditModeType.NONE;
                        }
                        //Arrow Up
                        else if (lastMousePos.X >= 420 && lastMousePos.Y >= 482 &&
                                lastMousePos.X <= 451 && lastMousePos.Y <= 512)
                        {
                            currentSceneryType += 1 * scalar;
                            if (currentSceneryType > TerrainKey.SceneryTypes.Count)
                                currentSceneryType = 1;
                            this.subeditMode = SubEditModeType.NONE;
                        }
                        //Scenery Portrait
                        else if (lastMousePos.X >= 459 && lastMousePos.Y >= 482 &&
                                lastMousePos.X <= 509 && lastMousePos.Y <= 517)
                        {
                            this.subeditMode = SubEditModeType.NONE;
                        }
                        //Obstacle
                        else if (lastMousePos.X >= 523 && lastMousePos.Y >= 483 &&
                                lastMousePos.X <= 562 && lastMousePos.Y <= 515)
                        {

                            this.subeditMode = SubEditModeType.OBSTACLE;
                        }

                    }
                    else if (this.editMode == EditModeType.MONSTERS)
                    {
                        //Monster Arrow Down
                        if (lastMousePos.X >= 380 && lastMousePos.Y >= 484 &&
                                lastMousePos.X <= 412 && lastMousePos.Y <= 514)
                        {
                            currentMonsterType -= 1 * scalar;
                            if (currentMonsterType < 0)
                                currentMonsterType = ActorKey.MonsterTypes.Count-1;
                            this.subeditMode = SubEditModeType.NONE;
                        }
                        //Monster Arrow Up
                        else if (lastMousePos.X >= 420 && lastMousePos.Y >= 482 &&
                                lastMousePos.X <= 451 && lastMousePos.Y <= 512)
                        {
                            currentMonsterType += 1 * scalar;
                            if (currentMonsterType > ActorKey.MonsterTypes.Count-1)
                                currentMonsterType = 0;
                            this.subeditMode = SubEditModeType.NONE;
                        }
                        //Start Point
                        else if (lastMousePos.X >= 545 && lastMousePos.Y >= 482 &&
                                lastMousePos.X <= 650 && lastMousePos.Y <= 512)
                        {
                            this.subeditMode = SubEditModeType.STARTPOINT;
                        }
                        //End Point
                        else if (lastMousePos.X >= 702 && lastMousePos.Y >= 482 &&
                                lastMousePos.X <= 800 && lastMousePos.Y <= 512)
                        {
                            this.subeditMode = SubEditModeType.ENDPOINT;
                        }
                    }
                    else if (this.editMode == EditModeType.ENEMIES)
                    {
                        //Arrow Down
                        if (lastMousePos.X >= 380 && lastMousePos.Y >= 484 &&
                                lastMousePos.X <= 412 && lastMousePos.Y <= 514)
                        {
                            currentEnemyType -= 1 * scalar;
                            if (currentEnemyType < 0)
                                currentEnemyType = ActorKey.EnemyTypes.Count - 1;

                            this.subeditMode = SubEditModeType.NONE;
                        }
                        //Arrow Up
                        else if (lastMousePos.X >= 420 && lastMousePos.Y >= 482 &&
                                lastMousePos.X <= 451 && lastMousePos.Y <= 512)
                        {
                            currentEnemyType += 1 * scalar;
                            if (currentEnemyType > ActorKey.EnemyTypes.Count-1)
                                currentEnemyType = 0;

                            this.subeditMode = SubEditModeType.NONE;
                        }
                        //UP
                        else if (lastMousePos.X >= 579 && lastMousePos.Y >= 481 &&
                                lastMousePos.X <= 616 && lastMousePos.Y <= 519)
                        {
                            this.subeditMode = SubEditModeType.AI_UP;
                        }
                        //RIGHT
                        else if (lastMousePos.X >= 618 && lastMousePos.Y >= 481 &&
                                lastMousePos.X <= 648 && lastMousePos.Y <= 519)
                        {
                            this.subeditMode = SubEditModeType.AI_RIGHT;
                        }
                        //DOWN
                        else if (lastMousePos.X >= 651 && lastMousePos.Y >= 481 &&
                                lastMousePos.X <= 685 && lastMousePos.Y <= 519)
                        {

                            this.subeditMode = SubEditModeType.AI_DOWN;
                        }
                        //LEFT
                        else if (lastMousePos.X >= 688 && lastMousePos.Y >= 481 &&
                                lastMousePos.X <= 720 && lastMousePos.Y <= 519)
                        {
                            this.subeditMode = SubEditModeType.AI_LEFT;
                        }
                        //AI TOGGLE
                        else if (lastMousePos.X >= 736 && lastMousePos.Y >= 487 &&
                                lastMousePos.X <= 780 && lastMousePos.Y <= 513)
                        {
                            this.drawAI= !this.drawAI;
                        }
                    }
                    else if (this.editMode == EditModeType.OTHER)
                    {
                        //Arrow Down
                        if (lastMousePos.X >= 380 && lastMousePos.Y >= 484 &&
                                lastMousePos.X <= 412 && lastMousePos.Y <= 514)
                        {
                            currentPowerUpType -= 1 * scalar;
                            if (currentPowerUpType < 0)
                                currentPowerUpType = ActorKey.PowerUpTypes.Count-1;
                        }
                        //Arrow Up
                        else if (lastMousePos.X >= 420 && lastMousePos.Y >= 482 &&
                                lastMousePos.X <= 451 && lastMousePos.Y <= 512)
                        {
                            currentPowerUpType += 1 * scalar;
                            if (currentPowerUpType > ActorKey.PowerUpTypes.Count-1)
                                currentPowerUpType = 0;
                        }
                    }
                }
            }
            //Over Selector Hud
            else if (lastMousePos.X <= 345 && lastMousePos.Y <= 47)
            {
                if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                }
                else if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Released && lastLeftState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Released;

                    //Grid
                    if (lastMousePos.X >= 6 && lastMousePos.Y >= 6 &&
                               lastMousePos.X <= 30 && lastMousePos.Y <= 47)
                    {
                        this.drawGrid = !this.drawGrid;
                    }
                    //World
                    else if (lastMousePos.X >= 36 && lastMousePos.Y >= 6 &&
                               lastMousePos.X <= 94 && lastMousePos.Y <= 47)
                    {
                        this.editMode = EditModeType.WORLD;
                        this.subeditMode = SubEditModeType.NONE;
                    }
                    //Scenery
                    else if (lastMousePos.X >= 105 && lastMousePos.Y >= 6 &&
                               lastMousePos.X <= 149 && lastMousePos.Y <= 47)
                    {
                        this.editMode = EditModeType.SCENERY;
                        this.subeditMode = SubEditModeType.NONE;
                    }
                    //Monster
                    else if (lastMousePos.X >= 156 && lastMousePos.Y >= 6 &&
                               lastMousePos.X <= 199 && lastMousePos.Y <= 47)
                    {
                        this.editMode = EditModeType.MONSTERS;
                        this.subeditMode = SubEditModeType.NONE;
                    }
                    //Enemies
                    else if (lastMousePos.X >= 204 && lastMousePos.Y >= 6 &&
                               lastMousePos.X <= 248 && lastMousePos.Y <= 47)
                    {
                        this.editMode = EditModeType.ENEMIES;
                        this.subeditMode = SubEditModeType.NONE;
                    }
                    //Other
                    else if (lastMousePos.X >= 253 && lastMousePos.Y >= 6 &&
                               lastMousePos.X <= 297 && lastMousePos.Y <= 47)
                    {
                        this.editMode = EditModeType.OTHER;
                        this.subeditMode = SubEditModeType.NONE;
                    }
                    //Puzzle
                    else if (lastMousePos.X >= 303 && lastMousePos.Y >= 6 &&
                               lastMousePos.X <= 345 && lastMousePos.Y <= 47)
                    {
                        this.editMode = EditModeType.PUZZLE;
                        this.subeditMode = SubEditModeType.NONE;
                    }
                }
            }
            else //In the World
            {
                Point currentTile = InputManager.Instance.CurrentTile;
                if (currentTile.X < 0 && currentTile.Y < 0)
                    return;

                if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                }
                else if (mouseButtons[(int)MouseButtonTypes.Left] == Microsoft.Xna.Framework.Input.ButtonState.Released && lastLeftState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastLeftState = Microsoft.Xna.Framework.Input.ButtonState.Released;
                    if (this.editMode == EditModeType.WORLD)
                    {
                        CurrentLevel.Instance.SetTile(currentTile, this.currentTileType);
                    }
                    else if (this.editMode == EditModeType.SCENERY)
                    {
                        if (this.subeditMode == SubEditModeType.NONE)
                        {
                            CurrentLevel.Instance.SetScenery(lastMousePos, currentTile, this.currentSceneryType);
                        }
                        else if (this.subeditMode == SubEditModeType.OBSTACLE)
                        {
                            CurrentLevel.Instance.SetObstacle(lastMousePos, currentTile, true);
                        }
                    }
                    else if (this.editMode == EditModeType.MONSTERS)
                    {
                        if (this.subeditMode == SubEditModeType.NONE)
                        {
                            CurrentLevel.Instance.SetMonsterType(lastMousePos, currentTile, this.currentMonsterType);
                        }
                        else if (this.subeditMode == SubEditModeType.STARTPOINT)
                        {
                            CurrentLevel.Instance.SetStartPoint(currentTile);
                        }
                        else if (this.subeditMode == SubEditModeType.ENDPOINT)
                        {
                            CurrentLevel.Instance.SetEndPoint(currentTile);
                        }
                    }
                    else if (this.editMode == EditModeType.ENEMIES)
                    {
                        if (this.subeditMode == SubEditModeType.NONE)
                        {
                            CurrentLevel.Instance.SetEnemyType(lastMousePos, currentTile, this.currentEnemyType);
                        }
                        else if (this.subeditMode == SubEditModeType.AI_LEFT)
                        {
                            CurrentLevel.Instance.SetEnemyAIDir(lastMousePos, currentTile, PathDirection.Left);
                        }
                        else if (this.subeditMode == SubEditModeType.AI_RIGHT)
                        {
                            CurrentLevel.Instance.SetEnemyAIDir(lastMousePos, currentTile, PathDirection.Right);
                        }
                        else if (this.subeditMode == SubEditModeType.AI_UP)
                        {
                            CurrentLevel.Instance.SetEnemyAIDir(lastMousePos, currentTile, PathDirection.Up);
                        }
                        else if (this.subeditMode == SubEditModeType.AI_DOWN)
                        {
                            CurrentLevel.Instance.SetEnemyAIDir(lastMousePos, currentTile, PathDirection.Down);
                        }
                    }
                    else if (this.editMode == EditModeType.OTHER)
                    {
                        CurrentLevel.Instance.SetPowerUpType(lastMousePos, currentTile, this.currentPowerUpType);
                    }
                }
                else if (mouseButtons[(int)MouseButtonTypes.Right] == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastRightState = Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                }
                else if (mouseButtons[(int)MouseButtonTypes.Right] == Microsoft.Xna.Framework.Input.ButtonState.Released && lastRightState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    lastRightState = Microsoft.Xna.Framework.Input.ButtonState.Released;
                    if (this.editMode == EditModeType.WORLD)
                    {
                        CurrentLevel.Instance.SetTile(currentTile, 0);
                    }
                    else if (this.editMode == EditModeType.SCENERY)
                    {
                        if (this.subeditMode == SubEditModeType.NONE)
                        {
                            CurrentLevel.Instance.SetScenery(lastMousePos, currentTile, 0);
                        }
                        else if (this.subeditMode == SubEditModeType.OBSTACLE)
                        {
                            CurrentLevel.Instance.SetObstacle(lastMousePos, currentTile, false);
                        }
                    }
                    else if (this.editMode == EditModeType.MONSTERS)
                    {
                        CurrentLevel.Instance.SetMonsterType(lastMousePos, currentTile, -1);
                    }
                    else if (this.editMode == EditModeType.ENEMIES)
                    {
                        CurrentLevel.Instance.SetEnemyType(lastMousePos, currentTile, -1);
                    }
                    else if (this.editMode == EditModeType.OTHER)
                    {
                        CurrentLevel.Instance.SetPowerUpType(lastMousePos, currentTile, -1);
                    }
                }
            }
        }
        #endregion
    }
}
