using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankWars
{
    class GamePiece
    {
        public static String[] PieceTypes = 
        {
            "0",    //north
            "1",    //east
            "2",    //south
            "3",    //west
            "water",
            "stone",
            "b100", //100% brick
            "b75",  //75% brick
            "b50",  //50% brick
            "b25",  //25% brick
            "med",  
            "coin",  
            "empty"  
        };

        public const int EmptyPieceIndex = 12;
        public const int PieceHeight = 40;
        public const int PieceWidth = 40;

        private const int textureOffsetX = 1;
        private const int textureOffsetY = 1;
        private const int texturePaddingX = 1;
        private const int texturePaddingY = 1;

        private string pieceType = "";

        public string PieceType
        {
            get { return pieceType; }
            set { pieceType = value; }
        }
        private string pieceSuffix = "";
        private int locx;
        private int locy;

        //*****************************************************
        //*******************Constructors**********************
        //*****************************************************

        //Constructor with suffix
        public GamePiece(string type, string suffix)    
        {
            pieceType = type;
            pieceSuffix = suffix;
        }

        //Constructor without suffix
        public GamePiece(string type)
        {
            pieceType = type;
            pieceSuffix = "";
        }

        //*****************************************************
        //*****************************************************

        //get coordinates or piece(Added by Sunimal)****************************


        public int getx()
        {
            return locx;
        }


        public int gety()
        {
            return locy;
        }
        


        //**********************************************************************

        //method to set peice type
        public void SetPiece(string type, string suffix)
        {
            pieceType = type;
            pieceSuffix = suffix;
        }

        //overloaded method to set peice type  
        public void SetPiece(string type)
        {
            pieceType = type;
            pieceSuffix = "";
        }
        //get the rectangle from the sprite sheet 
        public Rectangle GetSourceRect()
        {
            int x = textureOffsetX;
            int y = textureOffsetY;

            if (pieceSuffix.Contains("E"))
                x += PieceWidth + texturePaddingX;

            y += (Array.IndexOf(PieceTypes, pieceType) *
                 (PieceHeight + texturePaddingY));


            return new Rectangle(x, y, PieceWidth, PieceHeight);
        }






    }
}
