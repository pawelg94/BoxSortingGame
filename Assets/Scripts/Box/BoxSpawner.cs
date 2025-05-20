using UnityEngine;
using System;
using System.Collections.Generic;
using AStarPathfinding;

public class BoxSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxBoxesAtATime = 20;

    private float _timer;
    private int _currentBoxes;

    public event Action<Node> OnBoxSpawned;

    private List<Node> _cachedNodes;
    public void Initialize(List<Node> nodes)
    {
        _cachedNodes = nodes;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f && _currentBoxes < maxBoxesAtATime)
        {
            _timer = spawnInterval;
            TrySpawnBox();
        }
    }

    private void TrySpawnBox()
    {
        if (_cachedNodes.Count == 0)
        {
            Debug.LogWarning("[BoxSpawner] No available nodes for box spawning.");
            return;
        }

        Node selectedNode = SelectRandomFreeNode(_cachedNodes);
        if (selectedNode == null)
        {
            Debug.LogWarning("[BoxSpawner] Failed to find unoccupied node after multiple attempts.");
            return;
        }

        SpawnBoxAt(selectedNode);
    }

    private Node SelectRandomFreeNode(List<Node> candidates, int maxAttempts = 10)
    {
        while (maxAttempts-- > 0)
        {
            Node candidate = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            if (!candidate.IsOccupied)
                return candidate;
        }

        return null;
    }

    private void SpawnBoxAt(Node node)
    {
        if (!IsNodeValid(node))
        {
            Debug.Log("Occupied NODE, Skipping spawn...");
            return;
        }
            
        node.SetOccupied(true);

        Box.BoxColor color = UnityEngine.Random.value < 0.5f ? Box.BoxColor.Red : Box.BoxColor.Blue;

        Box box = GameManager.Instance.Pool.Spawn(color, node.transform.position);

        if (box.boxNodeReference != null)
            box.boxNodeReference.currentNode = node;

        if (box.dropBehaviour != null)
            box.dropBehaviour.DropTo(node.transform.position);

        _currentBoxes++;
        OnBoxSpawned?.Invoke(node);
    }

    private bool IsNodeValid(Node node)
    {
        var boxes = GameManager.Instance.BoxManager.GetAllBoxes();

        foreach (var item in boxes)
        {
            if (item.transform.position == node.transform.position)
            {                
                return false; 
            }
        }
        return true;
    }

    public void NotifyBoxRemoved()
    {
        _currentBoxes--;
    }
}
