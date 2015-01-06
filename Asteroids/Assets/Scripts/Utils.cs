using UnityEngine;
using System.Collections;

//ctrl + " takes you to the Unity documentation.
public class Utils : MonoBehaviour {
	
    private GameController gc;

    private static float wrapObjectX;
    private static float wrapObjectZ;

    public void Awake()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        //checks to se if gamecontroller is present. If not throws an error message
        if (!gc)
            Debug.LogError("No GameController Found");
        else
        {
            wrapObjectX = gc.WrapX;
            wrapObjectZ = gc.WrapZ;
        }
    }

	//Selects a random Quaternian value for the rotation
	public static Quaternion RandomYRotation ()
	{
		Quaternion randYRot = new Quaternion ();
		randYRot = Quaternion.Euler (0, Random.Range (0, 360), -180);
		return randYRot;
	}
	
	//creates a random vector
	public static Vector3 RandomVector3 (float min, float max)
	{
		Vector3 randVec3 = new Vector3 ();
		randVec3 = new Vector3 (Random.Range (min, max), Random.Range (min, max), Random.Range (min, max));
		return randVec3;
	}
	
	//creates a random vector
	public static Vector3 RandomVector3 (float min, float max, string axisToClamp)
	{
		Vector3 randVec3 = new Vector3 ();
		if(axisToClamp == "x" || axisToClamp == "X"){
			randVec3 = new Vector3 (0, Random.Range (min, max), Random.Range (min, max));
		}
		else if(axisToClamp == "y" || axisToClamp == "Y"){
			randVec3 = new Vector3 (Random.Range (min, max), 0,  Random.Range (min, max));
		}
		else if(axisToClamp == "z" || axisToClamp == "Z"){
			randVec3 = new Vector3 (Random.Range (min, max), Random.Range (min, max), 0);
		}
		return randVec3;
	}
	
	//creates a random vector
	public static Vector3 RandomVector3 (float MaxX, float MaxY, bool warp)
	{
		Vector3 randVec3 = new Vector3 ();
		randVec3 = new Vector3 (Random.Range (-MaxX, MaxX), 0, Random.Range (-MaxY, MaxY));
		
		return randVec3;
	}
	
	//creates a random vector
	public static Vector3 RandomVector3 (float min, float max, string axisToClamp, float clampValue)
	{
		Vector3 randVec3 = new Vector3 ();
		if(axisToClamp == "x" || axisToClamp == "X"){
			randVec3 = new Vector3 (clampValue, Random.Range (min, max), Random.Range (min, max));
		}
		else if(axisToClamp == "y" || axisToClamp == "Y"){
			randVec3 = new Vector3 (Random.Range (min, max), clampValue,  Random.Range (min, max));
		}
		else if(axisToClamp == "z" || axisToClamp == "Z"){
			randVec3 = new Vector3 (Random.Range (min, max), Random.Range (min, max), clampValue);
		}
		return randVec3;
	}
	
	//checks if it is time to wrap the ship.
	public static void CheckPosition(Transform curTransform, float wrapX, float wrapZ)
	{
		if(curTransform.position.x > wrapX){
			WrapObject(curTransform, "x");
		}
		else if (curTransform.position.x < -wrapX){
			WrapObject(curTransform, "x");	
		}
		
		if(curTransform.position.z > wrapZ){
			WrapObject(curTransform, "z");
		}
		else if (curTransform.position.z < -wrapZ){
			WrapObject(curTransform, "z");	
		}
	}
	
	//tells what to do when the ship should be wrapping.
	public static void WrapObject(Transform currentObject, string axisToWrap){
		
		Vector3 currentPosition = currentObject.transform.position;
		
		if(axisToWrap.ToLower() == "x"){
            if (currentObject.transform.position.x < (wrapObjectX * -1)) 
            {
                currentObject.transform.position = new Vector3(-currentPosition.x - 15, currentPosition.y, currentPosition.z);
            }
            else if(currentObject.transform.position.x > wrapObjectX)
            {
                currentObject.transform.position = new Vector3(-currentPosition.x + 15, currentPosition.y, currentPosition.z);
            }
				
		}
		else if (axisToWrap.ToLower() == "z") {

            if (currentObject.transform.position.z < (wrapObjectZ * -1)) 
            {
                currentObject.transform.position = new Vector3(currentPosition.x, currentPosition.y, -currentPosition.z - 15);
            }
            else if (currentObject.transform.position.z > wrapObjectZ) 
            {
                currentObject.transform.position = new Vector3(currentPosition.x, currentPosition.y, -currentPosition.z + 15);
            }
			
		}
	}

}
