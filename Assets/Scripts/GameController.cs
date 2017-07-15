using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState 
{
	START,
	RESTART,
	PAUSED,
	RUNNING,
	DIE,
	NULL
}

public class GameController : MonoBehaviour {

	private GameState currentState = GameState.START;
	private GameState previousState = GameState.NULL;

	void SwitchState(GameState nextState)
	{
		previousState = currentState;
		currentState = nextState;
	}

	void GameStateMachine()
	{
		switch(currentState)
		{
		case GameState.START:
			break;
		case GameState.RESTART:
			break;
		case GameState.PAUSED:
			break;
		case GameState.RUNNING:
			break;
		case GameState.DIE:
			break;
		default:
			break;
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update ()
	{
		GameStateMachine();
	}
}