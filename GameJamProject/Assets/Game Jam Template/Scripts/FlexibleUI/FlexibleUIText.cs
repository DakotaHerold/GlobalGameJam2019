using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


[RequireComponent(typeof(TextMeshProUGUI))]
public class FlexibleUIText : FlexibleUI
{
    private TextMeshProUGUI text;
    public bool isTitle; 

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        base.Initialize();
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        if (flexibleUIData == null)
            return; 

        // Set text attributes.
        if(!isTitle)
        {
            text.font = flexibleUIData.font; 
            text.color = flexibleUIData.textColor;
            text.fontSize = flexibleUIData.defaultFontSize;
            text.enableAutoSizing = flexibleUIData.autoSizeFont;
            text.fontStyle = flexibleUIData.defaultFontStyles;
        }
        else
        {
            // Title attributes
            text.font = flexibleUIData.titleFont; 
            text.fontSize = flexibleUIData.defaultTitleFontSize;
            text.color = flexibleUIData.titleTextColor;
            text.enableAutoSizing = flexibleUIData.autoSizeTitleFont;
            text.fontStyle = flexibleUIData.titleFontStyles; 
        }
        

    }



}
