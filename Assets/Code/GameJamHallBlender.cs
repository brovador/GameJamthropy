﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameJamHallBlender : MonoBehaviour {

	public float blend;
	private float previousBlend;

	public Material[] materials;

	// Update is called once per frame
	void Update () {
		blend = Mathf.Clamp01(blend);

		if(blend != previousBlend) {
			foreach(Material m in materials) {
				m.SetFloat("_Blend", blend);
			}

			previousBlend = blend;
		}
	}
}
