using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    Grid grid;

    private void Awake() {
        grid = GetComponent<Grid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback) {
        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.GetNodeFromWorldPoint(request.pathStart);
        Node targetNode = grid.GetNodeFromWorldPoint(request.pathEnd);

        //if (targetNode.isWalkable) {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                //Unoptimized
                //Node currentNode = openSet[0];
                //for (int i = 1; i < openSet.Count; i++)
                //{
                //    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                //    {
                //        currentNode = openSet[i];
                //    }
                //}
                //openSet.Remove(currentNode);

                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour)) {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementCost;
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) {
                            openSet.Add(neighbour);
                        }
                        else {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        //}
        if (pathSuccess) {
            wayPoints = RetracePath(startNode, targetNode);
        }
        else {
            Debug.Log("no path");
        }
        callback(new PathResult(wayPoints, pathSuccess, request.callback));
    }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] wayPoints = SimplifyPath(path);

        Array.Reverse(wayPoints);

        grid.path = path;

        return wayPoints;
    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld) {
                wayPoints.Add(path[i].position);
            }
            directionOld = directionNew;
        }

        return wayPoints.ToArray();
    }

    private int GetDistance(Node a, Node b) {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY) {
            return 14 * distY + 10 * (distX - distY);
        }
        else {
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
