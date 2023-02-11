using Chess.BoardNS;
using Chess.MathNS;
using Chess.MoveNS;
using Chess.PiecesNS.Base;

namespace Chess.PiecesNS
{
	[Piece(PieceType = PieceType.Bishop)]
	public class Bishop : Piece
	{
		public Bishop(Cell cell, Team team) : base(cell, PieceType.Bishop, team) { }
		protected override void InitMovement()
		{
			List<Move> moves = new List<Move>();
			Func<Board, Piece, Cell, bool> predicate = (board, piece, targetCell)
				=> !board.AreBlockingPiecesOnWay(Cell, targetCell) && (targetCell.Piece == null || targetCell.Piece.Team != Team);

			for (int step = 1; step < Board.CELL_COUNT; step++)
			{
				for (int i = 0; i <= 1; i++)
					for (int j = 0; j <= 1; j++)
					{
						Vector2Int offset = new Vector2Int(i == 0 ? step : -step, j == 0 ? step : -step);
						moves.Add(new Move(this, offset, predicate));
					}
			}

			Movement = new PieceMovement(moves);
		}
	}
}
