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

namespace NatrisRedux
{
    enum gameStateType { GameScreen, HighScoreScreen, MenuScreen, ExitGame }

    public class Engine : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static GraphicsDevice graphicsDevice;
        public static ContentManager myContent;

        gameStateType currentGameStateFlag;
        IGameState currentGameState;

        KeyboardState currentKeyState;
        KeyboardState prevKeyState;

        const int WindowWidth = 800;
        const int WindowHeight = 700;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            myContent = Content;
            graphicsDevice = GraphicsDevice;
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges(); 
            currentGameState = new MenuScreen();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime)
        {
            currentKeyState = Keyboard.GetState();

            if (currentKeyState.IsKeyDown(Keys.Escape)) this.Exit();

            currentGameState.ProcessInput(currentKeyState,prevKeyState);
            currentGameState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            prevKeyState = currentKeyState;

            if (currentGameStateFlag != currentGameState.stateSwitch())
            {
                currentGameStateFlag = currentGameState.stateSwitch();
                switch (currentGameStateFlag)
                {
                    case gameStateType.GameScreen:
                        currentGameState = new GameScreen();
                        break;
                    case gameStateType.HighScoreScreen:
                        currentGameState = new HighScoreScreen();
                        break;
                    case gameStateType.MenuScreen:
                        currentGameState = new MenuScreen();
                        break;
                    case gameStateType.ExitGame:
                        this.Exit();
                        break;
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            currentGameState.Draw();
            spriteBatch.End();

            //base.Draw(gameTime);
        }
    }
}
