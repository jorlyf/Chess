using Chess.BoardNS;
using Chess.MathNS;
using Chess.PiecesNS.Base;

namespace Chess.MoveNS
{
	public class Move
	{
		private Piece _piece;
		public Vector2Int Offset { get; }
		public Func<Board, Piece, Cell, bool> Predicate { get; }

		public Move(Piece piece, Vector2Int offset, Func<Board, Piece, Cell, bool> predicate)
		{
			_piece = piece;
			Offset = offset;
			Predicate = predicate;
		}

		public virtual bool IsMoveAllowed(Board board)
		{
			Vector2Int targetPosition = _piece.Position + Offset;
			if (!Board.IsCellPositionValid(targetPosition)) return false;
			Cell targetCell = board.Cells[targetPosition.X, targetPosition.Y];
			return Predicate.Invoke(board, _piece, targetCell);
		}
	}
}
