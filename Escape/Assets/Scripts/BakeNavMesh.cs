using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BakeNavMesh : MonoBehaviour
{
    [SerializeField] GameObject enemyAIPrefab;

    private bool environmentProcessed = false;
    private bool enemyAICreated = false;
    private EnemyAI enemyAI;
    public GameObject jumpCanvas;

    private void Start()
    {
        MakeEnemyAI();
        enemyAI = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();
    }
    private void Update()
    {
        if (!environmentProcessed)
        {
            ProcessEnvironment();
            environmentProcessed = true;
        }
        if(enemyAI.jumpScare)
        {
            jumpCanvas.SetActive(true);
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    private void ProcessEnvironment()
    {
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (var floor in floors)
        {
            NavMeshSurface surface = floor.AddComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }

        foreach (var wall in walls)
        {
            NavMeshObstacle obstacle = wall.AddComponent<NavMeshObstacle>();
            Collider wallCollider = wall.GetComponent<Collider>();

            if (wallCollider)
            {
                // Set the NavMeshObstacle's size to match the wall's collider size.
                obstacle.size = wallCollider.bounds.size;
                // Center the NavMeshObstacle to the wall's collider center (relative to the wall's transform).
                obstacle.center = wall.transform.InverseTransformPoint(wallCollider.bounds.center);
            }
            else
            {
                Debug.LogWarning($"Wall {wall.name} does not have a collider attached.");
            }
        }
    }


    private void MakeEnemyAI()
    {
        GameObject key = GameObject.FindGameObjectWithTag("Key");
        if (key != null)
        {
            Vector3 keyPos = key.transform.position;
            Vector3 enemyPos = keyPos;
            Instantiate(enemyAIPrefab, enemyPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No object with the 'Key' tag was found. Unable to determine position for enemy AI.");
        }
    }
}
