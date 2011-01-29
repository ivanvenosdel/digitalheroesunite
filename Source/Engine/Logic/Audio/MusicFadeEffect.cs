#region Using Statments
using System;

using Microsoft.Xna.Framework;
#endregion

namespace Engine.Logic.Audio
{
    public class MusicFadeEffect
    {
        #region Fields
        public float SourceVolume;
        public float TargetVolume;

        private TimeSpan time;
        private TimeSpan duration;
        #endregion

        #region Constructor
        public MusicFadeEffect(float sourceVolume, float targetVolume, TimeSpan duration)
        {
            SourceVolume = sourceVolume;
            TargetVolume = targetVolume;
            this.time = TimeSpan.Zero;
            this.duration = duration;
        }
        #endregion

        #region Public Methods
        public bool Update(TimeSpan time)
        {
            this.time += time;

            if (this.time >= this.duration)
            {
                this.time = this.duration;
                return true;
            }

            return false;
        }

        public float GetVolume()
        {
            return MathHelper.Lerp(SourceVolume, TargetVolume, (float)this.time.Ticks / this.duration.Ticks);
        }
        #endregion
    }
}
