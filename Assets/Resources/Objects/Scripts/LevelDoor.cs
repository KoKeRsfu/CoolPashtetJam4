using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerController;

public class LevelDoor : MonoBehaviour
{
    public int nextLevel;
	public GameObject blackScreen;
    
	public AudioClip openClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator NextScene()
	{
		GameObject soundPlayer = Instantiate(Resources.Load<GameObject>("Audio/Prefabs/SoundPlayer"));
		soundPlayer.GetComponent<SoundManager>().audioClip = openClip;
        yield return new WaitForSeconds(blackScreen.GetComponent<BlackScreenScript>().deathWaitTime +
                blackScreen.GetComponent<BlackScreenScript>().animationTime);
        SceneManager.LoadScene(nextLevel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && !collision.gameObject.GetComponent<PlayerController>().stopVariables.stop)
        {
            blackScreen.GetComponent<BlackScreenScript>()._target = 1;
            blackScreen.GetComponent<BlackScreenScript>().StartCoroutine("EffectAnimation");
            blackScreen.GetComponent<BlackScreenScript>().StartCoroutine("AwayAnimation");
            StartCoroutine(NextScene());
        }
    }
}
