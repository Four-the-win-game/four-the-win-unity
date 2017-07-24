using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyKI : Player {

	public int getNextMove (int player, GameBoard gameBoard) {
		//Get valid turns and then choose one randomly
		List<int> validTurns = gameBoard.getValidTurns();

		int randomTurn = Random.Range (0, validTurns.Count - 1);
		return randomTurn;
	}

	public string getName() {
		return "Easy KI";
	}
}
