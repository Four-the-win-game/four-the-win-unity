using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardInput : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		checkInputTouch ();
		checkMouseInput ();
	}

	private void checkInputTouch() {
		if (Input.touchCount == 1) {
			RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if (hitInfo) {
				hover (hitInfo.transform.gameObject);
			}
		}
	}

	private void checkMouseInput() {
		if (Input.GetMouseButtonUp (0)) {
			Vector2 pos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (pos), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if (hitInfo) {
				click (hitInfo.transform.gameObject);
			}
		} else if (!Input.GetMouseButton (0)) {
			Vector2 pos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (pos), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if (hitInfo) {
				hover (hitInfo.transform.gameObject);
			}
		}
	}

	private void hover(GameObject gameObject) {
		InputElement input = gameObject.GetComponent<InputElement> ();
		if (input != null) {
			input.onHover ();
		}
	}

	private void click(GameObject gameObject) {
		InputElement input = gameObject.GetComponent<InputElement> ();
		if (input != null) {
			input.onClick (GameManager.actualPlayer);

			if (GameManager.actualPlayer == 1) {
				GameManager.actualPlayer = 2;
			} else {
				GameManager.actualPlayer = 1;
			}
		}
	}
}
