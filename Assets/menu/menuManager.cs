using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour {

	public GameObject menuCanvas;
	public GameObject settingsCanvas;
	public GameObject singleplayerCanvas;
	public GameObject localMulitplayerCanvas;

	public InputField firstPlayer;
	public InputField secondPlayer;

	void Start() {
		menuCanvas.SetActive (true);
		settingsCanvas.SetActive (false);
		singleplayerCanvas.SetActive (false);
		localMulitplayerCanvas.SetActive (false);
	}

	public void startSingleplayer() {
		MenuAttributes.vsKi = true;
		//MenuAttributes.difficulty = 1; is already set in the DifficultyScript
		MenuAttributes.firstPlayerName = "YOU";

		SceneManager.LoadScene ("singleplayer");
	}

	public void startLocalMultiplayer() {
		MenuAttributes.vsKi = false;
		MenuAttributes.firstPlayerName = firstPlayer.text;
		MenuAttributes.secondPlayerName = secondPlayer.text;

		SceneManager.LoadScene ("singleplayer");
	}

	public void openSingleplayerMenu() {
		menuCanvas.SetActive (false);
		settingsCanvas.SetActive (false);
		singleplayerCanvas.SetActive (true);
		localMulitplayerCanvas.SetActive (false);
	}

	public void openSettings() {
		menuCanvas.SetActive (false);
		settingsCanvas.SetActive (true);
		singleplayerCanvas.SetActive (false);
		localMulitplayerCanvas.SetActive (false);
	}

	public void openMenu() {
		menuCanvas.SetActive (true);
		settingsCanvas.SetActive (false);
		singleplayerCanvas.SetActive (false);
		localMulitplayerCanvas.SetActive (false);
	}

	public void openLocalMultiplayerMenu() {
		menuCanvas.SetActive (false);
		settingsCanvas.SetActive (false);
		singleplayerCanvas.SetActive (false);
		localMulitplayerCanvas.SetActive (true);
	}
}
