using UnityEngine;

namespace Movers
{
    public class Fish_1_Mover : FishMover
    {
        protected override void Mover()
        {
            base.Mover();
            _horizontalSpeed = _horizontalSpeed - 0.001f;
        }
    }
}
