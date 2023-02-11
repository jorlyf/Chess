using Chess.BoardNS;
using Chess.MathNS;
using Chess.MoveNS;
using Chess.PiecesNS.Base;

namespace Chess.PiecesNS
{
	[Piece(PieceType = PieceType.Pawn)]
	public class Pawn : Piece
	{
		public Pawn(Cell cell, Team team) : base(cell, PieceType.Pawn, team) { }
		protected override void InitMovement()
		{
			List<Move> moves = new List<Move>();
			if (Team == Team.White)
			{
				moves.Add(new Move(this, new Vector2Int(0, 1), (board, piece, targetCell) =>
				{
					return Team == Team.White && targetCell.Piece == null;
				}));
				moves.Add(new Move(this, new Vector2Int(0, 2), (board, piece, targetCell) =>
				{
					return Team == Team.White && targetCell.Piece == null && !piece.Moved;
				}));
				moves.Add(new Move(this, new Vector2Int(-1, 1), (board, piece, targetCell) =>
				{
					return Team == Team.White && targetCell.Piece != null && targetCell.Piece.Team != piece.Team;
				}));
				moves.Add(new Move(this, new Vector2Int(1, 1), (board, piece, targetCell) =>
				{
					return Team == Team.White && targetCell.Piece != null && targetCell.Piece.Team != piece.Team;
				}));
			}
			else
			{
				moves.Add(new Move(this, new Vector2Int(0, -1), (board, piece, targetCell) =>
				{
					return Team == Team.Black && targetCell.Piece == null;
				}));
				moves.Add(new Move(this, new Vector2Int(0, -2), (board, piece, targetCell) =>
				{
					return Team == Team.Black && targetCell.Piece == null && !piece.Moved;
				}));
				moves.Add(new Move(this, new Vector2Int(-1, -1), (board, piece, targetCell) =>
				{
					return Team == Team.Black && targetCell.Piece != null && targetCell.Piece.Team != piece.Team;
				}));
				moves.Add(new Move(this, new Vector2Int(1, -1), (board, piece, targetCell) =>
				{
					return Team == Team.Black && targetCell.Piece != null && targetCell.Piece.Team != piece.Team;
				}));
			}
			Movement = new PieceMovement(moves);
		}
	}
}
