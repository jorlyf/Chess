using Chess.BoardNS;
using Chess.MathNS;
using Chess.PiecesNS.Base;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Chess.RendererNS
{
	public class Renderer
	{
		private RenderWindow _window;
		private Board _board;

		public static int CellSize { get; private set; }
		private static Vector2f _pieceSpriteScale;

		private static Color _whiteCellColor = new Color(240, 217, 181);
		private static Color _blackCellColor = new Color(181, 136, 99);
		private static Color _selectedCellColor = new Color(0, 139, 139);


		public Renderer(RenderWindow window, Board board)
		{
			_window = window;
			_board = board;

			UpdateSizes(_window.Size.X, _window.Size.Y);
			_window.Resized += (obj, e) => UpdateSizes(e.Width, e.Height);
		}

		public void Render()
		{
			_window.Clear();

			RenderBoardCells();
			RenderSelectedBoardCell();

			_window.Display();
		}
		public Vector2Int GetActualMousePosition()
		{
			Vector2i pos = Mouse.GetPosition(_window);

			Vector2Int position = new Vector2Int(pos.X, (int)_window.Size.Y - pos.Y);
			return position;
		}

		private void RenderBoardCells()
		{
			Cell[,] cells = _board.Cells;

			for (int x = 0; x < Board.CELL_COUNT; x++)
			{
				for (int y = 0; y < Board.CELL_COUNT; y++)
				{
					Color cellColor = (x + y) % 2 == 0 ? _whiteCellColor : _blackCellColor;

					RectangleShape rect = new RectangleShape(new Vector2f(CellSize, CellSize));
					rect.FillColor = cellColor;
					rect.Position = new Vector2f(x * CellSize, (Board.CELL_COUNT - 1 - y) * CellSize);

					_window.Draw(rect);

					Cell cell = cells[x, y];
					if (cell.Piece != null) RenderPiece(cell.Piece);
				}
			}
		}
		private void RenderSelectedBoardCell()
		{
			Cell? cell = _board.SelectedCell;
			if (cell == null) return;

			float radius = (CellSize - 25) / 2;

			CircleShape circle = new CircleShape(radius);
			circle.FillColor = Color.Transparent;
			circle.OutlineThickness = 3.0f;
			circle.OutlineColor = _selectedCellColor;
			Vector2f position = new Vector2f(cell.Position.X * CellSize, (Board.CELL_COUNT - 1 - cell.Position.Y) * CellSize);
			position += new Vector2f(CellSize / 2, CellSize / 2);
			circle.Origin = new Vector2f(radius, radius);
			circle.Position = position;
			_window.Draw(circle);

			if (cell.Piece == null) return;

			IEnumerable<Cell> allowedMoves = _board.GetAllowedPieceMoves(cell.Piece);
			radius = (CellSize - 25) / 4;
			for (int i = 0; i < allowedMoves.Count(); i++)
			{
				Cell allowedCell = allowedMoves.ElementAt(i);
				CircleShape moveShape = new CircleShape(radius);
				moveShape.FillColor = Color.Transparent;
				moveShape.OutlineThickness = 2.0f;
				moveShape.OutlineColor = Color.Red;
				Vector2f pos = new Vector2f(allowedCell.Position.X * CellSize, (Board.CELL_COUNT - 1 - allowedCell.Position.Y) * CellSize);
				pos += new Vector2f(CellSize / 2, CellSize / 2);
				moveShape.Origin = new Vector2f(radius, radius);
				moveShape.Position = pos;
				_window.Draw(moveShape);
			}
		}
		private void RenderPiece(Piece piece)
		{
			Sprite sprite = piece.Sprite;
			Texture texture = sprite.Texture;

			Vector2f position = new Vector2f(
				piece.Cell.Position.X * CellSize - texture.Size.X * _pieceSpriteScale.X / 2 + CellSize / 2,
				(Board.CELL_COUNT - 1 - piece.Cell.Position.Y) * CellSize - texture.Size.X * _pieceSpriteScale.X / 2 + CellSize / 2);
			sprite.Position = position;
			sprite.Scale = _pieceSpriteScale;

			_window.Draw(sprite);
		}
		private void UpdateSizes(uint width, uint height)
		{
			CellSize = (int)Math.Min(width / Board.CELL_COUNT, height / Board.CELL_COUNT);

			float textureSize = 60.0f;
			float spriteScale = CellSize / textureSize;
			_pieceSpriteScale = new Vector2f(spriteScale, spriteScale);
		}
	}
}
