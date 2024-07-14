using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeScript : MonoBehaviour
{
    public float maxEscapeTime = 3f;
    private bool escaping = false;
    private float escapeTime = 0f;
    private TextMeshProUGUI escapeMessage;

    // Start is called before the first frame update
    void Start()
    {
        escapeMessage = GetComponent<TextMeshProUGUI>();
        escapeMessage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (!escaping)
            {
                escapeTime = 0f;
                escapeMessage.enabled = true;
                escaping = true;
            }
            escapeTime += Time.deltaTime;
            escapeMessage.color = new Color(1f, 1f, 1f, escapeTime / maxEscapeTime * .5f + .5f);
            if (escapeTime >= maxEscapeTime)
                SceneManager.LoadScene(1);
        }
        else if (escaping)
        {
            if (escapeTime < -maxEscapeTime)
            {
                escapeMessage.enabled = false;
                escaping = false;
            }
            else
            {
                escapeTime -= 1.5f * Time.deltaTime;
                escapeMessage.color = new Color(1f, 1f, 1f, escapeTime / maxEscapeTime * .5f + .5f);
            }
        }
    }
}
