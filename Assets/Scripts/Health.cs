using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int HP = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Hurt (int dmg) {
		HP -= dmg;
	}
}
