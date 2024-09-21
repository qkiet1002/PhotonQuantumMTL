namespace Quantum
{
    using UnityEngine;

    public class PlayerSkill1View : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;

        private PlayerSkill1Info skill1Info;

        private void Update()
        {
            if (VerifiedFrame.TryGet<PlayerSkill1Info>(_entityView.EntityRef, out PlayerSkill1Info skill1Info))
            {
                if (skill1Info.Facing == PlayerFacing.Left)
                {
                    visual.rotation = rotationLeft;
                }
                else
                {
                    visual.rotation = rotationRight;
                }
            }

        }
    }
}
