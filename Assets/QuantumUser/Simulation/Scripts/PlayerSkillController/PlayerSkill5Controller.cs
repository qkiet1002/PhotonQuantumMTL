namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerSkill5Controller : SystemMainThreadFilter<PlayerSkill5Controller.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public PlayerSkill5Info* skill1;
        }
        public override void Update(Frame frame, ref Filter filter)
        {
            filter.Body->Velocity = filter.skill1->Direction * filter.skill1->Speed;
            // xoa vien dan
            filter.skill1->ExistTime -= frame.DeltaTime;
            if (filter.skill1->ExistTime < 0)
            {
                frame.Destroy(filter.Entity);
            }

        }
    }
}
