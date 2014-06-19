using UnityEngine;
using System.Collections;

public class References : MonoBehaviour {

	public static GameObject playerRef;
	public static GameObject enemyRef;
	public static string[] charArray; //names of objects tied to each available character attached to TemplateHero and TemplateEnemy

	// Use this for initialization
	void Start () {
		charArray = new string[] {"PearGuy","RedPear"};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
