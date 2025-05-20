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
        List<Node> candidates = new List<Node>();

        foreach (Node node in _cachedNodes)
        {
            if (!node.IsOccupied)
                candidates.Add(node);
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("[BoxSpawner] No available nodes for box spawning.");
            return;
        }

        Node selectedNode = SelectRandomFreeNode(candidates);
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
        node.IsOccupied = true;

        Box.BoxColor color = UnityEngine.Random.value < 0.5f ? Box.BoxColor.Red : Box.BoxColor.Blue;

        Box box = GameManager.Instance.Pool.Spawn(color, node.transform.position);        

        if (box.boxNodeReference != null)
            box.boxNodeReference.currentNode = node;

        if (box.dropBehaviour != null)
            box.dropBehaviour.DropTo(node.transform.position);

        _currentBoxes++;
        OnBoxSpawned?.Invoke(node);
    }

    public void NotifyBoxRemoved()
    {
        _currentBoxes--;
    }
}
