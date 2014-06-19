using UnityEngine;
using System.Collections;

public class EnemyOrb : MonoBehaviour {

	public GameObject explosion;
	public GameObject fizzle;
	public float scaleDuration = 2f;
	public float destroyTime = 2f;
	public int dmg = 1;

	private bool scaling = true;
	private float  scaleTime = 0f;

	void Start () {
		Destroy (gameObject, destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		var speed = 0.5f;
		if (scaling) {
			scaleTime += Time.deltaTime * speed;
			OrbScale (scaleDuration);
		}

		if (scaleTime > scaleDuration)
			scaling = false;
	}

	void OnExplode() {

	}
	
	void OnFizzle() {

	}

	void OnTriggerEnter2D (Collider2D col) {

		if (col.tag == "Player") {
			col.gameObject.GetComponent<Health>().Hurt(dmg);
			
			OnExplode();
			
			Destroy (gameObject);
		}
		
		else if (col.tag == "Ground") {
			OnFizzle ();
			Destroy (gameObject);
		}
		
		else if(col.gameObject.tag != "Enemy")
		{
			OnExplode();
			Destroy (gameObject);
		}
		
		
	}

	void OrbScale(float t) {
		var startScale = Vector3.zero;
		var endScale = Vector3.one;
		gameObject.transform.localScale = Vector3.Lerp (startScale, endScale, t);
	}
}
