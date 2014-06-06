using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour {

	[HideInInspector]
	public bool OffCooldown = true;
	[HideInInspector]
	public bool CooldownTrigger = false;

	public Rigidbody2D bolt;
	public float speed = 20f;
	public float coolDown = 1f;
	public float coolDownTimer = 0f;

	public float labelTime = 3f;
	public float labelTimer = 0f; 

	private PlayerControl playerCtrl;
	private Animator anim;

	void Awake () {
		anim = transform.root.gameObject.GetComponent<Animator> ();
		playerCtrl = transform.root.GetComponent<PlayerControl> ();
	}

	void Update () {

		//increments cooldown timer
		if (!OffCooldown)
			coolDownTimer += Time.deltaTime;

		//triggers warning if ability used when on cooldown
		if (Input.GetButtonDown("Fire1") && !OffCooldown) 
			CooldownTrigger = true;

		//fires bolt if fire is pressed
		if (Input.GetButtonDown("Fire1") && OffCooldown) {
			OffCooldown = false;
			anim.SetTrigger ("Shoot");
			audio.Play();
			//finds player position in relation to camera.
			Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
			//finds mouse position
			Vector2 mouse = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			var dir = (mouse - new Vector2 (playerScreenPoint.x,playerScreenPoint.y)).normalized;
			Rigidbody2D boltInstance = Instantiate(bolt, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			boltInstance.velocity = speed*dir;
		}

		//Reenables ability when cooldown timer hits 0 and resets timer
		if (coolDownTimer > coolDown) {
			coolDownTimer = 0;
			OffCooldown = true;
		}
	}
	
	void OnGUI(){
		GUI.color = Color.black;
		//label shows if player uses ability when on cooldown
		if (CooldownTrigger) {
			labelTimer += Time.deltaTime;
			GUI.Label (new Rect (10, 10, 300, 20), "The ability is on cooldown.");
			if (labelTimer > labelTime || OffCooldown) {
				labelTimer = 0;
				CooldownTrigger = false;
			}
		}
	}
}

