using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{

	protected void Start() 
	{
		Invoke("ChangeScene", 3f);
	}
	
	private void ChangeScene()
	{
		SceneManager.LoadScene(1);
	}

}
