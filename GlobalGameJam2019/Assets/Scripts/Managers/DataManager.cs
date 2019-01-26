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
        public string itemName;
        public Category itemCategory;
        public string itemBody; 
    }

    [Serializable]
    public enum Category
    {
        Collectible,
        ThrowOff,
        Intro,
        None
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
        public string Name; 
        public Category Category;
        public string Text;
    }

    [Serializable]
    public class PhraseReadData
    {
        public string Name; 
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
            readData = PhraseDataContainer.Load(WrittenDataPath).WrittenData;
            ConvertReadFormat(readData);

            // Testing
            foreach(PhraseData phrase in phrases)
            {
                Debug.Log(phrase.Name);
                Debug.Log(phrase.Category);
                Debug.Log(phrase.Text); 
            }
        }

        private void ConvertReadFormat(PhraseReadData[] readData)
        {
            phrases = new PhraseData[readData.Length];
            for (int iData = 0; iData < readData.Length; ++iData)
            {
                PhraseReadData data = readData[iData];
                Category cat;
                cat = (Category)Enum.Parse(typeof(Category), data.Category, true);

                phrases[iData] = new PhraseData();
                phrases[iData].Category = cat;
                phrases[iData].Name = data.Name; 
                phrases[iData].Text = data.Text;
            }
        }

        
    }
}
