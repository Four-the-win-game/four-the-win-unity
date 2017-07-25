using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeButton : MonoBehaviour {

	private string[] tags;

	// Use this for initialization
	void Start () {
		//get text and use the text as a tag for the localization
		Text[] texts = GetComponentsInChildren<Text>();
		int counter = 0;
		tags = new string[texts.Length];
		foreach (Text text in texts) {
			string tag = text.text;

			text.text = LocalizationText.GetText (tag);

			tags [counter] = tag;
		}
	}

	public void updateText() {
		Text[] texts = GetComponentsInChildren<Text>();
		for (int i = 0; i < texts.Length; i++) {
			texts[i].text = LocalizationText.GetText (tags[i]);
		}
	}
}
