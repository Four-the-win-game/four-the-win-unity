using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameBoard gameBoard;
	public GameObject pauseCanvas;
	public GameObject gameCanvas;
	public GameObject buttonRestart;
	public GameObject buttonResume;
	public Background background;

	private static int actualPlayer;
	private static bool gameOver;
	public static int tokensToWin = 4;

	public static int NONE = 0;
	public static int FIRSTPLAYER = 1;
	public static int SECONDPLAYER = 2;
	public static int DRAW = 3;
	public static int FIRSTPLAYERPEV = 4;
	public static int SECONDPLAYERPREV = 5;

	private Text winnerText;

	// Use this for initialization
	void Start () {
		winnerText = pauseCanvas.GetComponentInChildren<Text> ();
		restart ();
	}

	public void restart() {
		setActualPlayer (FIRSTPLAYER);
		pauseCanvas.SetActive (false);
		gameCanvas.SetActive (true);
		gameOver = false;
		gameBoard.reset ();
	}

	public void backToMenu() {
		Application.LoadLevel ("menu");
	}

	public void pause() {
		winnerText.text = "";
		pauseCanvas.SetActive (true);
		gameCanvas.SetActive (false);

		buttonResume.SetActive (true);
		buttonRestart.SetActive (false);
		gameOver = true;
	}

	public void resume() {
		pauseCanvas.SetActive (false);
		gameCanvas.SetActive (true);

		buttonResume.SetActive (false);
		buttonRestart.SetActive (true);
		gameOver = false;
	}

	public void gameEnded(int winner) {
		pauseCanvas.SetActive (true);
		gameCanvas.SetActive (false);
		gameOver = true;

		if (winner == FIRSTPLAYER) {
			winnerText.text = "PLAYER ONE WINS";
		} else if (winner == SECONDPLAYER) {
			winnerText.text = "PLAYER TWO WINS";
		} else if (winner == DRAW) {
			winnerText.text = "DRAW";
		}
	}

	public void gameBoardChanged() {
		int winner = calculateWinner(gameBoard);

		if (winner != NONE) {
			gameEnded (winner);
		}
	}

	private int calculateWinner(GameBoard gameBoard) {
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
					if(count >= tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= tokensToWin) {
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
					if(count >= tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= tokensToWin) {
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
					if(count >= tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= tokensToWin) {
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
					if(count >= tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= tokensToWin) {
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
					if(count >= tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= tokensToWin) {
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
					if(count >= tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == 2) { //second player
					count++;
					if(count >= tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}

		if (!movePossible() || (firstPlayerWon && secondPlayerWon)) {
			return DRAW;
		} else if (firstPlayerWon) {
			return FIRSTPLAYER;
		} else if (secondPlayerWon) {
			return SECONDPLAYER;
		} else {
			return NONE;
		}
	}

	private bool movePossible() {
		GridElement[ , ] board = gameBoard.getGameBoard ();
		int rows = gameBoard.boardRows;
		int columns = gameBoard.boardColumns;

		for (int row = 0; row < 2 * (rows + columns); row++) {
			if (row < rows) {
				if (gameBoard.canInsert (row)) {
					return true;
				}
			}
		}

		return false;
	}

	public void setActualPlayer(int player) {
		actualPlayer = player;

		if (player == FIRSTPLAYER) {
			background.setBackground (true);
		} else if (player == SECONDPLAYER) {
			background.setBackground (false);
		}
	}

	public static int getActualPlayer() {
		return actualPlayer;
	}

	public static bool isGameOver() {
		return gameOver;
	}
}
