using UnityEngine;
using System.Collections;

public class Asteroids : MonoBehaviour 
{
	private float move;
	//made private so all austeroids are set to 200 by default.
	public float Speed;
	
	private float wrapX;
	private float wrapZ;
	
	private float asteroidTimer;
	public static int asteroidsOnScreen = 0;

    public int Score;
	
	private Vector3 moveVector;
		
	//GameObject asteroid = GameObject.Find("Asteroid_Large_01");
	private GameController gc;
	
	public GameObject[] asteroids;
	
	public GameObject Explosion;
	// Use this for initialization
	void Awake()
	{
		gc = GameObject.Find("GameController").GetComponent<GameController>();
		//checks to se if gamecontroller is present. If not throws an error message
		if (!gc)
			Debug.LogError("No GameController Found");
		else
		{
			wrapX = gc.WrapX;
			wrapZ = gc.WrapZ;
		}
		moveVector = Utils.RandomVector3(-1, 1, "y", 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Moves the asteroid around screen.
		transform.Translate(moveVector * Speed * Time.deltaTime);
	}
	
	void FixedUpdate()
	{
		//Checks to see if the asteroid should wrap from one end of the screen to the next.
		Utils.CheckPosition(transform, wrapX, wrapZ);
	}
	
	void OnTriggerEnter(Collider c)
	{
		//Destroys the asteroid if the bullet hits it
		if (c.gameObject.tag == "Bullet") 
		{
			Vector3 currentPos = transform.position;
			string currentTag = transform.tag;
			
			int createAsteroids = 2;
			//What to do if large or medium asteroid
			if (currentTag == "a_large" || currentTag == "a_medium") 
			{
                gc.UpdateScore(Score);
                asteroidsOnScreen--;
				for (int i = 0; i < createAsteroids; i++) 
				{
					//spawns two asteroids.
					SpawnAsteroids(currentPos, Utils.RandomYRotation(), currentTag);	
				}
			}
			
 			
			//Learn shuriken this weekend
			Instantiate(Explosion, transform.position, transform.rotation);
			Destroy(gameObject);
			
			//if small the object is just destroyed.
			if(currentTag == "a_small")
			{
                gc.UpdateScore(Score);
                asteroidsOnScreen--;
				ZeroAsteroid();
			}
		}
	}
	
	//for spawning asteroids when the asteroid is hit by a bullet.
	public void SpawnAsteroids(Vector3 position, Quaternion rotation)
	{
		Instantiate(asteroids[Random.Range(0,3)], position, rotation);
		asteroidsOnScreen++;
	}
	
	//Decides which asteroid to spawn based on teh type of asteroid
	public void SpawnAsteroids(Vector3 position, Quaternion rotation, string asteroid)
	{
		if (asteroid == "a_large")
		{
			Instantiate(asteroids[Random.Range(0, 3)], position, rotation);
			asteroidsOnScreen++;
		}
		else if(asteroid == "a_medium")
		{
			Instantiate(asteroids[Random.Range(0, 3)], position, rotation);
			asteroidsOnScreen++;
		}
	}
	
	//if no asteroids are on the screen lets the game know it is time to build the next level
	public void ZeroAsteroid()
	{
		if(asteroidsOnScreen == 0)
		{
			gc.NextLevel = true;
		}
	}
}
