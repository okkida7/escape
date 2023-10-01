using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Vector2 patrolAreaMin; // Minimum x and z coordinates for patrol area
    public Vector2 patrolAreaMax; // Maximum x and z coordinates for patrol area
    private Vector3 nextPatrolPoint;
    public Animator anim;
    NavMeshAgent agent;
    GameObject player;
    public float chaseSpeed;
    public float patrolSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Generate initial patrol point
        nextPatrolPoint = GenerateRandomPatrolPoint();
        agent.destination = nextPatrolPoint;

        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);

            float distanceToPlayer = Vector3.Distance(agent.transform.position, player.transform.position);

            if (distanceToPlayer <= 3f && distanceToPlayer > 1f)
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isScreaming", false);
                anim.SetBool("isWalking", false);
                agent.speed = chaseSpeed;
                agent.destination = player.transform.position;
            }
            else if (distanceToPlayer < 1f)
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isScreaming", true);
                anim.SetBool("isWalking", false);
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene(3);

            }else if(distanceToPlayer > 2f)
            {
                GoToNextPatrolPoint();
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        anim.SetBool("isScreaming", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", true);
        nextPatrolPoint = GenerateRandomPatrolPoint();
        agent.speed = patrolSpeed;
        agent.destination = nextPatrolPoint;
    }

    Vector3 GenerateRandomPatrolPoint()
    {
        float randomX = Random.Range(patrolAreaMin.x, patrolAreaMax.x);
        float randomZ = Random.Range(patrolAreaMin.y, patrolAreaMax.y); // Note: We use 'y' because Vector2 does not have a 'z'
        float yPos = transform.position.y; // Keep the enemy at its current y position

        return new Vector3(randomX, yPos, randomZ);
    }
}
