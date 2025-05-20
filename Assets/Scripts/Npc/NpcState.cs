using UnityEngine;

public abstract class NpcState
{
    protected NpcFSMController controller;

    public NpcState(NpcFSMController controller)
    {
        this.controller = controller;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
