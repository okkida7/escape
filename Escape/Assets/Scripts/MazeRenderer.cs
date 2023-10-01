using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject mazeCellPrefab;
    public float cellSize = 1f;
    public Vector3 doorPosition;
    private void Start()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();
        for(int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for(int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                GameObject newCell = Instantiate(mazeCellPrefab, new Vector3((float)x * cellSize, 0f, (float)y * cellSize), Quaternion.identity);
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();
                bool top = maze[x,y].topWall;
                bool left = maze[x,y].leftWall;

                bool right = false;
                bool bottom = false;
                doorPosition = maze[x,y].saveDoor;
                if(x == mazeGenerator.mazeWidth - 1) right = maze[(int)doorPosition.x,(int)doorPosition.z].rightWall;
                if(y == 0) bottom = maze[(int)doorPosition.x,(int)doorPosition.z].bottomWall;

                mazeCell.Init(top, bottom, right, left);

                
            }
        }
    }

}
