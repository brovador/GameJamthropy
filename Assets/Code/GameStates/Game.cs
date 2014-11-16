using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {	

	[HideInInspector]public FSM fsm;
	public MainMenuState mainMenuState;
	public PlayingState playingState;
	public GameOverState gameOverState;
	public EnterHIghscore enterHighscore;

	// Use this for initialization
	void Start () {
		GameJolt.Init(36170, "db2812b57fad4e35ffed214e7bd0424f");

		playingState.node.gameObject.SetActive(false);
		mainMenuState.node.gameObject.SetActive(false);
		gameOverState.node.gameObject.SetActive(false);
		enterHighscore.node.gameObject.SetActive(false);

		fsm = gameObject.AddComponent< FSM >();
		fsm.ChangeState(mainMenuState);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
