namespace Chess.MathNS
{
	public class Vector2Int
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Vector2Int(int x = 0, int y = 0)
		{
			X = x;
			Y = y;
		}

		public static bool operator ==(Vector2Int a, Vector2Int b)
		{
			return a.X == b.X && a.Y == b.Y;
		}
		public static bool operator !=(Vector2Int a, Vector2Int b)
		{
			return !(a == b);
		}
		public static Vector2Int operator -(Vector2Int vector)
		{
			return new Vector2Int { X = -vector.X, Y = -vector.Y };
		}
		public static Vector2Int operator +(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.X + b.X, a.Y + b.Y);
		}
		public static Vector2Int operator -(Vector2Int a, Vector2Int b)
		{
			return a + (-b);
		}
		public override string ToString()
		{
			return $"{X} {Y}";
		}
	}
}
