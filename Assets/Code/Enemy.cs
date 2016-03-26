using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//Pases as parameters the enemy and the previous noise level
	public event System.Action< Enemy, int > OnEnemyShoutUp;

	public const int MaxNoiseLevel = 2;

	private AnimationState animationState;
	
	public ParticleSystem particlesBla;
	public GameObject particlesImpactPrefab;
	public AudioSource[] blablaSources;
	private AudioSource blaSource;
	public AudioClip noiseWarning;
	public AudioClip impactSound; //TODO: change this to player


	public void Start() {
		particlesBla.enableEmission = false;

		foreach(AudioSource blaSource in blablaSources) {
			blaSource.gameObject.SetActive(false);
		}
	}

	private void ChangeAnimation(string _animName) {
		GetComponent<Animation>().CrossFade(_animName);
		animationState = GetComponent<Animation>()[_animName];
		animationState.time = 0.0f;
	}

	private bool DidAnimFinished() {
		if(animationState == null)
			return true;

		return animationState.time > animationState.length;
	}

	private int _currentNoiseLevel;
	public int currentNoiseLevel {
		get {
			return _currentNoiseLevel;
		}
		set {
			if(_currentNoiseLevel == 0 && value != 0) {
				particlesBla.enableEmission = true;
				blaSource = blablaSources[Random.Range(0, blablaSources.Length)];
				blaSource.gameObject.SetActive(true);
				SoundManager.Instance.Play2DSound(noiseWarning);
			}

			_currentNoiseLevel = value;
			switch(value) {
				case 1:
					ChangeAnimation("talk");
					break;

				case 2:
					ChangeAnimation("dancing_loop");
					break;
			}
		}
	}


	#region Overriden

	void Update()
	{
		if(DidAnimFinished()) {
			//Debug.Log("Anim finished");
			if(currentNoiseLevel == 0) { //IDLE
				ChangeAnimation("idle_0" + Random.Range(1, 3));
			}
		}
	}

	void OnMouseDown()
	{
		ShoutUp();
	}

	#endregion


	#region Public
	public void ShoutUp(bool animated = true)
	{
		if (_currentNoiseLevel > 0) {
			int previousNoiseLevel = currentNoiseLevel;
			currentNoiseLevel = 0;

			GameObject.Instantiate(particlesImpactPrefab, transform.position + Vector3.up * 1.72f, Quaternion.identity);
			ChangeAnimation("get hit");
			blaSource.gameObject.SetActive(false);
			SoundManager.Instance.Play2DSound(impactSound);
			particlesBla.enableEmission = false;

			if (OnEnemyShoutUp != null) {
				OnEnemyShoutUp(this, previousNoiseLevel);
			}
		}
	}
	#endregion

}
