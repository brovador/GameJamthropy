using UnityEngine;
using System.Collections;

public class OrientationManager : MonoBehaviour {

	private float previousAspect;

	public event System.Action OnOrientationChanged;

	private static OrientationManager instance;
	public static OrientationManager Instance {
		get {
			if(instance == null) {
				instance = (new GameObject("OrientationManager")).AddComponent< OrientationManager >();
			}
			return instance;
		}
	}

	public static bool Instanced() {
		return instance != null;
	}

	void Update () {
		float aspect = (Screen.width / (float)Screen.height);
		if(!Mathf.Approximately(aspect, previousAspect)) {
			previousAspect = aspect;
			if(OnOrientationChanged != null) {
				OnOrientationChanged();
			}
		}
	}
}
