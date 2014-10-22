using UnityEngine;
using System.Collections;

public class InterpolateCamera : MonoBehaviour {

	public Transform point;
	public AnimationCurve curve;

	private bool go = false;
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			go = true;
		}
		//if(go)
			//transform.localPosition = Vector3.Lerp(transform.localPosition, point.localPosition,  * Time.deltaTime);
	}
}
