using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfinding
{
    public class Node : MonoBehaviour
    {
        public Node cameFrom;
        public List<Node> connections = new List<Node>();

        public float gScore;
        public float hScore;

        public bool IsOccupied { get; private set; }

        public float FScore()
        {
            return gScore + hScore;
        }

        public void SetOccupied(bool state)
        {
            IsOccupied = state;
        }

        private void OnDrawGizmos()
        {
            // Display Node Connections
            //if (connections.Count > 0)
            //{
            //    Gizmos.color = Color.blue;
            //    for (int i = 0; i < connections.Count; i++)
            //    {
            //        Gizmos.DrawLine(transform.position, connections[i].transform.position);
            //    }
            //}

            Gizmos.color = Color.yellow;
            if (IsOccupied)
                Gizmos.DrawCube(this.transform.position, Vector3.one * 0.2f);
        }
    }
}