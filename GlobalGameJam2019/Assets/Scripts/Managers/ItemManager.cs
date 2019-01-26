using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

namespace Jam
{
    public class ItemManager : MonoBehaviour
    {

        List<Collectible> potentialCollectibles;
        List<Fixable> fixableItems;
        List<Item> herrings;

        List<Collectible> collectedItems;
        List<Fixable> fixedItems;

        private DataManager dataManager; 

        void Awake()
        {
            dataManager = GetComponent<DataManager>(); 
            potentialCollectibles = new List<Collectible>();
            fixableItems = new List<Fixable>();
            herrings = new List<Item>();

            collectedItems = new List<Collectible>();
            fixedItems = new List<Fixable>();

            potentialCollectibles = FindObjectsOfType<Collectible>().ToList();
            fixableItems = FindObjectsOfType<Fixable>().ToList();

            herrings = FindObjectsOfType<Item>().ToList();

            SetItemData(); 
        }

        private void SetItemData()
        {
            //Debug.Log("\nCollectibles");
            foreach(Collectible c in potentialCollectibles)
            {
                c.gameObject.SetActive(true); 
                if (herrings.Contains(c))
                    herrings.Remove(c); 

                c.SetItemManager(this);
                ItemData data = dataManager.GetItemData(c.desiredType);
                c.SetItemData(data); 
                //Debug.Log(c.gameObject.name); 
            }

            
            //Debug.Log("\nFixable Items"); 
            foreach(Fixable f in fixableItems)
            {
                f.gameObject.SetActive(true); 
                if (fixableItems.Contains(f))
                    herrings.Remove(f); 

                f.SetItemManager(this);
                ItemData data = dataManager.GetItemData(f.desiredType);
                f.SetItemData(data);
                //Debug.Log(f.gameObject); 
            }

            //Debug.Log("\nHerrings");
            foreach(Item h in herrings)
            {
                h.gameObject.SetActive(true);
                h.SetItemManager(this);
                ItemData data = dataManager.GetItemData(h.desiredType);
                h.SetItemData(data);
                //Debug.Log(h.gameObject); 
            }
        }

        public void AddCollectible(Collectible item)
        {
            potentialCollectibles.Remove(item);
            collectedItems.Add(item);
            // Deactivate game object
            item.gameObject.SetActive(false); 
            if(potentialCollectibles.Count < 1)
            {
                // TODO: Game won 
            }
        }

        public void AddFixed(Fixable item)
        {
            fixableItems.Remove(item);
            fixedItems.Add(item);
            // TODO: Increase flashlight radius
        }
    }
}
