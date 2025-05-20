using UnityEngine;

public class PickUpBoxState : NpcState
{
    public PickUpBoxState(NpcFSMController controller) : base(controller) { }

    public override void Enter()
    {
        Vector2 npcPos = controller.npc.transform.position;
        Vector2 targetpos = controller.npc.targetTransform.position;

        float distance = Vector2.Distance(npcPos, targetpos);

        if (distance < 0.1f)
        {
            controller.npc.PickUpBox(controller.npc.targetBox);
            controller.TransitionTo(new MoveToDropZoneState(controller));
        }
        else
        {
            controller.TransitionTo(new IdleState(controller));
        }

        
    }
}
