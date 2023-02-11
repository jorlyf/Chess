using Chess.BoardNS;
using Chess.MathNS;
using Chess.MoveNS;
using Chess.PiecesNS.Base;

namespace Chess.PiecesNS
{
	[Piece(PieceType = PieceType.Queen)]
	public class Queen : Piece
	{
		public Queen(Cell cell, Team team) : base(cell, PieceType.Queen, team) { }
		protected override void InitMovement()
		{
			List<Move> moves = new List<Move>();

			Func<Board, Piece, Cell, bool> rookPredicate = (board, piece, targetCell)
				=> !board.AreBlockingPiecesOnWay(Cell, targetCell) && (targetCell.Piece == null || targetCell.Piece.Team != Team);

			for (int step = 1; step < Board.CELL_COUNT; step++)
			{
				Vector2Int offset;
				offset = new Vector2Int(0, step);
				moves.Add(new Move(this, offset, rookPredicate));
				offset = new Vector2Int(0, -step);
				moves.Add(new Move(this, offset, rookPredicate));
				offset = new Vector2Int(step, 0);
				moves.Add(new Move(this, offset, rookPredicate));
				offset = new Vector2Int(-step, 0);
				moves.Add(new Move(this, offset, rookPredicate));
			}

			Func<Board, Piece, Cell, bool> bishopPredicate = (board, piece, targetCell)
				=> !board.AreBlockingPiecesOnWay(Cell, targetCell) && (targetCell.Piece == null || targetCell.Piece.Team != Team);

			for (int step = 1; step < Board.CELL_COUNT; step++)
			{
				for (int i = 0; i <= 1; i++)
					for (int j = 0; j <= 1; j++)
					{
						Vector2Int offset = new Vector2Int(i == 0 ? step : -step, j == 0 ? step : -step);
						moves.Add(new Move(this, offset, bishopPredicate));
					}
			}

			Movement = new PieceMovement(moves);
		}
	}
}
