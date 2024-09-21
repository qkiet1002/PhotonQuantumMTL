﻿namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class BossBulletController : SystemMainThreadFilter<BossBulletController.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public BossBulletInfo* Bullet;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            // Cập nhật vận tốc đạn
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
