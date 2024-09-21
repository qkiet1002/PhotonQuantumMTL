namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerController : SystemMainThreadFilter<PlayerController.Filter>
    {
        public bool Check = false;
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public Transform2D* Transform;
            public PlayerInfo* PlayerInfo;
         
            
        }


        public override void Update(Frame frame, ref Filter filter)
        {
            var input = frame.GetPlayerInput(filter.PlayerInfo->PlayerRef);
            filter.Body->Velocity = input->Direction * filter.PlayerInfo->Speed;

            if(input->Direction.X > 0)
            {
                filter.PlayerInfo->Facing = PlayerFacing.Right;
            }
            else if (input->Direction.X < 0)
            {
                filter.PlayerInfo->Facing = PlayerFacing.Left;
            }
            if(filter.PlayerInfo->LV < 10)
            {
                if (input->SpawnBullet)
                {
                    var spawnedBullet = frame.Create(filter.PlayerInfo->Bullet);
                    var transform = frame.Get<Transform2D>(spawnedBullet);
                    transform.Position = filter.Transform->Position;
                    var bulletInfo = frame.Get<BulletInfo>(spawnedBullet);
                    bulletInfo.Owner = filter.Entity; // logic người bắn
                    bulletInfo.Facing = filter.PlayerInfo->Facing;
                    // dame
                    bulletInfo.Damage += filter.PlayerInfo->Damage;
                    //
                    frame.Set(spawnedBullet, transform);
                    frame.Set(spawnedBullet, bulletInfo);
                }
            }else if (filter.PlayerInfo->LV >= 10 && filter.PlayerInfo->LV < 20)
            {
                if (input->SpawnBullet)
                {
                    var spawnedBullet = frame.Create(filter.PlayerInfo->Skill1);
                    var transform = frame.Get<Transform2D>(spawnedBullet);
                    transform.Position = filter.Transform->Position;
                    var skill1 = frame.Get<PlayerSkill1Info>(spawnedBullet);
                    skill1.Owner = filter.Entity; // logic người bắn
                    skill1.Facing = filter.PlayerInfo->Facing;
                    // dame
                    skill1.Damage += filter.PlayerInfo->Damage +3;
                    //
                    frame.Set(spawnedBullet, transform);
                    frame.Set(spawnedBullet, skill1);
                }
            }
            else if (filter.PlayerInfo->LV >= 20 && filter.PlayerInfo->LV < 100)
            {
                if (input->SpawnBullet)
                {
                    var spawnedBullet = frame.Create(filter.PlayerInfo->Skill2);
                    var transform = frame.Get<Transform2D>(spawnedBullet);
                    transform.Position = filter.Transform->Position;
                    var skill = frame.Get<PlayerSkill2Info>(spawnedBullet);
                    skill.Owner = filter.Entity; // logic người bắn
                    skill.Facing = filter.PlayerInfo->Facing;
                    // dame
                    skill.Damage += filter.PlayerInfo->Damage + 5;
                    //
                    frame.Set(spawnedBullet, transform);
                    frame.Set(spawnedBullet, skill);
                }
            }
            else if (filter.PlayerInfo->LV >= 100)
            {
                int BULLET_AMOUNT = 12;
                if (input->SpawnBullet)
                {
                    for (int i = 0; i < BULLET_AMOUNT; i++)
                    {
                        // Tính toán góc và chuyển đổi sang radian
                        var angle = FP.FromFloat_UNSAFE(360) / BULLET_AMOUNT * i;
                        var radiantAngle = angle * FP.Deg2Rad;

                        // Tạo viên đạn mới
                        var spawnedBullet = frame.Create(
                            filter.PlayerInfo->Skill5);

                        // Lấy và thiết lập vị trí và hướng của đạn
                        var bulletTransform = frame.Get<Transform2D>(spawnedBullet);
                        bulletTransform.Position = filter.Transform->Position;

                        var bulletInfo = frame.Get<PlayerSkill5Info>(spawnedBullet);  // Thay thế bằng kiểu dữ liệu phù hợp (PlayerSkill5Info, BossBulletInfo,...)
                        bulletInfo.Direction = new FPVector2(FPMath.Cos(radiantAngle), FPMath.Sin(radiantAngle));
                        bulletInfo.Damage += filter.PlayerInfo->Damage + 20;
                        // Lưu lại các thông tin của viên đạn trong frame
                        frame.Set(spawnedBullet, bulletInfo);
                        frame.Set(spawnedBullet, bulletTransform);

                    }
                }
            }
            if (input->Eat)
            {
                if (filter.PlayerInfo->PlayerRef == filter.PlayerInfo->PlayerRef)
                {
                    if (filter.PlayerInfo->QuantityHP > 0)
                    {
                        filter.PlayerInfo->QuantityHP -= 1;
                        var hp = filter.PlayerInfo->Health;
                        filter.PlayerInfo->CurrentHealth = hp;


                    }
                }

            }
        }

    }
}
