using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GAME_STATE
        {
            MAIN_MENU, 
            RUNNING,
            COMPLETE
        }

        private GAME_STATE currentState; 
        public GAME_STATE CurrentState { get { return currentState; } }

        // Start is called before the first frame update
        void Start()
        {
            currentState = GAME_STATE.MAIN_MENU; 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartGame()
        {
            Debug.Log("Game Started");
            currentState = GAME_STATE.RUNNING;
            // TODO, Fill me in based on the game!
        }

        public void ResetGame()
        {
            Debug.Log("Game Reset"); 
            // TODO, Fill me in based on the game!
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

