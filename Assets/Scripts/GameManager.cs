using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameBoard gameBoard;
	public GameBoardInput gameBoardInput;
	public GameObject pauseCanvas;
	public GameObject gameCanvas;
	public GameObject buttonRestart;
	public GameObject buttonResume;
	public GameObject buttonShowBoard;
	public Background background;

	private int kiPlayer; //which player (first or second) is the ki, 0 is no ki
	private Player kiImplementation; //difficulty
	private bool calculatingTurn;

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

	private string firstPlayerName;
	private string secondPlayerName;

	// Use this for initialization
	void Start () {
		winnerText = pauseCanvas.GetComponentInChildren<Text> ();

		if (MenuAttributes.vsKi) {
			kiPlayer = SECONDPLAYER;

			if (MenuAttributes.difficulty == 1) {
				kiImplementation = new EasyKI ();
			} else if (MenuAttributes.difficulty == 2) {
				kiImplementation = new EasyKI ();
			} else {
				kiImplementation = new HardKI ();
			}

			firstPlayerName = MenuAttributes.firstPlayerName;
			secondPlayerName = kiImplementation.getName ();
		} else {
			kiPlayer = NONE;
			firstPlayerName = MenuAttributes.firstPlayerName;
			secondPlayerName = MenuAttributes.secondPlayerName;
		}

		restart ();
	}

	void Update() {
		if (actualPlayer == kiPlayer) {
			//calculate ki turn and send it to gameBoard and do this asynchron
			if (!calculatingTurn) {
				//Start calculating turn
				StartCoroutine("calcAndMoveKiTurn");
			} /* else {
				turn is calculating asynchron, do nothing
			} */
		}
	}

	public void restart() {
		setActualPlayer (FIRSTPLAYER);
		pauseCanvas.SetActive (false);
		gameCanvas.SetActive (true);
		gameOver = false;
		calculatingTurn = false;
		gameBoard.reset ();
	}

	private void calcAndMoveKiTurn() {
		int turn = kiImplementation.getNextMove (actualPlayer, gameBoard);
		gameBoard.insert (turn, actualPlayer);

		setActualPlayer((actualPlayer == FIRSTPLAYER) ? SECONDPLAYER : FIRSTPLAYER);

		gameBoardChanged ();
	}

	public void backToMenu() {
		SceneManager.LoadScene ("menu");
	}

	public void pause() {
		if (!gameOver) { //game was paused
			winnerText.text = "";
			buttonShowBoard.SetActive (false);
			pauseCanvas.SetActive (true);
			gameCanvas.SetActive (false);

			buttonResume.SetActive (true);
			buttonRestart.SetActive (false);
			gameOver = true;
		} else { //game was ended and paused
			pauseCanvas.SetActive (true);
			gameCanvas.SetActive (false);

			buttonResume.SetActive (false);
			buttonRestart.SetActive (true);
			gameOver = true;
		}
	}

	public void resume() {
		pauseCanvas.SetActive (false);
		gameCanvas.SetActive (true);

		buttonResume.SetActive (false);
		buttonRestart.SetActive (true);
		gameOver = false;
	}

	public void showBoard() {
		pauseCanvas.SetActive (false);
		gameCanvas.SetActive (true);
	}

	public void gameEnded(int winner) {
		buttonShowBoard.SetActive (true);
		pauseCanvas.SetActive (true);
		gameCanvas.SetActive (false);
		gameOver = true;

		if (winner == FIRSTPLAYER) {
			winnerText.text = "The Winner is: " + firstPlayerName;
		} else if (winner == SECONDPLAYER) {
			winnerText.text = "The Winenr is: " + secondPlayerName;
		} else if (winner == DRAW) {
			winnerText.text = "DRAW";
		}
	}

	public void gameBoardChanged() {
		int winner = gameBoard.calculateWinner(gameBoard);

		if (winner != NONE) {
			gameEnded (winner);
		}
	}

	public void setActualPlayer(int player) {
		actualPlayer = player;

		if (player == FIRSTPLAYER) {
			background.setBackground (true);
		} else if (player == SECONDPLAYER) {
			background.setBackground (false);
		}

		if (player == kiPlayer) {
			gameBoardInput.setHumansTurn (false);
		} else {
			gameBoardInput.setHumansTurn (true);
			calculatingTurn = false;
		}
	}

	public static int getActualPlayer() {
		return actualPlayer;
	}

	public static bool isGameOver() {
		return gameOver;
	}
}
