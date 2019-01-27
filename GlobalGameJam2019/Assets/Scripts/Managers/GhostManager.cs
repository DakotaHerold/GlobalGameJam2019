using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class GhostManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] floor1GhostSpawns;
        private Transform[] basementGhostSpawns;
        private Transform[] atticGhostSpawns; 

        [SerializeField]
        private GameObject ghostPrefab;
        private int numGhosts = 20;
        private List<Ghost> activeGhosts;
        private List<Ghost> inactiveGhosts; 

        // Start is called before the first frame update
        void Awake()
        {
            activeGhosts = new List<Ghost>();
            inactiveGhosts = new List<Ghost>(); 
        }

        private void Update()
        {
            
        }

        private void SetupGhosts()
        {
            for(int i = 0; i < numGhosts; ++i)
            {
                GameObject ghostGO = Instantiate(ghostPrefab);
                ghostGO.SetActive(false); 
                inactiveGhosts.Add(ghostGO.GetComponent<Ghost>());
            }
        }

        public void SetGhostInactive(Ghost ghost)
        {
            activeGhosts.Remove(ghost);
            inactiveGhosts.Add(ghost); 
        }

        public void DespawnAllGhosts()
        {
            foreach(Ghost g in activeGhosts)
            {
                g.DisableSneak();
                g.gameObject.SetActive(false); 
                if(!inactiveGhosts.Contains(g))
                {
                    inactiveGhosts.Add(g); 
                }
            }

            activeGhosts.Clear(); 
        }
    }
}
