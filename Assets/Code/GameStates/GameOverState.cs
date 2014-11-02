using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameOverState : FSM.State {

	public override void Enter () {
	}

	public override void Execute () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			fsm.ChangeState(fsm.GetComponent< Game >().mainMenuState);
		}
	}
}
