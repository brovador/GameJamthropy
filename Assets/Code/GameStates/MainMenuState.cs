using UnityEngine;
using System.Collections;

[System.Serializable]
public class MainMenuState : FSM.State {
	public GameObject typeWriter;

	public CameraManager cameraManager;
	public Camera cam;

	public Material screenMaterial;
	public Texture mainScreenTextue;
	public Texture instructionsTexture;

	private int textIdx = 0;

	public override void Enter () {
		textIdx = 0;

		cameraManager.MoveToTarget(cam, 0.0f);
		typeWriter.SetActive(false);

		screenMaterial.mainTexture = mainScreenTextue;
	}

	public override void Execute () {
		if(Input.anyKeyDown) {
			if(textIdx == 0) {
				screenMaterial.mainTexture = instructionsTexture;
				textIdx ++;
			} else {
				fsm.ChangeState(fsm.GetComponent< Game >().playingState);
			}
		}
	}
}
