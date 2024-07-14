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
	
	public AudioClip menuTheme;
	public MusicPlayer musicPlayer;
	
	public List<Button> levelButtons = new List<Button>();
	
    // Start is called before the first frame update
    void Start()
	{

		musicPlayer = GameObject.FindGameObjectWithTag("musicplayer").GetComponent<MusicPlayer>();
		
	    ChangeMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
		ChangeSoundVolume(PlayerPrefs.GetFloat("SoundVolume", 0.5f));
		
		musicPlayer.ChangeTrack(menuTheme);
		
		for (int i=1; i<5; i++) levelButtons[i].interactable = false;
		
		if (PlayerPrefs.GetInt("level2", 0) == 1) levelButtons[1].interactable = true;
		if (PlayerPrefs.GetInt("level3", 0) == 1) levelButtons[2].interactable = true;
		if (PlayerPrefs.GetInt("level4", 0) == 1) levelButtons[3].interactable = true;
		if (PlayerPrefs.GetInt("level5", 0) == 1) levelButtons[4].interactable = true;
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
	private void ClickSound() 
	{
		SoundManager a = Instantiate(soundPlayer).GetComponent<SoundManager>();
		a.audioClip = clickSound;
	}
	
	public void ChangeMusicVolume(float a) 
	{
		PlayerPrefs.SetFloat("MusicVolume", a);
		MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
		musicPlayer.audiosource.volume = a;
	}
	
	public void ChangeSoundVolume(float a) 
	{
		PlayerPrefs.SetFloat("SoundVolume", a);
		SoundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
	}
	
	public void PlayButton()
	{
		SceneManager.LoadScene(1);
		ClickSound();
	}
	
	public void MoveButton(int a) 
	{
		switch (a) //0 - главное меню, 1 - настройки, 2 - уровни
		{
		case 0:
			camController.Move(0,0);
			break;
		case 1:
			camController.Move(-100,0);
			break;
		case 2:
			camController.Move(100,0);
			break;
		}
		ClickSound();
	}
	
	public void LevelButton(int a) 
	{
		SceneManager.LoadScene(a);
	}
	
	public void ExitButton() 
	{
		ClickSound();
		Application.Quit();
	}
}
