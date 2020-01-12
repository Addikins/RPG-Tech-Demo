using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Listbox UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Radio Group", 0), DisallowMultipleComponent]
public class RadioButtonGroup : MonoBehaviour
{
    [Tooltip("Optional. Set a radio button as default to be selected on start. Leaving blank will cause no buttons to be selected at start.")]
    public Toggle defaultRadioButton;

    /// <summary>
    /// Automatically sets to the last radio button selected.
    /// </summary>
    [HideInInspector]
    public Toggle selectedRadioButton;

    private ToggleGroup defaultToggleGroup;
    private Toggle defaultToggleButton;

    void Awake()
    {
        defaultToggleGroup = GetComponent<ToggleGroup>();

        if (transform.childCount > 0)
        {
            Toggle firstToggle = transform.GetChild(0).GetComponent<Toggle>();
            if (firstToggle != null) { defaultToggleButton = firstToggle; }
        }

        UncheckAllAndSubscribe();

        ReassignRadioGroup(defaultToggleGroup);
        SelectRadioButton(defaultRadioButton);
    }

    void UncheckAllAndSubscribe()
    {
        foreach (Toggle radio in transform.GetComponentsInChildren<Toggle>())
        {
            radio.isOn = false;
            radio.onValueChanged.AddListener(delegate { SetSelectedToCurrent(radio); });
        }
    }

    void SetSelectedToCurrent(Toggle current)
    {
        if (current.isOn && selectedRadioButton != current) { selectedRadioButton = current; }
    }

    #region Public Functions
    /// <summary>
    /// Sets the specified toggle as the currently selected radio button.
    /// </summary>
    /// <param name="radioButton">Toggle to select.</param>
    public void SelectRadioButton(Toggle radioButton)
    {
        if(radioButton != null) { radioButton.isOn = true; }
    }

    /// <summary>
    /// Set all radio buttons to the specified ToggleGroup. This is also automatically called when calling AddRadioButton.
    /// </summary>
    /// <param name="group">Toggle Group to reassign every radio button to.</param>
    public void ReassignRadioGroup(ToggleGroup group)
    {
        foreach(Toggle radio in transform.GetComponentsInChildren<Toggle>())
        {
            radio.group = group;
        }
    }

    /// <summary>
    /// Add/Instantiate a radio button.
    /// </summary>
    public void AddRadioButton()
    {
        Instantiate(defaultToggleButton, transform);
        ReassignRadioGroup(defaultToggleGroup);
    }

    /// <summary>
    /// Add/Instantiate a radio button, at a set index.
    /// </summary>
    /// <param name="index">Index to insert this new radio button at.</param>
    public void AddRadioButton(int index)
    {
        Instantiate(defaultToggleButton, transform).transform.SetSiblingIndex(index);
        ReassignRadioGroup(defaultToggleGroup);
    }

    /// <summary>
    /// Add/Instantiate a radio button, with a set name.
    /// </summary>
    /// <param name="name">Name to set this new radio button to.</param>
    public void AddRadioButton(string name)
    {
        Instantiate(defaultToggleButton, transform).name = name;
        ReassignRadioGroup(defaultToggleGroup);
    }

    /// <summary>
    /// Add/Instantiate a radio button, with a set name at index.
    /// </summary>
    /// <param name="name">Name to set this new radio button to.</param>
    /// <param name="index">Index to insert this new radio button at.</param>
    public void AddRadioButton(string name, int index)
    {
        Toggle toggle = Instantiate(defaultToggleButton, transform) as Toggle;
        toggle.transform.name = name;
        toggle.transform.SetSiblingIndex(index);
        ReassignRadioGroup(defaultToggleGroup);
    }

    /// <summary>
    /// Remove/Destroy a radio button. This will destroy the last indexed radio button by default.
    /// </summary>
    public void RemoveRadioButton()
    {
        if (transform.childCount > 0) { Destroy(transform.GetChild(transform.childCount - 1)); }
    }

    /// <summary>
    /// Remove/Destroy a radio button at index.
    /// </summary>
    /// <param name="index">Index to remove a radio button from.</param>
    public void RemoveRadioButton(int index)
    {
        if (transform.childCount > index) { Destroy(transform.GetChild(index)); }
    }
    #endregion
}
