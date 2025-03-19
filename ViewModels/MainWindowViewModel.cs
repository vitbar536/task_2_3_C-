using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Task2_3.Models;

namespace Task2_3.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ChessPiece> _pieces; // Коллекция шахматных фигур
        private ChessPiece? _selectedPiece; // Выбранная фигура
        private int _targetRow; // Целевой ряд
        private int _targetColumn; // Целевой столбец
        private string _moveResult = string.Empty; // Результат хода
        private string _chessboardState = string.Empty; // Состояние шахматной доски

        public MainWindowViewModel()
        {
            _pieces = new ObservableCollection<ChessPiece>
            {
                new Queen(ChessColor.White, 7, 3),
                new Rook(ChessColor.Black, 0, 0),
                new Bishop(ChessColor.White, 7, 2)
            };

            MoveCommand = new RelayCommand(ExecuteMove, CanExecuteMove);
            UpdateChessboardState();
        }

        public ObservableCollection<ChessPiece> Pieces
        {
            get => _pieces;
            set => SetProperty(ref _pieces, value);
        }

        public ChessPiece? SelectedPiece
        {
            get => _selectedPiece;
            set
            {
                if (SetProperty(ref _selectedPiece, value))
                {
                    OnPropertyChanged(nameof(CanSelectTargetPosition));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int TargetRow
        {
            get => _targetRow;
            set
            {
                if (SetProperty(ref _targetRow, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int TargetColumn
        {
            get => _targetColumn;
            set
            {
                if (SetProperty(ref _targetColumn, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string MoveResult
        {
            get => _moveResult;
            set => SetProperty(ref _moveResult, value);
        }

        public string ChessboardState
        {
            get => _chessboardState;
            set => SetProperty(ref _chessboardState, value);
        }

        public bool CanSelectTargetPosition => SelectedPiece != null;

        public ICommand MoveCommand { get; }

        private bool CanExecuteMove()
        {
            return SelectedPiece != null && 
                   TargetRow >= 0 && TargetRow <= 7 && 
                   TargetColumn >= 0 && TargetColumn <= 7;
        }

        private void ExecuteMove()
        {
            if (SelectedPiece == null)
                return;

            bool moveSuccessful = SelectedPiece.MakeMove(TargetRow, TargetColumn);

            // Перевод сообщений на русский
            MoveResult = moveSuccessful
                ? $"Ход выполнен успешно: {GetRussianPieceName(SelectedPiece)} перемещен на {(char)('A' + TargetColumn)}{8 - TargetRow}"
                : $"Недопустимый ход для {GetRussianPieceName(SelectedPiece)} на {(char)('A' + TargetColumn)}{8 - TargetRow}";

            UpdateChessboardState();
            OnPropertyChanged(nameof(Pieces));
        }

        // Метод для получения названия фигуры на русском языке
        private string GetRussianPieceName(ChessPiece piece)
        {
            string colorName = piece.Color == ChessColor.White ? "Белый" : "Черный";
            string pieceName = piece.GetType().Name switch
            {
                "Queen" => "Ферзь",
                "Rook" => "Ладья",
                "Bishop" => "Слон",
                _ => "Неизвестная фигура"
            };

            return $"{colorName} {pieceName}";
        }

        private void UpdateChessboardState()
        {
            var board = new string[8, 8];
            
            // Инициализация пустой доски
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board[i, j] = "  ";

            // Размещение фигур на доске
            foreach (var piece in Pieces)
            {
                string symbol = piece.GetType().Name switch
                {
                    "Queen" => "Q",
                    "Rook" => "R",
                    "Bishop" => "B",
                    _ => "?"
                };
                
                string colorCode = piece.Color == ChessColor.White ? "W" : "B";
                board[piece.Row, piece.Column] = colorCode + symbol;
            }

            // Форматирование доски в виде строки
            var boardString = "  A0 B1 C2 D3 E4 F5 G6 H7\n";
            for (int i = 0; i < 8; i++)
            {
                boardString += (8 - i) + " ";
                for (int j = 0; j < 8; j++)
                {
                    boardString += board[i, j] + " ";
                }
                boardString += "\n";
            }

            ChessboardState = boardString;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true);
        }

        public bool CanExecute(object? parameter) => _canExecute();

        public void Execute(object? parameter) => _execute();
    }

    public static class CommandManager
    {
        public static event EventHandler? RequerySuggested;

        public static void InvalidateRequerySuggested()
        {
            RequerySuggested?.Invoke(null, EventArgs.Empty);
        }
    }
}
