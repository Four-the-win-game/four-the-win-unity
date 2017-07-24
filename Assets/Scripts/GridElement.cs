using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour {

	private float fadeDuration = 0.2f; //in seconds
	private float scaleMultiplier = 1.4f;

	public Color colorNonePlayer;
	public Color colorFistPlayer;
	public Color colorSecondPlayer;

	public Color colorFirstPlayerPrev;
	public Color colorSecondPlayerPrev;

	private float defaultScale;

	//0 is no player, 1 is first player, 2 is second player
	public int player;

	private SpriteRenderer spriteRenderer;

	public GridElement(int player) {
		this.player = player;
	}

	void Start () {
		defaultScale = this.transform.localScale.x;

		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = colorNonePlayer;
		player = 0;
	}

	void Update() {
		float scale = Mathf.Lerp (transform.localScale.x, defaultScale, Time.deltaTime / fadeDuration);

		setScale (scale);
	}

	public void setPlayer(int player) {
		this.player = player;
		if (player == GameManager.NONE) {
			spriteRenderer.color = colorNonePlayer;
		} else if (player == GameManager.FIRSTPLAYER) {
			spriteRenderer.color = colorFistPlayer;
			setScale (defaultScale * scaleMultiplier);
		} else if (player == GameManager.SECONDPLAYER) {
			spriteRenderer.color = colorSecondPlayer;
			setScale (defaultScale * scaleMultiplier);
		} else if (player == GameManager.FIRSTPLAYERPEV) {
			spriteRenderer.color = colorFirstPlayerPrev;
			setScale (defaultScale * scaleMultiplier);
		} else if (player == GameManager.SECONDPLAYERPREV) {
			spriteRenderer.color = colorSecondPlayerPrev;
			setScale (defaultScale * scaleMultiplier);
		}
	}

	public void setPlayerWithoutAnimation(int player) {
		this.player = player;
		if (player == GameManager.NONE) {
			spriteRenderer.color = colorNonePlayer;
		} else  if(player == GameManager.FIRSTPLAYER){
			spriteRenderer.color = colorFistPlayer;
		} else if(player == GameManager.SECONDPLAYER) {
			spriteRenderer.color = colorSecondPlayer;
		} else if (player == GameManager.FIRSTPLAYERPEV) {
			spriteRenderer.color = colorFirstPlayerPrev;
		} else if (player == GameManager.SECONDPLAYERPREV) {
			spriteRenderer.color = colorSecondPlayerPrev;
		}
	}

	private void setScale(float scale) {
		transform.localScale = new Vector3 (scale, scale, scale);
	}
}
