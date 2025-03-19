using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Task2_3.Models
{
    public class Bishop : ChessPiece
    {
        public Bishop(ChessColor color, int row, int column) 
            : base(color, row, column)
        {
        }

        protected override bool CanMoveTo(int newRow, int newColumn)
        {
            // Bishop can only move diagonally
            return Math.Abs(newRow - Row) == Math.Abs(newColumn - Column);
        }
    }
}