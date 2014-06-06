﻿using UnityEngine;
using System.Collections;
		
public class PlayerControl : MonoBehaviour {
	
	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]
	public bool jump = false;
	
	
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public AudioClip[] jumpClips;
	public float jumpForce = 1000f;
	public AudioClip[] taunts;
	public float tauntProbability = 50f;
	public float tauntDelay = 1f;
	
	
	private int tauntIndex;
	private Transform groundCheck;
	private bool grounded = false;
	private Animator anim;
	
	
	void Awake() {
		//Setting up references
		groundCheck = transform.Find ("groundCheck");
		anim = GetComponent<Animator>();
	}
	
	
	void Update() {
		//Player is grounded if linecast to the groundcheck position hits the ground
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 <<LayerMask.NameToLayer("Ground"));
		
		if (Input.GetButtonDown("Jump") && grounded)
			jump = true;
	}
	
	
	void FixedUpdate () {
		float h = Input.GetAxis("Horizontal");

		//finds player position in relation to camera.
		var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
		//finds mouse position
		Vector2 mouse = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);

		anim.SetFloat("Speed", Mathf.Abs(h));

		if (h * rigidbody2D.velocity.x < maxSpeed)
			rigidbody2D.AddForce(Vector2.right * h * moveForce);
		
		if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			rigidbody2D.velocity = new Vector2 (Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		if (h > 0 && !facingRight)
			Flip();

		else if (h < 0 && facingRight)
			Flip();

		if (mouse.x < playerScreenPoint.x && facingRight) 
			Flip ();

		else if (mouse.x > playerScreenPoint.x && !facingRight)
			Flip ();

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
}