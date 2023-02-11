using Chess.BoardNS;
using Chess.MathNS;
using Chess.MoveNS;
using Chess.PiecesNS.Base;

namespace Chess.PiecesNS
{
	[Piece(PieceType = PieceType.Knight)]
	public class Knight : Piece
	{
		public Knight(Cell cell, Team team) : base(cell, PieceType.Knight, team) { }
		protected override void InitMovement()
		{
			List<Move> moves = new List<Move>();

			Func<Board, Piece, Cell, bool> predicate = (board, piece, targetCell)
				=> targetCell.Piece == null || targetCell.Piece.Team != Team;
			moves.Add(new Move(this, new Vector2Int(1, 2), predicate));
			moves.Add(new Move(this, new Vector2Int(-1, -2), predicate));
			moves.Add(new Move(this, new Vector2Int(1, -2), predicate));
			moves.Add(new Move(this, new Vector2Int(-1, 2), predicate));
			moves.Add(new Move(this, new Vector2Int(2, 1), predicate));
			moves.Add(new Move(this, new Vector2Int(-2, -1), predicate));
			moves.Add(new Move(this, new Vector2Int(2, -1), predicate));
			moves.Add(new Move(this, new Vector2Int(-2, 1), predicate));

			Movement = new PieceMovement(moves);
		}
	}
}
