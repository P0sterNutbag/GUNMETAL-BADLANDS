using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeUIText : MonoBehaviour
{
    public GameObject textAmmoLeft;
    public GameObject textAmmoRight;
    public GameObject textRangefinder;
    public GameObject gunLeft;
    public GameObject gunRight;
    
    private float ammoLeft;
    private float ammoRight;
    private string aimDistance;

    TextMeshProUGUI textProAmmoLeft;
    TextMeshProUGUI textProAmmoRight;
    TextMeshProUGUI textProRangefinder;

    // Start is called before the first frame update
    void Start()
    {
        textProAmmoLeft = textAmmoLeft.GetComponent<TextMeshProUGUI>();
        textProAmmoRight = textAmmoRight.GetComponent<TextMeshProUGUI>();
        textProRangefinder = textRangefinder.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoLeft = gunLeft.GetComponent<PlayerGun>().ammo;
        ammoRight = gunRight.GetComponent<PlayerGun>().ammo;
        aimDistance = gunLeft.GetComponent<PlayerGun>().GetRange();

        textProAmmoLeft.text = ammoLeft.ToString();
        textProAmmoRight.text = ammoRight.ToString();
        textProRangefinder.text = aimDistance;
    }
}
