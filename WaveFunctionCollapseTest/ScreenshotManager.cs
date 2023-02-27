using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WaveFunctionCollapseTest
{
    static class ScreenshotManager
    {
        public static int currentMap = 0;

        public static void SetCurrentMap()
        {
            for(int i = 0; i < int.MaxValue - 1; i++)
            {
                if(!File.Exists(i.ToString() + ".jpg"))
                {
                    currentMap = i;
                    break;
                }
            }
        }

        static void TakeScreenshot(Game1 game)
        {
            /*
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {

                int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
                int h = GraphicsDevice.PresentationParameters.BackBufferHeight;

                //force a frame to be drawn (otherwise back buffer is empty) 
                Draw(new GameTime());

                //pull the picture from the buffer 
                int[] backBuffer = new int[w * h];
                GraphicsDevice.GetBackBufferData(backBuffer);

                //copy into a texture 
                Texture2D texture = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
                texture.SetData(backBuffer);

                //save to disk 
                Stream stream = File.OpenWrite((currentMap + 1).ToString() + ".jpg");

                texture.SaveAsJpeg(stream, w, h);
                stream.Dispose();

                texture.Dispose();
            }
            */
        }
    }
}
