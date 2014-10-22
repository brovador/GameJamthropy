using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public Animator playerAnimator;
	public EnemyController enemyController;
	public GameObject bulletPrefab, pointToBullet;


	private BezierPath bezier;

	void Start () 
	{
		bezier = new BezierPath();
		enemyController.OnNoiseEnemyShouted += HandleOnNoiseEnemyShouted;
	}

	void HandleOnNoiseEnemyShouted (Enemy _enemy)
	{
		if(Random.Range(0,2) == 1)
			playerAnimator.SetTrigger("hit_1");
		else
			playerAnimator.SetTrigger("hit_2");

		List<Vector3> points = new List<Vector3>()
		{
			transform.position,
			_enemy.transform.position,
		};

		GameObject bullet = Instantiate(bulletPrefab, pointToBullet.transform.position, Quaternion.identity) as GameObject;
		TweenPosition.Begin(bullet, .2f, _enemy.transform.position);
	}
	

	void Update () 
	{
	
	}
}
