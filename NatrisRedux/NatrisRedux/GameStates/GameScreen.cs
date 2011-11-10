using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NatrisRedux
{
    class GameScreen : IGameState
    {

        IList<Rectangle> uiRects;
        PrimitiveLine myUIBrush;

        public static Vector2 gameBoardOffset = new Vector2(16, 16);

        gameStateType stateSwitchFlag = gameStateType.GameScreen;

        enum PlayingStates { Playing, Freezing, Erasing }  //Freezing isn't used yet.
        PlayingStates PlayingState = PlayingStates.Playing;

        NatrisBoard gameBoard;

        Vector2 ScoreLabelPos = new Vector2(400, 140);
        Vector2 ScoreStringPos = new Vector2(530, 140);
        Vector2 LevelLabelPos = new Vector2(430, 235);
        Vector2 LevelStringPos = new Vector2(560, 235);
        Vector2 NextPieceStringPos = new Vector2(410, 420);
        SpriteFont myFont;

        float timeBetweenDrops = .5f;
        float timeSinceLastDrop = 0f;

        const float lockingTime = .5f;
        float lockingTimer = 0f;

        const float erasingTime = .3f;
        float erasingTimer = 0f;

        Queue<int> rowsToRemove;

        int playerScore = 0;
        int playerLevel = 1;
        int linesCleared = 0;

        public GameScreen()
        {
            gameBoard = new NatrisBoard();

            uiRects = new List<Rectangle>();
            uiRects.Add(new Rectangle((int)gameBoardOffset.X, (int)gameBoardOffset.Y, gameBoard.Width, gameBoard.Height)); //the outside of the game board
            uiRects.Add(new Rectangle(380, 20, 260, 60));
            uiRects.Add(new Rectangle(380, 100, 260, 260));
            uiRects.Add(new Rectangle(380, 400, 260, 260));

            myUIBrush = new PrimitiveLine(Engine.graphicsDevice, Engine.spriteBatch, Color.Blue);
            myFont = Engine.myContent.Load<SpriteFont>("UIMono24");
        }    

        public void Update(float elapsedSeconds)
        {

            switch (PlayingState)
            {

                case PlayingStates.Playing:

                    timeSinceLastDrop += elapsedSeconds;

                    if (timeSinceLastDrop >= timeBetweenDrops)
                    {
                        if (gameBoard.currentPiece.CanMove(Directions.Down)) gameBoard.currentPiece.Y += 1;
                        timeSinceLastDrop = 0f;
                    }

                    if (!gameBoard.currentPiece.CanMove(Directions.Down)) lockingTimer += elapsedSeconds;

                    if (lockingTimer >= lockingTime)
                    {
                        
                        gameBoard.currentPiece.lockPiece();
                        rowsToRemove = gameBoard.checkRowsForCompleteness();
                        linesCleared += rowsToRemove.Count;
                        if (rowsToRemove.Count > 0)
                        {
                            PlayingState = PlayingStates.Erasing;
                        }
                        else
                        {
                            playerScore += 5 * playerLevel;
                            gameBoard.QueueNextPiece();
                        }
                        lockingTimer = 0;
                    }

                    break;

                case PlayingStates.Erasing:

                    erasingTimer += elapsedSeconds;
                    if (erasingTimer >= erasingTime)
                    {
                        rowsToRemove = gameBoard.checkRowsForCompleteness();
                        if (rowsToRemove.Count > 0)
                        {
                            gameBoard.removeRow(rowsToRemove.Dequeue());
                            playerScore += 15 * playerLevel;
                        }
                        else
                        {
                            PlayingState = PlayingStates.Playing;
                            if (linesCleared >= 10)
                            {
                                linesCleared = 0;
                                playerLevel++;
                                timeBetweenDrops -= .05f;
                            }
                            gameBoard.QueueNextPiece();
                        }
                        erasingTimer = 0f;
                    }
                    break;
            }
        
        
        }

        public void ProcessInput(KeyboardState currentKeyState, KeyboardState lastKeyState)
        {
            if (currentKeyState.IsKeyUp(Keys.Up) && lastKeyState.IsKeyDown(Keys.Up))
            {
                    gameBoard.currentPiece.rotatePiece();
            }
            else if (currentKeyState.IsKeyUp(Keys.Left) && lastKeyState.IsKeyDown(Keys.Left))
            {
                if (gameBoard.currentPiece.CanMove(Directions.Left)) gameBoard.currentPiece.X -= 1;
            }
            else if (currentKeyState.IsKeyUp(Keys.Right) && lastKeyState.IsKeyDown(Keys.Right))
            {
                if (gameBoard.currentPiece.CanMove(Directions.Right)) gameBoard.currentPiece.X += 1;
            }
            else if (currentKeyState.IsKeyUp(Keys.Down) && lastKeyState.IsKeyDown(Keys.Down))
            {
                while (gameBoard.currentPiece.CanMove(Directions.Down)) { gameBoard.currentPiece.Y += 1; }
                
            }
        }

        public void Draw()
        {
            foreach (Rectangle thisRect in uiRects)
            {
                myUIBrush.CreateRectangle(thisRect);
            }

            gameBoard.Draw();

            if (PlayingState == PlayingStates.Playing)
            {
                gameBoard.currentPiece.draw();
            }

            Engine.spriteBatch.DrawString(myFont, "Score:", ScoreLabelPos, Color.White);
            Engine.spriteBatch.DrawString(myFont, playerScore.ToString(), ScoreStringPos, Color.White);
            Engine.spriteBatch.DrawString(myFont, "Level:", LevelLabelPos, Color.White);
            Engine.spriteBatch.DrawString(myFont, playerLevel.ToString(), LevelStringPos, Color.White);
            Engine.spriteBatch.DrawString(myFont, "Next Piece:", NextPieceStringPos, Color.White);
        }

        public gameStateType stateSwitch()
        {
            return stateSwitchFlag;
        }
    }
}
