using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public AudioSource audiosource;
	
	public float musicTime;
	public float musicVolume;
	
    void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		
		audiosource = this.GetComponent<AudioSource>(); 
    }
	
	public void ChangeTrack(AudioClip clip) 
	{
		musicVolume = PlayerPrefs.GetFloat("MusicVolume");
		audiosource.volume = musicVolume;
		
		musicTime = audiosource.time;
		
		audiosource.clip = clip;
		audiosource.Play();
		
		if (musicTime > clip.length) musicTime = 0;
		audiosource.time = musicTime;
	}
    
}
