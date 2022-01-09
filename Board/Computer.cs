using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyOtGame
{
    class Computer : Player
    {

        private Move bestMove;
        int CountPlayer = 0;
        int CountComputer = 0;

        public Computer(Board board, TypePiece typePiece)
            : base(board, typePiece)
        {

        }

        

        private int MiniMax(int level) //take the best move for computer
        {
            if (level == 0)
                return Evaluate();

            Player player = level % 2 != 0 ? board.player2 : board.player1;
            possibleMoves = GetPossibleMoves();
            int max = int.MinValue, min = int.MaxValue;
            foreach (Move move in possibleMoves)
            {
                if (IsLegal(move.dest.Y, move.dest.X))
                {
                    DoMove(move, player);
                    int grade = MiniMax(level - 1);
                    if (level % 2 != 0)
                    {
                        if (grade > max)
                        {
                            max = grade;
                            bestMove = move;
                        }
                    }
                    else
                    {
                        if (grade < min)
                        {
                            min = grade;
                            bestMove = move;
                        }
                    }
                    UnDoMove(move, player);
                }
            }
            return level % 2 != 0 ? max: min;
        }

        public void DoStep()
        {
            Thread compStep = new Thread(new ThreadStart(CompStep));
            compStep.Start();
        }

        private int Evaluate()
        {
            int grade = board.player2.GetGrade() - board.player1.GetGrade();


            return grade * 2 + (board.player2.pieces.Count - board.player1.pieces.Count);
        }

        private void UnDoMove(Move move, Player player) //delete the move from "DoMove" function
        {
            enemy = (player is Computer) ? board.player1 : board.player2;
            int newrow = move.source.row + Dir[move.dir, 0];
            int newcol = move.source.col + Dir[move.dir, 1];
            for (int j = 0; j < move.count; j++)
            {
                int key_enemy = newrow * Board.N + newcol;
                this.pieces.Remove(key_enemy);
                enemy.pieces.Add(key_enemy, new Piece(newrow, newcol, player == this ? TypePiece.BLACK : TypePiece.WHITE));
                newrow += Dir[move.dir, 0];
                newcol += Dir[move.dir, 1];
            }
            this.Remove(move.dest.Y, move.dest.X);
        }

        private void DoMove(Move move, Player player) //add new pawn and delete the enemy pawn 
        {
            enemy = player is Computer ? board.player1 : board.player2;
            int newrow = move.source.row + Dir[move.dir, 0];
            int newcol = move.source.col + Dir[move.dir, 1];
            for (int j = 0; j < move.count; j++)
            {
                int key_enemy = newrow * Board.N + newcol;
                enemy.pieces.Remove(key_enemy);
                this.pieces.Add(key_enemy, new Piece(newrow, newcol, player == this ? TypePiece.BLACK : TypePiece.WHITE));
                newrow += Dir[move.dir, 0];
                newcol += Dir[move.dir, 1];
            }
            this.Add(move.dest.Y, move.dest.X);
        }

        private bool Endgame() //check if game over
        {
            CountPlayer = enemy.pieces.Values.Count; CountComputer = pieces.Values.Count;
            return ((CountPlayer + CountComputer) == 64 || CountPlayer == 0 || CountComputer == 0);
        }

        private bool IsLegal(int newrow, int newcol)
        {
            return newrow >= 0 && newrow < Board.N && newcol >= 0 && newcol < Board.N;
        }

        private void CompStep() // add best move to piece
        {
            MiniMax(1);
            string dot = ".";
            for (int i = 1; i < 4; i++)
            {
                Board.gameForm.toolStripStatusLabel1.Text = "Computer think" + dot;
                Thread.Sleep(500);
                dot += ".";
            }
            Check(bestMove.dest.Y, bestMove.dest.X);
            Add(bestMove.dest.Y, bestMove.dest.X);
            CountPlayer = enemy.pieces.Values.Count; CountComputer = pieces.Values.Count;
            Board.gameForm.toolStripStatusLabel1.Text = "Computer : " + CountComputer + "                  You : " + CountPlayer;
            Board.gameForm.pictureBox1.Invalidate();            
        }
    }
}
