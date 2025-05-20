using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfinding
{
    public interface IPathfinding
    {
        public void Initialize();
        Node FindNearestNode(Vector2 position, bool ignoreOccupied = false);
        List<Node> GeneratePath(Node start, Node end);
        List<Node> GetAllNodes();
    }
}