#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Logic.Input;
#endregion

namespace Engine.Graphics.UI.Screens
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 12.13.2010
    /// Description: UI Screen Base
    /// </summary>
    public abstract class Screen
    {
        #region Fields
        protected ContentManager content;
        protected Keys lastKey;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public Screen() { }
        #endregion

        #region Public Methods
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, float masterAlpha);
        public abstract void OnMouseEvent(MouseState current, MouseState previous, float scrollDelta);
        public abstract void OnKeyEvent(KeyboardState keyboardState);

        public virtual void Initialize()
        {
            this.content = new ContentManager(DeviceManager.Instance.Content.ServiceProvider, "Content\\UI\\");
        }

        public void UnloadContent()
        {
            this.content.Unload();
        }
        #endregion
    }
}
