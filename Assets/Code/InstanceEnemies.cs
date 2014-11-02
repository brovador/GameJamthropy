using UnityEngine;
using System.Collections;

public class InstanceEnemies : MonoBehaviour {

	public GameObject enemyPrefab;

	public Vector3 offsetPosition, offsetRotation;

	public Texture[] enemiesTextures;

	private GameObject enemy;
	void Start () 
	{
		foreach(GameObject _chair in GameObject.FindGameObjectsWithTag("chair"))
		{
			enemy = Instantiate(enemyPrefab,_chair.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = _chair.transform;
			enemy.transform.localPosition = offsetPosition;
			enemy.transform.localRotation = Quaternion.Euler(offsetRotation);

			SkinnedMeshRenderer meshRenderer = enemy.transform.FindChild("sergeeo_01").GetComponent< SkinnedMeshRenderer >();
			meshRenderer.materials[0].SetTexture("_MainTex", enemiesTextures[Random.Range(0, enemiesTextures.Length)]);
		}
	}
}
