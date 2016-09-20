using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundSampleScript : MonoBehaviour {

#region Fields
	[SerializeField] AudioClip 				musicClip;
	[SerializeField] List<AudioClip> 			effectClips;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	IEnumerator Start () {

		AudioManager.Instance.PlayMusic(musicClip);	
		yield return new WaitForSeconds(3f);

		AudioManager.Instance.SetMusicVolume(0.5f);
		yield return new WaitForSeconds(3f);

		AudioManager.Instance.MuteMusic();
		yield return new WaitForSeconds(3f);

		AudioManager.Instance.SetMusicVolume(1f);
		AudioManager.Instance.UnMuteMusic();
		yield return new WaitForSeconds(3f);

		AudioManager.Instance.PauseMusic();
		yield return new WaitForSeconds(3f);


		AudioManager.Instance.SetMusicVolume(0.5f);
		AudioManager.Instance.ResumeMusic();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

}
