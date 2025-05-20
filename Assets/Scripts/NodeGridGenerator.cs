using AStarPathfinding;
using UnityEngine;

public class NodeGridGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public int width = 10;
    public int height = 10;
    public float spacing = 1f;
    public LayerMask unwalkableMask;
    public float unwalkableMaxDistance = 0.1f;

    private Node[,] nodeGrid;

    void Start()
    {
        GenerateGrid();
        ConnectNodes();
    }

    void GenerateGrid()
    {
        nodeGrid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x * spacing, y * spacing);
                pos += (Vector2)transform.position;

                if (Physics2D.OverlapCircle(pos, spacing * unwalkableMaxDistance, unwalkableMask))
                    continue;

                GameObject nodeObj = Instantiate(nodePrefab, pos, Quaternion.identity, transform);
                Node node = nodeObj.GetComponent<Node>();
                nodeGrid[x, y] = node;
            }
        }
    }

    void ConnectNodes()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = nodeGrid[x, y];
                if (node == null) continue;
               
                TryConnect(node, x + 1, y); 
                TryConnect(node, x - 1, y); 
                TryConnect(node, x, y + 1); 
                TryConnect(node, x, y - 1); 

                TryConnect(node, x + 1, y + 1);
                TryConnect(node, x - 1, y + 1);
                TryConnect(node, x + 1, y - 1); 
                TryConnect(node, x - 1, y - 1);
            }
        }
    }

    void TryConnect(Node from, int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return;

        Node to = nodeGrid[x, y];
        if (to == null || from.connections.Contains(to)) return;

        int fromX = Mathf.RoundToInt((from.transform.position.x - transform.position.x) / spacing);
        int fromY = Mathf.RoundToInt((from.transform.position.y - transform.position.y) / spacing);

        // Check for diagonal
        if (x != fromX && y != fromY)
        {            
            Node nodeA = nodeGrid[fromX, y]; 
            Node nodeB = nodeGrid[x, fromY];

            if (nodeA == null || nodeB == null)
                return; // prevnt cutting through corners
        }

        from.connections.Add(to);
    }

    public void GenerateGridInEditor()
    {
#if UNITY_EDITOR
        ClearGridInEditor();
        GenerateGrid();
        ConnectNodes();
#endif
    }

    public void ClearGridInEditor()
    {
#if UNITY_EDITOR
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
#endif
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 center = transform.position + new Vector3((width - 1) * spacing / 2f, (height - 1) * spacing / 2f, 0f);
        Vector3 size = new Vector3(width * spacing, height * spacing, 0f);

        Gizmos.DrawWireCube(center, size);
    }


}
