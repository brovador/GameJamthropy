using UnityEngine;
using System.Collections;

public class Looker : MonoBehaviour {

	public event System.Action<Looker> OnLookerShoutUp;
	public event System.Action<Looker> OnLookerReachDestiny;


	[HideInInspector]
	public Vector3 destPosition;
	[HideInInspector]
	public Vector3 startPosition;
	[HideInInspector]
	public float destinyMinDistance = 0.8f;
	[HideInInspector]
	public int noiseLevel = 1;

	public Material noNoiseMaterial;
	public Material noiseMaterial;
	public Animator lookerAnimator;

	public ParticleSystem particleBla;
	public AudioSource blablaSource;
	public AudioClip noiseWarning;

	private NavMeshAgent myNavMeshAgent;
	private Transform myTransform;
	private Renderer myRenderer;

	private bool destinyReached;
	private bool _makingNoise;
	private bool makingNoise {
		get {
			return _makingNoise;
		}
		set {
			_makingNoise = value;

			if (_makingNoise) {
				particleBla.enableEmission = true;
				blablaSource.gameObject.SetActive(true);
				SoundManager.Instance.Play2DSound(noiseWarning);
			} else {
				blablaSource.gameObject.SetActive(false);
				particleBla.enableEmission = false;
			}

			float noiseLevel = _makingNoise?1:0;
			lookerAnimator.SetFloat("talkLevel", noiseLevel);
		}
	}


	void Awake()
	{
		myNavMeshAgent = this.GetComponent<NavMeshAgent> ();
		myTransform = transform;
		myRenderer = renderer;
	}


	void Start()
	{
		myTransform.position = startPosition;
		myTransform.LookAt(destPosition);

		makingNoise = false;

		Utils.DoOnNextFrame(this, () => {
			myNavMeshAgent.destination = destPosition;
		});
	}


	void Update()
	{
		CheckDestinyReached();
	}


	void OnMouseDown()
	{
		ShoutUp();
	}

	#region Public

	public void ShoutUp(bool forced = false, bool animated = true)
	{
		if (makingNoise || forced) {

			if (animated) {
				lookerAnimator.SetTrigger("hitTrigger");
			}

			myNavMeshAgent.destination = startPosition;
			makingNoise = false;

			if (OnLookerShoutUp != null) {
				OnLookerShoutUp(this);
			}
		}
	}

	#endregion

	#region Private

	void CheckDestinyReached()
	{
		if (myNavMeshAgent.remainingDistance != 0
			&& myNavMeshAgent.remainingDistance < destinyMinDistance
		    && !makingNoise
		    && !destinyReached) {

			destinyReached = true;
			makingNoise = true;

			if (OnLookerReachDestiny != null) {
				OnLookerReachDestiny(this);
			}
		}
	}

	#endregion

}
