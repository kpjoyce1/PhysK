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
using PhysK;

namespace PhysKSample
{ 
    public class GameApplication : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private World world;
        private DebugView debugView;
        private Camera camera;

        private SpriteFont font;
        private int frames;
        private int fps;
        private TimeSpan fpsTimer;
        private Sprite[] taylors;

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
            camera = new Camera(GraphicsDevice);
            camera.Position = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) / 2;

            debugView = new DebugView(GraphicsDevice, world);
            

            font = Content.Load<SpriteFont>("spritefont");

            Random gen = new Random();

            float radius = 10;
            int taylorNumber = 2000;
            //10000 seems to be the limit of just drawing and bouncing off the screen
            //so the upper limit for collision checking is 10000
            taylors = new Sprite[taylorNumber];
            Particle[] particles = new Particle[taylorNumber];
            for (int i = 0; i < taylorNumber; i++)
            {

                taylors[i] = new Sprite(
                    Content.Load<Texture2D>($"circle{gen.Next(1, 5)}"),
                    new Vector2(gen.Next(0, GraphicsDevice.Viewport.Width - 50),
                        gen.Next(0, GraphicsDevice.Viewport.Width - 50))
                );

                taylors[i].SetCenterOrigin();

                particles[i] = new Particle(taylors[i].Position, new Vector2(gen.Next(-4, 4), gen.Next(-4, 4)), 21, 1f);
                /*
                    new Rigidbody(
                    new Circle(radius) { IsHollow = gen.Next(2) == 1 }, 
                    taylors[i].Position,
                    new Vector2(gen.Next(-4, 4), gen.Next(-4, 4)),
                    gen.Next(21, 21), 
                    1f
                );
                */
                taylors[i].Scale = new Vector2(radius * 2 / taylors[i].Texture.Width, radius * 2 / taylors[i].Texture.Height);

        }
        world.Items = particles;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            fpsTimer += gameTime.ElapsedGameTime;
            if(fpsTimer >= TimeSpan.FromSeconds(1))
            {
                fps = frames;
                frames = 0;
                fpsTimer = TimeSpan.Zero;
            }

            world.Update(gameTime);

            for (int i = 0; i < world.Items.Length; i++)
            {
                taylors[i].Position = world.Items[i].Position;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Wheat);
            frames++;

            spriteBatch.Begin();

            foreach (Sprite taylor in taylors)
            {
                taylor.Draw(spriteBatch);
            }

            spriteBatch.End();
   
            //debugView.Draw(camera.View, camera.Projection);
            
            spriteBatch.Begin();
            
            spriteBatch.DrawString(font, fps.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 40, 0), Color.White);
            
            spriteBatch.DrawString(font, world.Hamiltonian.ToString(), Vector2.Zero, Color.White);

            spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
