using UnityEngine;

public class DropBoxState : NpcState
{
    public DropBoxState(NpcFSMController controller) : base(controller) { }

    public override void Enter()
    {

        Vector2 npcPos = controller.npc.transform.position;
        Vector2 dropPos = controller.npc.GetDropZonePosition();

        float distance = Vector2.Distance(npcPos, dropPos);

        if (distance < 0.2f)
        {
            controller.npc.DropBox();
            controller.TransitionTo(new IdleState(controller));
        }
        else
        {
            controller.TransitionTo(new MoveToDropZoneState(controller));
        }
    }
}