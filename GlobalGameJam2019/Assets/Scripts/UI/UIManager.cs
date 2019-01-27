using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Jam
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private UIItemPanelManager uiPanel;

        [SerializeField]
        private ScreenFader fader; 

        // Start is called before the first frame update
        void Awake()
        {
          
        }

        public void StartPanel(ItemData data)
        {
            uiPanel.StopScroll();
            string title = GetTitleFromID(data.itemID); 
            uiPanel.SetItemName(title);
            uiPanel.SetItemText(data.itemBody);
            uiPanel.gameObject.SetActive(true);
            uiPanel.StartScroll(); 
        }

        private string GetTitleFromID(ID titleID)
        {
            string result = "";
            switch(titleID)
            {
                case ID.Bills:
                    result = titleID.ToString(); 
                    break;
                case ID.Bottles:
                    result = titleID.ToString();
                    break;
                case ID.CardboardBox:
                    result = "Cardboard Box"; 
                    break;
                case ID.Cellphone:
                    result = titleID.ToString();
                    break;
                case ID.Computer:
                    result = titleID.ToString();
                    break;
                case ID.Diary:
                    result = titleID.ToString();
                    break;
                case ID.Dictionary:
                    result = titleID.ToString();
                    break;
                case ID.Doily:
                    result = titleID.ToString();
                    break;
                case ID.GenericBook:
                    result = "Book";
                    break;
                case ID.GenericShoes:
                    result = "Shoes"; 
                    break;
                case ID.Intro:
                    result = ""; 
                    break;
                case ID.LetterOpened:
                    result = "Open Letter"; 
                    break;
                case ID.LetterUnopened:
                    result = "Unopened Letter"; 
                    break;
                case ID.Lockbox:
                    result = titleID.ToString();
                    break;
                case ID.OutroNeg:
                    result = ""; 
                    break;
                case ID.OutroPos:
                    result = ""; 
                    break;
                case ID.Slippers:
                    result = titleID.ToString(); 
                    break;
                case ID.Trunk:
                    result = titleID.ToString();
                    break;
                case ID.Typewriter:
                    result = titleID.ToString();
                    break; 
            }
            return result; 
        }

        public void SkipScroll()
        {
            uiPanel.SkipScroll(); 
        }

        public void ClosePanel()
        {
            fader.gameObject.SetActive(false);
            uiPanel.gameObject.SetActive(false);
            GameManager.Instance.DisableReading(); 
        }

        public void StartFadeIn()
        {
            fader.gameObject.SetActive(true); 
            fader.StartFade(); 
        }

        public void FadeInComplete()
        {
            GameManager.Instance.TransitionFloor(); 
        }

        public void StartFadeOut()
        {
            fader.StartFadeOut(); 
        }

        public void FullFadeComplete()
        {
            GameManager.Instance.TransitionComplete(); 
        }
        
    }
}

