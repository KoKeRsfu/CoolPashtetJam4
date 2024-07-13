using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
    public GameObject[] frames;
    public DimensionScriptable[] dimensions;
    public float size = 3.5f * 3f;
    public float duration = 0.5f;
    public GameObject player;
    public float shake_force = 1;
    public Material[] player_materials;
    private bool unlocked = true;
    private GameObject new_frame = null;
    private float new_frame_start = 0f;
    private float rotateTo = 0f;
    private float elapsedTime = 0f;
    private float blink_time = 0f;
    private bool broke = false;
    private bool blinked;
    private bool skin_changed = false;
    private float rotation;

    void Start()
    {
        Shade(frames[0].transform.GetChild(0).GetChild(1).gameObject, frames[0].transform.rotation.eulerAngles.x);
        Shade(frames[1].transform.GetChild(0).GetChild(1).gameObject, frames[1].transform.rotation.eulerAngles.x);
        Shade(frames[2].transform.GetChild(0).GetChild(1).gameObject, frames[2].transform.rotation.eulerAngles.x);
    }

    private void Shade(GameObject filter, float angle)
    {
        float color;
        if (angle >= 180f)
            angle -= 380f;
        if (angle > 0)
            color = 1f;
        else
            color = 0f;
        filter.GetComponent<SpriteRenderer>().color = new Color(color, color, color, Mathf.Abs(angle) / 500);
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
                    new_frame.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                    rotateTo = 45f;
                    new_frame_start = -90f;
                }
                else
                {
                    new_frame = Instantiate(frames[2]);
                    new_frame.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    rotateTo = -45;
                    new_frame_start = 90f;
                }
                elapsedTime = 0f;
                player.GetComponent<PlayerController>().Stop();
                broke = false;
                blinked = false;
                rotation = UnityEngine.Random.Range(-25f, 25f);
                if (rotateTo > 0)
                {
                    DimensionScriptable new_dimesion = dimensions[0];
                    dimensions[0] = dimensions[1];
                    dimensions[1] = dimensions[2];
                    dimensions[2] = new_dimesion;
                }
                else
                {
                    DimensionScriptable new_dimesion = dimensions[2];
                    dimensions[2] = dimensions[1];
                    dimensions[1] = dimensions[0];
                    dimensions[0] = new_dimesion;
                }
            }
        }
        else 
        {
            float t = elapsedTime / duration;
            t = ((t - 1.35f) * t * (0.3f - 2f * t)) * 1.6806722689f;
            frames[0].transform.rotation = Quaternion.Euler(45f + t * rotateTo, 0f, 0f);
            frames[1].transform.rotation = Quaternion.Euler(t * rotateTo, 0f, 0f);
            frames[2].transform.rotation = Quaternion.Euler(-45f + t * rotateTo, 0f, 0f);
            new_frame.transform.rotation = Quaternion.Euler(new_frame_start + t * rotateTo, 0f, 0f);
            Shade(frames[0].transform.GetChild(0).GetChild(1).gameObject, frames[0].transform.rotation.eulerAngles.x);
            Shade(frames[1].transform.GetChild(0).GetChild(1).gameObject, frames[1].transform.rotation.eulerAngles.x);
            Shade(frames[2].transform.GetChild(0).GetChild(1).gameObject, frames[2].transform.rotation.eulerAngles.x);
            Shade(new_frame.transform.GetChild(0).GetChild(1).gameObject, new_frame.transform.rotation.eulerAngles.x);

            t = elapsedTime / duration;
            t = -2f * t * (t - 1) + 1f;
            player.transform.GetChild(0).transform.localScale = new Vector3(t, t, t);

            t = elapsedTime / duration;
            t = (t - 1) * 4 * t * rotation;
            player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0f, 0f, t);

            if (!broke)
            {
                if (frames[1].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.max.y <
                    player.transform.position.y + 0.7 ||
                    frames[1].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.min.y >
                    player.transform.position.y - 0.7)
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
                    this.GetComponent<CinemachineImpulseSource>().GenerateImpulse(shake_force);
                    broke = true;
                }
            }
            if (!blinked && (frames[1].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.max.y <
                    player.transform.position.y + 1.3 ||
                    frames[1].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.min.y >
                    player.transform.position.y - 1.3))
            {
                blinked = true;
                player.transform.GetChild(0).GetComponent<SpriteRenderer>().material = player_materials[1];
                skin_changed = false;
                player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = dimensions[1].playerSprite[0];
                player.GetComponent<PlayerController>().playerSprite = dimensions[1].playerSprite;
                player.GetComponent<PlayerController>().lightLevel = dimensions[1].light;
                blink_time = elapsedTime;
            }
            else if (!skin_changed && (blink_time + .2f <= elapsedTime))
            {
                player.transform.GetChild(0).GetComponent<SpriteRenderer>().material = player_materials[0];
                skin_changed = true;
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
                frames[0].transform.rotation = Quaternion.Euler(45f, 0f, 0f);
                frames[1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                frames[2].transform.rotation = Quaternion.Euler(-45f, 0f, 0f);
                Shade(frames[0].transform.GetChild(0).GetChild(1).gameObject, frames[0].transform.rotation.eulerAngles.x);
                Shade(frames[1].transform.GetChild(0).GetChild(1).gameObject, frames[1].transform.rotation.eulerAngles.x);
                Shade(frames[2].transform.GetChild(0).GetChild(1).gameObject, frames[2].transform.rotation.eulerAngles.x);
                player.transform.GetChild(0).transform.localScale = new Vector3(1f, 1f, 1f);
                player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                player.GetComponent<PlayerController>().Resume();
                player.GetComponent<Rigidbody2D>().gravityScale = dimensions[1].gravity;
                player.transform.GetChild(0).GetComponent<SpriteRenderer>().material = player_materials[0];
                unlocked = true;
            }
        }
    }

    void FixedUpdate()
    {

    }
}
