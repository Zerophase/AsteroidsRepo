using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void OnGUI()
    {
    	//Creates the replay button on the Game over screen and this takes the player back to the first level.
    	//The text for the game over screen was made in the editor.
        if(GUI.Button( new Rect( (Screen.width / 2) - 75, (Screen.height / 2) - 100, 200, 150), "Press to play again."))
        {
            Application.LoadLevel(1);
        }
    }
}
