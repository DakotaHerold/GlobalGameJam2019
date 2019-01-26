using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class Collectible : Item
    {
        // Needs to be tracked and handle deletion 

        protected override void Awake()
        {
            base.Awake(); 
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnMouseDown()
        { 
            base.OnMouseDown();
            ItemManager.AddCollectible(this); 
        }

        public override void SetItemManager(ItemManager manager)
        {
            ItemManager = manager;
        }

        public override void SetItemData(ItemData newData)
        {
            itemData = newData;
        }

        public override ItemData GetItemData()
        {
            return itemData;
        }
    }
}
