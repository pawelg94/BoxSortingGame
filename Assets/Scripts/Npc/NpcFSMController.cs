using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(NpcController))]
public class NpcFSMController : MonoBehaviour
{
    [HideInInspector] public NpcState currentState;
    public NpcController npc;

    private void Awake()
    {
        npc = GetComponent<NpcController>();
    }

    private void Start()
    {
        TransitionTo(new IdleState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void TransitionTo(NpcState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
