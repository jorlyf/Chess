using Chess.BoardNS;
using Chess.MathNS;
using Chess.PiecesNS.Base;
using Chess.RendererNS;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Chess");
window.SetFramerateLimit(16);
window.Resized += (obj, e) =>
{
	window.SetView(new View(new Vector2f(e.Width / 2, e.Height / 2), new Vector2f(e.Width, e.Height)));
};

Board board = new Board();
Renderer renderer = new Renderer(window, board);

board.InitPieces();

window.Closed += (obj, e) => window.Close();
window.MouseButtonReleased += (obj, e) =>
{
	if (e.Button == Mouse.Button.Left && !board.IsGameEnded)
	{
		Vector2Int mousePos = renderer.GetActualMousePosition();
		if (mousePos.X >= Renderer.CellSize * Board.CELL_COUNT || mousePos.Y >= Renderer.CellSize * Board.CELL_COUNT) return;

		Cell? previousSelectedCell = board.SelectedCell;
		Vector2Int selectedCellPosition = new Vector2Int(mousePos.X / Renderer.CellSize, mousePos.Y / Renderer.CellSize);
		if (!Board.IsCellPositionValid(selectedCellPosition)) return;
		Cell? selectedCell = board.Cells[selectedCellPosition.X, selectedCellPosition.Y];
		if (selectedCell == null) return;

		if (selectedCell == previousSelectedCell)
		{
			board.DeselectCell();
			return;
		}

		Piece? previousSelectedPiece = null;
		if (previousSelectedCell != null)
		{
			previousSelectedPiece = previousSelectedCell.Piece;
		}
		Piece? selectedPiece = null;
		if (selectedCell != null)
		{
			selectedPiece = selectedCell.Piece;
		}
		if (previousSelectedPiece != null && selectedCell != null)
		{
			if (board.CanMovePiece(previousSelectedPiece, selectedCell))
			{
				board.MovePiece(previousSelectedPiece, selectedCell);
				board.DeselectCell();
				return;
			}
		}
		if (selectedCell != null)
			board.SelectCell(selectedCell);
	}
};

while (window.IsOpen)
{
	window.DispatchEvents();

	renderer.Render();
}