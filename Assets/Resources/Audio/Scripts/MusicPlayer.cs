using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	private AudioSource audiosource;
	
	public float musicTime;
	public float musicVolume;
	
    void Start()
	{
		audiosource = this.GetComponent<AudioSource>();
		
	     musicVolume = PlayerPrefs.GetFloat("MusicVolume");
		audiosource.volume = musicVolume;
	    
    }
	
	public void ChangeTrack(AudioClip clip) 
	{
		musicTime = audiosource.time;
		
		audiosource.clip = clip;
		audiosource.Play();
		audiosource.time = musicTime;
	}
    
}
