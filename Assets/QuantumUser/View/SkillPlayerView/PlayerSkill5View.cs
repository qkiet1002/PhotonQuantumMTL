namespace Quantum
{
    using UnityEngine;

    public class PlayerSkill5View : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;

        private PlayerSkill5Info skill1Info;

        private void Update()
        {
            if (VerifiedFrame.TryGet<PlayerSkill5Info>(_entityView.EntityRef, out PlayerSkill5Info skill1Info))
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
