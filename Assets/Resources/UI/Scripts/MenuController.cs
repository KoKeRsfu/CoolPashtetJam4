using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	[SerializeField] GameObject soundPlayer;
	[SerializeField] AudioClip clickSound;
	
	[SerializeField] CameraController camController;
	
    // Start is called before the first frame update
    void Start()
    {
        
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
