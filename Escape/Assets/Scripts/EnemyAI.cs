using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FindPlayer());

    }

    IEnumerator FindPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);

            float distance = Vector3.Distance(agent.transform.position, player.transform.position);
            if (distance <= 3f)
            {
                agent.destination = player.transform.position;
            }
        }
    }
}
