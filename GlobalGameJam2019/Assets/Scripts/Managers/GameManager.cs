using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jam
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GAME_STATE
        {
            MAIN_MENU,
            RUNNING,
            READING,
            TRANSITIONING,
            COMPLETE,
            RESTARTING
        }

        public enum FLOOR
        {
            FIRST,
            BASEMENT,
            ATTIC
        }

        [SerializeField]
        private OrthoSmoothFollow followCamera;

        [SerializeField]
        private Player player;
        [SerializeField]
        private Transform floor1AtticTransform;
        [SerializeField]
        private Transform floor2BasementTransform;
        [SerializeField]
        private Transform atticTransform;
        [SerializeField]
        private Transform basementTransform;

        private GAME_STATE currentState;
        public GAME_STATE CurrentState { get { return currentState; } }

        [SerializeField]
        private UIManager uiManager;

        private FLOOR currentFloor;
        private FLOOR dest;

        [SerializeField]
        private DataManager dataManager;
        private ItemData introData;
        private ItemData outroSuccessData;
        private ItemData outroFailData;

        [SerializeField]
        private ItemManager itemManager;

        [SerializeField]
        private AudioManager audioManager; 

        [SerializeField]
        private GhostManager ghostManager; 

        private bool itemJustPressed = false;

        private bool gameFinished = false; 
        // Start is called before the first frame update
        void Awake()
        {
            gameFinished = false; 
            introData = dataManager.GetIntro();
            outroSuccessData = dataManager.GetOutroPos();
            outroFailData = dataManager.GetOutroNeg();
            itemManager = GetItemManager();

            currentState = GAME_STATE.MAIN_MENU;

            // disable background game
            

            currentFloor = FLOOR.FIRST;
        }

        // Update is called once per frame
        void Update()
        {
    

            if (currentState == GAME_STATE.READING && !itemJustPressed)
            {
                if (InputHandler.Instance.MouseLeftPressed)
                {
                    uiManager.SkipScroll();
                }
            }
        }

        public void StartGame()
        {
            Debug.Log("Game Started");
            currentState = GAME_STATE.RUNNING;
            // TODO, Fill me in based on the game!
            player.Reset();
            itemManager.Reset();

            SetPanelText(introData);

        }

        public void ResetGame()
        {
            gameFinished = false; 
            Debug.Log("Game Reset");
            // need to reset item and ghost managers/containers
            player.Reset();
            itemManager.Reset();
            // need ghost reset function
            StartGame();
        }

        public void GameWon()
        {
            // TODO set panel text but remove title
            SetPanelText(outroSuccessData);
            gameFinished = true; 
        }

        public void GameOver()
        {
            // TODO set panel text but remove title
            SetPanelText(outroFailData);
            gameFinished = true; 
        }

        public void EnableReading()
        {
            currentState = GAME_STATE.READING;
        }

        public void DisableReading()
        {
            if (gameFinished)
            {
                Quit(); 
            }
            else
            {
                currentState = GAME_STATE.RUNNING;
            }
            
        }

        public void AllowSkip()
        {
            itemJustPressed = false;
        }

        public void SetPanelText(ItemData data)
        {
            EnableReading();
            itemJustPressed = true; 
            if (uiManager != null)
            {
                uiManager.StartPanel(data);
            }
            
        }

        public void ClosePanel()
        {
            uiManager.gameObject.SetActive(false);
            
        }

        public void StartFloorTransition(FLOOR destination)
        {
            currentState = GAME_STATE.TRANSITIONING;
            dest = destination;
            uiManager.StartFadeIn();
        }

        public void TransitionFloor()
        {
            // TODO: Move everything or reset here while black
            ghostManager.DespawnAllGhosts(); 
            switch (dest)
            {
                case FLOOR.FIRST:
                    if (currentFloor == FLOOR.ATTIC)
                    {
                        player.transform.position = floor1AtticTransform.position;
                        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                    else
                    {
                        player.transform.position = floor2BasementTransform.position;
                        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }

                    followCamera.SetPositionToTarget();
                    break;
                case FLOOR.ATTIC:
                    player.transform.position = atticTransform.position;
                    player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    followCamera.SetPositionToTarget();
                    break;
                case FLOOR.BASEMENT:
                    player.transform.position = basementTransform.position;
                    player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    followCamera.SetPositionToTarget();
                    break;
            }
            currentState = GAME_STATE.RUNNING;
            currentFloor = dest;

            uiManager.StartFadeOut();
        }

        public void TransitionComplete()
        {
            // TODO: any logic post transition
        }

        public void DealPlayerDamage()
        {
            player.TakeDamage();
        }

        public void NewVicinityItem()
        {
            itemManager.AuditHauntedItems();
        }

        public void RemoveHauntedItem(Item item)
        {
            itemManager.RemoveHauntedItem(item);
        }

        public Player GetPlayer()
        {
            return player; 
        }

        public ItemManager GetItemManager()
        {
            return itemManager; 
        }

        public void PlayScribbleSound()
        {
            audioManager.PlayWriteSound(); 
        }

        public AudioManager GetAudioManager()
        {
            return audioManager;
        }

        public void StopScribbleSound()
        {
            audioManager.StopWriteSound(); 
        }

        public FLOOR GetCurrentFloor()
        {
            return currentFloor; 
        }

        public void Quit()
        {
            //If we are running in a standalone build of the game
        #if UNITY_STANDALONE
            //Quit the application
            Application.Quit();
        #endif

            //If we are running in the editor
        #if UNITY_EDITOR
            //Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        }
    }
}

