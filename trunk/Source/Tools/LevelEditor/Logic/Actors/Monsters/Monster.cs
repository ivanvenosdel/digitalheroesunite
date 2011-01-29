#region Using Statements
using System;

using Microsoft.Xna.Framework;

using MonsterEscape.Graphics.Animations;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Enemies;
using MonsterEscape.Logic.Actors.Misc;
using MonsterEscape.Logic.ClassComponents;
using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Logic.Actors.Monsters
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010 ~ 9.27.2010
    /// Description: Abstract Monster Actor
    /// </summary>
    public abstract class Monster : Actor
    {
        protected bool carryingEgg;

        #region Constructor
        public Monster(Guid id, ActorType type)
            : base(id, type)
        {

        }

        public virtual void onCollision(Enemy enemy)
        {
            //Nothing
        }

        public virtual void onCollision(SpitActor spit)
        {
            //Nothing
        }

        public abstract void PlayAnimation(int anim, bool repeat);
        public abstract void QueueAnimation(int anim, bool repeat);
        #endregion
    }
}
