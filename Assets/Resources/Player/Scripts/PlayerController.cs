
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float speed = 2;
	[SerializeField] float maxSpeed = 5;
	[SerializeField] float jumpPower = 5;

	private float horizontal;
	private Rigidbody2D rb;
	private BoxCollider2D collider;
	private SpriteRenderer sprite;
	
	public float coyoteTime = 0;
	public float coyoteTimeMax;
	
	public float holdTime = 0;
	public float holdTimeMax;
	
	public float airTime = 0;
	public float airTimeMax;
	
	public bool stop = false;
	public Vector2 conservedVelocity;
	public float conservedGravity;
	
	public Sprite[] playerSprite;
	
	public RaycastHit2D hit;
	
	void Start()
	{
		rb = this.GetComponent<Rigidbody2D>();
		collider = this.GetComponent<BoxCollider2D>();
		sprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
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
		
		
		if (stop) return;
		
		if (coyoteTime > 0) coyoteTime -= 1 * Time.deltaTime;
		
		if (airTime > 0 && airTimeMax > 0) airTime -= 0.1f * Time.deltaTime;
		
		if (airTimeMax == 0) airTime = 1;
		
		horizontal = Input.GetAxisRaw("Horizontal");

		int layerMask = 1 << 6;
		hit = Physics2D.Raycast(transform.position, -Vector2.up, ((collider.size.y / 2) + collider.offset.y/2 + 0.2f), layerMask);
		
		if (hit.collider != null) 
		{
			coyoteTime = coyoteTimeMax;
			holdTime = holdTimeMax;
		}
		
		if (Input.GetKey(KeyCode.W))
		{
			if (coyoteTime > 0) 
			{
				coyoteTime = 0;
				rb.velocity = new Vector2(rb.velocity.x, 0);
				rb.AddForce(new Vector2(horizontal, jumpPower), ForceMode2D.Impulse);
			}		
			
			if (holdTime > 0)
			{
				holdTime -= 1 * Time.deltaTime;	
				
				rb.AddForce(new Vector2(horizontal, jumpPower * 0.7f), ForceMode2D.Force);
			}
		}
	}

	private void FixedUpdate()
	{
		if (stop) return;
		
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
	
	public void Stop() 
	{	
		conservedVelocity = new Vector2(rb.velocity.x * 5f, rb.velocity.y * 1f);
		conservedGravity = rb.gravityScale;
		rb.gravityScale = 0;
		rb.velocity = Vector2.zero;
		collider.enabled = false;
		stop = true;
	}
	
	public void Resume() 
	{
		rb.bodyType = RigidbodyType2D.Dynamic;
		collider.enabled = true;
		stop = false;
		rb.velocity = conservedVelocity;
		rb.gravityScale = conservedGravity;
		
		sprite.sprite = playerSprite[0];
	}
	
}
