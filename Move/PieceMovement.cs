using Chess.BoardNS;
using Chess.MathNS;

namespace Chess.MoveNS
{
	public class PieceMovement
	{
		private List<Move> _moves;

		public PieceMovement(List<Move> moves)
		{
			_moves = moves;
		}

		public Move? GetMoveByOffset(Vector2Int offset) => _moves.Where(x => x.Offset == offset).FirstOrDefault();
		public IEnumerable<Move> GetAvailableMoves(Board board)
		{
			return _moves.Where(x => x.IsMoveAllowed(board));
		}
	}
}
