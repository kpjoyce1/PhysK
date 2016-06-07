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
        PhysicsSprite tay, tay2, tay3, tay4;

        World world;
        public static SpriteFont font;

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
            tay = new PhysicsSprite(Content.Load<Texture2D>("circle"), Vector2.Zero, Color.White, 2f * Vector2.One, 1f, 0f);
            tay2 = new PhysicsSprite(Content.Load<Texture2D>("circle2"), Vector2.One * 200f, Color.White, -Vector2.One, 1f, 0f);
            tay3 = new PhysicsSprite(Content.Load<Texture2D>("circle3"), new Vector2(100, 400), Color.White, new Vector2(3, 2), 100f, -1f);
            tay4 = new PhysicsSprite(Content.Load<Texture2D>("circle4"), new Vector2(200, 600), Color.White, new Vector2(9, 2), 1f, -1f);

            world = new World(GraphicsDevice);

            font = Content.Load<SpriteFont>("spritefont");

            world.Items = new PhysicsSprite []{ tay, tay2, tay3, tay4 };

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

            // TODO: Add your update logic here
            world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            world.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
