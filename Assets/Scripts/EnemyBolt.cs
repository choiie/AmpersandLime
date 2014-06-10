using UnityEngine;
using System.Collections;

public class EnemyBolt : MonoBehaviour {

	[HideInInspector]
	public bool OffCooldown = false;
	[HideInInspector]
	public bool CooldownTrigger = false;
	public Rigidbody2D bolt;
	public float speed = 20f;
	public float cooldown = 1f;
	public float triggerHappy = 0.8f;
	[HideInInspector]
	public float cooldownTimer = 0f;
	public string targetTag = "Player";
	public float aim = 0.9f;

	public float labelTime = 3f;
	[HideInInspector]
	public float labelTimer = 0f; 

	private EnemyControl enemyCtrl;
	private Animator anim;
	private GameObject target;
	private bool noLineOfSight;

	void Awake () {
		target = GameObject.FindGameObjectWithTag(targetTag);
		anim = transform.root.gameObject.GetComponent<Animator> ();
		enemyCtrl = transform.root.GetComponent<EnemyControl> ();
	}

	void Update () {

		//checks for sightline blockers
		noLineOfSight = Physics2D.Linecast (transform.position, target.transform.position, 1 << LayerMask.NameToLayer("Ground"));
	
		//increments cooldown timer
		if (!OffCooldown)
			cooldownTimer += Time.deltaTime;

		//fires bolt if fire is pressed and sightline is clear
		if (OffCooldown && Random.Range (0,1) < triggerHappy && !noLineOfSight) {
			OffCooldown = false;
			anim.SetTrigger ("Shoot");
			audio.Play();
			//finds player position in relation to camera.
			Vector2 targetPos = target.transform.position;
			//finds mouse position
			Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
			Vector2 slightlyOff = new Vector2 (Random.Range (-1 + aim, 1 - aim),Random.Range (-1 + aim, 1 - aim));
			var dir = (((targetPos - enemyPos).normalized) + slightlyOff).normalized;
			Rigidbody2D boltInstance = Instantiate(bolt, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			boltInstance.velocity = speed*dir;
		}

		//adds brief cooldown if enemy decides not to fire
		else if (OffCooldown) {
			OffCooldown = false;
			cooldownTimer = Random.Range (0,3);
		}

		//Reenables ability when cooldown timer hits 0 and resets timer
		if (cooldownTimer > cooldown) {
			cooldownTimer = 0;
			OffCooldown = true;
		}
	}
}

