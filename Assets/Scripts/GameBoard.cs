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

	// Use this for initialization
	void Start () {
		board = new GridElement[boardRows, boardColumns];
		input = new GameObject[boardRows * 2 + boardColumns * 2];

		intitGameBoard ();
		initGameBoardInput ();
	}

	private void initGameBoardInput() {
		float cameraSize = getCameraSize ();
		float scaleSprite = getScaleSprite (cameraSize - 2);

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
		float gameBoardView = cameraSize - 2; //-2 because we want to have a gap left, right, up and down from one unit

		float scaleSprite = getScaleSprite (gameBoardView);

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
			return (gameBoardView / boardColumns);
		} else {
			return (gameBoardView / boardRows);
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

	public GridElement[,] getGameBoard() {
		return board;
	}
}