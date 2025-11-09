
using UnityEngine;
using UnityEngine.AI;

// Dave / GameDevelopment (2020) FULL 3D ENEMY AI in 6 MINUTES! || Unity Tutorial. [Online] Available at: https://www.youtube.com/watch?v=UjkSFoLxesw (Accessed: 3 November 2025) 
//Rehope Games (2023) How to Add MUSIC and SOUND EFFECTS to Game in Unity | Unity 2D Platformer Tutorial #16. [Online] Available at: https://www.youtube.com/watch?v=N8whM1GjH4w (Accessed: 7 November 2025) 
public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    
    
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

   
    public float timeBetweenAttacks;
    bool alreadyAttacked;


   
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

   
    private bool isInContactWithPlayer = false;

    AudioManager audioManager;
    int count = 0;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
       
        agent.SetDestination(player.position);

               
       float distanceToPlayer = Vector3.Distance(transform.position, player.position);
       float contactThreshold = 1.5f; 

        if (distanceToPlayer <= contactThreshold && !alreadyAttacked)
        {
            
            if (!alreadyAttacked)
            {
               
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }

           
        }

      if (distanceToPlayer < 100f && count == 0) 
      {
         audioManager.PlaySFX(audioManager.warning);
         count++;
        }
    }

   
   private void ResetAttack()
    {
        alreadyAttacked = false;
        count = 0; 
    }

 private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    } 
}