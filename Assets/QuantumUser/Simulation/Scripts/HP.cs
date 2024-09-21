namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;


    [Preserve]
    public unsafe class HP : SystemMainThreadFilter<HP.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
        }

        public override void Update(Frame f, ref Filter filter)
        {
        }


    }
}
