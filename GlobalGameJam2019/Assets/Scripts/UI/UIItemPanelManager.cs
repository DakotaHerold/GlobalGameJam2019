using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace Jam {
    public class UIItemPanelManager : MonoBehaviour
    {
        private UIManager uiManager;

        // Elements
        public TextMeshProUGUI itemNameElement; 
        public TextMeshProUGUI bodyTextElement;

        private float characterTypeDuration = 0.05f; // default 0.125f

        private string activeTextBlock;

        private string itemName;
        private string itemBody; 

        private Coroutine activeRoutine; 

        // Start is called before the first frame update
        void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();

            itemNameElement.text = ""; 
            bodyTextElement.text = "";
            // Disable if active 
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }

        }

        IEnumerator ScrollTextRoutine(string textBlock)
        {
            bodyTextElement.text = "";
            activeTextBlock = textBlock;

            foreach (char c in activeTextBlock)
            {
                bodyTextElement.text += c;
                yield return new WaitForSeconds(characterTypeDuration);
            }
            itemBody = "";
            
        }

        public void SkipTextType()
        {
            StopCoroutine(activeRoutine); 
            bodyTextElement.text = "";
            bodyTextElement.text = activeTextBlock; 
        }

        public void SetItemText(string newBody)
        {
            itemBody = newBody; 
        }

        public void SetItemName(string newName)
        {
            itemName = newName;
            itemNameElement.text = itemName; 
        }

        public void StartScroll()
        {
            activeRoutine = StartCoroutine(ScrollTextRoutine(itemBody)); 
        }

        public void StopScroll()
        {
            if(activeRoutine != null)
                StopCoroutine(activeRoutine); 
        }


    }
}
