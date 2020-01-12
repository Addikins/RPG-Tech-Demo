using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Listbox UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Combobox (Advanced Dropbox)", 1), DisallowMultipleComponent]
public class Combobox : MonoBehaviour
{
    [Header("Design")]
    [Tooltip("The direction to display the Dropdown list. By default, this is set to 'Below'.")]
    public DropdownDirection direction = DropdownDirection.Below;
    [Tooltip("The height of each element inside the Dropdown list.")]
    public float elementHeight;
    [Tooltip("If checked, the checkmark image to the far-left of the text will be hidden for the selected element in the Dropdown list.")]
    public bool hideCheckmark;
    [Tooltip("If checked, the Dropdown will adjust to accomidate for the Item Image on the far-right.")]
    public bool useThumbnailImage = true;
    [Tooltip("If checked, the selected element in the Dropdown list will remain highlighted, even after mouse events are active on it.")]
    public bool keepSelectionHighlighted = true;

    [Space]

    public CustomData theme = new CustomData();
    
    [Space]

    public Init initalization = new Init();

    private Dropdown dropdown;
    private Toggle toggle;
    private RectTransform toggleRT;
    private Image image;
    private Text text;
    private ScrollRect scrollRect;
    private ColorBlock colors;

    [System.Serializable]
    public class CustomData
    {
        [Tooltip("The background color of the actual selection box.")]
        public Color selection;
        [Tooltip("The background color of the element when no mouse events are influencing it.")]
        public Color element;
        [Tooltip("The background color of the element the mouse is under.")]
        public Color highlight;
        [Tooltip("The background color of the clicked or 'pressed' element.")]
        public Color pressed;
        [Tooltip("The color of the Text Elements in the Dropbox.")]
        public Color text;

        [Header("Scrollbar Specific")]
        [Tooltip("If checked, the Scoll Space variable will be a ratio of the Scrollbar's color. It will always be dimmer/darker than the Scrollbar color.")]
        public bool dimWithScrollbar = true;
        [Tooltip("The color of the scrollbar visual itself.")]
        public Color scrollbar;
        [Tooltip("The color of the scrollable space.")]
        public Color scrollSpace;

        [Space]
        
        public FontData fontData = new FontData();
    };

    [System.Serializable]
    public class OptionData
    {
        [Tooltip("Normal = Regular Element text.\nHeader = Special Element (non-clickable) text.\nSplitter = Special Element (non-clickable) image")]
        public OptionType type;
        public string text;
        public Sprite image;

        /// <summary>
        /// Create an OptionData Element with passed data.
        /// </summary>
        /// <param name="text">Text to set in the element</param>
        /// <param name="type">The type of OptionData Element this pertains to</param>
        public OptionData(string text, OptionType type)
        {
            this.type = type;
            this.text = text;
        }

        /// <summary>
        /// Create an OptionData Element with passed data.
        /// </summary>
        /// <param name="text">Text to set in the element</param>
        public OptionData(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Create an OptionData Element with passed data.
        /// </summary>
        /// <param name="image">Image to set in the element</param>
        /// <param name="type">The type of OptionData Element this pertains to</param>
        public OptionData(Sprite image, OptionType type)
        {
            this.type = type;
            this.image = image;
        }

        /// <summary>
        /// Create an OptionData Element with passed data.
        /// </summary>
        /// <param name="image">Image to set in the element</param>
        public OptionData(Sprite image)
        {
            this.image = image;
        }

        /// <summary>
        /// Create an OptionData Element with passed data.
        /// </summary>
        /// <param name="text">Text to set in the element</param>
        /// <param name="image">Image to set in the element</param>
        /// <param name="type">The type of OptionData Element this pertains to</param>
        public OptionData(string text, Sprite image, OptionType type)
        {
            this.type = type;
            this.text = text;
            this.image = image;
        }

        /// <summary>
        /// Create an OptionData Element with passed data.
        /// </summary>
        /// <param name="text">Text to set in the element</param>
        /// <param name="image">Image to set in the element</param>
        public OptionData(string text, Sprite image)
        {
            this.text = text;
            this.image = image;
        }
    };

    [System.Serializable]
    public class Init
    {
        [Tooltip("If checked, the options list below will update the Dropdown Options list when first entering Play Mode (on Awake). This can be ignored (left unchecked) for manual initialization.")]
        public bool initalizeOnAwake;
        [Tooltip("If checked, the options list below will update the Dropdown Options list in the Editor (outside Play Mode). It will also prevent Headers from being selected as the first element.")]
        public bool updateInEditor;
        [Tooltip("The color to apply to Special Elements, such as Headers and (optionally) Splitters.")]
        public Color specialColor = Color.white;
        [Tooltip("Should the Special Color be applied to the Splitter image, as well as the Header text?")]
        public bool applyColorToSplitters;
        [Tooltip("List all elements to be created for initialization.")]
        public List<OptionData> options;
    };

    public enum DropdownDirection { Below, Above, Left, Right };
    public enum OptionType { Normal, Header, Splitter };

    #region Private Functions
    void OnValidate()
    {
            if (dropdown == null) { dropdown = GetComponent<Dropdown>(); }
            if (toggle == null) { toggle = transform.Find("Template").GetComponentInChildren<Toggle>(); }
            if (toggleRT == null) { toggleRT = toggle.GetComponent<RectTransform>(); }
            if (image == null) { image = toggle.transform.GetChild(0).GetComponent<Image>(); }
            if (text == null) { text = toggle.transform.GetChild(2).GetComponent<Text>(); }
            if (scrollRect == null) { scrollRect = transform.Find("Template").GetComponent<ScrollRect>(); }

            if (initalization.updateInEditor)
            {
                SetOptions(initalization.options);
                if (dropdown.value == 0 && dropdown.options.Count > 1 && dropdown.options[0].text.EndsWith("[h]")) { dropdown.value = 1; }
            }

            if (elementHeight <= 0f)
            {
                elementHeight = toggleRT.rect.height;
            }
            else
            {
                RectTransform element = toggle.transform.parent.GetComponent<RectTransform>();

                element.anchoredPosition = Vector2.zero;
                element.sizeDelta = new Vector2(element.sizeDelta.x, elementHeight);
                transform.Find("Template").GetComponent<ScrollRect>().scrollSensitivity = elementHeight;
            }

            if (theme.fontData.font == null)
            {
                theme.fontData.alignByGeometry = text.alignByGeometry;
                theme.fontData.alignment = text.alignment;
                theme.fontData.bestFit = text.resizeTextForBestFit;
                theme.fontData.font = text.font;
                theme.fontData.fontSize = text.fontSize;
                theme.fontData.fontStyle = text.fontStyle;
                theme.fontData.horizontalOverflow = text.horizontalOverflow;
                theme.fontData.lineSpacing = text.lineSpacing;
                theme.fontData.maxSize = text.resizeTextMaxSize;
                theme.fontData.minSize = text.resizeTextMinSize;
                theme.fontData.richText = text.supportRichText;
                theme.fontData.verticalOverflow = text.verticalOverflow;
            }
            else
            {
                text.alignByGeometry = theme.fontData.alignByGeometry;
                text.alignment = theme.fontData.alignment;
                text.resizeTextForBestFit = theme.fontData.bestFit;
                text.font = theme.fontData.font;
                text.fontSize = theme.fontData.fontSize;
                text.fontStyle = theme.fontData.fontStyle;
                text.horizontalOverflow = theme.fontData.horizontalOverflow;
                text.lineSpacing = theme.fontData.lineSpacing;
                text.resizeTextMaxSize = theme.fontData.maxSize;
                text.resizeTextMinSize = theme.fontData.minSize;
                text.supportRichText = theme.fontData.richText;
                text.verticalOverflow = theme.fontData.verticalOverflow;
            }

            colors = dropdown.colors;

            if (theme.selection == Color.clear)
            {
                theme.selection = toggle.colors.normalColor;
            }
            else { colors.normalColor = theme.selection; dropdown.colors = colors; }

            if (theme.element == Color.clear)
            {
                theme.element = image.color;
            }
            else { image.color = theme.element; }

            colors = toggle.colors;

            if (theme.highlight == Color.clear)
            {
                theme.highlight = toggle.colors.highlightedColor;
            }
            else { colors.highlightedColor = theme.highlight; toggle.colors = colors; }

            if (theme.pressed == Color.clear)
            {
                theme.pressed = toggle.colors.pressedColor;
            }
            else { colors.pressedColor = theme.pressed; toggle.colors = colors; }

            if (theme.text == Color.clear)
            {
                theme.text = text.color;
            }
            else { text.color = theme.text; }

            colors = scrollRect.verticalScrollbar.colors;

            if (theme.scrollbar == Color.clear)
            {
                theme.scrollbar = scrollRect.verticalScrollbar.colors.normalColor;
                theme.scrollSpace = theme.scrollbar * 0.45f;
                theme.scrollSpace.a = theme.scrollbar.a;
            }
            else
            {
                colors.normalColor = theme.scrollbar;
                scrollRect.verticalScrollbar.colors = colors;

                if (theme.dimWithScrollbar)
                {
                    theme.scrollSpace = theme.scrollbar * 0.45f;
                    theme.scrollSpace.a = theme.scrollbar.a;
                }

                scrollRect.verticalScrollbar.transform.GetComponent<Image>().color = theme.scrollSpace;
            }
    }

    void Awake()
    {
        dropdown = GetComponent<Dropdown>();
        transform.Find("Template").GetComponent<Image>().enabled = false; //hide background of dropdown list

        if (initalization.initalizeOnAwake)
        {
            SetOptions(initalization.options);
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == dropdown.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(dropdown.transform.GetChild(0).gameObject);
            
            Invoke("UpdateDropdown", 0.15f); //delay update to allow Dropdown List to be created by Dropdown event (Unity)
        }

        //prevent headers from appearing as default on selection 0
        if (dropdown.value == 0 && dropdown.options[0].text.EndsWith("[h]") && dropdown.options.Count > 1) { dropdown.value = 1; }
    }

    void UpdateDropdown()
    {
        RectTransform dropdownDisplay = null;
        try
        {
            dropdownDisplay = dropdown.transform.Find("Dropdown List").GetComponent<RectTransform>();
        }
        catch { UpdateDropdown(); return; }

        if (dropdownDisplay != null)
        {
            //direction
            Vector2 containerSize = GetComponent<RectTransform>().rect.size;
            float dropdownHeight = dropdownDisplay.rect.height;
            if(direction == DropdownDirection.Below) { dropdownDisplay.anchoredPosition = new Vector2(0f, 2f); }
            else if (direction == DropdownDirection.Above) { dropdownDisplay.anchoredPosition = new Vector2(0f, (containerSize.y + dropdownHeight) - 2f); }
            else if (direction == DropdownDirection.Left) { dropdownDisplay.anchoredPosition = new Vector2(-containerSize.x + 2f, containerSize.y); }
            else if (direction == DropdownDirection.Right) { dropdownDisplay.anchoredPosition = new Vector2(containerSize.x - 2f, containerSize.y); }

            for (int i = 1; i < dropdownDisplay.GetChild(0).GetChild(0).childCount; i++)
            {
                Transform child = dropdownDisplay.GetChild(0).GetChild(0).GetChild(i);

                //headers
                if (child.name.EndsWith("[h]"))
                {
                    child.GetComponentInChildren<Text>().text = child.GetComponentInChildren<Text>().text.Replace("[h]", string.Empty);
                    child.GetComponent<Toggle>().interactable = false;
                    ColorBlock colors = child.GetComponent<Toggle>().colors;
                    colors.normalColor = initalization.specialColor;
                    colors.disabledColor = colors.normalColor;
                    child.GetComponent<Toggle>().colors = colors;
                }
                //splitters
                else if (child.name.EndsWith("[s]"))
                {
                    if (child.childCount == 4) { child.GetChild(3).gameObject.SetActive(false); }
                    child.GetChild(0).GetComponent<Image>().sprite = dropdown.options[i - 1].image;
                    child.GetChild(0).GetComponent<Image>().color = Color.white;

                    child.GetComponentInChildren<Text>().text = string.Empty;
                    child.GetComponent<Toggle>().interactable = false;
                    ColorBlock colors = child.GetComponent<Toggle>().colors;
                    if (initalization.applyColorToSplitters) { colors.normalColor = initalization.specialColor; }
                    colors.disabledColor = colors.normalColor;
                    child.GetComponent<Toggle>().colors = colors;
                }

                //checkmarks... Check
                child.GetChild(1).GetComponent<Image>().enabled = !hideCheckmark;

                //thumbnails
                if (child.childCount == 4 && useThumbnailImage)
                {
                    Vector2 pos = child.GetChild(2).GetComponent<RectTransform>().offsetMax;
                    pos.x -= child.GetChild(3).GetComponent<RectTransform>().rect.width;
                    child.GetChild(2).GetComponent<RectTransform>().offsetMax = pos;
                }
            }

            if (keepSelectionHighlighted)
            {
                Toggle dropdownElement = dropdownDisplay.GetChild(0).GetChild(0).GetChild(dropdown.value + 1).GetComponent<Toggle>();
                ColorBlock colors = dropdownElement.colors;
                colors.normalColor = colors.pressedColor;
                dropdownElement.colors = colors;
            }
        }
    }
    #endregion

    #region Common Functions
    /// <summary>
    /// Add a Header element to the Options List of this Dropdown.
    /// Headers are considered "Special Elements" and are not selectable in the Dropdown.
    /// </summary>
    /// <param name="text"></param>
    public void AddHeader(string text)
    {
        dropdown.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData(text + "[h]") });
    }

    /// <summary>
    /// Add a Splitter image element to the Options List of this Dropdown.
    /// Splitters are considered "Special Elements" and are not selectable in the Dropdown.
    /// </summary>
    /// <param name="splitter"></param>
    public void AddSplitter(Sprite splitter)
    {
        dropdown.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData("[s]", splitter) });
    }

    /// <summary>
    /// Add an Item as an element to the Options List of this Dropdown.
    /// </summary>
    /// <param name="data">The data to pull information from for this element. Only the text and/or image is read.</param>
    public void AddItem(OptionData data)
    {
        dropdown.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData(data.text, data.image) });
    }

    /// <summary>
    /// Add multiple elements to the Options List of this Dropdown.
    /// This method allows you to add Special Elements (Headers or Splitters) in addition to regular text/image provided by Dropdown.OptionData already.
    /// </summary>
    /// <param name="list">List of information to read from, and add as elements.</param>
    public void AddOptions(List<OptionData> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if(list[i].type == OptionType.Header) { AddHeader(list[i].text); }
            else if (list[i].type == OptionType.Splitter) { AddSplitter(list[i].image); }
            else if (list[i].type == OptionType.Normal) { AddItem(list[i]); }
        }
    }

    /// <summary>
    /// Set the Options List in this Dropdown to the list of elements provided.
    /// This method allows you to add Special Elements (Headers or Splitters) in addition to regular text/image provided by Dropdown.OptionData already.
    /// </summary>
    /// <param name="list">List of information to read from, and add as elements. This will be the list that Options will be set to.</param>
    public void SetOptions(List<OptionData> list)
    {
        dropdown.options.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].type == OptionType.Header) { AddHeader(list[i].text); }
            else if (list[i].type == OptionType.Splitter) { AddSplitter(list[i].image); }
            else if (list[i].type == OptionType.Normal) { AddItem(list[i]); }
        }
    }
    #endregion
}
