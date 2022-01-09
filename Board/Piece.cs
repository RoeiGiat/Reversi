using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyOtGame
{
    public enum TypePiece { WHITE, BLACK,POSSIABLE };

    class Piece
    {
        public static int PieceSize = 40;
        public int row;
        public int col;
        public TypePiece typePiece;

        public Piece(int row, int col, TypePiece typePiece)
        {
            this.row = row;
            this.col = col;
            this.typePiece = typePiece;
        }

        internal void Paint(System.Drawing.Graphics graphics)
        {
            switch (typePiece)
            {
                case TypePiece.BLACK:
                    graphics.DrawImage(Properties.Resources.Pawn_Black, col * PieceSize, row * PieceSize, PieceSize, PieceSize);
                    break;
                case TypePiece.WHITE:
                    graphics.DrawImage(Properties.Resources.Pawn_White, col * PieceSize, row * PieceSize, PieceSize, PieceSize);
                    break;
            }
        }
    }
}
