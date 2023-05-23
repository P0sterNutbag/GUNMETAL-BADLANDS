using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public GameObject targetPlayer;

    public GameObject ray;
    public float minRayDistince;
    public float maxRayDistince;

    public float speed;
    public float rangeToAttack;
    public float rangeToChase;
    public float rangeToChaseAgain;

    public EnemyShooting enemyShooting;

    enum state { chase, attack, patrol, shoot};

    private state currState;
    private Vector3 strafePosition;
    private bool hasStrafePosition = false;
    private int groundLayerMask = 1 << 3;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyShooting = gameObject.GetComponent<EnemyShooting>();
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
        enemyShooting.ShootPlayer(targetPlayer);
    }

    void Attack()
    {

        //print(hasStrafePosition);

        //float randFloat = Random.Range(minRayDistince, maxRayDistince + 1);


        if (Vector3.Distance(transform.position, strafePosition) > 1f && hasStrafePosition)
        {
            transform.LookAt(targetPlayer.transform.position);
            navMeshAgent.SetDestination(strafePosition);
        }
        else
        {
            // rangeToChaseAgain prevents the AI from flipping between chase and attack

            if (Vector3.Distance(targetPlayer.transform.position, transform.position) > rangeToAttack + rangeToChaseAgain)
            {
                hasStrafePosition = false;
                currState = state.chase;
            }
            else
            {
                // First Shoot, then get stafe position again
                ShootPlayer();
                GetStrafePosition();
                hasStrafePosition = true;
            }

        }   
    }

    void GetStrafePosition()
    {
        int randInt = 0;

        while(randInt == 0)
        {
            randInt = Random.Range(-1, 2); // Random.Range is [,), so the second number is exclusive, so this actually will just return a -1 or 1
        }

        float randFloat = Random.Range(minRayDistince, maxRayDistince + 1);
        Vector3 rayToShoot = ray.transform.position;
        ray.transform.Rotate(0, 0, randFloat * randInt, Space.Self);

        Transform rayTransform = ray.transform;
        RaycastHit hit;

        if (Physics.Raycast(rayToShoot, rayTransform.up, out hit, 30f, groundLayerMask))
        {
            
            //Debug.DrawRay(rayToShoot, rayTransform.up * hit.distance, Color.yellow);
            Vector3 hitPoint = hit.point;
            hitPoint.y = hit.point.y + 1;
            strafePosition = hitPoint;
        }
        /*else
        {
            Debug.DrawRay(rayToShoot, rayTransform.up * 1000, Color.green);
        }*/

        randInt *= -1;
        ray.transform.Rotate(0, 0, randFloat * randInt, Space.Self);
    }

    void Patrol()
    {
        // Right now it just stands still

        if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToChase)
        {
            hasStrafePosition = false;
            currState = state.chase;
        }
    }

    void Chace()
    {
        // Move the enemy towards the target

        navMeshAgent.SetDestination(targetPlayer.transform.position);

        if (Vector3.Distance(targetPlayer.transform.position, transform.position) <= rangeToAttack)
        {
            currState = state.attack;
        }

        transform.LookAt(targetPlayer.transform.position);
    }

    // Currently not being used
    void RotateTowardsTarget()
    {
        var rotationAngle = Quaternion.LookRotation(targetPlayer.transform.position - transform.position); // we get the angle has to be rotated
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 10); // we rotate the rotationAngle 
    }
}
