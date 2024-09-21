namespace Quantum
{
    using UnityEngine;

    public class PlayerSkill4View : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;

        private PlayerSkill4Info skill1Info;

        private void Update()
        {
            if (VerifiedFrame.TryGet<PlayerSkill4Info>(_entityView.EntityRef, out PlayerSkill4Info skill1Info))
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
