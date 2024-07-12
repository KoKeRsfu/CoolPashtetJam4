using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	[SerializeField] GameObject soundPlayer;
	[SerializeField] AudioClip clickSound;
	
	[SerializeField] CameraController camController;
	
	[SerializeField] Slider MusicSlider;
	[SerializeField] Slider SoundSlider;
	
    // Start is called before the first frame update
    void Start()
    {
	    ChangeMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
	    PlayerPrefs.GetFloat("SoundVolume", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
	/*
	private void ClickSound() 
	{
		soundPlayer a = Instantiate(soundPlayer).GetComponent<soundPlayer>();
		a.track = clickSound;
	}
	*/
	
	public void ChangeMusicVolume(float a) 
	{
		PlayerPrefs.SetFloat("MusicVolume", a);
		MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
	}
	
	public void PlayButton()
	{
		SceneManager.LoadScene(1);
		//ClickSound();
	}
	
	public void OptionsButton() 
	{
		camController.MoveToOptions();
		//ClickSound();
	}
	
	public void MenuButton() 
	{
		camController.MoveToMenu();
		//ClickSound();
	}
	
	public void ExitButton() 
	{
		//ClickSound();
		Application.Quit();
	}
}
