using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioSource seSource;

	[SerializeField, Foldout("Audio Settings")] AudioMixer audioMixer;
	[SerializeField, Foldout("Audio Settings")] Slider masterSlider;
	[SerializeField, Foldout("Audio Settings")] Slider soundEffectSlider;
	[SerializeField, Foldout("Audio Settings")] Slider musicSlider;

	// Change these to match mixer exposed group names
	const string masterVolume = "Master_Volume";
	const string soundEffectVolume = "SFX_Volume";
	const string musicVolume = "Music_Volume";	
	
	public List<String> parameters = new List<String>();
	
	[Header("Singleton")]
	public bool dontDestroyOnLoad = true;
	public static AudioManager Instance { get; private set; }

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		if (dontDestroyOnLoad)
			DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		LoadVolumeSettings();

		parameters = GetExposedParameters(audioMixer);
	}

	public void PlaySoundEffect(SoundEffect seToPlay)
	{
		seSource.spatialBlend = 0;
		seSource.pitch = seToPlay.pitch;
		seSource.outputAudioMixerGroup = seToPlay.audioGroup;

		AudioClip clip = seToPlay.GetClip();

		if (clip != null)
			seSource.PlayOneShot(clip, seToPlay.volume);
	}

	public void PlaySoundEffect(SoundEffect seToPlay, GameObject objToPlayFrom)
	{
		AudioSource objSource = objToPlayFrom.GetComponent<AudioSource>();

	
		if (objSource == null)
			objSource = objToPlayFrom.AddComponent<AudioSource>();

		objSource.volume = seToPlay.volume;
		objSource.pitch = seToPlay.pitch;
		objSource.spatialBlend = seToPlay.spatialBlend;
		objSource.outputAudioMixerGroup = seToPlay.audioGroup;

		AudioClip clip = seToPlay.GetClip();

		if (clip != null)
			objSource.PlayOneShot(clip);
	}
	
	#region Audio Settings

	// Set Master Volume with logarithmic scale (0.0001 to 1 range)
	public void SetMasterVolume(float level)
	{
		audioMixer.SetFloat(masterVolume, Mathf.Log10(level) * 20f);
		PlayerPrefs.SetFloat(masterVolume, level);  // Save setting to PlayerPrefs
		PlayerPrefs.Save();  // Ensure the setting is saved
	}

	// Set Sound FX Volume
	public void SetSoundFXVolume(float level)
	{
		audioMixer.SetFloat(soundEffectVolume, Mathf.Log10(level) * 20f);
		PlayerPrefs.SetFloat(soundEffectVolume, level);  // Save setting to PlayerPrefs
		PlayerPrefs.Save();  // Ensure the setting is saved
	}

	// Set Music Volume
	public void SetMusicVolume(float level)
	{
		audioMixer.SetFloat(musicVolume, Mathf.Log10(level) * 20f);
		PlayerPrefs.SetFloat(musicVolume, level);  // Save setting to PlayerPrefs
		PlayerPrefs.Save();  // Ensure the setting is saved
	}

	// Load saved volume settings from PlayerPrefs
	private void LoadVolumeSettings()
	{
		if (PlayerPrefs.HasKey(masterVolume))
		{
			float _MasterVolume = PlayerPrefs.GetFloat(masterVolume);
			audioMixer.SetFloat(masterVolume, Mathf.Log10(_MasterVolume) * 20f);
			try
			{
				SetMasterSlider(_MasterVolume);
			}
			catch (Exception e)
			{
				Debug.Log($"{e.Message}: Sliders prob not loaded yet");
			}
		}

		if (PlayerPrefs.HasKey(soundEffectVolume))
		{
			float _SfxVolume = PlayerPrefs.GetFloat(soundEffectVolume);
			audioMixer.SetFloat(soundEffectVolume, Mathf.Log10(_SfxVolume) * 20f);
			try
			{
				SetSfxSlider(_SfxVolume);
			}
			catch (Exception e)
			{
				Debug.Log($"{e.Message}: Sliders prob not loaded yet");
			}
		}

		if (PlayerPrefs.HasKey(musicVolume))
		{
			float _MusicVolume = PlayerPrefs.GetFloat(musicVolume);
			audioMixer.SetFloat(musicVolume, Mathf.Log10(_MusicVolume) * 20f);
			try
			{
				SetMusicSlider(_MusicVolume);
			}
			catch (Exception e)
			{
				Debug.Log($"{e.Message}: Sliders prob not loaded yet");
			}
		}
	}
	
	#endregion

	#region Methods

	private void SetMasterSlider(float volume)
	{
		masterSlider.value = volume;
		Debug.Log("MasterSlider set: " + volume);
	}
	private void SetSfxSlider(float volume)
	{
		soundEffectSlider.value = volume;
		Debug.Log("SfxSlider set: " + volume);
	}
	private void SetMusicSlider(float volume)
	{
		musicSlider.value = volume;
		Debug.Log("MusicSlider set: " + volume);
	}
	#endregion
	
	private List<string> GetExposedParameters(AudioMixer mixer)
	{
		List<string> exposedParams = new List<string>();

		// Using reflection to access the AudioMixer's ExposedParameters
		var dynMixer = new SerializedObject(mixer);
		var parameters = dynMixer.FindProperty("m_ExposedParameters");

		if (parameters != null && parameters.isArray)
		{
			for (int i = 0; i < parameters.arraySize; i++)
			{
				var param = parameters.GetArrayElementAtIndex(i);
				var nameProp = param.FindPropertyRelative("name");
				if (nameProp != null)
				{
					exposedParams.Add(nameProp.stringValue);
				}
			}
		}

		return exposedParams;
	}
}