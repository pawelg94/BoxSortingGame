using AStarPathfinding;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public IPathfinding Pathfinding => pathfinding;
    public IBoxManager BoxManager => boxManager;
    public IObjectPool Pool => pool;

    public NpcController NPC;

    [SerializeField] private AStarManager pathfinding;
    [SerializeField] private BoxManager boxManager;
    [SerializeField] private BoxPoolManager pool;
    [SerializeField] private BoxSpawner Spawner;


    public Transform redDropZone;
    public Transform blueDropZone;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Pathfinding.Initialize();        
        Spawner.Initialize(Pathfinding.GetAllNodes());
        NPC.Initialize(Pathfinding, BoxManager, Pool, GetDropZone);
    }
    public Vector2 GetDropZone(Box.BoxColor color)
    {
        return color == Box.BoxColor.Red ? redDropZone.position : blueDropZone.position;
    }
}
