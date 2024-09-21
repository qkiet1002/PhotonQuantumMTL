namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerSkill2Controller : SystemMainThreadFilter<PlayerSkill2Controller.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public PlayerSkill2Info* skill2;
        }
        public override void Update(Frame frame, ref Filter filter)
        {
            if (filter.skill2->Facing == PlayerFacing.Left)
            {
                filter.Body->Velocity = filter.skill2->Speed * FPVector2.Left;
            }
            else
            {
                filter.Body->Velocity = filter.skill2->Speed * FPVector2.Right;
            }
            // xoa vien dan
            filter.skill2->ExistTime -= frame.DeltaTime;
            if (filter.skill2->ExistTime < 0)
            {
                frame.Destroy(filter.Entity);
            }

        }
    }
}
