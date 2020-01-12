using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Progress Bar UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Progressbar", 1), DisallowMultipleComponent]
public class ProgressBar : MonoBehaviour {

    [Range(0f, 100f)]
    public float progress;
    [Range(0, 12), Tooltip("Number of decimal places to display. If this is an int-based bar, set to 0.")]
    public int decimalPlaces = 0;
    [Tooltip("Optonally set custom text to be used instead of the default percentage text. Use #value to get the actual value in string.\nFor example: 'Progress is at #value percent.'")]
    public string percentText;
    [Tooltip("Determines if a 0 is added before the number, to appear as 05. If not check, it will appear as just 5.")]
    public bool precedingZero;

    private Slider bar;
    private Text percentage;

    void Awake()
    {
        bar = GetComponent<Slider>();
        bar.minValue = 0f;
        bar.maxValue = 1f;
        bar.wholeNumbers = false;

        percentage = transform.GetChild(2).GetComponent<Text>();
    }

    void FixedUpdate()
    {
        bar.value = progress / 100f;
        string decimals = (precedingZero) ? "00." : "0.";
        for (int i = 0; i < decimalPlaces; i++) { decimals += "0"; }
        if(decimals == "0.") { decimals = (precedingZero) ? "00" : "0"; }
        percentage.text = (string.IsNullOrEmpty(percentText) ? progress + "%" : percentText.Replace("#value", progress.ToString(decimals)));
    }
}
