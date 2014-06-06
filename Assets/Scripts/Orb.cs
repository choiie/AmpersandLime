using UnityEngine;
using System.Collections;

public class Orb : MonoBehaviour {

	public GameObject explosion;

	void Start () {

		var startScale = Vector3.zero;
		var endScale = Vector3.one;
		float t = 0.0f;
		var speed = .5f;
		while (t < 1.0) {
			t += Time.deltaTime * speed;
			transform.localScale = Vector3.Lerp(startScale, endScale, t);
			if (t == 0.25 | t == 0.5 | t == 0.75)
				print (transform.localScale);
		}
		Destroy (gameObject, 2);
	
	}
	
	// Update is called once per frame
	void Update () {
	


	}
}
