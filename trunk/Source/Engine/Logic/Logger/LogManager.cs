#region Using Statements
using System;
using System.Runtime.InteropServices; //DllImport

using Microsoft.Xna.Framework;
#endregion

namespace Engine.Logic.Logger
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 5.6.2007
    /// Description: The Log File Manager
    /// </summary>
    public sealed class LogManager
    {
        #region Fields
        private static readonly LogManager instance = new LogManager();
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static LogManager Instance { get { return instance; } }
        #endregion

        //Import Windows MessageBox, but only works on PC, not XBOX
#if !XBOX
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint MessageBox(IntPtr hWnd, String text, String title, uint type);
#endif

        #region Constructors
        private LogManager() { }
        #endregion

        #region Public Methods
        /// <summary>
        /// A prompt for errors/warnings
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="title">Title of Window</param>
        /// <param name="type">Type of prompt</param>
        public void Alert(String text, String title, uint type)
        {
#if !XBOX
            MessageBox(new IntPtr(0), text, title, type);
#else
            //TODO: handle XBOX error prompts (if we even want this on XBOX)
#endif
            WriteLog(text + " -> " + title, null);
        }

        /// <summary>
        /// A prompt for errors/warnings
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="title">Title of Window</param>
        /// <param name="type">Type of prompt</param>
        /// <param name="e">An attached exception</param>
        public void Alert(String text, String title, uint type, Exception e)
        {
#if !XBOX
            MessageBox(new IntPtr(0), text, title, type);
#else
            //TODO: handle XBOX error prompts (if we even want this on XBOX)
#endif
            WriteLog(text + " -> " + title, e);
        }

        /// <summary>
        /// Writes text to our main log
        /// </summary>
        /// <param name="text">Text to record</param>
        /// <param name="e">An attached exception</param>
        public void WriteLog(String text, Exception e)
        {
            DateTime date = DateTime.Now;
            bool fileExists = true;

            //Write an file description
            if (!System.IO.File.Exists("Log.txt"))
                fileExists = false;

            System.IO.FileInfo m_LogFile = new System.IO.FileInfo("log.txt");
            System.IO.TextWriter Tex = m_LogFile.AppendText();

            if (!fileExists)
            {
                Tex.WriteLine("This File is used for Debugging Purposes.  " +
                    "If you encounter errors continuously, this file may lend some insight into the problem.");

                Tex.WriteLine("-----------------------------------------------------------------------------");
                Tex.Write(Tex.NewLine);
            }
            Tex.WriteLine(date.ToShortDateString() + " @ " + date.ToShortTimeString() + " | " + text);
            if (e != null)
                Tex.WriteLine("-> " + e.ToString());
            Tex.Close();
        }
        #endregion
    }
}
