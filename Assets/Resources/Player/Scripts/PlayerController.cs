﻿//нужно добавить к персонажу Rigidbody2D и Collider2D

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
	
	void Start()
	{
		rb = this.GetComponent<Rigidbody2D>();
		collider = this.GetComponent<BoxCollider2D>();
		sprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if (coyoteTime > 0) coyoteTime -= 1 * Time.deltaTime;
		
		horizontal = Input.GetAxisRaw("Horizontal");

		int layerMask = 1 << 6;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, ((collider.size.y / 2) + collider.offset.y/2 + 0.2f), layerMask);
		
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
		if (horizontal > 0) 
		{
			sprite.flipX = false;
		}
		if (horizontal < 0) 
		{
			sprite.flipX = true;
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
}
