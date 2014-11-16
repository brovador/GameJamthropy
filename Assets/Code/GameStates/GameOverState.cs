using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameOverState : FSM.State {

	public TypeWriter typewriter;

	public List< GameObject > rankings;

	public override void Enter () {
		//GameJolt.AddScore(typewriter.nWords, () => {}, () => {});
		GameJolt.GetHighscores(
			(_highscores) => {
				if(_highscores != null) {
					for(int i = 0; i < 10 && i < _highscores.Length; ++i) {
						SetRank(i, _highscores[i].Name, _highscores[i].Score);
					}
				}
			}
		);

		typewriter.enabled = false;
	}

	private void SetRank(int _pos, string _user, string _record) {
		if(_pos >= rankings.Count) {
			rankings.Add(GameObject.Instantiate(rankings[0], rankings[0].transform.position - Vector3.up * _pos * 0.8f, rankings[0].transform.rotation) as GameObject);
		}

		Game game = fsm.GetComponent< Game >();
		TextMesh position = rankings[_pos].transform.FindChild("PositionText").GetComponent< TextMesh >();
		TextMesh user = rankings[_pos].transform.FindChild("UserText").GetComponent< TextMesh >();
		TextMesh score = rankings[_pos].transform.FindChild("ScoreText").GetComponent< TextMesh >();

		position.text = "" + (_pos + 1);
		user.text = _user;
		score.text = _record;


		Color c = _user.ToLower() == game.enterHighscore.userEntered.ToLower() ? Color.red : Color.black;

		position.color = c;
		user.color = c;
		score.color = c;
	}

	public override void Execute () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			//fsm.ChangeState(fsm.GetComponent< Game >().mainMenuState);
			Application.LoadLevel("Game");
		}
	}
}
