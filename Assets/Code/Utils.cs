using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	private static IEnumerator DoOnNextFrame(System.Action _action) {
		yield return new WaitForEndOfFrame();
		_action();
	}
	
	public static void DoOnNextFrame(MonoBehaviour _go, System.Action _action) {
		if(!_go.gameObject.activeInHierarchy) {
			Debug.Log(_go.name + " is inactive");
		}
		
		_go.StartCoroutine(DoOnNextFrame(_action));
	}
	
	private static IEnumerator DoAfterSeconds(System.Action _action, float _seconds) {
		yield return new WaitForSeconds(_seconds);
		_action();
	}
	
	public static void DoAfterSeconds(MonoBehaviour _go, float _seconds, System.Action _action) {
		_go.StartCoroutine(DoAfterSeconds(_action, _seconds));
	}
}
