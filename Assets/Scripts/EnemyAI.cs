using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyAI : MonoBehaviour
{

    public GameObject targetPlayer;
    public float speed;
    public float rangeToShoot;

    enum state { moving, attacking, dead, idel};

    private state currState;
    private Vector3 strafePosition;
    private bool hasStrafePosition = false;


    // Start is called before the first frame update
    void Start()
    {
        currState = state.moving;
    }

    // Update is called once per frame
    void Update()
    {
        

        // we could make it check every second if we want more stalling;

        CheckForChangeInState();

        if (currState == state.attacking)
        {
            ShootPlayer();

            if (Vector3.Distance(transform.position, strafePosition) > 3 && hasStrafePosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, strafePosition, Time.deltaTime * speed);
            }
            else
            {
                

                hasStrafePosition = true;
                int x = Random.Range(-10, 10);
                int z = Random.Range(-10,10);
                strafePosition = transform.position;
                strafePosition.x = transform.position.x + x;
                strafePosition.z = transform.position.z + z;

            }
        }
        else if (currState == state.moving)
        {
            // Move towards the enemy 
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, Time.deltaTime * speed);
        }

        var rotationAngle = Quaternion.LookRotation(targetPlayer.transform.position - transform.position); // we get the angle has to be rotated
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 10); // we rotate the rotationAngle 

        
    }

    void CheckForChangeInState()
    {
        if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToShoot)
        {
            currState = state.attacking;
        }
        else
        {
            hasStrafePosition = false;
            currState = state.moving;
        }
    }

    void ShootPlayer()
    {
        //print("SHOOT");

    }
}
