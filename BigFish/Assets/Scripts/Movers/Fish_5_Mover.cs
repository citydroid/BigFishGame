using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movers
{
    public class Fish_5_Mover : FishMover
    {
        protected override void Mover()
        {
            base.Mover();
           // _horizontalSpeed = Mathf.Sin(Time.time);
        }
    }
}
