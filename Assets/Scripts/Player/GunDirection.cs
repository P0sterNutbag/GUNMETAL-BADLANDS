using UnityEngine;

public class GunDirection : MonoBehaviour
{
    public Camera fpsCam;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, fpsCam.transform.rotation, 0.1f);
    }
}
