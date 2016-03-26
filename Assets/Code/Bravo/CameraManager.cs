using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class CameraManager : MonoBehaviour {
	
	public Camera target;
	
	public Camera[] targets;
	
	public float speed = 4.0f;
	
	public AnimationCurve interpolationCurve;
	
	private float t;
	
	private Camera previousTarget;
	private Rect previousRect;
	
	private Vector3 pos0;
	private Quaternion rot0;
	private float fov0;
	private Rect viewport0;
	
	void Start() {
		//Disable all potential targets (not necessary, but helpful)
		foreach(Camera c in targets) {
			c.enabled = false;
		}
		
		MoveToTarget(target, 0.0001f);
	}
	
	public void MoveToTarget(Camera _target, float _time = 1.0f) {
		speed = 1.0f / _time;
		
		//Disable target camera
		_target.enabled = false;
		
		t = 0.0f;
		
		
		target = _target;
		transform.parent = _target.transform;
		previousRect = _target.rect;
		previousTarget = _target;
		
		pos0 = transform.localPosition;
		rot0 = transform.localRotation; //It's important to keep the rotation in local coordinates to avoid a gimbal lock
		fov0 = GetComponent<Camera>().fieldOfView;
		viewport0 = GetComponent<Camera>().rect;
		
		
		GetComponent<Camera>().transparencySortMode = _target.transparencySortMode;
	}
	
	// Update is called once per frame
	void Update () {
		if(target != previousTarget) {
			MoveToTarget(target);
		}
		
		if(target != null) {
			if(t < 1.0f) {
				t =  Mathf.Clamp(t + speed * Time.deltaTime, 0.0f, 1.0f);
				
				float tSpring = interpolationCurve.Evaluate(t);
				
				transform.localPosition = Vector3.Lerp    (pos0, Vector3.zero, tSpring);
				transform.localRotation = Quaternion.Slerp(rot0, Quaternion.identity, tSpring);
				GetComponent<Camera>().fieldOfView  = Mathf.Lerp(fov0, target.fieldOfView, tSpring);
				GetComponent<Camera>().rect = new Rect(
					Mathf.Lerp(viewport0.x,      target.rect.x,      tSpring),
					Mathf.Lerp(viewport0.y,      target.rect.y,      tSpring),
					Mathf.Lerp(viewport0.width,  target.rect.width,  tSpring),
					Mathf.Lerp(viewport0.height, target.rect.height, tSpring)
				);
			} else {
				if(target.rect != previousRect) {
					GetComponent<Camera>().rect = target.rect;
					previousRect = target.rect;
				}
				GetComponent<Camera>().fieldOfView = target.fieldOfView;
			}
			
		}
	}
}
