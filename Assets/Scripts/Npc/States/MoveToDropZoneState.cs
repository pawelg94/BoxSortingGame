using UnityEngine;

public class MoveToDropZoneState : NpcState
{
    public MoveToDropZoneState(NpcFSMController controller) : base(controller) { }

    public override void Enter()
    {
        Vector2 dropPos = controller.npc.GetDropZonePosition();
        controller.npc.targetTransform.position = dropPos;
        controller.npc.MoveToTarget();
    }

    public override void Update()
    {
        if (!controller.npc.IsMoving())
        {
            controller.TransitionTo(new DropBoxState(controller));
        }
    }
}
