namespace Chess.PiecesNS.Base
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PieceAttribute : Attribute
	{
		public PieceType PieceType { get; set; }
	}
}
