using AStarPathfinding;
using UnityEngine;

public class BoxNodeReference : MonoBehaviour
{
    public Node currentNode;

    public void ReleaseNode()
    {
        if (currentNode != null)
        {
            currentNode.SetOccupied(false);
            currentNode = null;
        }
    }
}
