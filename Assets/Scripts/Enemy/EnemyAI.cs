using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public GameObject targetPlayer;
    public float speed;
    public float rangeToAttack;
    public float rangeToChase;

    enum state { chase, attack, patrol};

    private state currState;
    private Vector3 strafePosition;
    private bool hasStrafePosition = false;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        currState = state.patrol;
    }

    // Update is called once per frame
    void Update()
    {
        

        // we could make it check every second if we want more stalling;

        CheckForChangeInState();

        if(currState == state.patrol)
        {
            Patrol();
        }
        else if (currState == state.chase)
        {
            Chace();
        }
        if (currState == state.attack)
        {
            //Attack();
        }
/*
        var rotationAngle = Quaternion.LookRotation(targetPlayer.transform.position - transform.position); // we get the angle has to be rotated
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 10); // we rotate the rotationAngle */

        
    }

    void CheckForChangeInState()
    {
        if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToAttack)
        {
            currState = state.attack;
        }
        else if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToChase)
        {
            hasStrafePosition = false;
            currState = state.chase;
        }
        else
        {
            currState = state.patrol;
        }
    }

    void ShootPlayer()
    {
        //print("SHOOT");

    }

    void Attack()
    {
        ShootPlayer();

        if (Vector3.Distance(transform.position, strafePosition) > 3 && hasStrafePosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, strafePosition, Time.deltaTime * speed);
        }
        else
        {
            // Shoot from the ray on top of the enemy

            /*hasStrafePosition = true;
            int x = Random.Range(-10, 10);
            int z = Random.Range(-10, 10);
            strafePosition = transform.position;
            strafePosition.x = transform.position.x + x;
            strafePosition.z = transform.position.z + z;*/

        }
    }

    void Patrol()
    {
        // Do nothing right now, go back and forth from random spots???
        navMeshAgent.destination = transform.position;
    }

    void Chace()
    {
        // Move the enemy towards the target

        navMeshAgent.destination = targetPlayer.transform.position;
        transform.LookAt(targetPlayer.transform);
    }
}
