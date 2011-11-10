using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NatrisRedux
{
    public class IPiece : BasePiece //Not an interface LOL, just another tetris piece.
    {
        public IPiece(PrimitiveLine blockBrush) : base(blockBrush)
        {
            rotationList.Add(new int[,] {  {0,0,0,0},
                                           {1,1,1,1},
                                           {0,0,0,0},
                                           {0,0,0,0}  });

            rotationList.Add(new int[,] {  {0,0,1,0},
                                           {0,0,1,0},
                                           {0,0,1,0},
                                           {0,0,1,0}  });

            pieceColor = Color.Red;
            base.Setup();
        }
    }
}
