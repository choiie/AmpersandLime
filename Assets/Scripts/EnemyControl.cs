using UnityEngine;
using System.Collections;

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

	private float movementTimeRange;
	private int tauntIndex;
	private Transform groundCheck;
	private bool grounded = false;
	private Animator anim;
	private GameObject player;
	private float t;
	private int i = 0;

	void Awake() {
		//Setting up references
		groundCheck = transform.Find ("groundCheck");
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag ("Player");
		EnemyMovement ();
	}


	void Update() {
		//Player is grounded if linecast to the groundcheck position hits the ground
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 <<LayerMask.NameToLayer("Ground"));

		if (Random.Range (0,100) > 98 && grounded)
			jump = true;
	}


	void FixedUpdate () {

		var relativeX = player.transform.position.x - transform.position.x;

		if (relativeX > 0 && !facingRight)
			Flip();

		else if (relativeX < 0 && facingRight)
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

	public IEnumerator Taunt() {
		float TauntChance = Random.Range (0f, 100f);
		if (TauntChance > tauntProbability) {
			yield return new WaitForSeconds(tauntDelay);
			if (!audio.isPlaying) {
				tauntIndex = TauntRandom();

				audio.clip = taunts[tauntIndex];
				audio.Play();
			}
		}
	}

	int TauntRandom() {
		int i = Random.Range (0, taunts.Length);

		if (i == tauntIndex)
			return TauntRandom();
		else
			return i;
	}

	void EnemyMovement() {

		var directionRoll = Random.Range (0, 100);

		if (directionRoll < 100 - forwardChance) {
			MoveForward ();
			return;
		}

		else if (directionRoll > 100 - backwardChance) {
			MoveBackward ();
			return;
		}

		else  {
			Remain();
			return;
		}
	}

	void MoveForward (){
		t = 0;
		movementTimeRange = Random.Range (0, maxMovementTimeRange);
		var h = player.transform.position.x - transform.position.x;
		print ( "Moving forward.");
		while (t < movementTimeRange) {
			t += Time.fixedDeltaTime;
			anim.SetFloat("Speed", Mathf.Abs (rigidbody2D.velocity.x / maxSpeed));
			//x velocity added only if grounded or air movement enabled
			if  (grounded || moveInAir == true) {
				if (Mathf.Sign(h) * rigidbody2D.velocity.x < maxSpeed) {
					rigidbody2D.AddForce(Vector2.right * Mathf.Sign(h) * moveForce);
				}
				if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
					rigidbody2D.velocity = new Vector2 (Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
				}
			}
		EnemyMovement ();
		return;
	}

	void MoveBackward (){
		t = 0;
		movementTimeRange = Random.Range (0, maxMovementTimeRange);
		var h = player.transform.position.x - transform.position.x;
		print ("Moving backward.");
		//x velocity added only if grounded or air movement enabled
		while (t < movementTimeRange) {
			t += Time.fixedDeltaTime;
			anim.SetFloat ("Speed", Mathf.Abs (rigidbody2D.velocity.x / maxSpeed));
			if (grounded || moveInAir == true) {
				if (Mathf.Sign (h) * rigidbody2D.velocity.x < maxSpeed)
					rigidbody2D.AddForce (-1 * Vector2.right * Mathf.Sign (h) * moveForce);

				if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
					rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed * -1, rigidbody2D.velocity.y);
			}
		}
		EnemyMovement ();
		return;
	}

	void Remain (){
		t = 0;
		movementTimeRange = Random.Range (0, maxMovementTimeRange);
		print ( "Remaining.");
			while (t < movementTimeRange) {
				t += Time.fixedDeltaTime;
				anim.SetFloat("Speed", 0);					
			}
		EnemyMovement ();
		return;
	}
}