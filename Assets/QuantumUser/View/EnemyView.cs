namespace Quantum
{
    using UnityEngine;

    public class EnemyView : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;
        public PhysicsBody2D body;
        public EnemyInfo EnemyInfo;
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

            if (VerifiedFrame.TryGet<EnemyInfo>(_entityView.EntityRef, out EnemyInfo))
            {
                if (EnemyInfo.Health == 0)
                {
                    healthBar.SetValue(0);
                }
                else
                {
                    float mauHienTai = EnemyInfo.CurrentHealth.AsFloat / EnemyInfo.Health.AsFloat;
                    healthBar.SetValue(mauHienTai);
                }
            }


        }
    }
}
