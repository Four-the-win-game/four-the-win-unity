using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class customToggle : MonoBehaviour {

	public Image background;
	public GameObject toggleElement;

	public bool checkedValue; 
	public Color enabledColor;
	public Color disabledColor;
	public float fadeDuration; //in seconds

	private Image toggleImage;
	private Color color;
	private float xPosition;

	// Use this for initialization
	void Start () {
		toggleImage = toggleElement.GetComponent<Image> ();
		setToggle (checkedValue);
	}
	
	// Update is called once per frame
	void Update () {
		background.color = Color.Lerp (background.color, color, Time.deltaTime / fadeDuration);

		float newXPosition = Mathf.Lerp (toggleElement.transform.localPosition.x, xPosition, Time.deltaTime / fadeDuration);
		toggleElement.transform.localPosition = new Vector3 (newXPosition, toggleElement.transform.localPosition.y, toggleElement.transform.localPosition.z);
	}

	private void setToggle(bool enabled) {
		if (enabled) {
			xPosition = 58.4f;
			color = enabledColor;
		} else {
			xPosition = -58.4f;
			color = disabledColor;
		}
	}

	public void toggle() {
		checkedValue = !checkedValue;
		setToggle (checkedValue);
	}
}
