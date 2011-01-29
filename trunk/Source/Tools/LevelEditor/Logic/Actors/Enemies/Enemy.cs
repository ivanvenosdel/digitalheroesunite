#region Using Statements
using System;

using Microsoft.Xna.Framework;

using MonsterEscape.Graphics.Animations;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Monsters;
using MonsterEscape.Logic.AI;
using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Logic.Actors.Enemies
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010 ~ 9.27.2010
    /// Description: Abstract Enemy Actor
    /// </summary>
    public abstract class Enemy : Actor
    {
        protected MonsterPaunch beingEatenByMonster = null;

        #region Properties
        public MonsterPaunch BeingEatenByMonster { get { return this.beingEatenByMonster; } set { this.beingEatenByMonster = value; } }
        public AIConfiguration AIConfig { get; set; }
        #endregion

        #region Constructor
        public Enemy(Guid id, ActorType type)
            : base(id, type)
        {

        }

        public override void Update(GameTime gameTime)
        {
            //Nothing
        }

        public void Initialize(AIConfiguration aiConfig)
        {
            this.AIConfig = aiConfig;
        }

        public virtual void onCollision(Monster monster)
        {
            //Nothing
        }

        public abstract void PlayAnimation(int anim, bool repeat);
        public abstract void QueueAnimation(int anim, bool repeat);
        #endregion
    }
}
