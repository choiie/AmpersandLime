using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour {
	
	public float charMenuXOffset = 50f;
	public Vector2 spawnPoint;
	public Vector2 enemySpawn;
	public GameObject TemplateHero;
	public GameObject TemplateEnemy;
	public float gravity = 1f;

	private GameObject player;
	private GameObject enemy;
	private GameObject bolt;
	private GameObject enemyBolt;
	private Rigidbody2D body;
	private Rigidbody2D enemyBody;
	private Component playerCtrl;
	private Component enemyCtrl;

	private GameObject pearGuy;
	private GameObject pearEnemy;
	private GameObject redPear;
	private GameObject redEnemy;

	private string[] charArray = new string[] {"PearGuy","RedPear"};
	private GameObject playerRef;

	private float titleBoxWidth = 300f;
	private float titleBoxHeight = 20f;
	private float charBoxWidth = 90f;
	private bool charMenu = true;
	private Animator genericAnim;
	private Animator greenAnim;
	private Animator redAnim;

	// Use this for initialization
	void Start () {
		spawnPoint = transform.position;
		enemySpawn = spawnPoint + new Vector2 (10,0);
		player = (GameObject) Instantiate (TemplateHero, spawnPoint, new Quaternion (0,0,0,0));
		playerRef = GameObject.Find ("References").GetComponent<References>().playerRef;
		Deactivate ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (charMenu) {
			GUI.Box (new Rect ((Screen.width-titleBoxWidth)/2, charMenuXOffset, titleBoxWidth, titleBoxHeight), "Character Selection");

			if (GUI.Button(new Rect ( (Screen.width/2) + 20,charMenuXOffset+titleBoxHeight+10 ,charBoxWidth,60), "GREEN GUY")) {
				pearGuy = player.transform.Find ("PearGuy").gameObject;
				pearGuy.SetActive(true);
				playerRef = pearGuy;
				SpawnEnemy ("PearGuy");
				Activate ();
			}
			else if (GUI.Button(new Rect ( (Screen.width/2) - (charBoxWidth+20),charMenuXOffset+titleBoxHeight+10 ,charBoxWidth,60), "RED GUY")) {
				redPear = player.transform.Find ("RedPear").gameObject;
				redPear.SetActive(true);
				playerRef = redPear;
				SpawnEnemy ("RedPear");
				Activate ();
			}
		}

	}

	void Deactivate () {
		body = player.rigidbody2D;
		body.gravityScale = 0;
		bolt = player.transform.Find ("bolt").gameObject;
		bolt.SetActive (false);
		player.GetComponent<PlayerControl> ().enabled = false;

		foreach (string element in charArray) {
			print (element);
			var tempHero = player.transform.Find(element).gameObject;
			tempHero.SetActive (false);
		}
	}

	void Activate() {
		body.gravityScale = gravity;
		bolt.SetActive (true);
		player.GetComponent<PlayerControl> ().enabled = true;
		charMenu = false;
	}

	void ActivateEnemy(string enemyChosen) {
		enemyBody.gravityScale = gravity;
		enemyBolt.SetActive (true);
		enemy.GetComponent<EnemyControl> ().enabled = true;
		var chosenEnemy = enemy.transform.Find (enemyChosen).gameObject;
		chosenEnemy.SetActive (true);
	}

	void SpawnEnemy (string chosen) {
		enemy = (GameObject) Instantiate (TemplateEnemy, enemySpawn, new Quaternion (0,0,0,0));
		enemyBody = enemy.rigidbody2D;
		enemyBody.gravityScale = 0;
		enemyBolt = enemy.transform.Find ("enemyBolt").gameObject;
		enemyBolt.SetActive (false);
		enemy.GetComponent<EnemyControl> ().enabled = false;

		foreach (string element in charArray) {
			var tempEnemy = enemy.transform.Find(element).gameObject;
			tempEnemy.SetActive (false);
		}

		var rollAgain = true;
		while (rollAgain) {
			var roll = charArray[Random.Range (0,charArray.Length)];
			if (roll != chosen) {
				ActivateEnemy(roll);
				rollAgain = false;
			}
		}
	}

}
