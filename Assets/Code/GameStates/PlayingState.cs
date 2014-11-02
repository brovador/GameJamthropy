//#define DEBUG_LOG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayingState : FSM.State {

	public CameraManager cameraManager;
	public Camera cam;

	public float decreaseSpeed = 0.05f;
	public float rightInputScore = 0.01f;
	public float wrontInputPenalty = 0.01f;
	public float noiseMultiplier = 0.33f;
	public UIEventListener rageButton;

	public EnemyController enemyController;
	public TypeWriter typeWriter;

	public UISlider concentrationSlider;
	public UISlider powerUpSlider;

	public MusicManager musicManager;

	public GameJamHallBlender gameJamHallBlender;


	private float tickTime = 10.0f;

	private float accumTick;
	private float nAnnoyers;
	private float maxNoise;

	private float concentrationPercent = 0.5f;
	private float powerUpPercent = 0.5f;
	
	private void Log(string _log) {
#if DEBUG_LOG
		Debug.Log(_log);
#endif
	}

	public override void Enter () {
		concentrationPercent = 0.5f;
		powerUpPercent = 0.0f;

		accumTick = tickTime;
		nAnnoyers = 1.0f;
		maxNoise  = 1.0f;

		cameraManager.MoveToTarget(cam, 2.0f);


		rageButton.onClick += OnClickRage;
		rageButton.gameObject.SetActive(false);

		typeWriter.gameObject.SetActive(true);
		typeWriter.OnInputRight += OnInputRight;
		typeWriter.OnInputWrong += OnInputWrong;
		enemyController.OnNoiseEnemyShouted += OnEnemyShouted;
		enemyController.OnLookerRemoved += OnLookerShouted;
	}

	public override void Exit ()
	{
		enemyController.ShoutOutEveryone(false);
		typeWriter.OnInputRight -= OnInputRight;
		typeWriter.OnInputWrong -= OnInputWrong;
		enemyController.OnNoiseEnemyShouted -= OnEnemyShouted; 
		enemyController.OnLookerRemoved -= OnLookerShouted;
	}


	public override void Execute () {
		/*if(enemyController.overallNoiseLevel != 0) {
			musicManager.SetVolume(MusicManager.LevelMusic.INTERMEDIO, 1);
		}
		if(enemyController.overallNoiseLevel == 0) {
			musicManager.SetVolume(MusicManager.LevelMusic.RE, 1);
		}*/


		//Make people annoy you
		accumTick -= Time.deltaTime;
		if(accumTick < 0.0f) {
			nAnnoyers += 0.33f;

			int nRespawn = (int)nAnnoyers;//Random.Range(1, (int)nAnnoyers);
			for(int i = 0; i < nRespawn; ++i) {
				Utils.DoAfterSeconds(fsm, Random.Range(0.0f, tickTime / 2.0f), () => {  //Don't respawn everything, do it along the next ticktime / 4 seconds

					if(Random.Range(0, 5) == 0) {
						enemyController.SpawnLooker();
					} else {
						enemyController.PerformNoise(Random.Range(1, (int)(maxNoise + 1)));
					}


				});
			}

			accumTick = tickTime;
		}

		//Log("nAnnoyers:" + (int)nAnnoyers + "-Percent:" + concentrationPercent);
		concentrationPercent -= (decreaseSpeed + decreaseSpeed * enemyController.overallNoiseLevel * noiseMultiplier) * Time.deltaTime;
		if(concentrationPercent < 0.0f) {
			concentrationPercent = 0.0f;
			fsm.ChangeState(fsm.GetComponent< Game >().gameOverState);
		}
		concentrationSlider.value = concentrationPercent;
		gameJamHallBlender.blend = (1.0f - concentrationPercent);

		powerUpSlider.value = powerUpPercent;

		if(powerUpSlider.value >= 1)
		{
			rageButton.gameObject.SetActive(true);
		}

		if (enemyController.overallNoiseLevel > 0) {
			musicManager.SetVolume(MusicManager.LevelMusic.INTERMEDIO, 1);
		} else {
			musicManager.SetVolume(MusicManager.LevelMusic.RELAX, 1);
		}
	}

	void OnClickRage(GameObject _target)
	{
		Debug.Log("RAGE");
		enemyController.ShoutOutEveryone();
		powerUpPercent = 0;
		rageButton.gameObject.SetActive(false);
	}

	public void OnInputRight() {
		concentrationPercent += rightInputScore;
		if(concentrationPercent > 1.0f) {
			float extra = concentrationPercent - 1.0f;
			concentrationPercent = 1.0f;

			powerUpPercent = Mathf.Clamp01(powerUpPercent + extra);
		}
	}

	public void OnInputWrong() {
		concentrationPercent -= wrontInputPenalty;
	}

	public void OnEnemyShouted(Enemy _enemy) {
//		if(enemyController.overallNoiseLevel == 0) {
//			musicManager.SetVolume(MusicManager.LevelMusic.RELAX, 1);
//		}
	}

	public void OnLookerShouted(Looker _enemy) {
//		if(enemyController.overallNoiseLevel == 0) {
//			musicManager.SetVolume(MusicManager.LevelMusic.RELAX, 1);
//		}
	}
}
