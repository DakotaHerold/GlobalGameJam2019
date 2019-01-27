using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jam
{
    public class ScreenFader : MonoBehaviour
    {
        private Image image;

        private float fadeTime = 0.75f;
        private UIManager uiManager;

        // Start is called before the first frame update
        void Awake()
        {
            image = GetComponent<Image>();
            image.color = Color.clear;
            uiManager = GetComponentInParent<UIManager>();
        }

        public void StartFade()
        {
            StartCoroutine(FadeTo(1.0f, fadeTime)); 
        }

        IEnumerator FadeTo(float aValue, float aTime)
        {
            float alpha = image.color.a;
            
            for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / aTime))
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                image.color = newColor;
                yield return null;
            }
            yield return new WaitForSeconds(1.5f);

            Debug.Log(image.color.a);
            if ((Mathf.Round(image.color.a*10)/10) == 1f)
            {
                FadeInComplete();
            }
            else
            {
                FadeComplete(); 
            }
             
        }


        private void FadeInComplete()
        {
            uiManager.FadeInComplete(); 
        }

        public void StartFadeOut()
        {
            StartCoroutine(FadeTo(0.0f, fadeTime));
        }

        private void FadeComplete()
        {
            uiManager.FullFadeComplete(); 
        }
    }
}
