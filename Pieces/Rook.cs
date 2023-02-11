using Chess.BoardNS;
using Chess.MathNS;
using Chess.MoveNS;
using Chess.PiecesNS.Base;

namespace Chess.PiecesNS
{
	[Piece(PieceType = PieceType.Rook)]
	public class Rook : Piece
	{
		public Rook(Cell cell, Team team) : base(cell, PieceType.Rook, team) { }
		protected override void InitMovement()
		{
			List<Move> moves = new List<Move>();

			Func<Board, Piece, Cell, bool> predicate = (board, piece, targetCell)
				=> !board.AreBlockingPiecesOnWay(Cell, targetCell) && (targetCell.Piece == null || targetCell.Piece.Team != Team);

			for (int step = 1; step < Board.CELL_COUNT; step++)
			{
				Vector2Int offset;
				offset = new Vector2Int(0, step);
				moves.Add(new Move(this, offset, predicate));
				offset = new Vector2Int(0, -step);
				moves.Add(new Move(this, offset, predicate));
				offset = new Vector2Int(step, 0);
				moves.Add(new Move(this, offset, predicate));
				offset = new Vector2Int(-step, 0);
				moves.Add(new Move(this, offset, predicate));
			}

			Movement = new PieceMovement(moves);
		}
	}
}
