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
            uiPanel.SetItemName(data.itemID.ToString());
            uiPanel.SetItemText(data.itemBody);
            uiPanel.gameObject.SetActive(true);
            uiPanel.StartScroll(); 
        }

        public void SkipScroll()
        {
            uiPanel.SkipScroll(); 
        }

        public void ClosePanel()
        {
            uiPanel.gameObject.SetActive(false);
            GameManager.Instance.DisableReading(); 
        }

        public void StartFadeIn()
        {
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
        
    }
}

