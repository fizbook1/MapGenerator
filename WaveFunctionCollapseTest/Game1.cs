using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace WaveFunctionCollapseTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        Map map = new Map();
        public static Texture2D texture;
        int screenShotCounter = 0;

        private bool generationStarted = false;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 1100;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("pixel");
            map.Clear();

            _graphics.PreferredBackBufferWidth = Map.width * 4;
            _graphics.PreferredBackBufferHeight = Map.height * 4;
            _graphics.ApplyChanges();

            ScreenshotManager.SetCurrentMap();
            //map.NewGenerate();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    map.Clear();
            

            if(Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                generationStarted = true;
                //map.NewGenerate();
                map.RandomGenerate();
            }
            bool bingus = map.IsGenerationFinished();
            if(generationStarted && !bingus)
            {
                //map.NewGenerate();
                map.RandomGenerate();
            }

            if(bingus)
            {
                generationStarted = false;

                int w = Map.width * 4;
                int h = Map.height * 4 ;

                //force a frame to be drawn (otherwise back buffer is empty) 
                Draw(new GameTime());

                //pull the picture from the buffer 
                int[] backBuffer = new int[w * h];
                GraphicsDevice.GetBackBufferData(backBuffer);

                //copy into a texture 
                Texture2D texture = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
                texture.SetData(backBuffer);

                //save to disk 
                Stream stream = File.OpenWrite(ScreenshotManager.currentMap.ToString() + ".jpg");

                texture.SaveAsJpeg(stream, w, h);
                stream.Dispose();

                texture.Dispose();

                ScreenshotManager.SetCurrentMap();
                map.Clear();
                map.RandomGenerate();
                generationStarted = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);
            spriteBatch.Begin(SpriteSortMode.Deferred);
            map.Draw(spriteBatch);
            // TODO: Add your drawing code here

            
            
                
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
