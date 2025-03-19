using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2_3.Models
{
    public enum ChessColor
    {
        White,
        Black
    }

    public abstract class ChessPiece
    {
        public ChessColor Color { get; }
        public int Row { get; private set; }
        public int Column { get; private set; }

        protected ChessPiece(ChessColor color, int row, int column)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7)
                throw new ArgumentOutOfRangeException("Position must be between 0 and 7");

            Color = color;
            Row = row;
            Column = column;
        }

        public bool MakeMove(int newRow, int newColumn)
        {
            if (newRow < 0 || newRow > 7 || newColumn < 0 || newColumn > 7)
                return false;

            if (CanMoveTo(newRow, newColumn))
            {
                Row = newRow;
                Column = newColumn;
                return true;
            }
            return false;
        }

        protected abstract bool CanMoveTo(int newRow, int newColumn);

        public override string ToString()
        {
            return $"{GetType().Name} ({Color}) at {(char)('A' + Column)}{8 - Row}";
        }
    }
}