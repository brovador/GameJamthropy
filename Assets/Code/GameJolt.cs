using UnityEngine;
using System.Collections;

public class GameJolt : MonoBehaviour {

	public static bool initialized = false;
	public static bool verified;

	public static void Init(int _gameID, string _privateKey) {
		if(initialized)
			return;


		GJAPI.Init(_gameID, _privateKey);

#if UNITY_EDITOR
		string _user = "Zalo";
		string _token = "da085b";
#else
		GJAPIHelper.Users.GetFromWeb(
			(_user, _token) => {
#endif
				if(_user != "") {
					GJAPI.Users.VerifyCallback = (
						(_verified) => {
							if(_verified) {
								verified = true;
								Debug.Log("User " + _user + " verified");
							}	
						}
					);
					GJAPI.Users.Verify(_user, _token);
				}
#if !UNITY_EDITOR
			}
		);
#endif
		initialized = true;
	}

	public static void AddScore(string _userName, int _highscore, System.Action _onDone, System.Action _onFail) {
		GJAPI.Scores.AddCallback = (
			(_success) => {
				if(_success)
					_onDone();
				else
					_onFail();
			}
		);
		if(_userName == "")
			GJAPI.Scores.Add("" + _highscore, (uint)_highscore);
		else
			GJAPI.Scores.AddForGuest("" + _highscore, (uint)_highscore, _userName);
	}

	public static void GetHighscores(System.Action< GJScore[] > _onSuccess) {
		GJAPI.Scores.GetMultipleCallback = (
			(_tables) => {
				_onSuccess(_tables);
			}
		);
		GJAPI.Scores.Get(false);
	}
}
