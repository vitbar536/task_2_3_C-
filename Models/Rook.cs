using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Task2_3.Models
{
    public class Rook : ChessPiece
    {
        public Rook(ChessColor color, int row, int column) 
            : base(color, row, column)
        {
        }

        protected override bool CanMoveTo(int newRow, int newColumn)
        {
            // Rook can move horizontally or vertically
            bool isHorizontalMove = newRow == Row && newColumn != Column;
            bool isVerticalMove = newRow != Row && newColumn == Column;

            return isHorizontalMove || isVerticalMove;
        }
    }
}