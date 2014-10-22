﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {	

	[HideInInspector]public FSM fsm;
	public MainMenuState mainMenuState;
	public PlayingState playingState;
	public GameOverState gameOverState;

	// Use this for initialization
	void Start () {
		playingState.node.gameObject.SetActive(false);
		mainMenuState.node.gameObject.SetActive(false);
		gameOverState.node.gameObject.SetActive(false);

		fsm = gameObject.AddComponent< FSM >();
		fsm.ChangeState(mainMenuState);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
