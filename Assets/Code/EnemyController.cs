using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public event System.Action<Enemy> OnNoiseEnemyShouted;
	public event System.Action<Looker> OnLookerRemoved;
		
	public List<Enemy> enemies;
	public List<Looker> lookers;

	[System.Serializable]
	public class LookersConfig {
		public Looker lookerPrefab;
		public float destroyTime = 2.0f;
		public Transform destPosition;
		public Transform[] spawners;
	}
	public LookersConfig lookersConfig;

	//FIXME: [HideInInspector] hide in inspector
	public int _overallNoiseLevel;
	public int overallNoiseLevel {
		get {
			return _overallNoiseLevel;
		}
	}

	#region Overriden

	void Start()
	{
		Enemy[] foundEnemies = GameObject.FindObjectsOfType<Enemy> ();
		enemies = new List<Enemy> (foundEnemies);
		lookers = new List<Looker>();

		foreach (Enemy enemy in enemies) {
			enemy.OnEnemyShoutUp += HandleOnEnemyClicked;
		}
	}


	//FIXME: remove this
	/*void OnGUI()
	{
		int x = 0, y = 0;
		int buttonWidth = 100, buttonHeight = 40;
		int buttonSeparator = 10;

		if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "PerformNoise")) {
			int noiseLevel = Random.Range(0, Enemy.MaxNoiseLevel - 1) + 1;
			PerformNoise(noiseLevel);
		}

		y += buttonHeight + buttonSeparator;
		if (GUI.Button (new Rect (x, y, buttonWidth, buttonHeight), "Spawn Looker")) {
			SpawnLooker();
		}

		y += buttonHeight + buttonSeparator;
		if (GUI.Button (new Rect (x, y, buttonWidth, buttonHeight), "Shut up, F**ck!!!")) {
			ShoutOutEveryone();
		}
	}*/
	//End FIXME

	#endregion


	#region Public

	public void PerformNoise(int noiseLevel)
	{
		Enemy candidate = FindNoiseEnemyCandidate (noiseLevel);
		if (candidate != null) {
			int noiseIncrement = noiseLevel - candidate.currentNoiseLevel;
			_overallNoiseLevel += noiseIncrement;

			candidate.currentNoiseLevel = noiseLevel;
		}
	}

	public void SpawnLooker()
	{
		if (CanSpawnLookers ()) {
			Vector3 lookerSpawnerPosition = SelectSpawnLookerPosition();

			//FIXME: add lookers pool
			Looker looker = GameObject.Instantiate(lookersConfig.lookerPrefab) as Looker;
			looker.startPosition = lookerSpawnerPosition;
			looker.destPosition = lookersConfig.destPosition.position;
			looker.OnLookerShoutUp += HandleOnLookerClicked;
			looker.OnLookerReachDestiny += HandleOnLookerReachDestiny;
			lookers.Add(looker);

		} else {
			Debug.Log("[EnemyController] Lookers config not setted");
		}

	}


	public void ShoutOutEveryone(bool animated = true)
	{
		//Copy to avoid error of list change in enumeration
		List<Enemy> enemiesCopy = new List<Enemy>(enemies);
		foreach (Enemy enemy in enemiesCopy) {
			enemy.ShoutUp(animated);
		}

		//Copy to avoid error of list change in enumeration
		List<Looker> lookersCopy = new List<Looker>(lookers);
		foreach (Looker looker in lookersCopy) {
			looker.ShoutUp(true, animated);
		}
	}

	#endregion


	#region Private

	private void RaiseOnNoiseEnemyClicked(Enemy _enemy)
	{
		if (OnNoiseEnemyShouted != null) {
			OnNoiseEnemyShouted(_enemy);
		}
	}


	private void RaiseOnLookerClicked(Looker looker)
	{
		if (OnLookerRemoved != null) {
			OnLookerRemoved(looker);
		}
	}


	private Enemy FindNoiseEnemyCandidate(int noiseLevel)
	{
		Enemy result = null;
		
		List<Enemy> enemyCandidates = new List<Enemy> (enemies);
		while (result == null && enemyCandidates.Count > 0) {

			int idx = Random.Range(0, enemyCandidates.Count);
			Enemy candidate = enemyCandidates[idx];
			enemyCandidates.RemoveAt(idx);

			if (candidate.currentNoiseLevel < noiseLevel) {
				result = candidate;
			}
		}

		return result;
	}


	private Vector3 SelectSpawnLookerPosition()
	{
		Vector3 result = Vector3.zero;

		if (CanSpawnLookers()) {
			int idx = Random.Range(0, lookersConfig.spawners.Length);
			result = lookersConfig.spawners[idx].position;
		}

		return result;
	}

	private bool CanSpawnLookers()
	{
		return (lookersConfig.spawners.Length > 0);
	}

	#endregion

	#region Event Handlers

	void HandleOnEnemyClicked (Enemy enemy, int previousNoiseLevel)
	{
		_overallNoiseLevel -= previousNoiseLevel;
		RaiseOnNoiseEnemyClicked (enemy);
	}


	void HandleOnLookerClicked (Looker looker)
	{
		//FIXME: change for pool
		looker.OnLookerShoutUp -= HandleOnLookerClicked;
		lookers.Remove(looker);
		Destroy(looker.gameObject, lookersConfig.destroyTime);

		_overallNoiseLevel -= looker.noiseLevel;
		RaiseOnLookerClicked (looker);
	}


	void HandleOnLookerReachDestiny (Looker looker)
	{
		_overallNoiseLevel += looker.noiseLevel;	
	}

	#endregion
}
