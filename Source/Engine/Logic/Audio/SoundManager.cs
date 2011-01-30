#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
#endregion

namespace Engine.Logic.Audio
{
    #region Enums
    /// <summary>
    /// CancelFade
    /// </summary>
    public enum FadeCancelOptions
    {
        /// <summary>
        /// Return to pre-fade volume
        /// </summary>
        Source,
        /// <summary>
        /// Snap to fade target volume
        /// </summary>
        Target,
        /// <summary>
        /// Keep current volume
        /// </summary>
        Current
    }
    #endregion

    /// <summary>
    /// Manages playback of sounds and music.
    /// </summary>
    public sealed class SoundManager 
    {
        #region Fields
        private static readonly SoundManager instance = new SoundManager();
        
        private const int MAX_SOUNDS = 32;

        private ContentManager content;

        private Dictionary<string, Song> songs = new Dictionary<string, Song>();
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        private Song currentSong = null;
        private SoundEffectInstance[] playingSounds = new SoundEffectInstance[MAX_SOUNDS];

        private bool isMusicPaused = false;

        private bool isFading = false;
        private MusicFadeEffect fadeEffect;
        #endregion

        #region Properties
        /// <summary> Instance </summary>
        public static SoundManager Instance { get { return instance; } }
        /// <summary> Enable Sounds</summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets the name of the currently playing song, or null if no song is playing.
        /// </summary>
        public string CurrentSong { get; private set; }
        /// <summary>
        /// Gets or sets the volume to play songs. 1.0f is max volume.
        /// </summary>
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }
        /// <summary>
        /// Gets or sets the master volume for all sounds. 1.0f is max volume.
        /// </summary>
        public float SoundVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }
        /// <summary>
        /// Gets whether a song is playing or paused (i.e. not stopped).
        /// </summary>
        public bool IsSongActive { get { return this.currentSong != null && MediaPlayer.State != MediaState.Stopped; } }
        /// <summary>
        /// Gets whether the current song is paused.
        /// </summary>
        public bool IsSongPaused { get { return this.currentSong != null && this.isMusicPaused; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new Sound Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        public SoundManager()
        {
            Enabled = true;
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            this.content = new ContentManager(DeviceManager.Instance.Content.ServiceProvider, 
                                                DeviceManager.Instance.Content.RootDirectory + "\\Audio");


            LoadSound("Sound/Bonus");
            LoadSound("Sound/DoubleRainbow");
            LoadSound("Sound/Jump");
            LoadSound("Sound/PlayerDeath");
            LoadSound("Sound/Blunt");
        }

        /// <summary>
        /// Loads a Song into the SoundManager.
        /// </summary>
        /// <param name="songName">Name of the song to load</param>
        public void LoadSong(string songName)
        {
            LoadSong(songName, songName);
        }

        /// <summary>
        /// Loads a Song into the SoundManager.
        /// </summary>
        /// <param name="songName">Name of the song to load</param>
        /// <param name="songPath">Path to the song asset file</param>
        public void LoadSong(string songName, string songPath)
        {
            if (this.songs.ContainsKey(songName))
            {
                throw new InvalidOperationException(string.Format("Song '{0}' has already been loaded", songName));
            }

            this.songs.Add(songName, this.content.Load<Song>(songPath));
        }

        /// <summary>
        /// Loads a SoundEffect into the SoundManager.
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        public void LoadSound(string soundName)
        {
            LoadSound(soundName, soundName);
        }

        /// <summary>
        /// Loads a SoundEffect into the SoundManager.
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        /// <param name="soundPath">Path to the song asset file</param>
        public void LoadSound(string soundName, string soundPath)
        {
            if (this.sounds.ContainsKey(soundName))
            {
                throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
            }

           this.sounds.Add(soundName, this.content.Load<SoundEffect>(soundPath));
        }

        /// <summary>
        /// Unloads all loaded songs and sounds.
        /// </summary>
        public void UnloadContent()
        {
            this.content.Unload();
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        public void PlaySong(string songName)
        {
            PlaySong(songName, false);
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">True if song should loop, false otherwise</param>
        public void PlaySong(string songName, bool loop)
        {
            if (CurrentSong != songName)
            {
                if (this.currentSong != null)
                {
                    MediaPlayer.Stop();
                }

                if (!this.songs.TryGetValue(songName, out this.currentSong))
                {
                    throw new ArgumentException(string.Format("Song '{0}' not found", songName));
                }

                CurrentSong = songName;

                this.isMusicPaused = false;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(this.currentSong);

                if (!Enabled)
                {
                    MediaPlayer.Pause();
                }
            }
        }

        /// <summary>
        /// Pauses the currently playing song. This is a no-op if the song is already paused,
        /// or if no song is currently playing.
        /// </summary>
        public void PauseSong()
        {
            if (this.currentSong != null && !this.isMusicPaused)
            {
                if (Enabled) MediaPlayer.Pause();
                this.isMusicPaused = true;
            }
        }

        /// <summary>
        /// Resumes the currently paused song. This is a no-op if the song is not paused,
        /// or if no song is currently playing.
        /// </summary>
        public void ResumeSong()
        {
            if (this.currentSong != null && this.isMusicPaused)
            {
                if (Enabled) MediaPlayer.Resume();
                this.isMusicPaused = false;
            }
        }

        /// <summary>
        /// Stops the currently playing song. This is a no-op if no song is currently playing.
        /// </summary>
        public void StopSong()
        {
            if (this.currentSong != null && MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                this.isMusicPaused = false;
            }
        }

        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public void FadeSong(float targetVolume, TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Duration must be a positive value");
            }

            this.fadeEffect = new MusicFadeEffect(MediaPlayer.Volume, targetVolume, duration);
            this.isFading = true;
        }

        /// <summary>
        /// Stop the current fade.
        /// </summary>
        /// <param name="option">Options for setting the music volume</param>
        public void CancelFade(FadeCancelOptions option)
        {
            if (this.isFading)
            {
                switch (option)
                {
                    case FadeCancelOptions.Source: MediaPlayer.Volume = this.fadeEffect.SourceVolume; break;
                    case FadeCancelOptions.Target: MediaPlayer.Volume = this.fadeEffect.TargetVolume; break;
                }

                this.isFading = false;
            }
        }

        /// <summary>
        /// Plays the sound of the given name.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        public void PlaySound(string soundName)
        {
            PlaySound(soundName, 1.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Plays the sound of the given name at the given volume.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        public void PlaySound(string soundName, float volume)
        {
            PlaySound(soundName, volume, 0.0f, 0.0f);
        }

        /// <summary>
        /// Plays the sound of the given name with the given parameters.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        public void PlaySound(string soundName, float volume, float pitch, float pan)
        {
            SoundEffect sound;

            if (!this.sounds.TryGetValue(soundName, out sound))
            {
                throw new ArgumentException(string.Format("Sound '{0}' not found", soundName));
            }

            int index = GetAvailableSoundIndex();

            if (index != -1)
            {
                this.playingSounds[index] = sound.CreateInstance();
                this.playingSounds[index].Volume = volume;
                this.playingSounds[index].Pitch = pitch;
                this.playingSounds[index].Pan = pan;
                
                if (Enabled)
                    this.playingSounds[index].Play();
                else
                    this.playingSounds[index].Pause();
            }
        }

        /// <summary>
        /// Stops currently playing sounds and song
        /// </summary>
        public void StopAllAudio()
        {
            StopSong();
            StopSounds();
        }

        /// <summary>
        /// Stops currently playing sounds.
        /// </summary>
        public void StopSounds()
        {
            for (int i = 0; i < this.playingSounds.Length; ++i)
            {
                if (this.playingSounds[i] != null)
                {
                    this.playingSounds[i].Stop();
                    this.playingSounds[i].Dispose();
                    this.playingSounds[i] = null;
                }
            }
        }

        /// <summary>
        /// Called per loop unless Enabled is set to false.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame</param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.playingSounds.Length; ++i)
            {
                if (this.playingSounds[i] != null && this.playingSounds[i].State == SoundState.Stopped)
                {
                    this.playingSounds[i].Dispose();
                    this.playingSounds[i] = null;
                }
            }

            if (this.currentSong != null && MediaPlayer.State == MediaState.Stopped)
            {
                this.currentSong = null;
                CurrentSong = null;
                this.isMusicPaused = false;
            }

            if (this.isFading && !this.isMusicPaused)
            {
                if (this.currentSong != null && MediaPlayer.State == MediaState.Playing)
                {
                    if (this.fadeEffect.Update(gameTime.ElapsedGameTime))
                    {
                        this.isFading = false;
                        //Kill songs that fade to nothing
                        if (this.fadeEffect.TargetVolume == 0.0f)
                        {
                            StopSong();
                        }
                    }

                    MediaPlayer.Volume = this.fadeEffect.GetVolume();
                }
                else
                {
                    this.isFading = false;
                }
            }
        }

        // Pauses all music and sound if disabled, resumes if enabled.
        public void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
            {
                for (int i = 0; i < this.playingSounds.Length; ++i)
                {
                    if (this.playingSounds[i] != null && this.playingSounds[i].State == SoundState.Paused)
                    {
                        this.playingSounds[i].Resume();
                    }
                }

                if (!this.isMusicPaused)
                {
                    MediaPlayer.Resume();
                }
            }
            else
            {
                for (int i = 0; i < this.playingSounds.Length; ++i)
                {
                    if (this.playingSounds[i] != null && this.playingSounds[i].State == SoundState.Playing)
                    {
                        this.playingSounds[i].Pause();
                    }
                }

                MediaPlayer.Pause();
            }
        }
        #endregion

        #region Private Methods
        // Acquires an open sound slot.
        private int GetAvailableSoundIndex()
        {
            for (int i = 0; i < this.playingSounds.Length; ++i)
            {
                if (this.playingSounds[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion
    }
}
