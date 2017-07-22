using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

	//GridPrefab has a size of 1 unit
	public GameObject gridPrefab;
	public GameObject inputPrefab;

	public int boardRows;
	public int boardColumns;

	private GridElement[,] board;
	private GameObject[] input;

	public static bool isPreview;
	private int[ , ] clonedValues;

	// Use this for initialization
	void Start () {
		board = new GridElement[boardRows, boardColumns];
		input = new GameObject[boardRows * 2 + boardColumns * 2];

		isPreview = false;
		previousPreviewPosition = -1;

		intitGameBoard ();
		initGameBoardInput ();
	}

	public void reset() {
		clonedValues = new int[boardRows, boardColumns];
		isPreview = false;
		previousPreviewPosition = -1;

		for (int i = 0; i < boardRows; i++) {
			for (int j = 0; j < boardColumns; j++) {
				board [i, j].setPlayer (GameManager.NONE);
				clonedValues [i, j] = 0;
			}
		}
	}

	private void initGameBoardInput() {
		float cameraSize = getCameraSize ();
		float scaleSprite = getScaleSprite (cameraSize);

		float boardX = (boardColumns + 2) * scaleSprite;
		float boardY = (boardRows + 2) * scaleSprite;		

		float spriteX, spriteY;

		for (int i = 0; i < 2 * boardRows + 2 * boardColumns; i++) {
			if (i < boardColumns) {
				//TOP
				spriteX = -(boardX / 2) + scaleSprite / 2 + scaleSprite * (i + 1); //i + 1 because we have to start from the 2nd position and not the first
				spriteY = (boardY / 2) - scaleSprite / 2;
			} else if (i < boardColumns + boardRows) {
				//RIGHT
				spriteX = (boardX / 2) - scaleSprite / 2;
				spriteY = (boardY / 2) - scaleSprite / 2 - scaleSprite * (i + 1 - boardColumns);
			} else if (i < boardColumns * 2 + boardRows) {
				//BOTTOM
				spriteX = (boardX / 2) - scaleSprite / 2 - scaleSprite * (i + 1 - boardColumns - boardRows);
				spriteY = -(boardY / 2) + scaleSprite / 2;
			} else {
				//LEFT
				spriteX = -(boardX / 2) + scaleSprite / 2;
				spriteY = -(boardY / 2) + scaleSprite / 2	 + scaleSprite * (i + 1 - boardColumns * 2 - boardRows);
			}

			GameObject inputObject = Instantiate (inputPrefab, new Vector3 (spriteX, spriteY, 0), Quaternion.identity);
			inputObject.transform.localScale = new Vector3 (scaleSprite, scaleSprite, scaleSprite);

			InputElement inputElement = inputObject.GetComponent<InputElement> ();
			inputElement.setPosition (i);
			inputElement.setGameBoard (this);

			input [i] = inputObject;
		}
	}

	private void intitGameBoard() {
		float cameraSize = getCameraSize ();

		float scaleSprite = getScaleSprite (cameraSize);

		float boardX = boardColumns * scaleSprite;
		float boardY = boardRows * scaleSprite;

		float spriteX, spriteY;

		for (int row = 0; row < boardRows; row++) {
			for (int column = 0; column < boardColumns; column++) {
				spriteX = -(boardX / 2) + scaleSprite / 2 + column * scaleSprite;
				spriteY = (boardY / 2) - scaleSprite / 2 - row * scaleSprite;

				GameObject gridObject = Instantiate(gridPrefab, new Vector3(spriteX, spriteY, 0), Quaternion.identity);
				gridObject.transform.localScale = new Vector3 (scaleSprite, scaleSprite, scaleSprite);

				board [row, column] = gridObject.GetComponent<GridElement>();
			}
		}
	}

	private float getScaleSprite(float gameBoardView) {
		if (boardColumns >= boardRows) {
			return (gameBoardView / (boardColumns + 2));
		} else {
			return (gameBoardView / (boardRows + 2));
		}
	}

	/**
	 * @return the size of the camera in unity units (the lower value from width and height)
	*/
	private float getCameraSize() {
		float screenAspect = (float) Screen.width / (float) Screen.height;
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = screenAspect * camHalfHeight;
		if (camHalfHeight >= camHalfWidth) {
			return camHalfWidth * 2.0f;
		} else {
			return camHalfHeight * 2.0f - 2;
		}
	}

	public void insert(int position, int player) {
		if(position < boardColumns) { //on top
			insertIntoColumn(position, true, player);
		} else if(position < boardColumns + boardRows) { //right side
			insertIntoRow(position - boardColumns, false, player);
		} else if(position < boardColumns * 2 + boardRows) { //bottom
			insertIntoColumn(boardColumns * 2 + boardRows - position - 1, false, player);
		} else if(position < boardColumns * 2 + boardRows * 2) { //left side
			insertIntoRow(boardColumns * 2 + boardRows * 2 - position - 1, true, player);
		}
	}

	public bool canInsert(int position) {
		if(position < boardColumns) { //on top
			return !fullColumn(position);
		} else if(position < boardColumns + boardRows) { //right side
			return !fullRow(position - boardColumns);
		} else if(position < boardColumns * 2 + boardRows) { //bottom
			return !fullColumn(boardColumns * 2 + boardRows - position - 1);
		} else if(position < boardColumns * 2 + boardRows * 2) { //left side
			return !fullRow(boardColumns * 2 + boardRows * 2 - position - 1);
		}

		return false;
	}

	private bool fullColumn(int column) {
		for(int i = 0; i < boardRows; i++) {
			if(board[i, column].player != GameManager.FIRSTPLAYER && board[i, column].player != GameManager.SECONDPLAYER) {
				return false;
			}
		}

		return true;
	}

	private bool fullRow(int row) {
		for(int i = 0; i < boardColumns; i++) {
			if(board[row, i].player != GameManager.FIRSTPLAYER && board[row, i].player != GameManager.SECONDPLAYER) {
				return false;
			}
		}

		return true;
	}

	private void insertIntoRow(int row, bool toRight, int value) {
		int lastFreeRow;

		if(toRight) {
			lastFreeRow = boardRows - 1;
			for(int i = lastFreeRow; i >= 0; i--) {
				if( board[row, i].player == 0 ) {
					//Search for next non 0 field
					for(int j = i; j>= 0; j--) {
						if(board[row, j].player != 0) {
							board[row, i].setPlayer(board[row, j].player);
							board [row, j].setPlayer (0);
							break;
						}
					}
				}
			}

			while(board[row, lastFreeRow].player != 0) {
				lastFreeRow--;
			}
		} else {
			lastFreeRow = 0;
			for(int i = 0; i < boardRows; i++) {
				if( board[row, i].player == 0 ) {
					//Search for next non 0 field
					for(int j = i; j < boardRows; j++) {
						if(board[row, j].player != 0) {
							board[row, i].setPlayer(board[row, j].player);
							board [row, j].setPlayer (0);
							break;
						}
					}
				}
			}

			while(board[row, lastFreeRow].player != 0) {
				lastFreeRow++;
			}
		}

		if(board[row, lastFreeRow].player == 0) {
			board[row, lastFreeRow].setPlayer(value);
		}
	}

	private void insertIntoColumn(int column, bool toBottom, int value) {
		int lastFreeColumn;

		if(toBottom) {
			lastFreeColumn = boardColumns - 1;
			for(int i = boardColumns - 1; i >= 0; i--) {
				if( board[i, column].player == 0 ) {
					//Search for next non 0 field
					for(int j = i; j>= 0; j--) {
						if(board[j, column].player != 0) {
							board[i, column].setPlayer(board[j, column].player);
							board[j, column].setPlayer(0);
							break;
						}
					}
				}
			}

			while(board[lastFreeColumn, column].player != 0) {
				lastFreeColumn--;
			}
		} else {
			lastFreeColumn = 0;
			for(int i = 0; i < boardColumns; i++) {
				if( board[i, column].player == 0 ) {
					//Search for next non 0 field
					for(int j = i; j < boardColumns; j++) {
						if(board[j, column].player != 0) {
							board[i, column].setPlayer(board[j, column].player);
							board[j, column].setPlayer(0);
							break;
						}
					}
				}
			}
			while(board[lastFreeColumn, column].player != 0) {
				lastFreeColumn++;
			}
		}
			
		if(board[lastFreeColumn, column].player == 0) {
			board[lastFreeColumn, column].setPlayer(value);
		}
	}

	public int calculateWinner(GameBoard gameBoard) {
		GridElement[ , ] board = gameBoard.getGameBoard ();
		int rows = gameBoard.boardRows;
		int columns = gameBoard.boardColumns;

		int x,y, prev, count, playerAtField, rowCounter;
		bool firstPlayerWon = false;
		bool secondPlayerWon = false;

		//check rows
		for(x = 0; x < columns; x++) {
			count = 1;
			prev = 0;
			for(y = 0; y < rows; y++) {
				playerAtField = board[y, x].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == 1) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}
			}
		}

		//check columns
		for(y = 0; y < rows; y++) {
			count = 1;
			prev = 0;
			for(x = 0; x < columns; x++) {
				playerAtField = board[y, x].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == 1) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}
			}
		}

		//check diagonal
		for(x = 0; x < columns; x++) {
			count = 1;
			prev = 0;
			rowCounter = 0;
			while(x + rowCounter < columns && rowCounter < rows) {
				playerAtField = board[rowCounter, x + rowCounter].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == 1) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}
		for(y = 0; y < rows; y++) {
			count = 1;
			prev = 0;
			rowCounter = 0;
			while(x + rowCounter < rows && rowCounter < columns) {
				playerAtField = board[y + rowCounter, rowCounter].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == 1) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}

		for(x = 0; x < columns; x++) {
			count = 1;
			prev = 0;
			rowCounter = 0;
			while(x - rowCounter >= 0 && rowCounter < rows) {
				playerAtField = board[rowCounter, x - rowCounter].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == 1) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}
		for(y = 0; y < rows; y++) {
			count = 1;
			prev = 0;
			rowCounter = 0;
			while(y - rowCounter >= 0 && rowCounter < columns) {
				playerAtField = board[y - rowCounter, rowCounter].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == 1) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}

		if (!movePossible() || (firstPlayerWon && secondPlayerWon)) {
			return GameManager.DRAW;
		} else if (firstPlayerWon) {
			return GameManager.FIRSTPLAYER;
		} else if (secondPlayerWon) {
			return GameManager.SECONDPLAYER;
		} else {
			return GameManager.NONE;
		}
	}

	private bool movePossible() {
		for (int row = 0; row < 2 * (boardRows + boardColumns); row++) {
			if (canInsert (row)) {
				return true;
			}
		}

		return false;
	}

	int previousPreviewPosition;

	public void showPreview(int position, int player) {
		if (player == GameManager.FIRSTPLAYER) {
			player = GameManager.FIRSTPLAYERPEV;
		} else if(player == GameManager.SECONDPLAYER){
			player = GameManager.SECONDPLAYERPREV;
		}

		if (isPreview && previousPreviewPosition == position) {
			//Preview did not change -> do nothing
		} else if(isPreview && previousPreviewPosition != position) {
			//Preview positon changed, reset field to original and insert preview
			loadElements (clonedValues);

			insert (position, player);
			previousPreviewPosition = position;
		} else {
			previousPreviewPosition = position;
			clonedValues = saveElements (board);

			insert (position, player);

			isPreview = true;
		}
	}

	public void cancelPreview() {
		if (isPreview) {
			isPreview = false;
			if (clonedValues != null)
				loadElements (clonedValues);
		}
	}

	public GridElement[,] getGameBoard() {
		return board;
	}

	private int[,] saveElements(GridElement[,] array) {
		int[,] clone = new int[ boardRows, boardColumns];
		for (int i = 0; i < boardRows; i++) {
			for (int j = 0; j < boardColumns; j++) {
				clone [i, j] = array [i, j].player;
			}
		}

		return clone;
	}

	private void loadElements(int[,] saved) {
		for (int i = 0; i < boardRows; i++) {
			for (int j = 0; j < boardColumns; j++) {
				board [i, j].setPlayerWithoutAnimation(saved[i, j]);
			}
		}
	}
}