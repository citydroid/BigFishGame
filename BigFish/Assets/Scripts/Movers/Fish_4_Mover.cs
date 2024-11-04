using UnityEngine;

namespace Movers
{
    public class Fish_4_Mover : FishMover
    {
        protected override void Mover()
        {
            _horizontalSpeed = _horizontalSpeed + 0.002f;
        }
    }
}
