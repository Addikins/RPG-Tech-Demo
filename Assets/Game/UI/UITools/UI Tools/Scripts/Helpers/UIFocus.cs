using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI Focus UI Tool - Extention Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Extentions/UI Focus", 2), DisallowMultipleComponent]
public class UIFocus : MonoBehaviour
{
    [Tooltip("If set, the specified UI object will be set as the initial focused object in the event system, on start.")]
    public RectTransform initialFocus;
    [Tooltip("If set, everytime Unselect() is called, this object will be the focus set in the event system.")]
    public RectTransform defaultFocus;

    /// <summary>
    /// Get or set the currently selected/focused gameobject of the UI Event system.
    /// </summary>
    public GameObject Focus { get { return EventSystem.current.currentSelectedGameObject; } set { if (Focus != value) { EventSystem.current.SetSelectedGameObject(value); Focus = value; } } }

    /// <summary>
    /// A direct reference to the UI Focus class, from the global/static call.
    /// </summary>
    public static UIFocus reference { get; protected set; }

    void Awake()
    {
        reference = this;

        if (defaultFocus == null) { try { defaultFocus = FindObjectOfType<Text>().GetComponent<RectTransform>(); } catch { defaultFocus = FindObjectOfType<RectTransform>(); } }
        EventSystem.current.SetSelectedGameObject((initialFocus != null) ? initialFocus.gameObject : null);
    }

    /// <summary>
    /// Unselect the currently selected UI element from the event system.
    /// </summary>
    /// <param name="selectDefault">Determines if the event system should select the defaultFocus or set to null</param>
    public void Unselect(bool selectDefault = true)
    {
        if (selectDefault)
        {
            if (defaultFocus == null) { try { defaultFocus = FindObjectOfType<Text>().GetComponent<RectTransform>(); } catch { defaultFocus = FindObjectOfType<RectTransform>(); } }
            Focus = (defaultFocus != null) ? defaultFocus.gameObject : null;
        }
        else
        {
            Focus = null;
        }
    }
}
