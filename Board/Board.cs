using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;


namespace MyOtGame
{
    class Board
    {
        int PlayerWins = 0;
        int ComputerWins = 0;
        int CountPlayer = 0;
        int CountComputer = 0;
        public static int N = 8;
        public Player player1; 
        public Player player2;
       // int turn = 0;
        public static GameForm gameForm;
        //public static Menu menu;
        protected List<Move> possibleMoves;
        Rectangle rect;

        public Board(GameForm form)
        {
            gameForm = form;
            player1 = new Player(this, TypePiece.BLACK);
            player2 = new Computer(this, TypePiece.WHITE);
        }

        internal void Paint(System.Drawing.Graphics graphics) //paint
        {
            possibleMoves = player1.GetPossibleMoves();
            foreach (Move move in possibleMoves)
            {
                rect = new Rectangle(move.dest.X * Piece.PieceSize, move.dest.Y* Piece.PieceSize, Piece.PieceSize-4, Piece.PieceSize-4);
                graphics.FillRectangle(new SolidBrush(Color.Gold), rect);
            }
            player1.Paint(graphics);
            player2.Paint(graphics);
        }

        internal void Click(System.Drawing.Point point)  
        {
            int row = point.Y / Piece.PieceSize;
            int col = point.X / Piece.PieceSize;
           // Player player = turn % 2 == 0 ? player2 : player1;
            if (Endgame())
            {
                string winner = "";
                if (CountComputer > CountPlayer)
                {
                    ComputerWins++;
                    winner = "COMPUTER WIN";
                }
                else if (CountComputer < CountPlayer)
                {
                    PlayerWins++;
                    winner = "YOU WIN!!";
                }
                else
                {
                    winner = "DRAW";
                    PlayerWins++;
                    ComputerWins++;
                }
                Board.gameForm.toolStripStatusLabel1.Text = winner;
                Thread.Sleep(1000);
            }
            else
            {
                    if (player1.Check(row, col))
                    {
                        player1.Add(row, col);
                        (player2 as Computer).DoStep();
                    }
            }
   
        }
        private bool Endgame() //check if game over
        {
            CountPlayer = player1.pieces.Values.Count; CountComputer = player2.pieces.Values.Count;
            return ((CountPlayer + CountComputer) == 64 || CountPlayer == 0 || CountComputer == 0);
        }
       
    }
}
