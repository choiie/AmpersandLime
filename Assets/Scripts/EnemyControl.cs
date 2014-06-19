using UnityEngine;
using System.Collections;
using System.Threading;

public class EnemyControl : MonoBehaviour {

	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]
	public bool jump = false;

	public bool moveInAir = false;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public AudioClip[] jumpClips;
	public float jumpForce = 1000f;
	public AudioClip[] taunts;
	public float tauntProbability = 50f;
	public float tauntDelay = 1f;
	public float forwardChance = 60f;
	public float backwardChance = 30f;
	public float maxMovementTimeRange = 3f;
	public float avoidRadius = 1f;

	private float movementTimeRange;
	private int tauntIndex;
	private Transform groundCheck;
	private bool grounded = false;
	private Animator anim;
	private GameObject player;
	private float t;
	private bool move = true;
	private bool avoiding = false;
	private float directionRoll;

	void Awake() {
		//Setting up references
		groundCheck = transform.Find ("groundCheck");
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag ("Player");
	}


	void Update() {
		//Player is grounded if linecast to the groundcheck position hits the ground
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 <<LayerMask.NameToLayer("Ground"));

		if (Random.Range (0,100) > 98 && grounded)
			jump = true;
	}


	void FixedUpdate () {

		var relativePos = player.transform.position - transform.position;

		if (move) {
			move = false;
			t = 0;
			movementTimeRange = Random.Range (0, maxMovementTimeRange);
			directionRoll = Random.Range (0,100);
		}

		else if (!move) {
			t += Time.deltaTime;
			if (t > movementTimeRange) {
				move = true;
				avoiding = false;
			}
		}

		if (Mathf.Abs (relativePos.x) < avoidRadius) {
			Avoid ();
			avoiding = true;
		}

		else if (directionRoll < forwardChance && !avoiding) {
			Move (1);
		}
		else if (directionRoll > 100 - backwardChance ) {
			Move (-1);
		}
		
		else {
			Stay();
		}

		if (relativePos.x > 0.5 && !facingRight)
			Flip();

		else if (relativePos.x < -0.5 && facingRight)
			Flip();

		if(jump) {
			anim.SetTrigger("Jump");

			/*
			int i = Random.Range(1,jumpClips.Length);
			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
			*/

			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			jump = false;
		}
	}

	void Flip() {
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	

	void Move (int dir) {
		var h = player.transform.position.x - transform.position.x;
		anim.SetFloat ("Speed", Mathf.Abs (rigidbody2D.velocity.x / maxSpeed));
		//x velocity added only if grounded or air movement enabled
		if (grounded || moveInAir == true) {
			if (Mathf.Sign (h) * rigidbody2D.velocity.x < maxSpeed) {
				rigidbody2D.AddForce (dir * Vector2.right * Mathf.Sign (h) * moveForce);
			}
			if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
				rigidbody2D.velocity = new Vector2 (dir * Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}

	}

	void Avoid () {
		var h = player.transform.position.x - transform.position.x;
		anim.SetFloat ("Speed", Mathf.Abs (rigidbody2D.velocity.x / maxSpeed));
		//x velocity added only if grounded or air movement enabled
		if (grounded || moveInAir == true) { 
			if (Mathf.Sign (h) * rigidbody2D.velocity.x < maxSpeed) {
				rigidbody2D.AddForce (-1 * Vector2.right * Mathf.Sign (h) * moveForce);
			}
			if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
				rigidbody2D.velocity = new Vector2 (-1 * Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}
	}


	void Stay (){
		anim.SetFloat("Speed", 0);					
	}
}