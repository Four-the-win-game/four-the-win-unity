using System;
using System.Collections;
using System.Collections.Generic;

public class GameProgress {

	public List<int[,]> boards;

	private int curPos;
	
	public GameProgress () {
		boards = new List<int[,]> ();
	}

	public void newState(int[,] board) {
		boards.Add (board);
		curPos = boards.Count - 1;
	}

	public int[,] getPrevious() {
		curPos--;
		if (curPos < 0)
			curPos = 0;

		return boards[curPos];
	}

	public int[,] getNext() {
		curPos++;
		if (curPos == boards.Count)
			curPos = boards.Count - 1;

		return boards [curPos];
	}
}