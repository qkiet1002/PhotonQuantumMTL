namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerSkill4Controller : SystemMainThreadFilter<PlayerSkill4Controller.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public PlayerSkill4Info* skill1;
        }
        public override void Update(Frame frame, ref Filter filter)
        {
            if (filter.skill1->Facing == PlayerFacing.Left)
            {
                filter.Body->Velocity = filter.skill1->Speed * FPVector2.Left;
            }
            else
            {
                filter.Body->Velocity = filter.skill1->Speed * FPVector2.Right;
            }
            // xoa vien dan
            filter.skill1->ExistTime -= frame.DeltaTime;
            if (filter.skill1->ExistTime < 0)
            {
                frame.Destroy(filter.Entity);
            }

        }
    }
}
