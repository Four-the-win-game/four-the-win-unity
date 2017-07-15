using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour {

	public GameObject menuCanvas;
	public GameObject settingsCanvas;

	void Start() {
		menuCanvas.SetActive (true);
		settingsCanvas.SetActive (false);
	}

	public void startSingleplayer() {
		Application.LoadLevel ("singleplayer");
	}

	public void openSettings() {
		menuCanvas.SetActive (false);
		settingsCanvas.SetActive (true);
	}

	public void openMenu() {
		menuCanvas.SetActive (true);
		settingsCanvas.SetActive (false);
	}
}
