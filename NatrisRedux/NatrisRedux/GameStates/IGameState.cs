using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;


namespace NatrisRedux
{
    interface IGameState
    {

        gameStateType stateSwitch();
      
        void Update(float elapsedSeconds);
        
        void ProcessInput(KeyboardState currentKeyState, KeyboardState lastKeyState);                                                                                                                                                                                                             

        void Draw();

    }
}
