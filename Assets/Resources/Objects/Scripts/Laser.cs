using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField] float distanceRay = 100;
	public Transform laserFirePoint;
	public LineRenderer line;
	
	protected void Update() 
	{
		ShootLaser();
	}
	
	public void ShootLaser() 
	{
		if (Physics2D.Raycast(transform.position, transform.right)) 
		{
			RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, transform.right);
			
			if (hit.collider.gameObject.layer == 3) 
			{
				if (!hit.collider.gameObject.GetComponent<PlayerController>().deathVariables.isDying) 
				{
					hit.collider.gameObject.GetComponent<PlayerController>().StartCoroutine("Death");
				}
			}
			
			Draw2DRay(laserFirePoint.position, hit.point);
		}
		else 
		{
			Draw2DRay(laserFirePoint.position, laserFirePoint.position + laserFirePoint.transform.right * distanceRay);
		}
	}
	
	void Draw2DRay(Vector2 startPos, Vector2 endPos) 
	{
		line.SetPosition(0, startPos);
		line.SetPosition(1, endPos);
	}
}
