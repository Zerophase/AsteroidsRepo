using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//HOMEWORK: Write Comments on every line.
public class GameController : MonoBehaviour {

	// Use this for initialization
	public float WrapX;
	public float WrapZ;
	
	private float spawnX, spawnZ;
	
	public int startAsteroids = 4;
	
	public GameObject[] asteroids;
	 
	 //Gives the player some lives
	public int playerLives = 3;
    private int iconCount = 0;

    //for giving the playerLives an Icon
    private Texture playerLiveIcon;

    //Make these private
    public int CurrentScore = 0;
    private int bonusLife;

	public GameObject PlayerShip;
	private MeshRenderer playerMesh;

    private Rect liveRect = new Rect(10, 70, 120, 50);

	private bool nextLevel = false;
	public bool NextLevel { set { nextLevel = value; } }

    private GUIText p1Score;
	//private Asteroids asteroid;
	void Awake()
	{
		//sets and keeps screen bounds for checking for wrap upon start up.
		WrapX = Camera.main.orthographicSize * Camera.main.aspect;
		WrapZ = Camera.main.orthographicSize;

        p1Score = GameObject.Find("p1Score").GetComponent<GUIText>();
        p1Score.text = "SCORE\n0";
		//asteroid = GameObject.Find("Asteroid_Large_01").GetComponent<Asteroids>();
	}
	
	void Start () 
	{
		//sets the values to use for the range of spawning
		spawnX = WrapX - 15;
		spawnZ = WrapZ - 15;
		
		//finds the icon for the player lives icon
        playerLiveIcon = Resources.Load<Texture>("single_ship");

        bonusLife = 10000;

        Asteroids.asteroidsOnScreen = 0;
        //creates the first asteroid field
		createAsteroidField();
	}

    void OnGUI()
    {
    	//Displays the player lives on screen based on lives count
        if (playerLives > iconCount)
        {
            liveRect = new Rect(10, 70, 40 * playerLives, 50);
            iconCount = playerLives;
        }
        //IF the lives count goes down takes an icon away.
        else if(playerLives < iconCount)
        {
            liveRect = new Rect(10, 70, 40 * playerLives, 50);
            iconCount = playerLives;
        }
        //responsible for laying out the lives on screen.
        GUILayout.BeginArea(liveRect);
        GUILayout.BeginHorizontal();

        for (int i = 0; i < playerLives; i++)
        {
            GUILayout.Label(playerLiveIcon);
        }


        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    //Updates the score displayed to the player
    public void UpdateScore(int score)
    {
        CurrentScore += score;

        p1Score.text = "SCORE\n" + CurrentScore.ToString();
    }

    //Spawns asteroid fields whenever needed
	void createAsteroidField()
	{
		//initial number of asteroids spawned
		int spawnedAsteroids = 0;
		
		Vector3 position = new Vector3();
		
		//Counts up the asteroids spawned until the start asteroid value has been reached for the field.
		while (spawnedAsteroids < startAsteroids)
		{
			//Chooses random start values for the asteroids to spawn at
			float xPos = Mathf.Lerp(-spawnX, spawnX, Random.value);
			float zPos = Mathf.Lerp(-spawnZ, spawnZ, Random.value);
			//assins the x and z positions generated previoulsy to a vector.
			position = new Vector3(xPos, 0.0f, zPos);
			
			//Tells the asteroids where to spawn and what rotation to have.
			SpawnAsteroids(position, Utils.RandomYRotation());
			
			//Counts up the amount of asteroids spawned.
			spawnedAsteroids++;
		}
		
		//increaseds the amount of asteroids to start with on the next field.
		startAsteroids++;
	}
	
	//See about just using the method in asteroids.
    //Need to have Game Controller spawn an empty game object with
    //asteroids script on it to move SpawnAsteroids to Game Controller.
    //and have Game Controller call methods from that?
	public void SpawnAsteroids(Vector3 position, Quaternion rotation)
	{
		//Makes sure noting is in the positoin the asteroid starts at.
		if (!Physics.CheckSphere(position, 256f)) 
		{
			//creates the asteroid
			Instantiate(asteroids[Random.Range(0,3)], position, rotation);
			//Counts up the amount of asteroids on the screen.
			Asteroids.asteroidsOnScreen++;
		}
		else 
		{
			//If something is in the way a new position is found for that asteroids, and spawn asteroids is called again.
			float xPos = Mathf.Lerp(-spawnX, spawnX, Random.value);
            float zPos = Mathf.Lerp(-spawnZ, spawnZ, Random.value);
            position = new Vector3(xPos, 0.0f, zPos);

            SpawnAsteroids(position, Utils.RandomYRotation());
		}

	}

	public void SpawnPlayer()
	{
		//Calls respawnPlayer method. the first float is how long the
		//game waits before calling the method the first time. After
		//the first time it repeats the function at the 3rd value, the
		//defined repeat rate.
		InvokeRepeating("respawnPlayer", 0.25f, 0.25f);
	}

	private void respawnPlayer()
	{
		//CheckSphere checks all colliders in the scene for overlaps.
		if (playerLives > 0  &&
		    !Physics.CheckSphere(Vector3.zero, 256f))
		{
			//Creates the player after he dies in the center of the screen
			Instantiate(PlayerShip, Vector3.zero, Quaternion.identity);
			//cancels repeated invoking of respawnplayer if he exists.
			CancelInvoke("respawnPlayer");
		}
        else if(playerLives == 0)
        {
        	//loads the game over screen when the player runs out of lives.
            Application.LoadLevel(2);
        }
	}
	// Update is called once per frame
	void Update () 
	{
		//Adds a player live whenever he earns 10,000 points
        if (CurrentScore >= bonusLife)
        {
            playerLives++;

            bonusLife += 10000;

            Debug.Log("Player Lives = " + playerLives);
        }

        //Responsible for creating the next asteroid field based on a timer.
		if (nextLevel) 
		{
			nextLevel = false;
			//Gives player a 2 second break.
			StartCoroutine(startNextLevel(2.0f));
		}
	}
	//The time to wait before starting the next asteroid field.
	IEnumerator startNextLevel(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		
		createAsteroidField();
	}
}
