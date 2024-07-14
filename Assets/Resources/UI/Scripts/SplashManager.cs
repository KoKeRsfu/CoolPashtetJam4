using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashManager : MonoBehaviour
{
	
	public List<string> splashes = new List<string>();
	
	[SerializeField] TextMeshProUGUI text;

	protected void Start() 
	{
		string newtext = "Мудрость разработчиков:\n" + splashes[Random.Range(0, splashes.Count)];
		newtext.Replace("\\n", "\n");
		text.text = newtext;
		Invoke("ChangeScene", 5f);
	}
	
	private void ChangeScene()
	{
		SceneManager.LoadScene(1);
	}

}
