using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;

public class EnemyAI : MonoBehaviour
{

    public GameObject targetPlayer;
    public float speed;
    public float rangeToAttack;
    public float rangeToChase;
    public GameObject ray;

    enum state { chase, attack, patrol};

    private state currState;
    private Vector3 strafePosition;
    private bool hasStrafePosition = false;
    private int groundLayerMask = 1 << 3;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        currState = state.patrol;

        navMeshAgent.speed = speed;
        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = false;
        navMeshAgent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        // we could make it check every second if we want more stalling;

        CheckForChangeInState();
        //print(currState);

        if (currState == state.patrol)
        {
            Patrol();
        }
        else if (currState == state.chase)
        {
            Chace();
            //RotateTowardsTarget();
        }
        else if (currState == state.attack)
        {
            Attack();
            //RotateTowardsTarget();
        }


    }

    void CheckForChangeInState()
    {
        if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToAttack || hasStrafePosition)
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
            hasStrafePosition = false;
            currState = state.patrol;
        }
    }

    void ShootPlayer()
    {
        //print("SHOOT");

    }

    void Attack()
    {
        //ShootPlayer();

        //print(hasStrafePosition);
        //print("slay");

        if (Vector3.Distance(transform.position, strafePosition) > 3f && hasStrafePosition)
        {
            navMeshAgent.SetDestination(strafePosition);
            //navMeshAgent.destination = strafePosition; 
            //transform.LookAt(targetPlayer.transform);
        }
        else
        {
            // Shoot from the ray on top of the enemy????

            if (Vector3.Distance(targetPlayer.transform.position, transform.position) > rangeToAttack)
            {
                hasStrafePosition = false;
            }
            else
            {
                GetStrafePosition();
                hasStrafePosition = true;
                print(strafePosition);
            }
            //navMeshAgent.ResetPath();
            /*int x = Random.Range(-10, 10);
            int z = Random.Range(-10, 10);
            strafePosition = transform.position;
            strafePosition.x = transform.position.x + x;
            strafePosition.z = transform.position.z + z;*/

        }
    }

    void GetStrafePosition()
    {
        Vector3 rayToShoot = ray.transform.position;
        ray.transform.Rotate(0, 0, -45, Space.Self);

        Transform rayTransform = ray.transform;
        RaycastHit hit;

        if (Physics.Raycast(rayToShoot, rayTransform.right, out hit, 30f, groundLayerMask))
        {
            
            Debug.DrawRay(rayToShoot, rayTransform.right * hit.distance, Color.yellow);
            Vector3 hitPoint = hit.point;
            hitPoint.y = hit.point.y;
            strafePosition = hitPoint;
        }
        else
        {
            Debug.DrawRay(rayToShoot, rayTransform.right * 1000, Color.green);
        }

        ray.transform.Rotate(0, 0, 45, Space.Self);
    }

    void Patrol()
    {
        // Do nothing right now, go back and forth from random spots???
        //navMeshAgent.destination = transform.position;
    }

    void Chace()
    {
        // Move the enemy towards the target
        navMeshAgent.SetDestination(targetPlayer.transform.position);
        //transform.LookAt(targetPlayer.transform);
    }

    void RotateTowardsTarget()
    {
        var rotationAngle = Quaternion.LookRotation(targetPlayer.transform.position - transform.position); // we get the angle has to be rotated
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 10); // we rotate the rotationAngle 
    }
}
