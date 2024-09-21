namespace Quantum
{
    using UnityEngine;

    public class PlayerSkill3View : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;

        private PlayerSkill3Info skill1Info;

        private void Update()
        {
            if (VerifiedFrame.TryGet<PlayerSkill3Info>(_entityView.EntityRef, out PlayerSkill3Info skill1Info))
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
