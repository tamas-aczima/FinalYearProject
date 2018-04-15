using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public bool onlyShowPath;
    public LayerMask unwalkableMask;
    private Node[,] grid;
    private Vector2 gridSize;
    [SerializeField] private float nodeRadius;
    [SerializeField] private int costMultiplier;
    [SerializeField] private int obstacleProximityPenalty;
    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY;

    private void Start() {
        gridSize = new Vector2(10, 10);
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        GenerateGrid();
    }

    private void Update() {
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                if (Physics.CheckSphere(grid[x, y].position, nodeRadius, unwalkableMask)) {
                    grid[x, y].isWalkable = false;
                }
                else {
                    grid[x, y].isWalkable = true;
                }
            }
        }
    }

    public int MaxSize {
        get { return gridSizeX * gridSizeY; }
    }

    private void GenerateGrid() {
        grid = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                float xCoord = x * nodeDiameter + nodeRadius;
                float zCoord = y * nodeDiameter + nodeRadius;
                Vector3 point = new Vector3(xCoord, 0, zCoord);
                bool walkable = !Physics.CheckSphere(point, nodeRadius, unwalkableMask);
                int movementCost = costMultiplier;
                if (!walkable)
                {
                    movementCost += obstacleProximityPenalty;
                }
                grid[x, y] = new Node(walkable, point, x, y, movementCost);
            }
        }
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = worldPosition.x / gridSize.x;
        float percentY = worldPosition.z / gridSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        //if (terrain != null)
        //{
        //    Gizmos.DrawWireCube(new Vector3(terrain.terrainData.heightmapWidth / 2, 20, terrain.terrainData.heightmapHeight / 2), new Vector3(gridSize.x, 40, gridSize.y));
        //}

        if (onlyShowPath)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
        //else
        //{
        //if (grid != null)
        //{
        //    foreach (Node n in grid)
        //    {
        //        Gizmos.color = n.isWalkable ? Color.white : Color.red;

        //        if (path != null)
        //        {
        //            if (path.Contains(n))
        //            {
        //                Gizmos.color = Color.black;
        //            }
        //        }

        //        Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - 0.1f));
        //    }
        //}
        //}
    }
}

