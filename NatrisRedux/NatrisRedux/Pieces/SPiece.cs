using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NatrisRedux
{
    public class SPiece : BasePiece
        {
        public SPiece(PrimitiveLine blockBrush):base(blockBrush)
            {
                rotationList.Add(new int[,] {  {0,0,0,0},
                                               {0,0,1,1},
                                               {0,1,1,0},
                                               {0,0,0,0}  });

                rotationList.Add(new int[,] {  {0,0,1,0},
                                               {0,0,1,1},
                                               {0,0,0,1},
                                               {0,0,0,0}  });

                pieceColor = Color.Blue ;
                base.Setup();
            }
        }
  
}
