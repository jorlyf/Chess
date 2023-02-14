using Chess.BoardNS;
using Chess.MathNS;
using Chess.MoveNS;
using Chess.PiecesNS.Base;

namespace Chess.PiecesNS
{
	[Piece(PieceType = PieceType.King)]
	public class King : Piece
	{
		public King(Cell cell, Team team) : base(cell, PieceType.King, team) { }
		protected override void InitMovement()
		{
			List<Move> moves = new List<Move>();

			Func<Board, Piece, Cell, bool> predicate = (board, piece, targetCell)
				=> targetCell.Piece == null || targetCell.Piece.Team != Team;

			moves.Add(new Move(this, new Vector2Int(1, 1), predicate));
			moves.Add(new Move(this, new Vector2Int(-1, 1), predicate));
			moves.Add(new Move(this, new Vector2Int(1, -1), predicate));
			moves.Add(new Move(this, new Vector2Int(-1, -1), predicate));
			moves.Add(new Move(this, new Vector2Int(0, 1), predicate));
			moves.Add(new Move(this, new Vector2Int(1, 0), predicate));
			moves.Add(new Move(this, new Vector2Int(0, -1), predicate));
			moves.Add(new Move(this, new Vector2Int(-1, 0), predicate));

			// Castling
			Func<Board, Piece, Cell, bool> castlingPredicate = (board, piece, targetCell) =>
			{
				Vector2Int offset = targetCell.Position - piece.Position;
				Vector2Int rookPosition = offset.X > 0 ?
					new Vector2Int(7, piece.Position.Y) :
					new Vector2Int(0, piece.Position.Y);
				Cell rookCell = board.Cells[rookPosition.X, rookPosition.Y];
				if (piece.Moved || rookCell.Piece == null || rookCell.Piece.Moved) return false;

				if (board.AreBlockingPiecesOnWay(piece.Cell, targetCell)) return false;

				for (int i = 1; i <= 2; i++)
				{
					Vector2Int pos = piece.Position + (offset.X > 0 ? new Vector2Int(i, 0) : new Vector2Int(-i, 0));
					Cell cellAtPos = board.Cells[pos.X, pos.Y];
					if (board.IsCellUnderAttack(board, cellAtPos, piece.Team)) return false;
				}

				return true;
			};

			moves.Add(new Move(this, new Vector2Int(2, 0), castlingPredicate));
			moves.Add(new Move(this, new Vector2Int(-2, 0), castlingPredicate));

			Movement = new PieceMovement(moves);
		}
	}
}
