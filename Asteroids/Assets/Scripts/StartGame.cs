using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	void OnGUI()
    {
    	//Gives the player a button to press to start the game.
    	//Text and asteroid / space ship art are rendered in the scene.
        if(GUI.Button(new Rect((Screen.width / 2) - 100, (Screen.height / 2) + 50, 200, 150), "Start Game"))
        {
            Application.LoadLevel(1);
        }
    }
}
