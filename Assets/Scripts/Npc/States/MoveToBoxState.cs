using UnityEngine;

public class MoveToBoxState : NpcState
{
    public MoveToBoxState(NpcFSMController controller) : base(controller) { }

    public override void Enter()
    {
        controller.npc.MoveToTarget();
    }

    public override void Update()
    {
        if (!controller.npc.IsMoving())
        {
            controller.TransitionTo(new PickUpBoxState(controller));
        }
    }
}
