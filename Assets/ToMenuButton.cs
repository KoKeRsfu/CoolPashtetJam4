using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenuButton : MonoBehaviour
{
    public GameObject blackScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator NextScene(int a)
    {
        blackScreen.GetComponent<BlackScreenScript>().StartCoroutine("AwayAnimation");
        yield return new WaitForSeconds(blackScreen.GetComponent<BlackScreenScript>().deathWaitTime +
                blackScreen.GetComponent<BlackScreenScript>().animationTime);
        SceneManager.LoadScene(a);
    }

    private void MoveToMenu()
    {
        StartCoroutine(NextScene(1));
    }
}
