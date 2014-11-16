using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnterHIghscore : FSM.State {

	public TextMesh textMesh;
	public TypeWriter typeWriter;

	public string userEntered;

	public override void Enter () {
		if(GameJolt.verified) {
			SubmitHighscore("");
		}
	}

	public override void Execute () {
		foreach(char c in Input.inputString) {
			if(c != '\n')
				textMesh.text += c;
		}
		if(Input.GetKeyDown(KeyCode.Backspace) && textMesh.text.Length > 0) {
			textMesh.text = textMesh.text.Remove(textMesh.text.Length - 2);
		}
		if(Input.GetKeyDown(KeyCode.Return)) {
			SubmitHighscore(textMesh.text);
		}
	}

	void SubmitHighscore(string _username) {
		userEntered = _username;
		node.gameObject.SetActive(false);
		GameJolt.AddScore(_username, typeWriter.nWords, 
			() => {
				fsm.ChangeState(fsm.GetComponent< Game >().gameOverState);
			},
			() => {
				fsm.ChangeState(fsm.GetComponent< Game >().gameOverState);
			}
		);
	}
}
