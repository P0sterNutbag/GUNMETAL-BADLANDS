using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public GameObject targetPlayer;
    public float speed;
    public float rangeToAttack;
    public float rangeToChase;
    public GameObject ray;
    public float minRayDistince;
    public float maxRayDistince;
    public float rangeToChaseAgain;

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

    /*bool CheckForChangeInState()
    {
        print(currState);
        if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToAttack && currState != state.attack)
        {
            currState = state.attack;
            return true;
        }
        else if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToChase && currState != state.chase)
        {
            hasStrafePosition = false;
            currState = state.chase;
            return true;
        }
        else if (currState != state.patrol)
        {
            hasStrafePosition = false;
            currState = state.patrol;

        }

        return false;
    }*/

    void ShootPlayer()
    {
        //print("SHOOT");

    }

    void Attack()
    {
        //ShootPlayer();

        //print(hasStrafePosition);

        //float randFloat = Random.Range(minRayDistince, maxRayDistince + 1);
        Vector3 rayToShoot = ray.transform.position;

        ray.transform.Rotate(0, 0, randFloat * randInt, Space.Self);

        Transform rayTransform = ray.transform;
        RaycastHit hit;

        if (Physics.Raycast(rayToShoot, rayTransform.up, out hit, 30f, groundLayerMask))
        {

            Debug.DrawRay(rayToShoot, rayTransform.up * hit.distance, Color.yellow);
           /* Vector3 hitPoint = hit.point;
            hitPoint.y = hit.point.y + 1;
            strafePosition = hitPoint;*/
        }
        else
        {
            Debug.DrawRay(rayToShoot, rayTransform.up * 1000, Color.green);
        }

        randInt *= -1;
        ray.transform.Rotate(0, 0, randFloat * randInt, Space.Self);
        randInt *= -1;

        if (Vector3.Distance(transform.position, strafePosition) > 1f && hasStrafePosition)
        {
            //print("here");
            navMeshAgent.SetDestination(strafePosition);
            //transform.LookAt(strafePosition);
            //navMeshAgent.destination = strafePosition; 
            //transform.LookAt(targetPlayer.transform);
        }
        else
        {
            // Shoot from the ray on top of the enemy????

            if (Vector3.Distance(targetPlayer.transform.position, transform.position) > rangeToAttack + rangeToChaseAgain)
            {
                hasStrafePosition = false;
                currState = state.chase;
                //print("chase");
            }
            else
            {
                GetStrafePosition();
                hasStrafePosition = true;
            }
            //navMeshAgent.ResetPath();
            /*int x = Random.Range(-10, 10);
            int z = Random.Range(-10, 10);
            strafePosition = transform.position;
            strafePosition.x = transform.position.x + x;
            strafePosition.z = transform.position.z + z;*/

        }   
    }

    int randInt;
    float randFloat;
    void GetStrafePosition()
    {
        randInt = 0;

        while(randInt == 0)
        {
            randInt = Random.Range(-1, 2);
        }

        randFloat = Random.Range(minRayDistince, maxRayDistince + 1);
        Vector3 rayToShoot = ray.transform.position;
        ray.transform.Rotate(0, 0, randFloat * randInt, Space.Self);

        Transform rayTransform = ray.transform;
        RaycastHit hit;

        if (Physics.Raycast(rayToShoot, rayTransform.up, out hit, 30f, groundLayerMask))
        {
            
            Debug.DrawRay(rayToShoot, rayTransform.up * hit.distance, Color.yellow);
            Vector3 hitPoint = hit.point;
            hitPoint.y = hit.point.y + 1;
            strafePosition = hitPoint;
        }
        else
        {
            Debug.DrawRay(rayToShoot, rayTransform.up * 1000, Color.green);
        }

        randInt *= -1;
        ray.transform.Rotate(0, 0, randFloat * randInt, Space.Self);
        randInt *= -1;
    }

    void Patrol()
    {
        if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToChase)
        {
            hasStrafePosition = false;
            currState = state.chase;
            //print("chase");
        }

        // Do nothing right now, go back and forth from random spots???
        //navMeshAgent.destination = transform.position;
        //CheckForChangeInState();
    }

    void Chace()
    {
        // Move the enemy towards the target

        navMeshAgent.SetDestination(targetPlayer.transform.position);

        if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToAttack)
        {
            //print("attack");
            currState = state.attack;
        }


        //CheckForChangeInState();
        transform.LookAt(targetPlayer.transform.position);
    }

    void RotateTowardsTarget()
    {
        var rotationAngle = Quaternion.LookRotation(targetPlayer.transform.position - transform.position); // we get the angle has to be rotated
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 10); // we rotate the rotationAngle 
    }
}
