using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeUIText : MonoBehaviour
{
    public GameObject textAmmoLeft;
    public GameObject textAmmoRight;
    public GameObject gunLeft;
    public GameObject gunRight;
    
    private float ammoLeft;
    private float ammoRight;

    TextMeshProUGUI textProAmmoLeft;
    TextMeshProUGUI textProAmmoRight;

    // Start is called before the first frame update
    void Start()
    {
        textProAmmoLeft = textAmmoLeft.GetComponent<TextMeshProUGUI>();
        textProAmmoRight = textAmmoRight.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoLeft = gunLeft.GetComponent<PlayerGun>().ammo;
        ammoRight = gunRight.GetComponent<PlayerGun>().ammo;

        textProAmmoLeft.text = ammoLeft.ToString();
        textProAmmoRight.text = ammoRight.ToString();
    }
}
