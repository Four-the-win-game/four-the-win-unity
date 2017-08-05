using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class customInput : MonoBehaviour {

	public string settingsTag;

	public InputField inputField;

	void Start() {
		//Load value
		Debug.Log("load: " + PlayerPrefs.GetString(settingsTag, LocalizationText.GetText ("player")));	 
		inputField.text = PlayerPrefs.GetString(settingsTag, LocalizationText.GetText ("player"));
	}

	public void nameChanged(string name) {
		Debug.Log ("Changed: " + inputField.text);
		PlayerPrefs.SetString (settingsTag, inputField.text);
	}
}
