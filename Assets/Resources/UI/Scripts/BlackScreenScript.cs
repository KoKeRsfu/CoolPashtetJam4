using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class BlackScreenScript : MonoBehaviour
{
    public float animationTime;
    public float deathWaitTime;

    private int position = 0;
    private const int minPosition = 0;
    private const int maxPosition = 80 * 8;

    private float elapsedTime = 0f;

    public IEnumerator AmakeAnimation()
    {
        elapsedTime = Time.deltaTime;
        while (elapsedTime < animationTime)
        {
            position = (int)(elapsedTime / animationTime * maxPosition);
            this.transform.position = new Vector3((float)position / 8f, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = new Vector3((float)position / 8f, 0f, 0f);
    }

    public IEnumerator AwayAnimation()
    {
        yield return new WaitForSeconds(deathWaitTime);
        this.transform.position = new Vector3((float)position / -8f, 0f, 0f);
        elapsedTime = Time.deltaTime;
        while (elapsedTime < animationTime)
        {
            position = (int)(elapsedTime / animationTime * maxPosition);
            this.transform.position = new Vector3(-80f + (float)position / 8f, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = new Vector3(0f, 0f, 0f);
    }

    void Start()
    {
        this.transform.position = new Vector3(0f, 0f, 0f);
    }

    private void Awake()
    {
        this.StartCoroutine(AmakeAnimation());
    }

    void Update()
    {
        
    }
}
