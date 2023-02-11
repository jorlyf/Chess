using Chess.MathNS;
using Chess.PiecesNS.Base;

namespace Chess.BoardNS
{
	public class Cell
	{
		public Vector2Int Position { get; }
		public Piece? Piece { get; private set; }

		public Cell(Vector2Int position)
		{
			Position = position;
		}

		public Piece CreatePiece(PieceType pieceType, Team team)
		{
			Type? type = Type.GetType($"Chess.PiecesNS.{pieceType}");
			if (type == null) throw new ArgumentException($"PieceType {pieceType} is not valid.");

			object[]? pieceAttribute = type.GetCustomAttributes(typeof(PieceAttribute), false);
			if (pieceAttribute.Length == 0) throw new Exception("Piece don't have the PieceAttribute.");

			if (((PieceAttribute)pieceAttribute[0]).PieceType != pieceType) throw new Exception("Piece have another piece type.");

			Piece = (Piece)Activator.CreateInstance(type, new object[] { this, team })!;
			return Piece;
		}
		public void SetPiece(Piece piece)
		{
			Piece = piece;
		}
		public void RemovePiece()
		{
			Piece = null;
		}
		public Cell Clone()
		{
			Cell copy = new Cell(new Vector2Int(Position.X, Position.Y));

			if (Piece != null)
			{
				copy.Piece = copy.CreatePiece(Piece.Type, Piece.Team);
			}

			return copy;
		}
	}
}
