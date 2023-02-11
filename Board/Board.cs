using Chess.MathNS;
using Chess.PiecesNS;
using Chess.PiecesNS.Base;

namespace Chess.BoardNS
{
	public class Board
	{
		private Cell[,] _cells;
		public Cell[,] Cells => _cells;

		public List<Piece> Pieces { get; private set; }

		public Cell? SelectedCell { get; private set; } = null;

		private King _whiteKing;
		private King _blackKing;

		public Team CurrentMoveTeam { get; private set; }

		public Board()
		{
			_cells = new Cell[CELL_COUNT, CELL_COUNT];
			for (int x = 0; x < CELL_COUNT; x++)
			{
				for (int y = 0; y < CELL_COUNT; y++)
				{
					_cells[x, y] = new Cell(new Vector2Int(x, y));
				}
			}
		}
		public Board(Board copy)
		{
			_cells = new Cell[CELL_COUNT, CELL_COUNT];
			for (int x = 0; x < CELL_COUNT; x++)
			{
				for (int y = 0; y < CELL_COUNT; y++)
				{
					_cells[x, y] = copy.Cells[x, y].Clone();
				}
			}
			InitPieces(copy);
		}

		public Board GetNextMoveBoard(Piece piece, Cell target)
		{
			Board nextMoveBoard = new Board(this);
			nextMoveBoard.Cells[piece.Position.X, piece.Position.Y].RemovePiece();
			nextMoveBoard.Cells[target.Position.X, target.Position.Y] = target.Clone();
			return nextMoveBoard;
		}
		public bool CanMovePiece(Piece piece, Cell target)
		{
			if (CurrentMoveTeam != piece.Team) return false;

			Board nextMoveBoard = GetNextMoveBoard(piece, target);
			if (IsKingUnderAttack(nextMoveBoard, piece.Team)) return false;

			return piece.CanMoveToCell(this, target);
		}
		public void MovePiece(Piece piece, Cell target)
		{
			piece.Cell.RemovePiece();
			piece.SetCell(target);
			target.SetPiece(piece);
			piece.Moved = true;

			CurrentMoveTeam = CurrentMoveTeam == Team.White ? Team.Black : Team.White;
		}
		public bool IsKingUnderAttack(Board board, Team team)
		{
			King king = team == Team.White ? _whiteKing : _blackKing;

			for (int i = 0; i < Pieces.Count; i++)
			{
				Piece piece = Pieces[i];
				if (piece is King) continue;
				if (piece.Team == team) continue;

				if (piece.CanMoveToCell(board, king.Cell)) return true;
			}

			return false;
		}
		public bool IsCheckMate(Board board, Team team)
		{
			King king = team == Team.White ? _whiteKing : _blackKing;

			throw new NotImplementedException();
		}
		public IEnumerable<Cell> GetAllowedPieceMoves(Piece piece)
		{
			if (piece.Team != CurrentMoveTeam) return Enumerable.Empty<Cell>();
			return piece.GetAllowedCells(this);
		}
		public void InitPieces()
		{
			Pieces = new List<Piece>();
			CurrentMoveTeam = Team.White;

			Pieces.Add(_cells[0, 0].CreatePiece(PieceType.Rook, Team.White));
			Pieces.Add(_cells[1, 0].CreatePiece(PieceType.Knight, Team.White));
			Pieces.Add(_cells[2, 0].CreatePiece(PieceType.Bishop, Team.White));
			Pieces.Add(_cells[3, 0].CreatePiece(PieceType.Queen, Team.White));
			_whiteKing = (King)_cells[4, 0].CreatePiece(PieceType.King, Team.White);
			Pieces.Add(_whiteKing);
			Pieces.Add(_cells[5, 0].CreatePiece(PieceType.Bishop, Team.White));
			Pieces.Add(_cells[6, 0].CreatePiece(PieceType.Knight, Team.White));
			Pieces.Add(_cells[7, 0].CreatePiece(PieceType.Rook, Team.White));

			Pieces.Add(_cells[0, 1].CreatePiece(PieceType.Pawn, Team.White));
			Pieces.Add(_cells[1, 1].CreatePiece(PieceType.Pawn, Team.White));
			Pieces.Add(_cells[2, 1].CreatePiece(PieceType.Pawn, Team.White));
			Pieces.Add(_cells[3, 1].CreatePiece(PieceType.Pawn, Team.White));
			Pieces.Add(_cells[4, 1].CreatePiece(PieceType.Pawn, Team.White));
			Pieces.Add(_cells[5, 1].CreatePiece(PieceType.Pawn, Team.White));
			Pieces.Add(_cells[6, 1].CreatePiece(PieceType.Pawn, Team.White));
			Pieces.Add(_cells[7, 1].CreatePiece(PieceType.Pawn, Team.White));

			Pieces.Add(_cells[0, 7].CreatePiece(PieceType.Rook, Team.Black));
			Pieces.Add(_cells[1, 7].CreatePiece(PieceType.Knight, Team.Black));
			Pieces.Add(_cells[2, 7].CreatePiece(PieceType.Bishop, Team.Black));
			Pieces.Add(_cells[3, 7].CreatePiece(PieceType.Queen, Team.Black));
			_blackKing = (King)_cells[4, 7].CreatePiece(PieceType.King, Team.Black);
			Pieces.Add(_blackKing);
			Pieces.Add(_cells[5, 7].CreatePiece(PieceType.Bishop, Team.Black));
			Pieces.Add(_cells[6, 7].CreatePiece(PieceType.Knight, Team.Black));
			Pieces.Add(_cells[7, 7].CreatePiece(PieceType.Rook, Team.Black));

			Pieces.Add(_cells[0, 6].CreatePiece(PieceType.Pawn, Team.Black));
			Pieces.Add(_cells[1, 6].CreatePiece(PieceType.Pawn, Team.Black));
			Pieces.Add(_cells[2, 6].CreatePiece(PieceType.Pawn, Team.Black));
			Pieces.Add(_cells[3, 6].CreatePiece(PieceType.Pawn, Team.Black));
			Pieces.Add(_cells[4, 6].CreatePiece(PieceType.Pawn, Team.Black));
			Pieces.Add(_cells[5, 6].CreatePiece(PieceType.Pawn, Team.Black));
			Pieces.Add(_cells[6, 6].CreatePiece(PieceType.Pawn, Team.Black));
			Pieces.Add(_cells[7, 6].CreatePiece(PieceType.Pawn, Team.Black));
		}
		public void InitPieces(Board board)
		{
			Pieces = new List<Piece>();
			CurrentMoveTeam = board.CurrentMoveTeam;

			for (int x = 0; x < CELL_COUNT; x++)
			{
				for (int y = 0; y < CELL_COUNT; y++)
				{
					if (_cells[x, y].Piece != null)
					{
						Pieces.Add(_cells[x, y].Piece!);
					}
				}
			}

			_whiteKing = board._whiteKing;
			_blackKing = board._blackKing;
		}
		public void SelectCell(Cell cell) => SelectedCell = cell;
		public void DeselectCell() => SelectedCell = null;
		public bool AreBlockingPiecesOnWay(Cell startCell, Cell endCell)
		{
			Vector2Int offset = endCell.Position - startCell.Position;
			if (Math.Abs(offset.X) == Math.Abs(offset.Y))
			{
				Vector2Int pos;
				for (int i = 1; i < Math.Abs(offset.X); i++)
				{
					pos = startCell.Position + new Vector2Int(offset.X > 0 ? i : -i, offset.Y > 0 ? i : -i);
					if (!IsCellPositionValid(pos)) continue;
					Cell cellAtPosition = Cells[pos.X, pos.Y];
					Piece? pieceAtPosition = cellAtPosition.Piece;
					if (pieceAtPosition != null)
					{
						return true;
					}
				}
				return false;
			}
			if (Math.Abs(offset.X) > 1 && offset.Y == 0)
			{
				Vector2Int pos;
				for (int i = 1; i < Math.Abs(offset.X); i++)
				{
					pos = startCell.Position + new Vector2Int(offset.Y > 0 ? i : -i, 0);
					if (!IsCellPositionValid(pos)) continue;
					Cell cellAtPosition = Cells[pos.X, pos.Y];
					Piece? pieceAtPosition = cellAtPosition.Piece;
					if (pieceAtPosition != null)
					{
						return true;
					}
				}
				return false;
			}
			if (Math.Abs(offset.Y) > 1 && offset.X == 0)
			{
				Vector2Int pos;
				for (int i = 1; i < Math.Abs(offset.Y); i++)
				{
					pos = startCell.Position + new Vector2Int(0, offset.Y > 0 ? i : -i);
					if (!IsCellPositionValid(pos)) continue;
					Cell cellAtPosition = Cells[pos.X, pos.Y];
					Piece? pieceAtPosition = cellAtPosition.Piece;
					if (pieceAtPosition != null)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}
		public static bool IsCellPositionValid(Vector2Int position)
			=> !(position.X < 0 || position.X >= CELL_COUNT || position.Y < 0 || position.Y >= CELL_COUNT);

		public readonly static int CELL_COUNT = 8;
	}
}
