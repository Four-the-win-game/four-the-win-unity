using System;

public class HardKI : Player {

	private int rows;
	private int columns;

	private GameBoard board;

	public HardKI () {

	}

	public int getNextMove (int player, GameBoard gameBoard) {
		columns = gameBoard.boardColumns;
		rows = gameBoard.boardRows;

		board = gameBoard.clone ();


		return 0;
	}

	public String getName() {
		return "Hard AI";
	}
}

