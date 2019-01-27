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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameManager.Instance.CurrentState == GameManager.GAME_STATE.TRANSITIONING)
                return;

            
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GameManager.Instance.StartFloorTransition(DestFloor);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //transitioning = false;
            }
                 
        }


    }
}
