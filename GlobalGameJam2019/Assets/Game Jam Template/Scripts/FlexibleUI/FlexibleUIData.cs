using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Jam; 

[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIData : ScriptableObject
{

    [Header("Button Colors")]
    public ColorBlock buttonColorBlock;

    [Header("Panel Color")]
    public Color imageColor;

    [Header("Slider Colors")]
    public ColorBlock sliderColorBlock;

    [Header("Title Text Attributes")]
    public TMP_FontAsset titleFont;
    public Color titleTextColor;
    public bool autoSizeTitleFont;
    public float defaultTitleFontSize;
    [EnumFlag("Default Font Styles")]
    public FontStyles defaultFontStyles;

    [Header("Default Text Attributes")]
    public TMP_FontAsset font;
    public Color textColor;
    public bool autoSizeFont;
    public float defaultFontSize;
    [EnumFlag("Title Font Styles")]
    public FontStyles titleFontStyles;
}

