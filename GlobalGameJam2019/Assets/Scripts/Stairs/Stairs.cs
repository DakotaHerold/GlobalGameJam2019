using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class Stairs : MonoBehaviour
    {

        public Jam.GameManager.FLOOR CurrentFloor;
        public Jam.GameManager.FLOOR DestFloor;


        // Start is called before the first frame update
        void Awake()
        {
   
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GameManager.Instance.StartFloorTransition(DestFloor); 
            }
        }

    }
}
