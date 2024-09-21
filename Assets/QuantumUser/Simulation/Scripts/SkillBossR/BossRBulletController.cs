namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class BossRBulletController : SystemMainThreadFilter<BossRBulletController.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public BossRBulletInfo* Bullet;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            filter.Body->Velocity = filter.Bullet->Direction * filter.Bullet->Speed;

            // xoa vien dan
            filter.Bullet->ExistTime -= frame.DeltaTime;
            if (filter.Bullet->ExistTime < 0)
            {
                frame.Destroy(filter.Entity);
            }
        }


    }
}
