using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

namespace Jam
{
    public class ItemManager : MonoBehaviour
    {
        enum INTERVAL
        {
            ONE,
            TWO,
            THREE
        }

        class HauntedItem
        {
            public float startMotionTimer;
            public float stopMotionTimer;
            public float targetInterval;
            public float stopInterval; 
            public bool active; 
            public Item item; 
        }

        List<Collectible> potentialCollectibles;
        List<Fixable> fixableItems;
        List<Item> herrings;

        List<Collectible> collectedItems;
        List<Fixable> fixedItems;

        List<HauntedItem> hauntedItems; 

        private DataManager dataManager;

        
        private const float INTERVAL_1 = 0.75f;
        private const float INTERVAL_2 = 10.0f;
        private const float INTERVAL_3 = 20.0f;

        private const float INTERVAL_1_FREQ_MAX = 7.0F;
        private const float INTERVAL_1_MAG_MAX = 3.0F;
        private const float INTERVAL_2_FREQ_MAX = 13.0F;
        private const float INTERVAL_2_MAG_MAX = 3.0F;
        private const float INTERVAL_3_FREQ_MAX = 13.0F;
        private const float INTERVAL_3_MAG_MAX = 3.0F;

        private const float STOP_INTERVAL = 6.0f; 

        void Awake()
        {
            Reset();

            
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.CurrentState == GameManager.GAME_STATE.READING)
                return; 

            UpdateHauntedItems(); 
        }

        private void UpdateHauntedItems()
        {
            for(int iItem = hauntedItems.Count-1; iItem > -1; --iItem)
            {
               
                if(hauntedItems[iItem].item == null)
                {
                    RemoveHauntedItem(hauntedItems[iItem].item); 
                    continue; 
                }

                //bool stoppingMotion = hauntedItems[iItem].item.IsStopping(); 
                

                if (hauntedItems[iItem].active)
                {
                    hauntedItems[iItem].stopMotionTimer += Time.deltaTime;
                    if (hauntedItems[iItem].stopMotionTimer >= hauntedItems[iItem].stopInterval)
                    {
                        hauntedItems[iItem].targetInterval = Random.Range(1.0f, INTERVAL_1); 
                        hauntedItems[iItem].stopMotionTimer = 0.0f;
                        hauntedItems[iItem].active = false;
                        hauntedItems[iItem].item.StopRotate();
                        hauntedItems[iItem].item.StopShake();
                    }
                }
                else
                {
                    hauntedItems[iItem].startMotionTimer += Time.deltaTime;


                    if (hauntedItems[iItem].startMotionTimer >= hauntedItems[iItem].targetInterval)
                    {
                        //hauntedItems[iItem].targetInterval = Random.Range(1.0f, INTERVAL_1);
                        hauntedItems[iItem].startMotionTimer = 0.0f; 
                        hauntedItems[iItem].active = true;
                        SetItemIntervalMovement(hauntedItems[iItem].item, INTERVAL.ONE);
                    }
                }


            }
        }

        private void SetItemIntervalMovement(Item item, INTERVAL interval)
        {
            // Determine whether to rotate or shake
            int randOne = Random.Range(0, 2); 
            if(randOne == 0)
            {
                item.Rotate(); 
            }
            else
            {
                // Randomly determine axis 
                int randTwo = Random.Range(0, 3);
                Jam.Item.SHAKE_AXIS axis;
                axis = Item.SHAKE_AXIS.None; 
                switch(randTwo)
                {
                    case 0:
                        axis = Item.SHAKE_AXIS.X; 
                        break;
                    case 1:
                        axis = Item.SHAKE_AXIS.Y;
                        break;
                    case 2:
                        axis = Item.SHAKE_AXIS.Z;
                        break; 
                }

                // Check if the motion should start on the left or right 
                bool negated = (Random.Range(0, 2) == 0) ? true : false; 

                // Apply interval intensity
                switch(interval)
                {
                    case INTERVAL.ONE:
                        // Calc frequency and magnitude
                        float freq1 = Random.Range(0.0f, INTERVAL_1_FREQ_MAX);
                        float mag1 = Random.Range(0.0f, INTERVAL_1_MAG_MAX);
                        item.Shake(axis, freq1, mag1, negated); 
                        break;
                    case INTERVAL.TWO:
                        // Calc frequency and magnitude
                        float freq2 = Random.Range(0.0f, INTERVAL_2_FREQ_MAX);
                        float mag2 = Random.Range(0.0f, INTERVAL_2_MAG_MAX);
                        item.Shake(axis, freq2, mag2, negated);
                        break;
                    case INTERVAL.THREE:
                        // Calc frequency and magnitude
                        float freq3 = Random.Range(0.0f, INTERVAL_3_FREQ_MAX);
                        float mag3 = Random.Range(0.0f, INTERVAL_3_MAG_MAX);
                        item.Shake(axis, freq3, mag3, negated);
                        break; 
                }
            }
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

        public void ItemPressed(Item item)
        {
            GameManager.Instance.SetPanelText(item.GetItemData()); 
        }

        public void AddCollectible(Collectible item)
        {
            Debug.Log(potentialCollectibles);
            potentialCollectibles.Remove(item);
            collectedItems.Add(item);
            // Deactivate game object
            item.gameObject.SetActive(false); 
            if(potentialCollectibles.Count < 1)
            {
                GameManager.Instance.GameWon(); 
            }
        }

        public void AddFixed(Fixable item)
        {
            if(fixableItems.Contains(item))
                fixableItems.Remove(item);

            if (!fixedItems.Contains(item))
            {
                fixedItems.Add(item);
                GameManager.Instance.GetPlayer().BrightenFlashlight();
            }
        }

        public void AuditHauntedItems()
        {
            List<Item> playerItems = GameManager.Instance.GetPlayer().GetVicinityItems(); 
            foreach(Item i in playerItems)
            {
                bool contains = false;
                for(int jItem = hauntedItems.Count-1; jItem > -1; --jItem)
                {
                    if(hauntedItems[jItem].item == i)
                    {
                        contains = true;
                        RemoveHauntedItem(hauntedItems[jItem].item);
                        break; 
                    }
                }
                if (contains)
                    continue; 
                else
                {
                    HauntedItem newItem = new HauntedItem();
                    newItem.targetInterval = Random.Range(1.0f, INTERVAL_1);
                    newItem.stopInterval = STOP_INTERVAL; 
                    newItem.startMotionTimer = 0.0f;
                    newItem.stopMotionTimer = 0.0f; 

                    newItem.item = i;
                    hauntedItems.Add(newItem); 
                }
            }
        }

        public void ResetHauntedItem(Item item)
        {
            foreach(HauntedItem hi in hauntedItems)
            {
                if(hi.item == item)
                {
                    hi.stopMotionTimer = 0.0f;
                    hi.active = false;
                    hi.startMotionTimer = 0.0f; 
                    hi.item.StopRotate();
                    hi.item.StopShake();
                }
            }
        }

        public void RemoveHauntedItem(Item item)
        {
            hauntedItems.RemoveAll(x => x.item == item);
            if(item != null)
            {
                item.StopRotate();
                item.StopShake();
            }
            
            //Debug.Log(hauntedItems.Count);
        }

        public void Reset()
        {
            dataManager = GetComponent<DataManager>();
            potentialCollectibles = new List<Collectible>();
            fixableItems = new List<Fixable>();
            herrings = new List<Item>();

            collectedItems = new List<Collectible>();
            fixedItems = new List<Fixable>();
            hauntedItems = new List<HauntedItem>();

            potentialCollectibles = FindObjectsOfType<Collectible>().ToList();
            fixableItems = FindObjectsOfType<Fixable>().ToList();

            herrings = FindObjectsOfType<Item>().ToList();

            SetItemData();
        }
    }
}
