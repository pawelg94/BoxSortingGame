using AStarPathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NpcController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float stuckMovementThreshold = 0.5f;

    [Header("Dependencies")]
    [SerializeField] private Animator animator;
    [SerializeField] private NpcFSMController fsmController;

    [HideInInspector] public Box targetBox;
    [HideInInspector] public Transform targetTransform;
    private Box carriedBox;

    private Rigidbody2D rb;
    private Vector2 lookDirection = Vector2.down;

    private IPathfinding pathfinding;
    private IBoxManager boxManager;
    private IObjectPool boxPool;
    private Func<Box.BoxColor, Vector2> getDropZone;

    private List<Node> path;
    private int currentPathIndex;
    private bool hasPath;

    private NpcCarryVisual carryVisual;

    [SerializeField] private NpcFSMController npcFSMController;

    private Vector2 lastPosition;
    private float stuckTimer = 0f;
    private float stuckCheckDelay; // seconds

    public void Initialize(IPathfinding pathfinding, IBoxManager boxManager, IObjectPool boxPool, Func<Box.BoxColor, Vector2> getDropZone)
    {
        this.pathfinding = pathfinding;
        this.boxManager = boxManager;
        this.boxPool = boxPool;
        this.getDropZone = getDropZone;
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        carryVisual = GetComponent<NpcCarryVisual>();

        stuckCheckDelay = UnityEngine.Random.Range(1f, 3f);
    }

    private void Update()
    {
        UpdateAnimation();
    }


    private void FixedUpdate()
    {
        if (!IsPathValid())
        {
            rb.linearVelocity = Vector2.zero;
            if (!(fsmController.currentState is IdleState))
                MoveToTarget();
            return;
        }

        MoveAlongPath();
        CheckStuck();
    }
    private void UpdateAnimation()
    {
        Vector2 velocity = rb.linearVelocity;
        if (velocity.magnitude > 0.1f)
            lookDirection = velocity.normalized;

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", velocity.magnitude);
    }
    private void MoveAlongPath()
    {
        Vector2 targetPos = path[currentPathIndex].transform.position;
        Vector2 moveDir = (targetPos - rb.position).normalized;
        rb.linearVelocity = moveDir * speed;

        if (Vector2.Distance(rb.position, targetPos) < 0.1f)
            currentPathIndex++;
    }

    private void CheckStuck()
    {
        if (fsmController.currentState is IdleState) return;

        float distanceMoved = Vector2.Distance(transform.position, lastPosition);
        if (distanceMoved < stuckMovementThreshold)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer > stuckCheckDelay)
            {
                Debug.LogWarning($"{name} is stuck. Recalculating path...");
                stuckTimer = 0f;
                MoveToTarget();
            }
        }
        else
        {
            stuckTimer = 0f;
            lastPosition = transform.position;
        }
    }

    public void MoveToTarget()
    {
        if (targetTransform == null || pathfinding == null) return;

        Node start = pathfinding.FindNearestNode(transform.position);
        Node end = pathfinding.FindNearestNode(targetTransform.position, true);

        if (start == null || end == null) return;

        path = pathfinding.GeneratePath(start, end);
        currentPathIndex = 0;
        hasPath = path != null && path.Count > 0;
    }

    public void MoveToNearestFreeNode(Vector2 worldTargetPos)
    {
        if (pathfinding == null) return;

        Node start = pathfinding.FindNearestNode(transform.position);
        Node end = pathfinding.FindNearestNode(worldTargetPos);

        if (start == null || end == null) return;

        path = pathfinding.GeneratePath(start, end);
        currentPathIndex = 0;
        hasPath = path != null && path.Count > 0;
    }

    public bool IsMoving()
    {
        return hasPath && path != null && currentPathIndex < path.Count;
    }
    public Box FindClosestUnclaimedBox()
    {
        return boxManager.FindClosestUnclaimedBox(transform.position);
    }

    public void PickUpBox(Box box)
    {
        carriedBox = box;
        box.gameObject.SetActive(false);

        box.boxNodeReference?.ReleaseNode();
        carryVisual?.ShowBox(box.boxColor);
    }

    public void DropBox()
    {
        if (carriedBox == null) return;

        carriedBox.transform.position = transform.position;
        carriedBox.gameObject.SetActive(true);

        boxPool.Despawn(carriedBox);
        carriedBox = null;
        targetBox = null;

        carryVisual?.HideBox();
    }

    public Vector2 GetDropZonePosition()
    {
        return targetBox != null ? getDropZone(targetBox.boxColor) : transform.position;
    }

    private bool IsPathValid()
    {
        if (!hasPath || path == null || currentPathIndex >= path.Count)
            return false;

        return true;
    }

    public void SetTarget(Box box, Transform target)
    {
        targetBox = box;
        targetTransform = target;
    }

    private void OnDrawGizmos()
    {
        if (path == null)
            return;

        if (path.Count > 0)
        {
            Gizmos.color = Color.blue;
            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.DrawLine(path[i].transform.position, path[i - 1].transform.position);
            }
        }
    }
}
