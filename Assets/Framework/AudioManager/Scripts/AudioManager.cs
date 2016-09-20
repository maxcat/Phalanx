using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton
{
#region Static Fields
	static GameObject 			parentObj;
#endregion

#region Fields
	static AudioManager 			instance;

	[SerializeField] float 			musicVolume = 1f;
	[SerializeField] float 			effectVolume = 1f;
	[SerializeField] bool 			isMusicMuted = false;
	[SerializeField] bool 			isEffectMuted = false;

	List<AudioFlow> 			effectAudioFlowList;
	AudioSource 				musicSource;
#endregion

#region Static Getter and Setter
	public static AudioManager Instance
	{
		get {
			if (parentObj == null)
			{
				parentObj = new GameObject("SoundManager");
				DontDestroyOnLoad(parentObj);
				instance = parentObj.AddComponent<AudioManager>();
				instance.init();
			}
			return instance;
		}
	}

	public float MusicVolume
	{
		get { return musicVolume; }
	}

	public float EffectVolume
	{
		get { return effectVolume; }
	}

	public bool IsMusicMuted
	{
		get { return isMusicMuted; }
	}

	public bool IsEffectMuted
	{
		get { return isEffectMuted; }
	}
#endregion

#region Static Functions
	public static void Remove()
	{
		instance.clear();
		Destroy(parentObj);
	}
#endregion

#region Override MonoBehaviour
	void Awake()
	{
	}

	void OnApplicationPause()
	{
	}

	void OnLevelWasLoaded(int level)
	{
		// clear all effect source
		clearAllEffectSource();
	}
#endregion

#region Implement Virtual Functions
	protected override void init()
	{
		parentObj.AddComponent<AudioListener>();
		musicVolume = PlayerPrefs.GetFloat("MUSIC_VOLUME", 1f);
		effectVolume = PlayerPrefs.GetFloat("EFFECT_VOLUME", 1f);

		isMusicMuted = PlayerPrefs.GetInt("IS_MUSIC_MUTED", 0) > 0 ? true : false;
		isEffectMuted = PlayerPrefs.GetInt("IS_EFFECT_MUTED", 0) > 0 ? true: false;

		musicSource = parentObj.AddComponent<AudioSource>();
		musicSource.playOnAwake = false;
		musicSource.loop = true;

		effectAudioFlowList = new List<AudioFlow>();
	}

	protected override void clear()
	{
	}
#endregion

#region Public API
	public void SetMusicVolume(float volume)
	{
		musicVolume = volume;
		PlayerPrefs.SetFloat("MUSIC_VOLUME", musicVolume);

		if(!isMusicMuted)
		{
			musicSource.volume = musicVolume;
		}
	}

	public void SetEffectVolume(float volume)
	{
		effectVolume = volume;
		PlayerPrefs.SetFloat("EFFECT_VOLUME", effectVolume);

		if(!isEffectMuted)
		{
			updateEffectVolume(effectVolume);
		}
	}

	public void MuteMusic()
	{
		if(!isMusicMuted)
		{
			isMusicMuted = true;
			PlayerPrefs.SetInt("IS_MUSIC_MUTED", 1);
			musicSource.volume = 0f;
		}
	}

	public void UnMuteMusic()
	{
		if(isMusicMuted)
		{
			isMusicMuted = false;
			PlayerPrefs.SetInt("IS_MUSIC_MUTED", 0);
			musicSource.volume = musicVolume;
		}
	}

	public void MuteEffect()
	{
		if(!isEffectMuted)
		{
			isEffectMuted = true;
			PlayerPrefs.SetInt("IS_EFFECT_MUTED", 1);
			updateEffectVolume(0f);
		}
	}

	public void UnMuteEffect()
	{
		if(isEffectMuted)
		{
			isEffectMuted = false;
			PlayerPrefs.SetInt("IS_EFFECT_MUTED", 0);

			updateEffectVolume(effectVolume);
		}
	}

	public void PlayMusic(AudioClip clip)
	{
		musicSource.clip = clip;				
		musicSource.volume = musicVolume;
		musicSource.Play();
	}

	public void PlayEffect(AudioClip clip)
	{
		new Flow(playEffectEnumerator(clip)).Start(this);
	}

	public Flow GenerateEffectFlow(AudioClip clip)
	{
		return new Flow(playEffectEnumerator(clip));
	}

	public void PauseMusic()
	{
		musicSource.Pause();
	}

	public void ResumeMusic()
	{
		musicSource.UnPause();
		if(!isMusicMuted)
		{
			musicSource.volume = musicVolume;
		}
	}

	public void PauseEffect()
	{
		pauseEffects(true);
	}

	public void ResumeEffect()
	{
		pauseEffects(false);
		if(!isEffectMuted)
		{
			updateEffectVolume(effectVolume);
		}
	}
#endregion

#region Private Functions
	void clearAllEffectSource()
	{
		for(int i = 0; i < effectAudioFlowList.Count; i ++)
		{
			AudioFlow flow = effectAudioFlowList[i];
			if(flow.Source != null)
			{
				GameObject.Destroy(flow.Source);
			}
		}
		effectAudioFlowList.Clear();
	}

	void updateEffectVolume(float volume)
	{
		for(int i = 0; i < effectAudioFlowList.Count; i ++)
		{
			AudioFlow flow = effectAudioFlowList[i];
			if(flow.Source != null)
			{
				flow.Source.volume = volume;
			}
		}
	}

	void pauseEffects(bool pause)
	{
		for(int i = 0; i < effectAudioFlowList.Count; i ++)
		{
			AudioFlow flow = effectAudioFlowList[i];
			if(flow.Source != null)
			{
				if(pause)
					flow.Pause();
				else
					flow.Resume();
			}
		}
	}

	IEnumerator playEffectEnumerator(AudioClip clip)
	{
		AudioSource effectSource = parentObj.AddComponent<AudioSource>();
		effectSource.loop = false;
		effectSource.playOnAwake = false;
		effectSource.volume = effectVolume;

		AudioFlow effectFlow = new AudioFlow(effectSource);
		effectAudioFlowList.Add(effectFlow);
		yield return effectFlow;

		if(!GameUtilities.IsEmpty(effectAudioFlowList))
		{
			effectAudioFlowList.Remove(effectFlow);	
		}
		GameObject.Destroy(effectSource);
		yield return null;
	}
#endregion
}
