using UnityEngine;

public class GunDirection : MonoBehaviour
{
    public Camera fpsCam;
    public float lookRotation = 0.02f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, fpsCam.transform.rotation, lookRotation);
    }
}
