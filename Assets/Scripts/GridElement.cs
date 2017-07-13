using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour {

	public static int NONE = 0;
	public static int FIRST = 1;
	public static int SECOND = 2;

	public Color colorNonePlayer;
	public Color colorFistPlayer;
	public Color colorSecondPlayer;

	//0 is no player, 1 is first player, 2 is second player
	public int player;

	private SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = colorNonePlayer;
		player = 0;
	}

	public void setPlayer(int player) {
		this.player = player;
		if (player == NONE) {
			spriteRenderer.color = colorNonePlayer;
		} else  if(player == FIRST){
			spriteRenderer.color = colorFistPlayer;
		} else if(player == SECOND) {
			spriteRenderer.color = colorSecondPlayer;
		}
	}
}
