namespace Quantum
{
    using UnityEngine;

    public class PlayerSkill1View2 : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;

        private PlayerSkill2Info skill2Info;

        private void Update()
        {
            if (VerifiedFrame.TryGet<PlayerSkill2Info>(_entityView.EntityRef, out PlayerSkill2Info skill2Info))
            {
                if (skill2Info.Facing == PlayerFacing.Left)
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
