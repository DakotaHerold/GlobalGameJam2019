using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class GhostManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] floor1GhostSpawns;
        [SerializeField]
        private Transform[] basementGhostSpawns;
        [SerializeField]
        private Transform[] atticGhostSpawns; 

        [SerializeField]
        private GameObject ghostPrefab;
        private int numGhosts = 20;
        private List<Ghost> activeGhosts;
        private List<Ghost> inactiveGhosts;

        private const float SPAWN_INTERVAL = 5.0f;
        private float currentTimer = 0.0f; 

        private int spawnIndex = 0; 

        // Start is called before the first frame update
        void Awake()
        {
            activeGhosts = new List<Ghost>();
            inactiveGhosts = new List<Ghost>();
            SetupGhosts();
            currentTimer = 0.0f;
        }

        private void Update()
        {
            // TODO: Remove main menu and make running
            if (GameManager.Instance.CurrentState != GameManager.GAME_STATE.MAIN_MENU)
                return;

            currentTimer += Time.deltaTime; 
            if(currentTimer >= SPAWN_INTERVAL)
            {
                currentTimer = 0.0f; 
                if(activeGhosts.Count < 6)
                    SpawnGhost(); 
            }
        }

        private void SpawnGhost()
        {
            Ghost newGhost = inactiveGhosts[0];
            // Double check, likely removed elsewhere 
            if (inactiveGhosts.Contains(newGhost))
                inactiveGhosts.Remove(newGhost); 
            
            GameManager.FLOOR floor = GameManager.Instance.GetCurrentFloor();
            Transform spawnPos = floor1GhostSpawns[0]; 
            switch (floor)
            {
                case GameManager.FLOOR.FIRST:
                    spawnPos = floor1GhostSpawns[spawnIndex];
                    newGhost.transform.position = spawnPos.position; 
                    break;
                case GameManager.FLOOR.BASEMENT:
                    spawnPos = basementGhostSpawns[spawnIndex];
                    newGhost.transform.position = spawnPos.position;
                    break;
                case GameManager.FLOOR.ATTIC:
                    spawnPos = atticGhostSpawns[spawnIndex];
                    newGhost.transform.position = spawnPos.position;
                    break; 
            }

            spawnIndex++;
            if (spawnIndex > floor1GhostSpawns.Length - 1)
                spawnIndex = 0; 

            // Activate ghost 
            newGhost.SpawnGhost();  

            activeGhosts.Add(newGhost);
        }

        private void SetupGhosts()
        {
            for(int i = 0; i < numGhosts; ++i)
            {
                GameObject ghostGO = Instantiate(ghostPrefab);
                ghostGO.SetActive(false);
                ghostGO.GetComponent<Ghost>().DisableSneak(); 
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
