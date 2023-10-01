using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BakeNavMesh : MonoBehaviour
{
    // Rebake NavMesh at runtime
    //public NavMeshSurface surface;
    //public NavMeshObstacle obstacle;

    GameObject[] floors = new GameObject[] {};
    GameObject[] walls = new GameObject[] {};
    NavMeshSurface[] surfaces = new NavMeshSurface[] {};
    
    bool enemyAI = false; // has the enemy AI been instantiated?
    [SerializeField] GameObject enemyAIPrefab; 

    void Start()
    {
        /*
        //surface.BuildNavMesh();
        floors = GameObject.FindGameObjectsWithTag("Floor");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        surfaces = new NavMeshSurface[floors.Length];

        for (int i = 0; i < floors.Length; i++)
        {
            floors[i].AddComponent<NavMeshSurface>();
            surfaces[i] = floors[i].GetComponent<NavMeshSurface>();
        }
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].AddComponent<NavMeshObstacle>();
            walls[i].GetComponent<NavMeshObstacle>().center = new Vector3(0, 0, 0);
        }

        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        } */
    }

    private void Update()
    {
        if (floors.Length == 0)
        {
            floors = GameObject.FindGameObjectsWithTag("Floor");
            walls = GameObject.FindGameObjectsWithTag("Wall");
            surfaces = new NavMeshSurface[floors.Length];

            for (int i = 0; i < floors.Length; i++)
            {
                floors[i].AddComponent<NavMeshSurface>();
                surfaces[i] = floors[i].GetComponent<NavMeshSurface>();
            }
            
            for (int i = 0; i < walls.Length; i++)
            {
                
                walls[i].AddComponent<NavMeshObstacle>();
                walls[i].GetComponent<NavMeshObstacle>().center = new Vector3(0, 0, 0);
                 
            } 

            for (int i = 0; i < surfaces.Length; i++)
            {
                surfaces[i].BuildNavMesh();
            }
        }
        
        if (!enemyAI)
        {
            makeEnemyAI();
            //Instantiate(enemyAIPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            //enemyAI = true;

        } 
    }

    
    void makeEnemyAI()
    {
        GameObject key = GameObject.FindGameObjectWithTag("Key");
        Vector3 key_pos = key.transform.position;

        Vector3 enemy_pos = new Vector3(key_pos.x - .1f, key_pos.y, key_pos.z - .1f);
        Instantiate(enemyAIPrefab, enemy_pos, Quaternion.identity);
        enemyAI = true;
    }
    
}
