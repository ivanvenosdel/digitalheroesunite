using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Engine;
namespace GameStateManagement
{
    /// <summary>
    /// The credits menu screen.
    /// </summary>
    class CreditsScreen : MenuScreen
    {
        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public CreditsScreen()
            : base("Credits")
        { 
            MenuEntry backMenuEntry = new MenuEntry("Main Menu");
            MenuEntries.Add(backMenuEntry);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 viewportSize = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            Color color = Color.White * TransitionAlpha;
            Vector2 origin = (viewportSize) / 2;
            origin.Y = 28;
            Vector2 line1 = new Vector2(360, origin.Y);
            Vector2 line2 = new Vector2(190, origin.Y + 80);
            Vector2 line3 = new Vector2(190, origin.Y + 120);
            Vector2 line4 = new Vector2(190, origin.Y + 160);
            Vector2 line5 = new Vector2(190, origin.Y + 200);
            Vector2 line6 = new Vector2(190, origin.Y + 240);
            Vector2 line7 = new Vector2(190, origin.Y + 280);
            Vector2 line8 = new Vector2(190, origin.Y + 320);
            Vector2 line9 = new Vector2(190, origin.Y + 360);
            Vector2 line10 = new Vector2(190, origin.Y + 400);
            Vector2 line11 = new Vector2(190, origin.Y + 440);
            Vector2 line12 = new Vector2(190, origin.Y + 480);
            Vector2 line13 = new Vector2(190, origin.Y + 520);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font2, "Credits", line1, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Jim Howard        -Lead Designer", line2, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "James Kirk        -Lead Programmer", line3, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Richie Fieber     -Lead Artist", line4, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Adam Acuna       -Artist", line5, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Angela Clark      -Artist", line6, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Daryl Bunker      -Artist", line7, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Ivan Ven Osdel   -Programmer", line8, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Jeramy De Vos    -Programmer", line9, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Scott Jonker      -Programmer", line10, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Phillip Nitecki    -Programmer", line11, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Brady Hansen      -Miscellaneous", line12, color);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Mitch Mohror      -Miscellaneous", line13, color);

            ScreenManager.SpriteBatch.End();
        }

    }
}
/*
public CreditsScreen(string message, bool includeUsageText)
        {
            const string usageText = "Credits";
            
            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }
}*/