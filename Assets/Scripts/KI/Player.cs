using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Player {

	int getNextMove (int player, GameBoard gameBoard);

	string getName();
}
