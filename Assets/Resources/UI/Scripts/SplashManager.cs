using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashManager : MonoBehaviour
{
	
	public List<string> splashes = new List<string>();
	
	[SerializeField] TextMeshProUGUI text;

	public GameObject blackScreen;


    protected void Start() 
	{
		string newtext = "Мудрость разработчиков:\n" + splashes[Random.Range(0, splashes.Count)];
		newtext.Replace("\\n", "\n");
		text.text = newtext;
		Invoke("ChangeScene", 5f);
	}

    private IEnumerator NextScene(int a)
    {
        blackScreen.GetComponent<BlackScreenScript>().StartCoroutine("AwayAnimation");
        yield return new WaitForSeconds(blackScreen.GetComponent<BlackScreenScript>().deathWaitTime +
                blackScreen.GetComponent<BlackScreenScript>().animationTime);
        SceneManager.LoadScene(a);
    }

    private void ChangeScene()
	{
        StartCoroutine(NextScene(1));
    }

}
