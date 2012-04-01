using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankWars
{
    class GameBoard
    {
        private static GameBoard instance;

        public static int GameBoardWidth = GameManager.MAXSIZE;
        public static int GameBoardHeight = GameManager.MAXSIZE;

        public static GamePiece[,] boardSquares = new GamePiece[GameBoardWidth, GameBoardHeight];

        public static GameBoard getGameBoard
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameBoard();
                }
                return instance;
            }
        }

        private GameBoard()
        {
            ClearBoard();
        }


        public void ClearBoard()
        {
            for (int x = 0; x < GameBoardWidth; x++)
                for (int y = 0; y < GameBoardHeight; y++)
                    boardSquares[x, y] = new GamePiece("empty");
        }

        public Rectangle GetSourceRect(int x, int y)
        {
            return boardSquares[x, y].GetSourceRect();
        }

        public void SetSquare(int x, int y, string pieceName)
        {
            boardSquares[x, y].SetPiece(pieceName);
        }

        public void SetSquare(int x, int y, string pieceName, string suffix)
        {
            boardSquares[x, y].SetPiece(pieceName,suffix);
        }
    }
}
