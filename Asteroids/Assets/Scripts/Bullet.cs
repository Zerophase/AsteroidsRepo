using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	//Speed of bullet
	public float bulletSpeed;
	private float move;
	
	//Positions which bullets wrap at
	private float wrapX;
	private float wrapZ;
	
	Transform explosion;
	
	//Grabbing the Player Ship object
	GameObject player;
	
	//Variable for gamecontroller script
	private GameController gc;
	
	void Awake()
	{
		//initializing gamecontroller var
		gc = GameObject.Find("GameController").GetComponent<GameController>();
		//checks to se if gamecontroller is present. If not throws an error message
		if (!gc)
			Debug.LogError("No GameController Found");
		else
		{
			wrapX = gc.WrapX;
			wrapZ = gc.WrapZ;
		}
		//length bullet lives for.
		float bulletLife = 2f;
		//destroys bullet when duration equal o bulletlife passes.
		Destroy(gameObject, bulletLife);
	}
	
	void Update () 
	{
		//movement of bullet adjusted for computer frame rate.
		move = bulletSpeed * Time.deltaTime;
		transform.Translate(Vector3.forward * move);
	}
	
	void FixedUpdate()
	{
		//grabs the checkposition method from Utils script.
		Utils.CheckPosition(transform, wrapX, wrapZ);
	}
	
	//Destroys the bullet upon entering an asteroid
	void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
	}
}
