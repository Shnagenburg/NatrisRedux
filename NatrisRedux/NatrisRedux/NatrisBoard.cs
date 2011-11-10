using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NatrisRedux
{
    enum pieceColors { Red, Orange, Yellow, Green, Blue, Indigo, Violet, Black };

    class NatrisBoard
    {
        public static Color[] myColors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet, Color.Black };

 

        int widthInPixels;
        public int Width { get { return widthInPixels; } }

        int heightInPixels;
        public int Height { get { return heightInPixels; } }

        public static int[, ,] CellMap;

        public BasePiece currentPiece;
        BasePiece nextPiece;
        
        public static Vector2 gameBoardOffset = new Vector2(16, 16);

        PrimitiveLine blockBrush;
               


        public NatrisBoard()
        {
            widthInPixels = (Constants.gridWidth * Constants.blocksize) + 2 * Constants.innerBoardPadding + (Constants.gridWidth + 1) * Constants.blockPadding;
            heightInPixels = (Constants.gridHeight * Constants.blocksize) + 2 * Constants.innerBoardPadding + (Constants.gridHeight + 1) * Constants.blockPadding;

            CellMap = new int[Constants.gridWidth, Constants.gridHeight, 2];
            blockBrush = new PrimitiveLine(Engine.graphicsDevice, Engine.spriteBatch, Color.White);
            ClearBoard();
            currentPiece = GetNewRandomPiece();
            nextPiece = GetNewRandomPiece();
            nextPiece.setPreview(true);
        }

        void ClearBoard()
        {
            for (int y = 0; y < Constants.gridHeight; y++)
            {
                for (int x = 0; x < Constants.gridWidth; x++)
                {
                    CellMap[x, y, Constants.block] = 0;
                    CellMap[x, y, Constants.color] = (int)pieceColors.Black;
                }
            }
        }

        public void Draw()
        {
            for (int y = 0; y < Constants.gridHeight; y++)
            {
                for (int x = 0; x < Constants.gridWidth; x++)
                {
                    if (CellMap[x, y, 0] == 1)
                    {
                        drawFilledSquare(x, y, myColors[CellMap[x, y, 1] - 1]);
                    }
                }
            }

            nextPiece.draw();
        }

        public void QueueNextPiece()
        {
            currentPiece = nextPiece;
            currentPiece.setPreview(false);
            nextPiece = GetNewRandomPiece();
            nextPiece.setPreview(true);
        }

        BasePiece GetNewRandomPiece()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, Constants.numberOfPieceTypes);

            switch (randomNumber)
            {
                case 1:
                    return new IPiece(blockBrush);
                case 2:
                    return new JPiece(blockBrush);
                case 3:
                    return new LPiece(blockBrush);
                case 4:
                    return new OPiece(blockBrush);
                case 5:
                    return new SPiece(blockBrush);
                case 6:
                    return new TPiece(blockBrush);
                case 7:
                    return new ZPiece(blockBrush);
            }
            return null;
        }

        public Queue<int> checkRowsForCompleteness()
        {
            Queue<int> rowsToErase = new Queue<int>();

            for (int row = 19; row >= 0; row--)
            {
                bool isTrue = true;
                for (int x = 0; x < 10; x++)
                {
                    if (CellMap[x, row, 0] == 0) isTrue = false;
                }
                if (isTrue == true) rowsToErase.Enqueue(row);
            }

            return rowsToErase;

        }


        public void removeRow(int row)
        {
            int i = 0;
            for (int y = row; y > 0; y--)
            {
                for (int x = 0; x < 10; x++)
                {
                    CellMap[x, y, 0] = CellMap[x, y - 1, 0];
                    CellMap[x, y, 1] = CellMap[x, y - 1, 1];
                }
            }
            for (int x = 0; x < 10; x++)
            {
                CellMap[x, 0, 0] = 0;
                CellMap[x, 0, 1] = 7;
            }
        }

        public void drawFilledSquare(int X, int Y, Color myColor)
        {
            int x = X;
            int y = Y;
            int xRect = (int)gameBoardOffset.X + Constants.innerBoardPadding + Constants.blockPadding * x + Constants.blocksize * x;
            int yRect = (int)gameBoardOffset.Y + Constants.innerBoardPadding + Constants.blockPadding * y + Constants.blocksize * y;

            Rectangle myRect = new Rectangle(xRect, yRect, Constants.blocksize, Constants.blocksize);
            blockBrush.CreateFilledRect(myRect, myColor);

        }

    }
}
