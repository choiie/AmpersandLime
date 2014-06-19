using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int HP = 100;

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
