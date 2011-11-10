using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NatrisRedux
{

    class MenuScreen : IGameState
    {
        Texture2D menuBG;
        Texture2D buttonTexture;
        Texture2D selectedButtonTexture;

        SpriteFont buttonText;

        IList<menuButton> buttonList;
        menuButton currentButton;

        int currentButtonIndex;
        int maxButtonIndex;

        Vector2 menuPos = new Vector2(275, 150);
        int menuInteriorPadding = 10;
        int menuButtonOffsetY = 15;
        int buttonWidth = 250;
        int buttonHeight = 50;

        int buttonTextPaddingY = 2;
        int buttonTextPaddingX = 18;

        gameStateType stateSwitchFlag = gameStateType.MenuScreen;

        public MenuScreen()
        {
            menuBG = Engine.myContent.Load<Texture2D>("menuBG");
            buttonTexture = Engine.myContent.Load<Texture2D>("button");
            selectedButtonTexture = Engine.myContent.Load<Texture2D>("selectedbutton");

            buttonText = Engine.myContent.Load<SpriteFont>("buttonText");

            buttonList = new List<menuButton>();
            buttonList.Add(new menuButton("Start Game", 0));
            buttonList.Add(new menuButton("High Scores", 1));
            buttonList.Add(new menuButton("Exit Game", 2));

            currentButtonIndex = 0;
            maxButtonIndex = buttonList.Count-1;         
        }

        public gameStateType stateSwitch()
        {
            return stateSwitchFlag;
        }

        public void Update(float elapsedSeconds)
        {
            SetButton();
        }

        public void ProcessInput(KeyboardState currentKeyState, KeyboardState lastKeyState)
        {
            if (lastKeyState.IsKeyDown(Keys.Down) && currentKeyState.IsKeyUp(Keys.Down))
            {
                NextMenuItem();
            }
            else if (lastKeyState.IsKeyDown(Keys.Up) && currentKeyState.IsKeyUp(Keys.Up))
            {
                PrevMenuItem();
            }
            else if (lastKeyState.IsKeyDown(Keys.Enter) && currentKeyState.IsKeyUp(Keys.Enter))
            {
                switch(currentButtonIndex)
                {
                    case 0:
                        stateSwitchFlag = gameStateType.GameScreen;
                        break;
                    case 1:
                        stateSwitchFlag = gameStateType.HighScoreScreen;
                        break;
                    case 2:
                        stateSwitchFlag = gameStateType.ExitGame;
                        break;
                }
            }               
        }

        public void Draw()
        {
            Engine.spriteBatch.Draw(menuBG,new Vector2(0,0),Color.White);
            Rectangle myRect;

            Texture2D thisTexure;
            foreach (menuButton Button in buttonList)
            {
                if (Button.buttonID == currentButtonIndex) thisTexure = selectedButtonTexture;
                else thisTexure = buttonTexture;

                myRect = new Rectangle((int)menuPos.X + menuInteriorPadding, (int)menuPos.Y + menuInteriorPadding + (menuButtonOffsetY + buttonHeight) * Button.buttonID, buttonWidth, buttonHeight);

                Engine.spriteBatch.Draw(thisTexure, myRect, Color.White);
                Engine.spriteBatch.DrawString(buttonText, Button.buttonName, new Vector2(myRect.X + buttonTextPaddingX, myRect.Y + buttonTextPaddingY), Color.White);
            }

        }

        public void NextMenuItem()
        {
            if (currentButtonIndex < maxButtonIndex) currentButtonIndex++;
            else currentButtonIndex = 0;
        }

        public void PrevMenuItem()
        {
            if (currentButtonIndex > 0) currentButtonIndex--;
            else currentButtonIndex = maxButtonIndex;
        }

        public void SetButton()
        {
            currentButton = buttonList.ElementAt(currentButtonIndex);
        }

    }
}
