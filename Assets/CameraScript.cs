using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Mathf.Abs(Camera.main.aspect - 16.0f / 9.0f) < 0.001f)
        {
            this.GetComponent<CinemachineVirtualCamera>().Follow = null;
            transform.position = new Vector3(0f, 0f, -10f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
