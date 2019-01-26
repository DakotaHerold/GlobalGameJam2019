using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class Collectible : Item
    {
        // Needs to be tracked and handle deletion 

        protected override void Awake()
        {
            base.Awake(); 
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnMouseDown()
        { 
            base.OnMouseDown();
            ItemManager.AddCollectible(this); 
        }
    }
}
