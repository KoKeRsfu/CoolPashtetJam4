using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float speed = 2;
	[SerializeField] float maxSpeed = 5;
	[SerializeField] float jumpPower = 5;

	private float horizontal;
	private Rigidbody2D rb;
	private BoxCollider2D collider;
	private SpriteRenderer sprite;
	private Light2D light;
	
	public float airTime = 0;
	public float airTimeMax;
	
	public float lightLevel;
	
	public Sprite[] playerSprite;
	
	[System.Serializable]
	public class GamefeelMechanics
	{
		public float coyoteTime = 0;
		public float coyoteTimeMax;
	
		public float holdTime = 0;
		public float holdTimeMax;
	}
	
	public GamefeelMechanics gamefeel;
	
	[System.Serializable]
	public class stopMechanics 
	{	
		public bool stop = false;
		public Vector2 conservedVelocity;
		public float conservedGravity;
	}
	
	public stopMechanics stopVariables;
	
	[System.Serializable] 
	public class AnimationVariables 
	{
		public float animSpeed;
		
		public float currentFrame;
		public int calculatedFrame;
		public float prevMin;
		public float minFrame;
		public float maxFrame;
	}
	
	public AnimationVariables animVariables;
	
	private RaycastHit2D hit;
	
	void Start()
	{
		rb = this.GetComponent<Rigidbody2D>();
		collider = this.GetComponent<BoxCollider2D>();
		sprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
		light = this.transform.GetChild(1).GetComponent<Light2D>();
	}

	void Update()
	{
		/*
		if (Input.GetKeyDown(KeyCode.H)) 
		{
			Stop();
		}
		if (Input.GetKeyDown(KeyCode.J)) 
		{
			Resume();
		}*/
		
		light.intensity = lightLevel;
		
		if (stopVariables.stop) return;
		
		if (animVariables.currentFrame < animVariables.maxFrame) animVariables.currentFrame += animVariables.animSpeed * Time.deltaTime;
		else animVariables.currentFrame = animVariables.minFrame;
		
		animVariables.calculatedFrame = Mathf.RoundToInt(animVariables.currentFrame);
		sprite.sprite = playerSprite[animVariables.calculatedFrame];
		
		if (gamefeel.coyoteTime > 0) gamefeel.coyoteTime -= 1 * Time.deltaTime;
		
		if (airTime > 0 && airTimeMax > 0) airTime -= 0.1f * Time.deltaTime;
		
		if (airTimeMax == 0) airTime = 1;
		
		horizontal = Input.GetAxisRaw("Horizontal");

		int layerMask = 1 << 6;
		
		float distance = ((collider.size.y / 2) + collider.offset.y/2 + 0.35f);
		hit = Physics2D.Raycast(transform.position, -Vector2.up, distance, layerMask);
		//Debug.DrawRay(transform.position, -Vector2.up * distance, Color.yellow, 0.1f);
		
		
		if (hit.collider != null) 
		{
			gamefeel.coyoteTime = gamefeel.coyoteTimeMax;
			gamefeel.holdTime = gamefeel.holdTimeMax;
		}
		
		if (Input.GetKey(KeyCode.W))
		{
			if (gamefeel.coyoteTime > 0) 
			{
				gamefeel.coyoteTime = 0;
				rb.velocity = new Vector2(rb.velocity.x, 0);
				rb.AddForce(new Vector2(horizontal, jumpPower), ForceMode2D.Impulse);
			}		
			
			if (gamefeel.holdTime > 0)
			{
				gamefeel.holdTime -= 1 * Time.deltaTime;	
				
				rb.AddForce(new Vector2(horizontal, jumpPower * 0.7f), ForceMode2D.Force);
			}
		}
	}

	private void FixedUpdate()
	{
		if (stopVariables.stop) return;
		
		if (horizontal > 0) 
		{
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		if (horizontal < 0) 
		{
			transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		}
		
		
		if (horizontal == 0)
		{
			rb.velocity = new Vector2(rb.velocity.x * 0.88f, rb.velocity.y);
		}
		
		if (gamefeel.coyoteTime <= 0) 
		{
			if (rb.velocity.y > 0) ChangeAnimationState(2);
			else ChangeAnimationState(3);
		}
		else
		{
			if (horizontal == 0)
			{
				ChangeAnimationState(0);
			}
			else
			{
				ChangeAnimationState(1);
			}
		}
		
		rb.velocity = new Vector2(rb.velocity.x * 0.88f, rb.velocity.y);

		if (rb.velocity.x > maxSpeed)
		{
			rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
		}
		if (rb.velocity.x < -maxSpeed)
		{
			rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
		}
		
		rb.velocity = new Vector2(rb.velocity.x + horizontal * speed, rb.velocity.y);
	}
	
	public void ChangeAnimationState(int a)
	{
		switch(a) //0 - idle, 1 - run, 2 - jump, 3 - fall
		{
		case 0:
			animVariables.minFrame = -0.45f;
			animVariables.maxFrame = 3.45f;
			break;
		case 1:
			animVariables.minFrame = 3.55F;
			animVariables.maxFrame = 7.45f;
			break;
		case 2:
			animVariables.minFrame = 7.55f;
			animVariables.maxFrame = 8.45f;
			break;
		case 3:
			animVariables.minFrame = 8.55f;
			animVariables.maxFrame = 10.45f;
			break;
		}
		
		if (animVariables.prevMin != animVariables.minFrame) animVariables.currentFrame = animVariables.minFrame;
		animVariables.prevMin = animVariables.minFrame;
	}
	
	public void Stop() 
	{	
		stopVariables.conservedVelocity = new Vector2(rb.velocity.x * 5f, rb.velocity.y * 1f);
		stopVariables.conservedGravity = rb.gravityScale;
		rb.gravityScale = 0;
		rb.velocity = Vector2.zero;
		collider.enabled = false;
		stopVariables.stop = true;
	}
	
	public void Resume() 
	{
		rb.bodyType = RigidbodyType2D.Dynamic;
		collider.enabled = true;
		stopVariables.stop = false;
		rb.velocity = stopVariables.conservedVelocity;
		rb.gravityScale = stopVariables.conservedGravity;
		
		sprite.sprite = playerSprite[0];
	}
	
	
	public IEnumerator Death() 
	{
		sprite.enabled = false;
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
