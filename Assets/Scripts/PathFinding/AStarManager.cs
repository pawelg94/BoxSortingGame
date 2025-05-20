using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfinding
{
    public class AStarManager : MonoBehaviour, IPathfinding
    {
        [SerializeField] private Transform walkableRoot;

        public List<Node> Nodes = new List<Node>();
        public void Initialize()
        {
            if (walkableRoot == null)
            {
                Debug.LogError("[AStarManager] Walkable root is not assigned!");
                return;
            }

            Nodes.Clear();
            Nodes.AddRange(walkableRoot.GetComponentsInChildren<Node>());
            Debug.Log($"[AStarManager] Initialized with {Nodes.Count} nodes.");
        }
        public List<Node> GeneratePath(Node start, Node end)
        {
            List<Node> openSet = new List<Node>();

            foreach (Node n in Nodes)
            {
                n.gScore = float.MaxValue;
            }

            start.gScore = 0;
            start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                int lowestF = default;

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FScore() < openSet[lowestF].FScore())
                    {
                        lowestF = i;
                    }
                }

                Node currentNode = openSet[lowestF];
                openSet.Remove(currentNode);

                if (currentNode == end)
                {
                    List<Node> path = new List<Node>();

                    path.Insert(0, end);

                    while (currentNode != start)
                    {
                        currentNode = currentNode.cameFrom;
                        path.Add(currentNode);
                    }

                    path.Reverse();
                    return path;
                }

                foreach (Node connectedNode in currentNode.connections)
                {
                    float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);

                    if (heldGScore < connectedNode.gScore)
                    {
                        connectedNode.cameFrom = currentNode;
                        connectedNode.gScore = heldGScore;
                        connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);

                        if (!openSet.Contains(connectedNode))
                        {
                            openSet.Add(connectedNode);
                        }
                    }
                }
            }

            return null;
        }

        public Node FindNearestNode(Vector2 pos, bool ignoreOccupied = false)
        {
            Node foundNode = null;
            float minDistance = float.MaxValue;

            foreach (Node node in Nodes)
            {
                if (!ignoreOccupied && node.IsOccupied)
                    continue;

                float currentDistance = Vector2.Distance(pos, node.transform.position);

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    foundNode = node;
                }
            }

            return foundNode;
        }
        public Node FindNearestNotOccupiedNode(Vector2 pos)
        {
            Node foundNode = null;
            float minDistance = float.MaxValue;

            foreach (Node node in Nodes)
            {
                if (node.IsOccupied)
                    continue;

                float currentDistance = Vector2.Distance(pos, node.transform.position);

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    foundNode = node;
                }
            }

            return foundNode;
        }

        public Node FindFurthestNode(Vector2 pos)
        {
            Node foundNode = null;
            float maxDistance = default;

            foreach (Node node in Nodes)
            {
                float currentDistance = Vector2.Distance(pos, node.transform.position);
                if (currentDistance > maxDistance)
                {
                    maxDistance = currentDistance;
                    foundNode = node;
                }
            }

            return foundNode;
        }

        public List<Node> GetAllNodes()
        {
            return Nodes;
        }
    }

}