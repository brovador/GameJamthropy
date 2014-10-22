using UnityEngine;
using System.Collections;

public class InstanceEnemies : MonoBehaviour {

	public GameObject enemyPrefab;

	public Vector3 offsetPosition, offsetRotation;

	private GameObject enemy;
	void Start () 
	{
		foreach(GameObject _chair in GameObject.FindGameObjectsWithTag("chair"))
		{
			enemy = Instantiate(enemyPrefab,_chair.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = _chair.transform;
			enemy.transform.localPosition = offsetPosition;
			enemy.transform.localRotation = Quaternion.Euler(offsetRotation);
		}
	}
}
