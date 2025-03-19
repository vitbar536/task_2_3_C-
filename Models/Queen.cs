using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Task2_3.Models
{
    public class Queen : ChessPiece
    {
        public Queen(ChessColor color, int row, int column) 
            : base(color, row, column)
        {
        }

        protected override bool CanMoveTo(int newRow, int newColumn)
        {
            // Queen can move diagonally, horizontally, or vertically
            bool isDiagonalMove = Math.Abs(newRow - Row) == Math.Abs(newColumn - Column);
            bool isHorizontalMove = newRow == Row && newColumn != Column;
            bool isVerticalMove = newRow != Row && newColumn == Column;

            return isDiagonalMove || isHorizontalMove || isVerticalMove;
        }
    }
}