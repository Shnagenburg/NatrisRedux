using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace NatrisRedux
{
    class HighScoreScreen : IGameState
    {
        gameStateType stateSwitchFlag = gameStateType.HighScoreScreen;

        public gameStateType stateSwitch()
        {
            return stateSwitchFlag;
        }


        public void Update(float elapsedSeconds)
        { }


        public void ProcessInput(KeyboardState currentKeyState, KeyboardState lastKeyState)
        { }

        public void Draw()
        { }

    }
}
