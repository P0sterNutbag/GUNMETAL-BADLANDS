using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

	Ray cameraRay;              // The ray that is cast from the camera to the mouse position
	RaycastHit cameraRayHit;    // The object that the ray hits

	void Update()
	{
		// Cast a ray from the camera to the mouse cursor
		cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		// If the ray strikes an object...
		if (Physics.Raycast(cameraRay, out cameraRayHit))
		{
			// ...and if that object is the ground...
			if (cameraRayHit.transform.gameObject.layer == 3)
			{
				// ...make the cube rotate (only on the Y axis) to face the ray hit's position 
				Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
				transform.LookAt(targetPosition);
			}
		}
	/* Get the mouse position
	Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	mousePosition.y = transform.position.y; // Match the player's z-position

	// Calculate the direction from the player to the mouse
	Vector3 direction = mousePosition - transform.position;

	// Calculate the angle in degrees
	float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

	// Rotate the player towards the mouse
	transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	/*Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
	//Vector3 mousePosition = Input.mousePosition;
	mousePosition.y = transform.position.y;
	Debug.Log(cam.ScreenToWorldPoint(Input.mousePosition));
	transform.LookAt(mousePosition);*/
}
}
