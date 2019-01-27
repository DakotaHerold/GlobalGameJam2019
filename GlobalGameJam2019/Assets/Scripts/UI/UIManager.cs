using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        UIItemPanelManager uiPanel; 

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
        
    }
}

