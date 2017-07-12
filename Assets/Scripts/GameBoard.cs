using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

	//GridPrefab has a size of 1 unit
	public GameObject gridPrefab;
	public GameObject inputPrefab;

	public int boardRows;
	public int boardColumns;

	private GameObject[,] board;
	private GameObject[] input;

	// Use this for initialization
	void Start () {
		board = new GameObject[boardRows, boardColumns];
		input = new GameObject[boardRows * 2 + boardColumns * 2];

		intitGameBoard ();
		initGameBoardInput ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void initGameBoardInput() {
		float cameraSize = getCameraSize ();
		float scaleSprite = getScaleSprite (cameraSize - 2);

		float spriteX, spriteY;

		for (int i = 0; i < 2 * boardRows + 2 * boardColumns; i++) {
			if (i < boardColumns) {
				//TOP
				spriteX = -(cameraSize / 2) + scaleSprite * 1.5f + scaleSprite * (i);
				spriteY = (cameraSize / 2) - scaleSprite / 2;
			} else if (i < boardColumns + boardRows) {
				//RIGHT
				spriteX = (cameraSize / 2) - scaleSprite / 2;
				spriteY = (cameraSize / 2) - scaleSprite * 1.5f - scaleSprite * (i - boardColumns);
			} else if (i < boardColumns * 2 + boardRows) {
				//BOTTOM
				spriteX = (cameraSize / 2) - scaleSprite * 1.5f - scaleSprite * (i - boardColumns - boardRows);
				spriteY = -(cameraSize / 2) + scaleSprite / 2;
			} else {
				//LEFT
				spriteX = -(cameraSize / 2) + scaleSprite / 2;
				spriteY = -(cameraSize / 2) + scaleSprite * 1.5f + scaleSprite * (i - boardColumns * 2 - boardRows);
			}

			GameObject inputObject = Instantiate (inputPrefab, new Vector3 (spriteX, spriteY, 0), Quaternion.identity);
			inputObject.transform.localScale = new Vector3 (scaleSprite, scaleSprite, scaleSprite);

			//TODO add i to gameObject

			input [i] = inputObject;
		}
	}

	private void intitGameBoard() {
		float cameraSize = getCameraSize ();
		float gameBoardView = cameraSize - 2; 

		float scaleSprite = getScaleSprite (gameBoardView);

		float spriteX, spriteY;

		for (int row = 0; row < boardRows; row++) {
			for (int column = 0; column < boardColumns; column++) {
				spriteX = -(gameBoardView / 2) + scaleSprite / 2 + column * scaleSprite;
				spriteY = -(gameBoardView / 2) + scaleSprite / 2 + row * scaleSprite;

				GameObject gridObject = Instantiate(gridPrefab, new Vector3(spriteX, spriteY, 0), Quaternion.identity);
				gridObject.transform.localScale = new Vector3 (scaleSprite, scaleSprite, scaleSprite);

				board [row, column] = gridObject;
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
			return camHalfHeight * 2.0f;
		}
	}
}
