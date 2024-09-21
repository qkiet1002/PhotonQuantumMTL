namespace Quantum
{
    using Spine;
    using UnityEngine;

    public class BossRView : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;
        private PhysicsBody2D body;
        private BossRInfo bossRInfo;
        private HealthBar healthBar;

        private void Awake()
        {
            healthBar = GetComponentInChildren<HealthBar>();
        }
        private void Update()
        {
            if (VerifiedFrame == null) return;
            if (VerifiedFrame.TryGet<PhysicsBody2D>(_entityView.EntityRef, out body))
            {
                if (body.Velocity.X < 0)
                {
                    visual.rotation = rotationLeft;
                }
                else if (body.Velocity.X > 0)
                {
                    visual.rotation = rotationRight;
                }
            }

            if (VerifiedFrame.TryGet<BossRInfo>(_entityView.EntityRef, out bossRInfo))
            {
                if (bossRInfo.Health == 0)
                {
                    healthBar.SetValue(0);
                }
                else
                {
                    float mauHienTai = bossRInfo.CurrentHealth.AsFloat / bossRInfo.Health.AsFloat;
                    healthBar.SetValue(mauHienTai);
                }
            }

       
        }
    }
}
