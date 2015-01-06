using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	/*****************************************************
	 * public variables to allow tweeking of control keys
	 * ***************************************************/
	public string keyTurnLeft;
	public string keyTurnRight;
	public string keyThrust;
	public string keyFire;
	// How fast can we turn?
	public float thrustTurnPower;
	//how fast can we move?
	public float thrustPower;
	//Positions which bullets wrap at
	private float wrapX;
	private  float wrapZ;
	//Gameobject for controlling ship thrusters and bool for whether they are on.
	private GameObject thrusters;
	private bool thrustersOn;
	
	//declaring GameController script reference
	private GameController gc;
	//declaring a bullet in player scrippt.
	public GameObject Bullet;
	private MeshRenderer playerMesh;
	private MeshRenderer thrusterMesh;
	private bool warp;



	//location bullet fires from
	public Transform FireLocation;
	
	private float spaceTime;

	private float timeLeftOfDelay = 1.5f;
	private const float audioDelay = 0.65f;

	public AudioClip[] Sounds = new AudioClip[2];
	
	private enum SoundEffects { LASER, THRUST };
	
	public GameObject PlayerShip;
	// Use this for initialization
	void Awake()
	{
		gameObject.name = "Player_Ship";
		//initializes GameController script reference
		gc = GameObject.Find("GameController").GetComponent<GameController>();
		playerMesh = GameObject.Find("Player_Ship/Player").GetComponent<MeshRenderer>();
		thrusterMesh = GameObject.Find("Player_Ship/Player").GetComponent<MeshRenderer>();
		//Finds gameobjec wih a thrusters tag.
		thrusters = GameObject.FindGameObjectWithTag("Thrusters");
		
		//if no gc shoos error out, if not sets wrap positions.
		if (!gc)
			Debug.LogError("No GameController Found");
		else
		{
			wrapX = gc.WrapX;
			wrapZ = gc.WrapZ;
		}
		
		//sets this game object isactive to false and thrusters on to false.
		if (thrusters) 
		{
			thrusters.SetActive(false);
			thrustersOn = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//These someting == null methods are for refinding the objects attached to the player when he dies.
		if (thrusters == null)
			thrusters = GameObject.FindGameObjectWithTag("Thrusters");

		if(PlayerShip == null)
			PlayerShip = GameObject.FindGameObjectWithTag("Player");

		if(playerMesh == null)
			playerMesh = GameObject.Find("Player_Ship/Player").GetComponent<MeshRenderer>();

		if(thrusterMesh == null)
			thrusterMesh = GameObject.Find("Player_Ship/Player").GetComponent<MeshRenderer>();
		//handle ship rotation
		//if (this.gameObject != null) 
		//{
			if(Input.GetKey (keyTurnLeft))
			{
				rigidbody.AddTorque(Vector3.up * -thrustTurnPower, ForceMode.Acceleration);
			}
			else if(Input.GetKey (keyTurnRight)){
				rigidbody.AddTorque(Vector3.up * thrustTurnPower, ForceMode.Acceleration);
			}
			else{
				rigidbody.angularVelocity = Vector3.zero;
			}
		
		//handle thrust
			if(Input.GetKey (keyThrust))
			{
				//adds forward force to the ship.
				rigidbody.AddForce(transform.forward * thrustPower, ForceMode.Acceleration);
				//if thrusters aren't on when pressing key the game activates them.
				if(!thrustersOn)
				{
					thrusters.SetActive(true);
					thrustersOn = true;
				}
				if (!audio.isPlaying && timeLeftOfDelay > audioDelay)
				{
					audio.clip = Sounds[(int)SoundEffects.THRUST];
					audio.Play();
					timeLeftOfDelay = 0.0f;
				}
				else
				{
					timeLeftOfDelay += Time.deltaTime;
				}
			}
		//turns thrusters off if key is not pressed
			else if (Input.GetKeyUp (keyThrust))
			{
				thrusters.SetActive(false);
				thrustersOn = false;
				audio.Stop ();
			}
		//tell game to create bullet
			if (Input.GetKeyDown(keyFire))
			{
				//creates bullet
				audio.PlayOneShot(Sounds[(int)SoundEffects.LASER]);
				Instantiate(Bullet, FireLocation.position, FireLocation.rotation);
			}
		
			hyperSpace();
	}

	//Responsible for causing hyperspace
	bool prevent = true;
	void hyperSpace()
	{
		//When the player presses e and isn't warps a random location is selected for him to warp to.
		//The player mesh is turned off to give the effect of warping.
		if (Input.GetKeyDown("e") && !warp) 
		{
			playerMesh.enabled = false;
			thrusterMesh.enabled = false;
			transform.position = Utils.RandomVector3(wrapX, wrapZ, true);
			warp = true;
			prevent = false;
		}
		
		//Responsible for setting the time before the player pops out of hyper space
		//And letting the game know it is time to do that.
		if (warp) 
		{
			spaceTime += Time.deltaTime;
			if (spaceTime >= 0.5f) 
				warp = false;
		}
		
		//Pops the player out of hyper space by telling his meshes to turn back on.
		if (!warp && !prevent) 
		{
			playerMesh.enabled = true;
			thrusterMesh.enabled = true;
			prevent = true;
			spaceTime = 0;
		}
	}
	
	void FixedUpdate()
	{
		//checks for wrap
		Utils.CheckPosition(transform, wrapX, wrapZ);
	}
	
	//What happens when the player hits an asteroid
	void OnTriggerEnter(Collider c)
	{
			//Stops Player audio, takes away a life, destroys the gameobject, and then
			//start the respawn function in the GameController.
			if (c.gameObject.tag == "a_large" || c.gameObject.tag == "a_medium" 
				|| c.gameObject.tag == "a_small") 
			{
				audio.Stop();
				gc.playerLives--;

				Destroy(gameObject);
				gc.SpawnPlayer();
			}
	}
}
