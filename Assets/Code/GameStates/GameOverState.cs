using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameOverState : FSM.State {

	public TypeWriter typewriter;

	public override void Enter () {
		typewriter.enabled = false;
	}

	public override void Execute () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			//fsm.ChangeState(fsm.GetComponent< Game >().mainMenuState);
			Application.LoadLevel("Game");
		}
	}
}
