using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputElement : MonoBehaviour {

	private int position;
	public Color color;
	public Color hover;

	private SpriteRenderer spriteRenderer;
	private GameBoard gameBoard;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		spriteRenderer.color = color;
	}

	public void onHover() {
		spriteRenderer.color = hover;
	}

	public void onClick(int player) {
		gameBoard.insert (position, player);
	}

	public void setPosition(int position) {
		this.position = position;
	}

	public void setGameBoard(GameBoard gameBoard) {
		this.gameBoard = gameBoard;
	}
}
