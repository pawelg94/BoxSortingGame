using UnityEngine;

public class IdleState : NpcState
{
    private float timer;

    public IdleState(NpcFSMController controller) : base(controller) { }

    public override void Enter()
    {
        timer = Random.Range(0.4f, 0.8f); // simulate delay
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            controller.TransitionTo(new SearchBoxState(controller));
        }
    }
}