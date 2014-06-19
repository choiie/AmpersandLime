using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void OnGUI() {
		GUI.Box (new Rect (Screen.width/2-50, 90, 100, 90), "Loader Menu");

		if (GUI.Button(new Rect (Screen.width/2-40, 200,80,20), "Start Game")) {
			Application.LoadLevel("scene01");
		}
	}
}
