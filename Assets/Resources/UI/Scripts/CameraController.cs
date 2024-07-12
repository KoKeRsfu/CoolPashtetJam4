using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] Camera cam;
	private GameObject camTarget;
	
	public float camMovementMultiplier;
    // Start is called before the first frame update
    void Start()
    {    
	    camTarget = this.transform.GetChild(0).gameObject;	
    }

    // Update is called once per frame
    void Update()
	{
		Vector2 mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
		camTarget.transform.position = new Vector2(this.transform.position.x + (mousePos.x * camMovementMultiplier), this.transform.position.y + (mousePos.y * camMovementMultiplier));
		//Debug.Log(Vector2.Distance(this.transform.position, new Vector2()));
	}
    
	public void Move(float x, float y) 
	{
		transform.position = new Vector2(x,y);
	}
}
