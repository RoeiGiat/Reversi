using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace MyOtGame
{
    class Player
    {
        public Dictionary<int, Piece> pieces;
        protected List<Move> possibleMoves;
        private TypePiece typePiece;
        protected Board board;
        protected Player enemy;
        public int[,] Dir = 
        {
            {-1,-1}, //UpLeft
            {-1,0},  //Up
            {-1,1},  //UpRight
            {0,-1},  //Left
            {0,1},   //Right
            {1,-1},  //DownLeft
            {1,0},   //Down
            {1,1}    //DownRight
        };

        public Player(Board board, TypePiece typePiece)
        {
            this.board = board;
            this.typePiece = typePiece;
            enemy = board.player1 == this ? board.player2 : board.player1;
            pieces = new Dictionary<int, Piece>();
            if (typePiece == TypePiece.BLACK)
            {
                pieces.Add(3 * Board.N + 3, new Piece(3, 3, TypePiece.BLACK)); //starting position
                pieces.Add(4 * Board.N + 4, new Piece(4, 4, TypePiece.BLACK)); //starting position
            }
            else
            {
                pieces.Add(3 * Board.N + 4, new Piece(3, 4, TypePiece.WHITE)); //starting position
                pieces.Add(4 * Board.N + 3, new Piece(4, 3, TypePiece.WHITE)); //starting position
            }
        }


        internal void Paint(System.Drawing.Graphics graphics)
        {
            List<Piece> pieces = this.pieces.Values.ToList();
            foreach (Piece piece in pieces)
                piece.Paint(graphics);

        }

        internal bool Check(int row, int col)
        {
            enemy = board.player1 == this ? board.player2 : board.player1;
            bool approval = false;
            int index = 0;
            while (index != 8) //Get *all* possible moves
            {
                for (int i = 0; i < 8; i++)
                {
                    index++;
                    int newrow = row + Dir[i, 0];
                    int newcol = col + Dir[i, 1];
                    int count = CheckDir(row, col, i, enemy);
                    if (count > 0)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            int key_enemy = newrow * Board.N + newcol;
                            enemy.pieces.Remove(key_enemy); //Deletes enemy pawn
                            pieces.Add(key_enemy, new Piece(newrow, newcol, this.typePiece)); //Add new pawn
                            newrow += Dir[i, 0];
                            newcol += Dir[i, 1];
                        }
                        approval = true;
                    }
                }
            }
            if (approval)
                return true;//A legal move
            return false;
        }

        public int CheckDir(int row, int col, int i,Player player)
        {
            int count = 0;
            int key_new = 0;
            int newrow = row + Dir[i, 0];
            int newcol = col + Dir[i, 1];
            key_new = newrow * Board.N + newcol;
            while (IsLegal(newrow, newcol) && player.pieces.ContainsKey(key_new))
            {
                newrow += Dir[i, 0];
                newcol += Dir[i, 1];
                key_new = newrow * Board.N + newcol;
                count++; //How many pawns eaten
            }
            if (IsLegal(newrow, newcol) && !NotExist(newrow, newcol) && count > 0)
                return count;
            return 0;
        }

        private bool IsLegal(int newrow, int newcol)
        {
            return newrow >= 0 && newrow < Board.N && newcol >= 0 && newcol < Board.N;
        }


        internal void Add(int row, int col) // add to pices new pawn
        {
            int key_new = row * Board.N + col;
            pieces.Add(key_new, new Piece(row, col, this.typePiece));

        }
        internal void Remove(int row, int col) //remove from pices pawn
        {
            int key_new = row * Board.N + col;
            pieces.Remove(key_new);
        }

        public List<Move> GetPossibleMoves() //get all possible moves
        {
            enemy = board.player1 == this ? board.player2 : board.player1;
            List<Move> possibleMoves = new List<Move>();
            foreach (Piece piece in pieces.Values)
            {
                for (int i = 0; i < 8; i++)
                {
                    bool ok = false;
                    int newrow = piece.row + Dir[i, 0];
                    int newcol = piece.col + Dir[i, 1];
                    int key_new = newrow * Board.N + newcol;
                    int count = 0;
                    if (!NotExist(piece.row, piece.col))
                    {                      
                        while (IsLegal(newrow, newcol) && enemy.pieces.ContainsKey(key_new))
                        {
                            newrow += Dir[i, 0];
                            newcol += Dir[i, 1];
                            key_new = newrow * Board.N + newcol;
                            ok = true;
                            count++;
                        }
                    }
                    if (ok && NotExist(newrow, newcol))
                        possibleMoves.Add(new Move(piece, new Point(newcol, newrow), i, count, this));
                }
            }
            return possibleMoves;
        }

        internal bool NotExist(int row, int col) //Checks that the new move is in the limits of the board
        {
            int key = row * Board.N + col;
            return !pieces.ContainsKey(key);
        }

        internal int GetGrade() 
        {
            int[,] mark = {
                          { 1000, -100, 100, 100, 100, 100, -100, 1000 },
                          { -100, -100,  50,  50,  50,  50, -100, -100 },
                          {  100,   50,  80,  80,  80,  80,   50,  100 },
                          {  100,   50,  80,  80,  80,  80,   50,  100 },
                          {  100,   50,  80,  80,  80,  80,   50,  100 },
                          {  100,   50,  80,  80,  80,  80,   50,  100 },
                          { -100, -100,  50,  50,  50,  50, -100, -100 },
                          { 1000, -100, 100, 100, 100, 100, -100, 1000 }
                      };
            int grade = 0;

            foreach (Piece piece in this.pieces.Values)
            {
                grade += mark[piece.row, piece.col];
            }
            return grade;
        }

    }
}
