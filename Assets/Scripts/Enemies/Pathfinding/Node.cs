using UnityEngine;

public class Node : IHeapItem<Node> {

    public bool isWalkable;
    public Vector3 position;
    public int gridX;
    public int gridY;
    public int movementCost;

    public int gCost;
    public int hCost;
    public Node parent;
    private int heapIndex;

    public Node(bool walkable, Vector3 pos, int gX, int gY, int cost) {
        isWalkable = walkable;
        position = pos;
        gridX = gX;
        gridY = gY;
        movementCost = cost;
    }

    public int fCost {
        get { return gCost + hCost; }
    }

    public int HeapIndex {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
