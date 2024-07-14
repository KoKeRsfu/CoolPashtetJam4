		using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cinemachine;

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

	public bool air;
	public float airTime = 0f;
	public float airTimeMax = 5f;
	public float airRestoreMult = 2f;
	
	public float lightLevel;
	
	public float friction;
	
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
	
	[System.Serializable]
	public class DeathVariables
	{
		public AudioClip deathClip;
		public GameObject blackScreen;
		public GameObject bloodParticles;
		public GameObject goreParticles;
		public bool isDying = false;
		
		public List<GameObject> blood = new List<GameObject>();
	
		public float _current = 0, _target = 0, _current2 = 0, _target2 = 0;
		public float t, t2;
		
		public float deathTime;
		
		public CinemachineVirtualCamera vcam;
		public float vcam_angle;
		
		public LensDistortion lensdis_value;
		public PaniniProjection paniniproj_value;
		public Vignette vignette_value;
	}
	
	public DeathVariables deathVariables;
	
	private RaycastHit2D hit;
	
	void Start()
	{
		rb = this.GetComponent<Rigidbody2D>();
		collider = this.GetComponent<BoxCollider2D>();
		sprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
		light = this.transform.GetChild(1).GetComponent<Light2D>();
		
		VolumeProfile volume = Camera.main.GetComponent<Volume>().profile;
		
		LensDistortion lensdis;
		ChromaticAberration chromab;
		FilmGrain filmgr;
		PaniniProjection paniniproj;
		Vignette vignette;
		LiftGammaGain lgg;
		if (volume.TryGet<LensDistortion>(out lensdis)) deathVariables.lensdis_value = lensdis;
		if (volume.TryGet<PaniniProjection>(out paniniproj)) deathVariables.paniniproj_value = paniniproj;
		if (volume.TryGet<Vignette>(out vignette)) deathVariables.vignette_value = vignette;
	}

	void Update()
	{

		deathVariables._current = Mathf.MoveTowards(deathVariables._current, deathVariables._target, deathVariables.t * Time.deltaTime);
		deathVariables._current2 = Mathf.MoveTowards(deathVariables._current2, deathVariables._target2, deathVariables.t2 * Time.deltaTime);

		deathVariables.lensdis_value.intensity.value = (deathVariables._current * 0.15f) + 0.2f;
		deathVariables.lensdis_value.scale.value = 1f - (deathVariables._current * 0.25f);
		deathVariables.paniniproj_value.distance.value = (deathVariables._current * 0.7f);
		deathVariables.vignette_value.intensity.value = (deathVariables._current * 0.3f);

		if (deathVariables.isDying)
		{
			// чё это за пиздец разберись
			//deathVariables.vcam.transform.rotation = Quaternion.EulerAngles(0f,0f, 1f - (deathVariables._current*(1f/deathVariables.vcam_angle)));
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			StartCoroutine("Death");
		}

		light.intensity = lightLevel;

		if (stopVariables.stop) return;

		if (!deathVariables.isDying)
		{
			if (!air)
			{
				if (airTime < 0f)
					StartCoroutine("Death");
				airTime -= Time.deltaTime;
			}
			else
			{
				airTime += airRestoreMult * Time.deltaTime;
				if (airTime > airTimeMax)
					airTime = airTimeMax;
			}
		}
		
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
		
		rb.velocity = new Vector2(rb.velocity.x * friction, rb.velocity.y);
		
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
			animVariables.maxFrame = 3.4f;
			break;
		case 1:
			animVariables.minFrame = 3.55F;
			animVariables.maxFrame = 7.4f;
			break;
		case 2:
			animVariables.minFrame = 7.55f;
			animVariables.maxFrame = 8.4f;
			break;
		case 3:
			animVariables.minFrame = 8.55f;
			animVariables.maxFrame = 10f;
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
		deathVariables._target = 1;
		deathVariables._target2 = 1;
		
		//ГОООООООООООООООЛ
		
		deathVariables.isDying = true;
		sprite.enabled = false;
		
		this.GetComponent<CinemachineImpulseSource>().GenerateImpulseAt(transform.position, new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)));
		
		Instantiate(deathVariables.bloodParticles, transform.position, deathVariables.bloodParticles.transform.rotation);
		Instantiate(deathVariables.goreParticles, transform.position, deathVariables.goreParticles.transform.rotation);
		
		int r = Random.Range(4,8);
		for (int i=0;i<r;i++) 
		{
			Instantiate(deathVariables.blood[Random.Range(0,deathVariables.blood.Count)],	
				new Vector3(transform.position.x + Random.Range(-1.2f,1.2f), transform.position.y + Random.Range(-1.6f,1.2f), 0), 
				Quaternion.Euler(0,0,Random.Range(-45f,45f)));
		}

        GameObject soundPlayer = Instantiate(Resources.Load<GameObject>("Audio/Prefabs/SoundPlayer"));
        soundPlayer.GetComponent<SoundManager>().audioClip = deathVariables.deathClip;
        deathVariables.blackScreen.GetComponent<BlackScreenScript>().StartCoroutine("AwayAnimation");
		yield return new WaitForSeconds(deathVariables.deathTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
