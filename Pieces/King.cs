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

			Movement = new PieceMovement(moves);
		}
	}
}
