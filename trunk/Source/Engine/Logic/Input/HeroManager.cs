#region Using Statements
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Engine.Graphics.Animations;
using Engine.World;
#endregion

namespace Engine.Logic.Input
{
    enum MoveDirection
    {
        Left,
        Right,
        None
    }

    public sealed class Heromanager
    {
        #region Fields
        private static readonly Heromanager instance = new Heromanager();

        private const float WALK_RATE = 6.0f;
        private const float SLOW_FALL_RATE = WALK_RATE / 1.5f;
        public const float JUMP_RATE = 15.0f; 
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        private Heromanager() 
        {
        }
        #endregion

        #region Properties
        public static Heromanager Instance { get { return instance; } }
        #endregion

        #region Fields
        private MoveDirection jumpDirection = MoveDirection.None;
        private MoveDirection walkDirection = MoveDirection.None;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes the Input Manager
        /// </summary>
        public void Initialize()
        {
            InputManager.Instance.OnKeyEvent += new KeyEvent(this.OnKeyEvent);
        }
        #endregion

        #region Event Methods
        private void OnKeyEvent(KeyboardState keyboardState)
        {
            if (GameWorld.Instance.Hero != null)
            {
                float dX = 0;

                //Translate Right
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    if (GameWorld.Instance.hero.Jumping)
                    {
                        dX = SLOW_FALL_RATE;
                        GameWorld.Instance.hero.PlayAnimation(AnimPackageHero.JUMP_RIGHT, true);
                    }
                    else
                    {
                        dX = WALK_RATE;
                        if (GameWorld.Instance.hero.GetSprite().CurrentAnimation != AnimPackageHero.RUN_RIGHT)
                            GameWorld.Instance.hero.PlayAnimation(AnimPackageHero.RUN_RIGHT, true);
                    }
                }
                //Translate Left
                else if (keyboardState.IsKeyDown(Keys.Left))
                {
                    if (GameWorld.Instance.hero.Jumping)
                    {
                        dX = -SLOW_FALL_RATE;
                        GameWorld.Instance.hero.PlayAnimation(AnimPackageHero.JUMP_LEFT, true);
                    }
                    else
                    {
                        dX = -WALK_RATE;
                        if (GameWorld.Instance.hero.GetSprite().CurrentAnimation != AnimPackageHero.RUN_LEFT)
                            GameWorld.Instance.hero.PlayAnimation(AnimPackageHero.RUN_LEFT, true);
                    }
                }
                else
                {
                    if (GameWorld.Instance.hero.GetSprite().CurrentAnimation != AnimPackageHero.STAND)
                        GameWorld.Instance.hero.PlayAnimation(AnimPackageHero.STAND, true);
                    this.walkDirection = MoveDirection.None;
                }

                //Translate RightControl
                if (keyboardState.IsKeyDown(Keys.Z))
                {
                    if (!GameWorld.Instance.hero.Jumping)
                    {
                        this.jumpDirection = this.walkDirection;
                        GameWorld.Instance.hero.BeginJump(WorldTile.TILE_SIZE * 5);
                    }
                }

                //Translate Whip Attack
                /*
                if (keyboardState.IsKeyDown(Keys.X)
                {
                    if(GameWorld.Instance.hero.whipAttack)
                    {
                        this.whipAttackDirection = this.walkDirection;
                        GameWorld.Instance.hero.BeginAttack(WordTile.TILE_SIZE * 2);
                }
                 */

                GameWorld.Instance.Hero.Walk(dX);
            }
        }
        #endregion
    }
}