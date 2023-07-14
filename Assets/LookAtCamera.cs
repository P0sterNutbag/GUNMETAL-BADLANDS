using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	public UIManagerOverworld uiManager;

	void Update()
	{
		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(cameraRay, out RaycastHit cameraRayHit))
		{
			GameObject hitObj = cameraRayHit.transform.gameObject;

			// look at camera
			if (hitObj.layer == 3)
			{
				Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
				transform.LookAt(targetPosition);
			}
			
			// interact with NPC
			if (hitObj.CompareTag("NPC"))
			{
				uiManager.targetTransform = hitObj.transform;
            }
			else
            {
				if (uiManager.targetTransform != null)
                {
					uiManager.targetTransform = null;
                }
            }
		}
	}
}
