using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Range(5,100)]
    public int mazeWidth = 5, mazeHeight = 5;
    public int startX, startY;
    MazeCell[,] maze;
    Vector2Int currentCell;
    public GameObject keyPrefab;
    public GameObject doorPrefab;
    private KeyController keyController;
    public GameObject keyPanel;
    private DoorController doorController;
    public GameObject doorWithoutKey;
    public GameObject doorWithKey;
    List<Vector2Int> deadEnds = new List<Vector2Int>();

    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];
        for(int x = 0; x < mazeWidth; x++){
            for(int y = 0; y < mazeHeight; y++){
                maze[x,y] = new MazeCell(x,y);
            }
        }
        CarvePath(startX, startY);
        PlaceKeyAtRandomDeadEnd(); 
        PlaceDoorAtRandomEdge(new Vector3(0, 0, 0));
        //PlaceEnemy();
        return maze;
    }
    List<Direction> directions = new List<Direction>
    {
        Direction.Up,
        Direction.Down,
        Direction.Left,
        Direction.Right
    };
    List<Direction> GetRandomDirections()
    {
        List<Direction> dir = new List<Direction>(directions);
        List<Direction> rndDir = new List<Direction>();

        while(dir.Count > 0)
        {
            int rnd = Random.Range(0,dir.Count);
            rndDir.Add(dir[rnd]);
            dir.RemoveAt(rnd);
        }
        return rndDir;
    }
    bool IsCellValid(int x, int y)
    {
        if(x < 0 || x >= mazeWidth || y < 0 || y >= mazeHeight || maze[x,y].visited)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    Vector2Int CheckNeighbour()
    {
        List<Direction> rndDir = GetRandomDirections();
        for(int i = 0; i < rndDir.Count; i++)
        {
            Vector2Int neighbour = currentCell;
            switch(rndDir[i])
            {
                case Direction.Up:
                    neighbour.y += 1;
                    break;
                case Direction.Down:
                    neighbour.y -= 1;
                    break;
                case Direction.Left:
                    neighbour.x -= 1;
                    break;
                case Direction.Right:
                    neighbour.x += 1;
                    break;
            }
            if(IsCellValid(neighbour.x, neighbour.y))
            {
                return neighbour;
            }
        }
        return currentCell;
    }
    void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if(primaryCell.x > secondaryCell.x)
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        }else if(primaryCell.x < secondaryCell.x)
        {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        }else if(primaryCell.y < secondaryCell.y)
        {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        }else if(primaryCell.y > secondaryCell.y)
        {
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
    }
    void CarvePath(int x, int y)
    {
        if(x < 0 || y < 0 || x >= mazeWidth || y >= mazeHeight)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds. Resetting to (0,0)");
        }
        currentCell = new Vector2Int(x,y);
        List<Vector2Int> path = new List<Vector2Int>();
        bool deadEnd = false;
        while(!deadEnd)
        {
            Vector2Int nextCell = CheckNeighbour();
            if(nextCell == currentCell)
            {
                deadEnds.Add(currentCell);
                for(int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i];
                    path.RemoveAt(i);
                    nextCell = CheckNeighbour();
                    if(nextCell != currentCell)
                    {
                        break;
                    }
                }
                if(nextCell == currentCell)
                {
                    deadEnd = true;
                }
            }else{
                BreakWalls(currentCell, nextCell);
                maze[currentCell.x, currentCell.y].visited = true;
                currentCell = nextCell;
                path.Add(currentCell);
            }
        }
        
    }
    float CalculateDistance(Vector3 pointA, Vector3 pointB)
    {
        return Vector3.Distance(pointA, pointB);
    }

    List<Vector3> GetFarEdgeCells(Vector3 playerPosition, float threshold)
    {
        List<Vector3> edgeCells = new List<Vector3>();
        List<Vector3> farCells = new List<Vector3>();
        for(int x = 0; x < mazeWidth; x++)
        {
            edgeCells.Add(new Vector3(x, 0, 0));
            edgeCells.Add(new Vector3(x, 0, mazeHeight - 1));
        }

        // Collect left and right edge cells.
        for(int y = 0; y < mazeHeight; y++)
        {
            edgeCells.Add(new Vector3(0, 0, y));
            edgeCells.Add(new Vector3(mazeWidth - 1, 0, y));
        }

        // Filter out cells that are too close to the player.
        foreach (Vector3 cell in edgeCells)
        {
            if(CalculateDistance(playerPosition, cell) >= threshold)
            {
                farCells.Add(cell);
            }
        }

        return farCells;
    }
    void Start()
    {
        keyController = GameObject.FindWithTag("Key").GetComponent<KeyController>();
        doorController = GameObject.FindWithTag("Door").GetComponent<DoorController>();
    }
    void Update()
    {
        if(keyController.isNear == true)
        {
            keyPanel.SetActive(true);
            Debug.Log("key is near");
        }else
        {
            keyPanel.SetActive(false);
        }
        if(doorController.isNear && doorController.isOpen)
        {
            doorWithoutKey.SetActive(false);
            doorWithKey.SetActive(true);
        }else if(doorController.isNear)
        {
            doorWithoutKey.SetActive(true);
            doorWithKey.SetActive(false);
        }else
        {
            doorWithoutKey.SetActive(false);
            doorWithKey.SetActive(false);
        }
    }

    void PlaceKeyAtRandomDeadEnd()
    {
        if(deadEnds.Count == 0) return;
        Vector2Int keyPosition = deadEnds[Random.Range(0, deadEnds.Count)];
        Vector3 worldPosition = new Vector3(keyPosition.x, 0.07f, keyPosition.y);
        Instantiate(keyPrefab, worldPosition, Quaternion.identity);
        
    }
    void PlaceDoorAtRandomEdge(Vector3 playerPosition)
    {
        float distanceThreshold = Mathf.Max(mazeWidth, mazeHeight) / 2.0f;  // Adjust this value if needed.
        List<Vector3> farCells = GetFarEdgeCells(playerPosition, distanceThreshold);

        if(farCells.Count == 0)
        {
            Debug.LogError("No suitable location found for the door. Increase the maze size or reduce the threshold.");
            return;
        }

        Vector3 doorPosition = farCells[Random.Range(0, farCells.Count)];

        // Adjusting the door's position, rotation, and wall data based on the edge.
        Quaternion doorRotation = Quaternion.identity; 
        float doorDepth = 0.5f; // half of the 0.1 scale on Z axis

        if(doorPosition.x == 0)
        {
            doorRotation = Quaternion.Euler(0, 90, 0);
            doorPosition.x -= doorDepth;
            maze[(int)doorPosition.x, (int)doorPosition.z].leftWall = false;
            maze[(int)doorPosition.x, (int)doorPosition.z].saveDoor = doorPosition;
            Debug.Log(1);
        }
        else if(doorPosition.x == mazeWidth - 1)
        {
            doorRotation = Quaternion.Euler(0, -90, 0);
            doorPosition.x += doorDepth;
            maze[(int)doorPosition.x, (int)doorPosition.z].rightWall = false;
            maze[(int)doorPosition.x, (int)doorPosition.z].saveDoor = doorPosition;
            Debug.Log(2);
        }
        else if(doorPosition.z == 0)
        {
            doorRotation = Quaternion.Euler(0, 180, 0);
            doorPosition.z -= doorDepth;
            maze[(int)doorPosition.x, (int)doorPosition.z].bottomWall = false;
            maze[(int)doorPosition.x, (int)doorPosition.z].saveDoor = doorPosition;
            Debug.Log(3);
        }
        else if(doorPosition.z == mazeHeight - 1)
        {
            doorRotation = Quaternion.Euler(0, 0, 0);
            doorPosition.z += doorDepth;
            maze[(int)doorPosition.x, (int)doorPosition.z].topWall = false;
            maze[(int)doorPosition.x, (int)doorPosition.z].saveDoor = doorPosition;
            Debug.Log(4);
        }
        doorPosition.y = 0.08f;

        Instantiate(doorPrefab, doorPosition, doorRotation);
    }
}


public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public class MazeCell
{
    public bool visited;
    public int x,y;
    public Vector3 saveDoor;

    public bool topWall;
    public bool leftWall;
    public bool rightWall;
    public bool bottomWall;

    public Vector2Int position{
        get {
            return new Vector2Int(x,y);
        }
    }

    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;

        visited = false;
        saveDoor = new Vector3(0,0,0);

        topWall = leftWall = true;
        rightWall = bottomWall = true;
    }
}

