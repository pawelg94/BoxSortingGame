using UnityEngine;

public class SearchBoxState : NpcState
{
    public SearchBoxState(NpcFSMController controller) : base(controller) { }

    public override void Enter()
    {
        Box target = controller.npc.FindClosestUnclaimedBox();

        if (target != null)
        {
            controller.npc.targetBox = target;
            controller.npc.targetTransform = target.transform;
            controller.TransitionTo(new MoveToBoxState(controller));
        }
        else
        {
            controller.TransitionTo(new IdleState(controller));
        }
    }
}
