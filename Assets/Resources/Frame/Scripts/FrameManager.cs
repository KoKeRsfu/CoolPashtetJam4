using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
    public GameObject[] frames;
    public float size = 3.5f * 3f;
    public float duration = 0.5f;
    public GameObject player;
    private bool unlocked = true;
    private GameObject new_frame = null;
    private float new_frame_start = 0f;
    private float rotateTo = 0f;
    private float elapsedTime = 0f;
    private bool broke = false;

    void Start()
    {

    }

    void Update()
    {
        if (unlocked) 
        {
            float direction = Input.GetAxisRaw("Mouse ScrollWheel");
            if (direction != 0)
            {
                unlocked = false;
                if (direction > 0)
                {
                    new_frame = Instantiate(frames[0]);
                    new_frame.transform.rotation = Quaternion.Euler(-60f, 0f, 0f);
                    rotateTo = 30f;
                    new_frame_start = -60f;
                }
                else
                {
                    new_frame = Instantiate(frames[2]);
                    new_frame.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
                    rotateTo = -30f;
                    new_frame_start = 60f;
                }
                elapsedTime = 0f;
                player.GetComponent<PlayerController>().Stop();
                broke = false;
            }
        }
        else 
        {
            float t = elapsedTime / duration;
            t = ((t - 1.35f) * t * (0.3f - 2f * t)) * 1.6806722689f;
            frames[0].transform.rotation = Quaternion.Euler(30f + t * rotateTo, 0f, 0f);
            frames[1].transform.rotation = Quaternion.Euler(t * rotateTo, 0f, 0f);
            frames[2].transform.rotation = Quaternion.Euler(-30f + t * rotateTo, 0f, 0f);
            new_frame.transform.rotation = Quaternion.Euler(new_frame_start + t * rotateTo, 0f, 0f);

            t = elapsedTime / duration;
            t = -3f * t * (t - 1) + 1.5f;
            player.transform.GetChild(0).transform.localScale = new Vector3(t, t, t);

            if (!broke)
                if (frames[1].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.max.y < 
                    player.transform.position.y + 0.95 || 
                    frames[1].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.min.y >
                    player.transform.position.y - 1.05)
                {
                    if (rotateTo > 0)
                    {
                        frames[1].transform.GetChild(1).transform.position = new Vector3(0f, -1f, 0f) + player.transform.position;
                        frames[1].transform.GetChild(1).transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    }
                    else 
                    {
                        frames[1].transform.GetChild(1).transform.position = new Vector3(0f, .8f, 0f) + player.transform.position;
                        frames[1].transform.GetChild(1).transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                    }
                    frames[1].transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                    broke = true;
                }

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= duration)
            {
                Destroy(new_frame);
                if (rotateTo > 0)
                {
                    new_frame = frames[0];
                    frames[0] = frames[1];
                    frames[1] = frames[2];
                    frames[2] = new_frame;
                }
                else
                {
                    new_frame = frames[2];
                    frames[2] = frames[1];
                    frames[1] = frames[0];
                    frames[0] = new_frame;
                }
                frames[0].transform.rotation = Quaternion.Euler(30f, 0f, 0f);
                frames[1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                frames[2].transform.rotation = Quaternion.Euler(-30f, 0f, 0f);
                player.transform.GetChild(0).transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                player.GetComponent<PlayerController>().Resume();
                unlocked = true;
            }
        }
    }

    void FixedUpdate()
    {

    }
}
