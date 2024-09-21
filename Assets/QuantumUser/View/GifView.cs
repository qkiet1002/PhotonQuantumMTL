namespace Quantum
{
    using UnityEngine;

    public class GifView : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;

        private GifInfo gitfInfo;

        private void Update()
        {
            if (VerifiedFrame.TryGet<GifInfo>(_entityView.EntityRef, out GifInfo gifInfo))
            {
            }

        }
    }
}
