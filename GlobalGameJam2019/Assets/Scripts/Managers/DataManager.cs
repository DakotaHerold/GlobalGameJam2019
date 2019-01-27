using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace Jam
{
    public struct ItemData
    {
        public ID itemID;
        public Category itemCategory;
        public string itemBody; 
    }

    [Serializable]
    public enum Category
    {
        Collectible,
        ThrowOff,
        Intro,
        None, 
        Outro
    }

    [Serializable]
    public enum ID
    {
        Diary,
        Trunk,
        Doily,
        Lockbox,
        LetterOpened, 
        LetterUnopened, 
        Typewriter, 
        Cellphone, 
        Dictionary, 
        GenericBook, 
        CardboardBox, 
        Bills, 
        Computer, 
        Bottles, 
        Slippers, 
        GenericShoes, 
        Intro, 
        OutroPos,
        OutroNeg
    }

    [Serializable]
    public class PhraseDataContainer
    {
        // NOTE: This must be the same name as the google sheet name!
        public PhraseReadData[] WrittenData;

        public static PhraseDataContainer Load(string PhraseDataPath)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, PhraseDataPath + ".json");
            //Debug.Log("Loading from path: " + filePath); 

            var serialized = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PhraseDataContainer>(serialized);
        }
    }

    [Serializable]
    public class PhraseData
    {
        public ID ID; 
        public Category Category;
        public string Text;
    }

    [Serializable]
    public class PhraseReadData
    {
        public string ID; 
        public string Category;
        public string Text;
    }


    public class DataManager : MonoBehaviour
    {
        private string WrittenDataPath = "WrittenData";

        [HideInInspector]
        public PhraseData[] phrases;

        private PhraseReadData[] readData;

        private void Awake()
        {
            ProcessData(); 

            // Testing
            //foreach(PhraseData phrase in phrases)
            //{
            //    Debug.Log(phrase.ID);
            //    Debug.Log(phrase.Category);
            //    Debug.Log(phrase.Text); 
            //}
        }

        private void ProcessData()
        {
            readData = PhraseDataContainer.Load(WrittenDataPath).WrittenData;
            ConvertReadFormat(readData);
        }

        private void ConvertReadFormat(PhraseReadData[] readData)
        {
            phrases = new PhraseData[readData.Length];
            for (int iData = 0; iData < readData.Length; ++iData)
            {
                PhraseReadData data = readData[iData];
                Category cat;
                cat = (Category)Enum.Parse(typeof(Category), data.Category, true);

                ID itemID;
                itemID = (ID)Enum.Parse(typeof(ID), data.ID, true); 

                phrases[iData] = new PhraseData();
                phrases[iData].Category = cat;
                phrases[iData].ID = itemID; 
                phrases[iData].Text = data.Text;
            }
        }

        public ItemData GetItemData(ID itemID)
        {
            if(phrases.Length < 1)
            {
                ProcessData(); 
            }

            ItemData data;
            data.itemBody = "";
            data.itemID = ID.Diary;
            data.itemCategory = Category.None; 
            for(int iPhrase = 0; iPhrase < phrases.Length; ++iPhrase)
            {
                if(phrases[iPhrase].ID == itemID)
                {
                    data.itemID = phrases[iPhrase].ID;
                    data.itemBody = phrases[iPhrase].Text;
                    data.itemCategory = phrases[iPhrase].Category;
                    break; 
                }
            }
            return data; 
        }

        public ItemData GetOutroNeg()
        {
            return GetItemData(ID.OutroNeg); 
        }

        public ItemData GetOutroPos()
        {
            return GetItemData(ID.OutroPos);
        }

        public ItemData GetIntro()
        {
            return GetItemData(ID.Intro);
        }
    }
}
