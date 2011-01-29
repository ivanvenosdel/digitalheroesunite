#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Graphics.Cameras;
using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Graphics.Backgrounds
{
    /// <summary>
    /// Background for the forest worlds
    /// </summary>
    public class ForestBackground : Background
    {
        /// <summary> The the vertical space between waves </summary>
        private const int VERTICAL_SPACING = 80;
        /// <summary> The y starting position of the repeating chunk </summary>
        private const int VERTICAL_START = -300;
        /// <summary> The height of the repeating chunk of background </summary>
        private const int VERTICAL_REPEAT = 350;

        private int widthIterations = 1;
        private int heightIterations = 1;

        private Texture2D[] bubbles = new Texture2D[5];

        private Texture2D wave1;
        private Texture2D wave2;

        private float[] bubblesScale = new float[5];
        private bool[] bubbleWaining = new bool[5];
        private Vector2[] bubblesPos = new Vector2[5];
        private Vector2[] bubbleOrigin = new Vector2[5];

        private Vector2[] wavePos = new Vector2[6];
        private Vector2[] waveOffset = new Vector2[6];
        private bool[] waveWainingX = new bool[6];
        private bool[] waveWainingY = new bool[6];
        private int[] waveCycleCount = new int[6];

        public ForestBackground() 
            : base()
        {
            Base = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Levels\Backgrounds\Forest\base");
            bubbles[0] = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Levels\Backgrounds\Forest\bubbles1");
            bubbles[1] = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Levels\Backgrounds\Forest\bubbles2");
            bubbles[2] = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Levels\Backgrounds\Forest\bubbles3");
            bubbles[3] = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Levels\Backgrounds\Forest\bubbles4");
            bubbles[4] = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Levels\Backgrounds\Forest\bubbles5");
            wave1 = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Levels\Backgrounds\Forest\wave1");
            wave2 = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Levels\Backgrounds\Forest\wave2");

            bubblesPos[0] = new Vector2(DeviceManager.SCREEN_WIDTH - 150, DeviceManager.SCREEN_HEIGHT - 150);
            bubblesPos[1] = new Vector2(DeviceManager.SCREEN_WIDTH - 220, DeviceManager.SCREEN_HEIGHT - 350);
            bubblesPos[2] = new Vector2(DeviceManager.SCREEN_WIDTH - 450, DeviceManager.SCREEN_HEIGHT - 200);
            bubblesPos[3] = new Vector2(DeviceManager.SCREEN_WIDTH - 500, DeviceManager.SCREEN_HEIGHT - 110);
            bubblesPos[4] = new Vector2(150, 200);

            bubbleOrigin[0] = new Vector2(bubbles[0].Width / 2, bubbles[0].Height / 2);
            bubbleOrigin[1] = new Vector2(bubbles[1].Width / 2, bubbles[1].Height / 2);
            bubbleOrigin[2] = new Vector2(bubbles[2].Width / 2, bubbles[2].Height / 2);
            bubbleOrigin[3] = new Vector2(bubbles[3].Width / 2, bubbles[3].Height / 2);
            bubbleOrigin[4] = new Vector2(bubbles[4].Width / 2, bubbles[4].Height / 2);

            wavePos[0] = new Vector2(-wave1.Width / 2, 0 * VERTICAL_SPACING + VERTICAL_START);
            wavePos[1] = new Vector2(-wave1.Width / 2, 1 * VERTICAL_SPACING + VERTICAL_START);
            wavePos[2] = new Vector2(-wave1.Width / 2, 3 * VERTICAL_SPACING + VERTICAL_START);
            wavePos[3] = new Vector2(-wave2.Width / 2, 2 * VERTICAL_SPACING + VERTICAL_START);
            wavePos[4] = new Vector2(-wave2.Width / 2, 4 * VERTICAL_SPACING + VERTICAL_START);
            wavePos[5] = new Vector2(-wave2.Width / 2, 5 * VERTICAL_SPACING + VERTICAL_START);

            waveWainingX[2] = true;
            waveWainingX[4] = true;
            waveWainingX[5] = true;
        }

        public override void Initialize()
        {
            for (int i = 0; i < bubblesScale.Length; ++i)
            {
                bubblesScale[i] = WinphoneMath.RandomFloat(1.0f, 1.2f);
            }

            for (int i = 0; i < waveOffset.Length; ++i)
            {
                waveOffset[i] = new Vector2(WinphoneMath.RandomFloat(-120.0f, 120.0f), 0);
            }

            //Determine how big our world is, and thus how big our background needs to be
            base.Initialize();

            //Divide our fullevel rectangle (the full level plus buffer space) by the smallest repeating object
            widthIterations = FullLevelRectangle.Width / (wave1.Width / 2);
            widthIterations = widthIterations <= 0 ? 1 : widthIterations;
            heightIterations = FullLevelRectangle.Height / (wave1.Height + VERTICAL_SPACING + VERTICAL_REPEAT + VERTICAL_START);
            heightIterations = heightIterations <= 0 ? 1 : heightIterations;
        }

        public override void Update(GameTime gameTime)
        {
            //Update Bubbles
            float bubbleModifier = gameTime.ElapsedGameTime.Milliseconds / 10000.0f;
            for (int i = 0; i < bubblesScale.Length; ++i)
            {
                bubblesScale[i] += bubbleWaining[i] ? -bubbleModifier : bubbleModifier;

                if (bubblesScale[i] >= 1.2f)
                {
                    bubblesScale[i] = 1.2f;
                    bubbleWaining[i] = true;
                }
                else if (bubblesScale[i] <= 1.0f)
                {
                    bubblesScale[i] = 1.0f;
                    bubbleWaining[i] = false;
                }
            }

            //Update Wave
            float waveModifierX = gameTime.ElapsedGameTime.Milliseconds / 16.0f;
            float waveModifierY = gameTime.ElapsedGameTime.Milliseconds / 55.0f;

            for (int i = 0; i < 3; ++i)
            {
                waveOffset[i].X += waveWainingX[i] ? -waveModifierX : waveModifierX;
                waveOffset[i].Y += waveWainingY[i] ? -waveModifierY : waveModifierY;

                if (waveOffset[i].Y >= 5)
                {
                    waveOffset[i].Y = 5;
                    waveWainingY[i] = true;
                }
                else if (waveOffset[i].Y <= -8)
                {
                    ++waveCycleCount[i];
                    waveOffset[i].Y = -8;
                    waveWainingY[i] = false;
                    if (waveCycleCount[i] >= 3)
                    {
                        waveCycleCount[i] = 0;
                        waveWainingX[i] = !waveWainingX[i];
                    }
                }
            }

            for (int i = 3; i < 6; ++i)
            {
                waveOffset[i].X += waveWainingX[i] ? -waveModifierX : waveModifierX;
                waveOffset[i].Y += waveWainingY[i] ? -waveModifierY : waveModifierY;

                if (waveOffset[i].Y >= 10)
                {
                    waveOffset[i].Y = 10;
                    waveWainingY[i] = true;
                }
                else if (waveOffset[i].Y <= -5)
                {
                    ++waveCycleCount[i];
                    waveOffset[i].Y = -5;
                    waveWainingY[i] = false;
                    if (waveCycleCount[i] >= 3)
                    {
                        waveCycleCount[i] = 0;
                        waveWainingX[i] = !waveWainingX[i];
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Base
            spriteBatch.Draw(Base, FullLevelRectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);

            //Waves
            for (int h = 0; h < heightIterations; ++h)
            {
                for (int w = 0; w < widthIterations; ++w)
                {
                    Vector2 iterationPosition = new Vector2(wave2.Width * w, (wave2.Height + VERTICAL_SPACING + VERTICAL_REPEAT) * h);
                    for (int i = 0; i < 3; ++i)
                    {
                        spriteBatch.Draw(wave1, iterationPosition + wavePos[i] + waveOffset[i], null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    }
                }
            }

            for (int h = 0; h < heightIterations; ++h)
            {
                for (int w = 0; w < widthIterations; ++w)
                {
                    Vector2 iterationPosition = new Vector2(wave2.Width * w, (wave2.Height + VERTICAL_SPACING + VERTICAL_REPEAT) * h);
                    for (int i = 3; i < 6; ++i)
                    {
                        spriteBatch.Draw(wave2, iterationPosition + wavePos[i] + waveOffset[i], null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    }
                }
            }

            //Bubbles
            for (int h = 0; h < heightIterations; ++h)
            {
                for (int w = 0; w < widthIterations; ++w)
                {
                    Vector2 iterationPosition = new Vector2(wave2.Width * w, (wave2.Height + VERTICAL_SPACING + VERTICAL_REPEAT) * h);
                    for (int i = 0; i < bubblesScale.Length; ++i)
                    {
                        spriteBatch.Draw(bubbles[i], iterationPosition + bubblesPos[i], null, Color.White, 0.0f, bubbleOrigin[i], bubblesScale[i], SpriteEffects.None, 1.0f);
                    }
                }
            }
        }
    }
}
