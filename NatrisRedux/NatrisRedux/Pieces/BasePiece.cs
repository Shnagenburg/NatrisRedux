using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NatrisRedux
{
    public enum Directions { Up, Left, Down, Right }

    public class BasePiece
    {
        public static Color[] myColors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet, Color.Black }; 

        public int rotationSquareWidth = 4;
        public int rotationSquareHeight = 4;
        public int[,] rotationSquare;
        protected IList<int[,]> rotationList;
        protected int rotationIndex;
        protected int numRotations;

        bool Preview = false;

        public Color pieceColor;

        public int X = 3;
        public int Y = -1;

        PrimitiveLine blockBrush;

        public int highestRow;
        public int lowestRow;
       

        public BasePiece(PrimitiveLine BlockBrush)
        {
            rotationList = new List<int[,]>();
            rotationIndex = 1;
            blockBrush = BlockBrush;
        }

        //this function is a stupid hack to overcome the fact that i don't knwo how to properly deal with inherited constructors
        public void Setup()
        {
            numRotations = rotationList.Count;
            rotationSquare = rotationList.ElementAt(rotationIndex - 1);

        }

        public void rotatePiece() 
        {
            if (CanRotate())
            {
               
                if (rotationIndex < numRotations) rotationIndex++;
                else rotationIndex = 1;

                rotationSquare = rotationList.ElementAt(rotationIndex - 1); //yeah i did that. fuck 0. 

            }
        }

        public void draw()
        {
            Vector2 PieceOffset;
            if (Preview) { PieceOffset = new Vector2(320, 500); }
            else PieceOffset = new Vector2((int)NatrisBoard.gameBoardOffset.X + Constants.innerBoardPadding, (int)NatrisBoard.gameBoardOffset.Y + Constants.innerBoardPadding);
            for (int x = 0; x < rotationSquareWidth; x++)
            {
                for (int y = 0; y < rotationSquareHeight; y++)
                {
                    if (rotationSquare[y, x] == 1)
                    {
                        int xPos = (int)PieceOffset.X + (Constants.blockPadding + Constants.blocksize) * (X + x);
                        int yPos = (int)PieceOffset.Y + (Constants.blockPadding + Constants.blocksize) * (Y + y);
                        blockBrush.CreateFilledRect(new Rectangle(xPos, yPos, Constants.blocksize, Constants.blocksize), pieceColor);
                    }
                }
            }
        }


        public bool CanMove(Directions myDirection)
        {
            switch (myDirection)
            {
                case Directions.Left:
                    //first check to see if there is anything in the board that would block it from moving
                    for (int col = 0; col <= 3; col++)
                    {
                        for (int row = 0; row <= 3; row++)
                        {
                          if (rotationSquare[row, col] == 1)
                          {
                                int myRow = ArrayYtoBoardY(row);
                                int myCol = ArrayXtoBoardX(col)-1;
                                if (myRow < 0 || myCol < 0 || myRow > 19) break; //the method i'm using for collision detection breaks under certain conditions. whoops. this if statement catches the problem cases.
                                if (NatrisBoard.CellMap[myCol, myRow, 0] == 1) return false;
                            }
                        }
                    }
                    //then check the left border
                    if(X <= 0)  
                    {
                        int squareColumnToCheck = BoardXtoArrayX(0);
                        for (int row = 0; row < 4; row++)
                        {
                            if(rotationSquare[row,squareColumnToCheck]==1) return false;
                        }                        
                    }
                    return true;

                case Directions.Down:

                    //first check to see if there is anything in the board that would block it from moving
                    for (int row = 3; row >= 0; row--)
                    {
                        for (int col = 0; col <= 3; col++)
                        {
                            if (rotationSquare[row,col] == 1)
                            {
                                int myRow = ArrayYtoBoardY(row)+1;                             
                                int myCol = ArrayXtoBoardX(col);
                                if (myRow < 0 || myCol < 0 || myRow > 19 || myCol > 9) break; //the method i'm using for collision detection breaks under certain conditions. whoops. this if statement catches the problem cases.
                                if (NatrisBoard.CellMap[myCol, myRow, 0] == 1) return false;
                            }
                        }
                    }

                    //then check the bottom border
                    if (Y >= 16)
                    {
                        int squareRowtoCheck = BoardYtoArrayY(19);
                        for (int col = 0; col < 4; col++)
                        {
                            if (rotationSquare[squareRowtoCheck, col] == 1) return false;
                        }
                    }
                    return true;

                case Directions.Right:
                    //first check to see if there is anything in the board that would block it from moving
                    for (int col = 0; col <= 3; col++)
                    {
                        for (int row = 0; row <= 3; row++)
                        {
                            if (rotationSquare[row, col] == 1)
                            {
                                int myRow = ArrayYtoBoardY(row);
                                int myCol = ArrayXtoBoardX(col) + 1;
                                if (myRow < 0 || myCol < 0 || myRow > 19 || myCol > 9 ) break; //the method i'm using for collision detection breaks under certain conditions. whoops. this if statement catches the problem cases.
                                if (NatrisBoard.CellMap[myCol, myRow, 0] == 1) return false;
                            }
                        }
                    }
                    //then check the right border
                    if (X >= 6)
                    {
                        int squareColumnToCheck = BoardXtoArrayX(9);
                        for (int row = 0; row < 4; row++)
                        {
                            if (rotationSquare[row, squareColumnToCheck] == 1) return false;
                        }
                    } 
                    return true;
            }

            return false;
        }

        bool CanRotate()
        {
            int[,] nextRotation = new int[,] {  {1,1,1,1},
                                                {1,1,1,1},
                                                {1,1,1,1},
                                                {1,1,1,1}  };
            if (rotationIndex < numRotations) nextRotation = rotationList.ElementAt(rotationIndex);
            else if (rotationIndex == numRotations) nextRotation = rotationList.ElementAt(0); //

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    int CheckX=ArrayXtoBoardX(x);
                    int CheckY=ArrayYtoBoardY(y);

                    if (nextRotation[y, x] == 1)
                    {
                        if (CheckX < 0 || CheckX > 9 || CheckY < 0 || CheckY > 19) return false;
                        else if (NatrisBoard.CellMap[CheckX, CheckY, 0] == 1) return false;
                    }
                }
            }

            return true;
        }

        public int BoardXtoArrayX(int x)
        {
            return x - X;

        }

        public int BoardYtoArrayY(int y)
        {
            return y - Y;
        }

        public int ArrayXtoBoardX(int x)
        {
            return x + X;
        }

        public int ArrayYtoBoardY(int y)
        {
            return y + Y;
        }

        public void lockPiece()
        {
            for (int x = 0; x < rotationSquareWidth; x++)
            {
                for (int y = 0; y < rotationSquareHeight; y++)
                {
                    if (rotationSquare[y, x] == 1)
                    {
                        NatrisBoard.CellMap[ArrayXtoBoardX(x), ArrayYtoBoardY(y), 0] = 1;
                        NatrisBoard.CellMap[ArrayXtoBoardX(x), ArrayYtoBoardY(y), 1] = getColorIndex(pieceColor);
                    }
                }
            }
        }

        int getColorIndex(Color myColor)
        {
            for (int i = 0; i <= myColors.Count(); i++)
            {
                if (myColors[i] == myColor) return i+1;
            }
            return -1;
        }


        public void setPreview(bool isTrue)
        {
            Preview = isTrue;
        }


    }


}
