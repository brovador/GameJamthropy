using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Utility class to play sounds from anywhere. All game sounds must be played from this class for volume normalization and performance.
/// A pool of AudioSources is created on Awake of this Manager. When you play a 2D Sound, a free AudioSouce is taken from the pool if there is at least an
/// AudioSource that is not playing a clip, but there is a max number of sounds that can be played at the same time equal to the number of AudioSources of the pool.
/// 
/// To be sure no sound is stopped, you can use 3D sound methods, that allow you to pass by parameter your custom AudioSource.
/// </summary>
/// <author> Gonzalo De Santos </author>
/// <refactoring> Adrian Mesa </refactoring>
public class SoundManager : MonoBehaviour
{

	/// <summary>
	/// Max audio sources setted to 10 => 10 sounds at most playing at the same time. Change it if needed.
	/// </summary>
	private const int MAX_AUDIO_SOURCES = 10;
	
	private AudioSourcePool audioSourcePool;
	
#region Singleton
	
	private static SoundManager instance;
	
	public static SoundManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("SoundManager").AddComponent<SoundManager>();
			}
			return instance;
		}
	}
	
	public void OnApplicationQuit ()
	{
		instance = null;
	}
	
#endregion
	
	void Awake()
	{
		audioSourcePool = new AudioSourcePool(MAX_AUDIO_SOURCES, gameObject, true);
		DontDestroyOnLoad(this.gameObject);
	}
	
	
#region 2D methods
	
	/// <summary>
	/// Play a sound in a free AudioSource. Other older sounds can be stopped using this method.
	/// </summary>
	/// <param name="clip">
	/// Clip to play.
	/// </param>
	public void Play2DSound(AudioClip _clip)
	{
		PlaySound(_clip, 1f, 0f, 0f, 0f, null, Vector3.zero);
	}
	
	/// <summary>
	/// Play a sound in a free AudioSource. Other older sounds can be stopped using this method.
	/// </summary>
	/// <param name="clip">
	/// Clip to play.
	/// </param>
	/// <param name="pitch">
	/// Clip pitch.
	/// </param>
	/// <param name="delay">
	/// Delay before play the sound in seconds.
	/// </param>
	public void Play2DSound(AudioClip _clip, float _pitch, float _delay, float _volume = 1.0f)
	{
		PlaySound(_clip, _pitch, _delay, 0f, 0f, null, Vector3.zero);
	}
	
#endregion
	
#region 3D methods
	
	/// <summary>
	/// Play a sound in a free AudioSource. 
	/// </summary>
	/// <param name="clip">
	/// Clip to play.
	/// </param>
	/// <param name ="source">
	/// Source where play the sound. Use one if you want to play a localized sound in 3D space or you want to be sure no sound stop playing.
	/// </param>
	public void Play3DSound(AudioClip _clip, AudioSource _source)
	{
		if (_source == null)
			Debug.LogError("An AudioSource is needed if you want to play a sound localized in 3D space or have a own AudioSource to avoid sound overlapping");
		
		PlaySound(_clip, _source.pitch, 0f, _source.maxDistance, _source.minDistance, _source, Vector3.zero);
	}
	
	/// <summary>
	/// Play a sound in a free AudioSource. Other older sounds can be stopped using this method.
	/// </summary>
	/// <param name="clip">
	/// Clip to play.
	/// </param>
	/// <param name="pitch">
	/// Clip pitch.
	/// </param>
	/// <param name="delay">
	/// Delay before play the sound in seconds.
	/// </param>
	/// <param name ="source">
	/// Source where play the sound. Use one if you want to play a localized sound in 3D space or you want to be sure no sound stop playing.
	/// </param>
	public void Play3DSound(AudioClip _clip, float _delay, AudioSource _source)
	{
		if (_source == null)
			Debug.LogError("An AudioSource is needed if you want to play a sound localized in 3D space or have a own AudioSource to avoid sound overlapping");
		
		PlaySound(_clip, _source.pitch, _delay, _source.maxDistance, _source.minDistance, _source, Vector3.zero);
	}
	
	public void Play3DSound(AudioClip _clip, float _maxVolumeDistance, float _minVolumeDistance, Vector3 _location)
	{		
		PlaySound(_clip, 1f, 0f, _maxVolumeDistance, _minVolumeDistance, null, _location);
	}
	
	public void Play3DSound(AudioClip _clip, float _delay, float _maxVolumeDistance, float _minVolumeDistance, Vector3 _location)
	{		
		PlaySound(_clip, 1f, _delay, _maxVolumeDistance, _minVolumeDistance, null, _location);
	}
	
#endregion
	
	/// <summary>
	/// Set sound manager play volume.
	/// </summary>
	/// <param name="newVolume">
	/// New volume between 0 and 1
	/// </param>
	public void setVolume(float _volume)
	{
		AudioListener.volume = _volume;
	}
	
	public void FreeResources()
	{
		audioSourcePool.FreeResources();
	}
	
#region Internal aux methods
	
	/// <summary>
	/// Play a sound. If audioSource is null, we get the next AudioSource from the pool.
	/// </summary>
	private void PlaySound(AudioClip _clip, float _pitch, float _delay, float _maxVolumeDistance, float _minVolumeDistance, AudioSource _audioSource, Vector3 _location, float _volume = 1.0f)
	{
		if(_clip == null)
			return;
		
		if(_audioSource == null)
		{
			_audioSource = audioSourcePool.getAudioSource();
			
			_audioSource.playOnAwake = false;
			_audioSource.loop = false;
			_audioSource.rolloffMode = AudioRolloffMode.Linear;
		}
		
		if(_audioSource!=null)
		{
			if(_location != Vector3.zero)
			{
				_audioSource.transform.position = _location;
			}
			
			_audioSource.clip  = _clip;
			_audioSource.pitch = _pitch;
			_audioSource.minDistance = _minVolumeDistance;
			_audioSource.maxDistance = _maxVolumeDistance;
			_audioSource.volume = _volume;
			
			if (_audioSource.enabled)
				_audioSource.Play((ulong)(44100 / _clip.frequency * _clip.frequency * _delay));	
		}
	}
	
#endregion
}

