using UnityEngine;
using System.Collections;

public class AudioFlow : Flow {

#region Fields
	AudioSource 				audioSource;
#endregion

#region Getter and Setter
	public AudioSource Source
	{
		get { return audioSource; }
	}
#endregion

#region Constructor
	public AudioFlow(AudioSource audioSource)
		: base ()
	{
		this.audioSource = audioSource;
	}
#endregion

#region Implement Virtual Functions
	protected override IEnumerator main()
	{
		if(audioSource == null)
		{
			Debug.LogWarning("[WARNING]SoundFlow->main: audio source is null.");
			yield break;
		}		
		
		audioSource.Play();
		while(audioSource.isPlaying)
		{
			yield return null;
		}

		if(audioSource != null)
		{
			audioSource.Stop();
		}
	}	

	public override void Kill()
	{
		base.Kill();
		if(audioSource != null)
		{
			audioSource.Stop();
		}
	}

	public override void Pause()
	{
		base.Pause();
		if(audioSource != null)	
		{
			audioSource.Pause();
		}
	}

	public override void Resume()
	{
		base.Resume();
		if(audioSource != null)
		{
			audioSource.UnPause();
		}
	}
#endregion
}
