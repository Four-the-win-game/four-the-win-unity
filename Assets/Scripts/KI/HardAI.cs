using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HardAI: Player, AiListener {

	private long timeCalculating;

	private GameBoardData board;

	private int playerMe;
	private int minDeep;

	private int countRatings;
	private List<int> validTurns;

	private String name;

	public HardAI (int playerMe, int deep, long timeCalculating) {
		this.playerMe = playerMe;
		countRatings = 0;
		this.deep = deep;
		this.timeCalculating = timeCalculating;

		name = "AI";
	}

	public HardAI(int playerMe, int deep, long timeCalculating,  String name) {
		this.playerMe = playerMe;
		countRatings = 0;
		this.deep = deep;
		this.timeCalculating = timeCalculating;

		this.name = name;
	}

	public void calcNextMove (int player, GameBoardData gameBoard) {
		countRatings = 0;

		board = gameBoard;
		validTurns = board.getValidTurns ();

		turnHighestRating = validTurns [0];
		highestRating = int.MinValue;

		int deep = this.deep;
		//the ai calculates too long in the first moves
		//reduce the deep 
		if(gameBoard.numberBlocks <= 4) {
			deep--;
		}

		for (int i = 0; i < validTurns.Count; i++) {
			DeepSearch deepSearch = new DeepSearch (board, validTurns[i], deep, playerMe, playerMe, int.MinValue, int.MaxValue, timeCalculating);
			deepSearch.setAiListener (this);
			deepSearch.Start ();
		}
	}

	//Choose the highest rating
	int highestRating;
	int turnHighestRating;

	public void calculatedRating(int turn, int rating) {
		countRatings++;

		//Debug.Log ("Rating: " + rating + ", validTurn: " + turn);
		if (rating > highestRating) {
			highestRating = rating;
			turnHighestRating = turn;
		}
	}

	public bool finishedCalc() {
		if (validTurns == null) {
			return false;
		}
		return countRatings == validTurns.Count;
	}

	public int getMove() {
		return turnHighestRating;
	}

	public String getName() {
		return name;
	}
}

