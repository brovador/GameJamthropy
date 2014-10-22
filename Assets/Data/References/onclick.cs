using UnityEngine;
using System.Collections;

public class onclick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<UIEventListener>().onClick += hola;
	}
	
	void hola(GameObject target)
	{
		Debug.Log("HOLA");
	}
}
