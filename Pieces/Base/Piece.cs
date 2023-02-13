using Chess.BoardNS;
using Chess.MathNS;
using Chess.MoveNS;
using SFML.Graphics;

namespace Chess.PiecesNS.Base
{
	public abstract class Piece
	{
		private Texture _texture = null!;
		private Sprite _sprite = null!;
		public Sprite Sprite => _sprite;

		public Cell Cell { get; private set; }

		public PieceType Type { get; protected set; }
		public Team Team { get; }
		public Vector2Int Position => Cell.Position;
		public bool Moved { get; set; } = false;
		protected PieceMovement Movement { get; set; }

		public Piece(Cell cell, PieceType type, Team team)
		{
			Cell = cell;
			Type = type;
			Team = team;

			InitMovement();
			LoadSprite();
		}
		protected abstract void InitMovement();
		public bool CanMoveToCell(Board board, Cell cell)
		{
			Vector2Int offset = cell.Position - Position;

			Move? move = Movement.GetMoveByOffset(offset);
			if (move != null) return move.IsMoveAllowed(board);

			return false;
		}
		public IEnumerable<Cell> GetAllowedCells(Board board)
		{
			return Movement.GetAvailableMoves(board).Select(x =>
			{
				Vector2Int targetPosition = Position + x.Offset;
				return board.Cells[targetPosition.X, targetPosition.Y];
			});
		}
		public IEnumerable<Move> GetAllowedMoves(Board board)
		{
			return Movement.GetAvailableMoves(board);
		}

		public void SetCell(Cell cell)
		{
			Cell = cell;
		}
		private void LoadSprite()
		{
			_texture = new Texture($"Resources/{(Team == Team.White ? "w" : "b")}_{Type}.png");
			_sprite = new Sprite(_texture);
		}
	}
}
