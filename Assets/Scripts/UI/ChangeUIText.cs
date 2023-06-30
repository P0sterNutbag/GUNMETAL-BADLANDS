using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeUIText : MonoBehaviour
{
    public GameObject textAmmoLeft;
    public GameObject textAmmoRight;
    public GameObject textRangefinder;
    public GameObject textHealth;
    public GameObject textSpeed;
    public GameObject gunLeft;
    public GameObject gunRight;
    public GameObject player;
    
    private float ammoLeft;
    private float ammoRight;
    private string aimDistance;
    private float health;
    private float speed;

    TextMeshProUGUI textProAmmoLeft;
    TextMeshProUGUI textProAmmoRight;
    TextMeshProUGUI textProRangefinder;
    TextMeshProUGUI textProHealth;
    TextMeshProUGUI textProSpeed;

    // Start is called before the first frame update
    void Start()
    {
        textProAmmoLeft = textAmmoLeft.GetComponent<TextMeshProUGUI>();
        textProAmmoRight = textAmmoRight.GetComponent<TextMeshProUGUI>();
        textProRangefinder = textRangefinder.GetComponent<TextMeshProUGUI>();
        textProHealth = textHealth.GetComponent<TextMeshProUGUI>();
        textProSpeed = textSpeed.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoLeft = gunLeft.GetComponent<PlayerGun>().stats.ammo;
        ammoRight = gunRight.GetComponent<PlayerGun>().stats.ammo;
        aimDistance = gunLeft.GetComponent<PlayerGun>().GetRange();
        health = player.GetComponent<PlayerHealth>().health;
        speed = Mathf.Round(player.GetComponent<PlayerMovement>().currentSpeed*5);

        textProAmmoLeft.text = ammoLeft.ToString();
        textProAmmoRight.text = ammoRight.ToString();
        textProRangefinder.text = aimDistance;
        textProHealth.text = health.ToString();
        textProSpeed.text = speed.ToString()+" kpm";
    }
}
