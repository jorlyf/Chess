using Chess.MathNS;
using Chess.MoveNS;
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

		public bool IsGameEnded { get; private set; } = false;

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
			Cell movedCell = nextMoveBoard.Cells[piece.Position.X, piece.Position.Y];
			Cell targetCell = nextMoveBoard.Cells[target.Position.X, target.Position.Y];
			if (targetCell.Piece != null)
			{
				nextMoveBoard.Pieces.Remove(targetCell.Piece);
				targetCell.RemovePiece();
			}
			if (movedCell.Piece != null)
			{
				Piece newPiece = targetCell.CreatePiece(movedCell.Piece.Type, movedCell.Piece.Team);
				nextMoveBoard.Pieces.Add(movedCell.Piece);
				if (newPiece is King newKing)
				{
					if (newKing.Team == Team.White)
						nextMoveBoard._whiteKing = newKing;
					else nextMoveBoard._blackKing = newKing;
				}

				nextMoveBoard.Pieces.Remove(movedCell.Piece);
				movedCell.RemovePiece();
			}

			return nextMoveBoard;
		}
		public bool CanMovePiece(Piece piece, Cell target)
		{
			if (CurrentMoveTeam != piece.Team) return false;

			if (!piece.CanMoveToCell(this, target)) return false;
			Board nextMoveBoard = GetNextMoveBoard(piece, target);
			if (IsKingUnderAttack(nextMoveBoard, piece.Team)) return false;

			return true;
		}
		public void MovePiece(Piece piece, Cell target)
		{
			Vector2Int offset = target.Position - piece.Position;

			Team moveTeamAfterThis = CurrentMoveTeam == Team.White ? Team.Black : Team.White;

			piece.Cell.RemovePiece();
			piece.SetCell(target);
			target.SetPiece(piece);
			piece.Moved = true;

			if (piece is King && Math.Abs(offset.X) == 2) // castling
			{
				Vector2Int rookPosition = target.Position + (offset.X > 0 ? new Vector2Int(1, 0) : new Vector2Int(-2, 0));
				Cell rookCell = Cells[rookPosition.X, rookPosition.Y];
				Piece? rook = rookCell.Piece;
				if (rook == null) throw new Exception("MovePiece castling. Rook is null.");
				rookCell.RemovePiece();
				rook.Moved = true;

				Vector2Int newRookPosition = target.Position + (offset.X > 0 ? new Vector2Int(-1, 0) : new Vector2Int(1, 0));
				Cell newRookCell = Cells[newRookPosition.X, newRookPosition.Y];
				newRookCell.SetPiece(rook);
				rook.SetCell(newRookCell);
			}

			// checkmate checking
			if (IsKingUnderAttack(this, moveTeamAfterThis))
			{
				if (IsCheckMate(this, moveTeamAfterThis))
				{
					Console.WriteLine($"GG! {CurrentMoveTeam} win.");
					IsGameEnded = true;
				}
			}

			CurrentMoveTeam = moveTeamAfterThis;
		}
		public bool IsKingUnderAttack(Board board, Team team)
		{
			King king = team == Team.White ? board._whiteKing : board._blackKing;

			for (int i = 0; i < board.Pieces.Count; i++)
			{
				Piece piece = board.Pieces[i];
				if (piece.Team == king.Team) continue;

				if (piece.CanMoveToCell(board, king.Cell)) return true;
			}

			return false;
		}
		public bool IsCellUnderAttack(Board board, Cell cell, Team team)
		{
			for (int i = 0; i < board.Pieces.Count; i++)
			{
				Piece piece = board.Pieces[i];
				if (piece.Team == team) continue;

				if (piece.CanMoveToCell(board, cell)) return true;
			}

			return false;
		}
		public bool IsCheckMate(Board board, Team team)
		{
			King king = team == Team.White ? _whiteKing : _blackKing;

			IEnumerable<Cell> kingMoves = GetAllowedPieceMoves(king);
			if (kingMoves.Count() == 0)
			{
				int count = 0;
				for (int i = 0; i < board.Pieces.Count; i++)
				{
					Piece piece = board.Pieces[i];
					if (piece.Team != team) continue;
					IEnumerable<Cell> pieceMoves = GetAllowedPieceMoves(piece);
					count += pieceMoves.Count();
				}
				if (count == 0)
				{
					return true;
				}
			}

			return false;
		}
		public IEnumerable<Cell> GetAllowedPieceMoves(Piece piece)
		{
			if (piece.Team != CurrentMoveTeam) return Enumerable.Empty<Cell>();
			IEnumerable<Move> moves = piece.GetAllowedMoves(this);
			moves = moves.Where(x =>
			{
				Vector2Int targetPiecePosition = piece.Position + x.Offset;
				Cell targetCell = Cells[targetPiecePosition.X, targetPiecePosition.Y];
				return CanMovePiece(piece, targetCell);
			});
			return moves.Select(x =>
			{
				Vector2Int targetPiecePosition = piece.Position + x.Offset;
				return Cells[targetPiecePosition.X, targetPiecePosition.Y];
			});
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
						if (_cells[x, y].Piece is King)
						{
							if (_cells[x, y].Piece!.Team == Team.White) _whiteKing = (King)_cells[x, y].Piece!;
							else _blackKing = (King)_cells[x, y].Piece!;
						}
					}
				}
			}
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
					pos = startCell.Position + new Vector2Int(offset.X > 0 ? i : -i, 0);
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
