using UnityEngine;
using System.Collections;

public class Orb : MonoBehaviour {

	public GameObject explosion;
	public float scaleTime = 0.0f;
	public float destroyTime = 2f;

	void Start () {
		Destroy (gameObject, destroyTime);
		OrbScale ();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OrbScale() {
		var startScale = Vector3.zero;
		var endScale = Vector3.one;
		var speed = 0.5f;
		while (scaleTime < 1.0) {
			scaleTime += Time.deltaTime * speed;
			gameObject.transform.localScale = Vector3.Lerp (startScale, endScale, scaleTime);
		}
	}
}
