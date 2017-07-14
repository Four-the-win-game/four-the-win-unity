using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputElement : MonoBehaviour {

	private float fadeDuration = 0.3f; //in seconds

	public Color color;
	public Color hover;

	private int position;
	private Color actualColor;
	private SpriteRenderer spriteRenderer;
	private GameBoard gameBoard;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		actualColor = color;
	}

	void LateUpdate() {
		spriteRenderer.color = Color.Lerp (spriteRenderer.color, actualColor, Time.deltaTime / fadeDuration);
	}

	public void onHover(int player) {
		actualColor = hover;

		gameBoard.showPreview (position, player);
	}

	public bool canInsert() {
		return gameBoard.canInsert (position);
	}

	public void onClick(int player) {
		gameBoard.cancelPreview ();
		gameBoard.insert (position, player);
	}

	public void setPosition(int position) {
		this.position = position;
	}

	public void setGameBoard(GameBoard gameBoard) {
		this.gameBoard = gameBoard;
	}
}
