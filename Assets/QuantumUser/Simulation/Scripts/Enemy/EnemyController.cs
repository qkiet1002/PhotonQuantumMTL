namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class EnemyController : SystemMainThreadFilter<EnemyController.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public EnemyInfo* enemy;
            public Transform2D* Transform;
        }
        public override void Update(Frame f, ref Filter filter)
        {
            filter.enemy->Time += f.DeltaTime;
            if(filter.enemy->Time > filter.enemy->ChangeDirectionTime)
            {
                filter.enemy->ChangeDirectionTime = filter.enemy->Time+f.Global->RngSession.Next(FP._2,FP._3);
                filter.enemy->Direction = new FPVector2(f.Global->RngSession.Next(-FP._1,FP._1),f.Global->RngSession.Next(-FP._1,FP._1)).Normalized;
            }
            filter.Body->Velocity = filter.enemy->Direction;
        }


    }
}
