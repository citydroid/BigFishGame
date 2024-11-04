using UnityEngine;

namespace Movers
{
    public class Fish_1_Mover : FishMover, IMovable
    {
        protected override void Mover()
        {
            base.Mover();
            _horizontalSpeed = _horizontalSpeed - 0.0005f;
        }
        public void Move()
        {
         //   base.Mover();
         //   _horizontalSpeed = _horizontalSpeed - 0.002f;
        }
    }
}
