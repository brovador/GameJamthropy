using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	private static MusicManager instance;

	public enum LevelMusic
	{
		RELAX,
		INTERMEDIO
	}
	public AudioSource[] audioSource;

	private LevelMusic levelMusic;
	
	void Awake()
	{
		instance = this;
	}

	/*void Update()
	{
		if(Input.GetKeyDown(KeyCode.S))
			SetVolume(LevelMusic.INTERMEDIO, 1);
		if(Input.GetKeyDown(KeyCode.A))
			SetVolume(LevelMusic.RELAX, 1);
	}*/

	public void SetVolume(LevelMusic _levelMusic, int _volume)
	{
		int i = 0;
		foreach(AudioSource _audioSource in audioSource)
		{
			TweenVolume.Begin(audioSource[i].gameObject, 2, 0);
			i++;
		}

		if(_levelMusic == LevelMusic.INTERMEDIO)
		{
			TweenVolume.Begin(audioSource[1].gameObject, 2, _volume);
		}
		if(_levelMusic == LevelMusic.RELAX)
		{
			TweenVolume.Begin(audioSource[0].gameObject, 2, _volume);
		}
	}

}
