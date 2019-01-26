using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class Fixable : Item
    {
        public Vector3 disorientedEulerRot; 

        private bool isFixed;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            isFixed = false;
            
            transform.rotation = Quaternion.Euler(disorientedEulerRot.x, disorientedEulerRot.y, disorientedEulerRot.z); 
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnMouseDown()
        {
            if(!isFixed)
                FixItem(); 
            base.OnMouseDown(); 
        }

        private void FixItem()
        {
            isFixed = true;
            stoppingRotate = true; 
        }
    }
}
