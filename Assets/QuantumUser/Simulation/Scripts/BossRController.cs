namespace Quantum
{
    using Photon.Deterministic;
    using System.Diagnostics;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class BossRController : SystemMainThreadFilter<BossRController.Filter>
    {
        FPVector2 movementRangeMin = new FPVector2(-FP._2, -FP._2); // Giới hạn tối thiểu của vùng di chuyển
        FPVector2 movementRangeMax = new FPVector2(FP._2, FP._2);  // Giới hạn tối đa của vùng di chuyển

        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public BossRInfo* BossRInfo;
            public Transform2D* Transform;

        }

        public override void Update(Frame frame, ref Filter filter)
        {
            filter.BossRInfo->Time += frame.DeltaTime;
            if (filter.BossRInfo->Time > filter.BossRInfo->ChangeDirectionTime)
            {
                // Cập nhật thời gian thay đổi hướng
                filter.BossRInfo->ChangeDirectionTime = filter.BossRInfo->Time + frame.Global->RngSession.Next(FP._2, FP._3);

                // Tạo một hướng ngẫu nhiên trong vùng di chuyển
                FPVector2 newDirection = new FPVector2(
                    frame.Global->RngSession.Next(movementRangeMin.X, movementRangeMax.X),
                    frame.Global->RngSession.Next(movementRangeMin.Y, movementRangeMax.Y)
                ).Normalized;

                // Cập nhật hướng di chuyển
                filter.BossRInfo->Direction = newDirection;
            }


            // Tính toán vận tốc trong vùng di chuyển
            // Điều chỉnh vận tốc sao cho nó nằm trong giới hạn của vùng di chuyển
            filter.Body->Velocity = filter.BossRInfo->Direction * 3; // desiredSpeed là tốc độ mong muốn
            if (filter.BossRInfo->IsAttacking == 1)
            {
                var players = frame.GetComponentIterator<PlayerInfo>();
                FPVector2 closestPlayerDirection = FPVector2.Zero;
                FP minDistanceSquared = FP.MaxValue;

                foreach (var pair in players)
                {
                    var playerEntity = pair.Entity;
                    var playerTransform = frame.Get<Transform2D>(playerEntity);

                    // Tính toán hướng di chuyển đến người chơi
                    FPVector2 directionToPlayer = (playerTransform.Position - filter.Transform->Position).Normalized;
                    FPVector2 deltaPosition = playerTransform.Position - filter.Transform->Position;
                    FP distanceSquared = deltaPosition.X * deltaPosition.X + deltaPosition.Y * deltaPosition.Y;

                    // Cập nhật người chơi gần nhất
                    if (distanceSquared < minDistanceSquared)
                    {
                        minDistanceSquared = distanceSquared;
                        closestPlayerDirection = directionToPlayer;
                    }
                    // Cập nhật hướng di chuyển của boss về phía người chơi gần nhất
                    filter.BossRInfo->Direction = closestPlayerDirection;

                    // dùng skill 1
                    if (filter.BossRInfo->Time > filter.BossRInfo->UseSkillTime1)
                    {
                        filter.BossRInfo->UseSkillTime1 = filter.BossRInfo->Time + frame.Global->RngSession.Next(FP._4, FP._6);

                        var spawnedBullet = frame.Create(filter.BossRInfo->Skill1);
                        var bulletTransform = frame.Get<Transform2D>(spawnedBullet);
                        bulletTransform.Position = filter.Transform->Position;
                        var bulletInfo = frame.Get<BossRBulletInfo>(spawnedBullet);
                        // Cập nhật lại thông tin của viên đạn trong frame
                        frame.Set(spawnedBullet, bulletInfo);
                        frame.Set(spawnedBullet, bulletTransform);


                    }

                    // dùng skill 2
                    if (filter.BossRInfo->Time > filter.BossRInfo->UseSkillTime2)
                    {
                        filter.BossRInfo->UseSkillTime2 = filter.BossRInfo->Time + frame.Global->RngSession.Next(FP._4, FP._6);

                        var spawnedBullet = frame.Create(filter.BossRInfo->Skill2);
                        var bulletTransform = frame.Get<Transform2D>(spawnedBullet);
                        bulletTransform.Position = filter.Transform->Position;
                        var bulletInfo = frame.Get<BossBulletInfo>(spawnedBullet);
                        // Cập nhật lại thông tin của viên đạn trong frame
                        int randomDirection = frame.Global->RngSession.Next(0, 4);
                        if (randomDirection == 1)
                        {
                            bulletInfo.Direction = FPVector2.Right;
                        }else if(randomDirection == 2)
                        {
                            bulletInfo.Direction = FPVector2.Right;
                        }
                        else if(randomDirection == 3)
                        {
                            bulletInfo.Direction = FPVector2.Up;
                        }
                        else
                        {
                            bulletInfo.Direction = FPVector2.Down;
                        }

                        frame.Set(spawnedBullet, bulletInfo);
                        frame.Set(spawnedBullet, bulletTransform);


                    }
                }

            }

        }

    }
}
