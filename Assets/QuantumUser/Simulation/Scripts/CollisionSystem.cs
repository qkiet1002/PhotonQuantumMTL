namespace Quantum
{
    using Photon.Deterministic;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using UnityEngine;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionSystem : SystemSignalsOnly, ISignalOnCollisionEnter2D, ISignalOnCollisionExit2D
    {
        FP dame = 1;
        public BossRController bossRcontroller;


        public void OnCollisionEnter2D(Frame frame, CollisionInfo2D info)
        {


            if (frame.TryGet<BulletInfo>(info.Entity, out var bulletInfo))
            {
                info.IgnoreCollision = true;
                // Attack Boss
                if (frame.TryGet<BossInfo>(info.Other, out var bossInfo))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    bossInfo.CurrentHealth -= bulletInfo.Damage;
                    frame.Set(info.Other, bossInfo);
                    frame.Destroy(info.Entity);
                    if (bossInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(bossInfo.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
                // Attack BossR
                if (frame.TryGet<BossRInfo>(info.Other, out var bossRInfo))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    bossRInfo.CurrentHealth -= bulletInfo.Damage;
                    bossRInfo.IsAttacking = 1;
                    frame.Set(info.Other, bossRInfo);
                    frame.Destroy(info.Entity);

                    //gif
                    if (bossRInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(bossRInfo.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
                // Enemy
                if (frame.TryGet<EnemyInfo>(info.Other, out var enemyInfo1))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    enemyInfo1.CurrentHealth -= bulletInfo.Damage;
                    frame.Set(info.Other, enemyInfo1);
                    frame.Destroy(info.Entity);
                    if (enemyInfo1.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(enemyInfo1.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
            }

            if (frame.TryGet<BossBulletInfo>(info.Entity, out var bossBulletInfo))
            {
                info.IgnoreCollision = true;

                if (frame.TryGet<PlayerInfo>(info.Other, out var playerInfo))
                {
                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    playerInfo.CurrentHealth -= bossBulletInfo.Damage;
                    frame.Set(info.Other, playerInfo);
                    frame.Destroy(info.Entity);
                    if (playerInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnPlayerDead();
                    }
                }
            }

            // nhặt bình máu
            if (frame.TryGet<HealthIfno>(info.Entity, out var healthIfno))
            {
                info.IgnoreCollision = true;

                if (frame.TryGet<PlayerInfo>(info.Other, out var playerInfo))
                {
                    //if(playerInfo.CurrentHealth < playerInfo.Health)
                    //{
                    //    playerInfo.CurrentHealth += healthIfno.Health;
                    //    if(playerInfo.CurrentHealth > playerInfo.Health)
                    //    {
                    //        var hp = playerInfo.Health;
                    //        playerInfo.CurrentHealth = hp;
                    //    }
                    //}
                    playerInfo.QuantityHP += 1;

                    frame.Set(info.Other, playerInfo);
                    frame.Destroy(info.Entity);

                }
            }

            // nhặt bình thuoc
            if (frame.TryGet<GifInfo>(info.Entity, out var gifInfo))
            {
                info.IgnoreCollision = true;

                if (frame.TryGet<PlayerInfo>(info.Other, out var playerInfo))
                {
                    playerInfo.Health += gifInfo.HP;
                    playerInfo.Damage += gifInfo.Damage;
                    playerInfo.Speed += gifInfo.Speed;
                    playerInfo.LV += gifInfo.Exp;
                    frame.Set(info.Other, playerInfo);
                    frame.Destroy(info.Entity);

                }
            }
            // BossR -> Player
            if (frame.TryGet<BossRBulletInfo>(info.Entity, out var bossRBulletInfo))
            {
                info.IgnoreCollision = true;
                // Attack Player
                if (frame.TryGet<PlayerInfo>(info.Other, out var playerInfo))
                {
                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    playerInfo.CurrentHealth -= bossRBulletInfo.Damege;
                    frame.Set(info.Other, playerInfo);
                    frame.Destroy(info.Entity);
                    if (playerInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnPlayerDead();
                    }
                }
            }


            // SKILL 1 PLAYER
            if (frame.TryGet<PlayerSkill1Info>(info.Entity, out var skill1Info))
            {
                info.IgnoreCollision = true;
                // Attack Boss
                if (frame.TryGet<BossInfo>(info.Other, out var bossInfo))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    bossInfo.CurrentHealth -= skill1Info.Damage;
                    frame.Set(info.Other, bossInfo);
                    frame.Destroy(info.Entity);
                    if (bossInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(bossInfo.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
                // Attack BossR
                if (frame.TryGet<BossRInfo>(info.Other, out var bossRInfo))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    bossRInfo.CurrentHealth -= skill1Info.Damage;
                    bossRInfo.IsAttacking = 1;
                    frame.Set(info.Other, bossRInfo);
                    frame.Destroy(info.Entity);

                    //gif
                    if (bossRInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(bossRInfo.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
                if (frame.TryGet<EnemyInfo>(info.Other, out var enemyInfo1))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    enemyInfo1.CurrentHealth -= skill1Info.Damage;
                    frame.Set(info.Other, enemyInfo1);
                    frame.Destroy(info.Entity);
                    if (enemyInfo1.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(enemyInfo1.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
            }

            // SKILL 2 PLAYER
            if (frame.TryGet<PlayerSkill2Info>(info.Entity, out var skill2Info))
            {
                info.IgnoreCollision = true;
                // Attack Boss
                if (frame.TryGet<BossInfo>(info.Other, out var bossInfo))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    bossInfo.CurrentHealth -= skill2Info.Damage;
                    frame.Set(info.Other, bossInfo);
                    frame.Destroy(info.Entity);
                    if (bossInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(bossInfo.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
                // Attack BossR
                if (frame.TryGet<BossRInfo>(info.Other, out var bossRInfo))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    bossRInfo.CurrentHealth -= skill2Info.Damage;
                    bossRInfo.IsAttacking = 1;
                    frame.Set(info.Other, bossRInfo);
                    frame.Destroy(info.Entity);

                    //gif
                    if (bossRInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(bossRInfo.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
                if (frame.TryGet<EnemyInfo>(info.Other, out var enemyInfo1))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    enemyInfo1.CurrentHealth -= skill2Info.Damage;
                    frame.Set(info.Other, enemyInfo1);
                    frame.Destroy(info.Entity);
                    if (enemyInfo1.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(enemyInfo1.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
            }

            // SKILL 5 PLAYER
            if (frame.TryGet<PlayerSkill5Info>(info.Entity, out var skill5Info))
            {
                info.IgnoreCollision = true;
                // Attack Boss
                if (frame.TryGet<BossInfo>(info.Other, out var bossInfo))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    bossInfo.CurrentHealth -= skill5Info.Damage;
                    frame.Set(info.Other, bossInfo);
                    frame.Destroy(info.Entity);
                    if (bossInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(bossInfo.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
                // Attack BossR
                if (frame.TryGet<BossRInfo>(info.Other, out var bossRInfo))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    bossRInfo.CurrentHealth -= skill5Info.Damage;
                    bossRInfo.IsAttacking = 1;
                    frame.Set(info.Other, bossRInfo);
                    frame.Destroy(info.Entity);

                    //gif
                    if (bossRInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(bossRInfo.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
                if (frame.TryGet<EnemyInfo>(info.Other, out var enemyInfo1))
                {

                    // bossInfo.CurrentHealth = -1;// sau gan dame vaof
                    enemyInfo1.CurrentHealth -= skill5Info.Damage;
                    frame.Set(info.Other, enemyInfo1);
                    frame.Destroy(info.Entity);
                    if (enemyInfo1.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        var itemEntity = frame.Create(enemyInfo1.Gif); // Đảm bảo rằng bossInfo có tham chiếu đến prefab của vật phẩm
                        var itemTransform = frame.Get<Transform2D>(itemEntity);
                        itemTransform.Position = frame.Get<Transform2D>(info.Other).Position; // Đặt vị trí của vật phẩm ở vị trí của quái vật
                        frame.Set(itemEntity, itemTransform);

                        frame.Destroy(info.Other);
                    }
                }
            }
        }

        public void OnCollisionExit2D(Frame frame, ExitInfo2D info)
        {
            // enemy
            if (frame.TryGet<EnemyInfo>(info.Entity, out var enemyInfo))
            {
                if (frame.TryGet<PlayerInfo>(info.Other, out var playerInfo))
                {
                    playerInfo.CurrentHealth -= enemyInfo.Damage;
                    frame.Set(info.Other, playerInfo);

                    if (playerInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnPlayerDead();
                    }
                }
            }
        }
    }
}
