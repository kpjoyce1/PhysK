using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PhysK
{ 
    public class GameApplication : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        World world;
        public static SpriteFont font;
        int frames;
        int fps;
        TimeSpan fpsTimer;

        public GameApplication()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            world = new World(GraphicsDevice);

            font = Content.Load<SpriteFont>("spritefont");

            Random gen = new Random();

            int TaylorNumber = 400;
            //10000 seems to be the limit of just drawing and bouncing off the screen
            //so the upper limit for collision checking is 10000
            PhysicsSprite[] Taylors = new PhysicsSprite[TaylorNumber];
            for (int i = 0; i < TaylorNumber; i++)
            {

                Taylors[i] = new PhysicsSprite(Content.Load<Texture2D>(string.Format("circle{0}",gen.Next(1, 5))), 
                    new Vector2(gen.Next(0, GraphicsDevice.Viewport.Width - 50), gen.Next(0, GraphicsDevice.Viewport.Width - 50)),
                    Color.White, new Vector2(gen.Next(-4, 4), gen.Next(-4, 4)),
                     gen.Next(21, 21), 1f);
            }
            world.Items = Taylors;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            fpsTimer += gameTime.ElapsedGameTime;
            if(fpsTimer > TimeSpan.FromSeconds(1))
            {
                fps = frames;
                frames = 0;
                fpsTimer = TimeSpan.Zero;
            }

            // TODO: Add your update logic here
            world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            frames++;
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            world.Draw(spriteBatch);
            spriteBatch.DrawString(font, fps.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 40, 0), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
