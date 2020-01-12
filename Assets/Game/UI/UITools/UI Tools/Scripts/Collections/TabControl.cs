using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// TabControl UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Tab Control", 6), DisallowMultipleComponent]
public class TabControl : MonoBehaviour
{
    public Transform tabsContainer, contentContainer;
    public GameObject tabTemplate, containerTemplate;
    public ScrollRect tabsScrollRect;

    [Tooltip("The method used when Tab buttons will overflow its parent.\nNone - no affects will be applied, and Overflow will happen.\nScale - Tabs will be resized to fit in the parent.\nScroll - A invisible scrollbar will be created with a mask, so Tabs will be scrollable on overflow.")]
    public TabOverflowMethod overflow = TabOverflowMethod.Scroll;
    [Tooltip("Dertmines if the selected tab will force the repositioning of the tabs container so the object is completely on screen. Only applicible if overflow is set to Scroll.")]
    public bool repositionOverflow = true;

    public List<TabData> tabs = new List<TabData>();

    [System.Serializable]
    public class TabData
    {
        public string tabTitle;
        public Transform tabContent;

        public Button tabButton;
    };

    private static TabData _selectedTab = null;

    public static TabData selectedTab
    {
        get { return _selectedTab; }
    }

    // Use this for initialization
    void Start()
    {
        _selectedTab = null;
        if(tabs.Count > 0)
        {
            List<TabData> tab = new List<TabData>();
            tab.AddRange(tabs);

            ClearTabs();

            for (int i = 0; i < tab.Count; i++)
            {
                AddTab(tab[i].tabTitle);
            }
        }
        
        if (tabs.Count > 0) { ShowTab(tabs[0]); }
    }

    #region Public Functions
    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    public void AddTab()
    {
        TabData tabData = new TabData();
        tabData.tabTitle = "Tab " + tabsContainer.childCount;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + tabsContainer.childCount;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = "Tab " + tabsContainer.childCount; }
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + contentContainer.childCount;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Add(tabData);
        ApplyOverflowMethod();
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="select">Selects the tab once its added</param>
    public void AddTab(bool select)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = "Tab " + tabsContainer.childCount;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + tabsContainer.childCount;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = "Tab " + tabsContainer.childCount; }
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + contentContainer.childCount;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Add(tabData);
        ApplyOverflowMethod();

        if (select) { SelectTab(tabData); }
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="name">Name to set for this tab</param>
    public void AddTab(string name)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = name;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + name;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = name; }
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + name;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Add(tabData);
        ApplyOverflowMethod();
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="name">Name to set for this tab</param>
    /// <param name="size">Width to set for this tab</param>
    public void AddTab(string name, float size = 150f)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = name;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + name;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = name; }
        btn.GetComponent<LayoutElement>().minWidth = (size <= 0f) ? 150f : size;
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + name;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Add(tabData);
        ApplyOverflowMethod();
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="name">Name to set for this tab</param>
    /// <param name="size">Width to set for this tab</param>
    /// <param name="select">Selects the tab once its added</param>
    public void AddTab(string name, float size = 150f, bool select = true)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = name;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + name;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = name; }
        btn.GetComponent<LayoutElement>().minWidth = (size <= 0f) ? 150f : size;
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + name;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Add(tabData);
        ApplyOverflowMethod();

        if (select) { SelectTab(tabData); }
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="name">Name to set for this tab</param>
    /// <param name="select">Selects the tab once its added</param>
    public void AddTab(string name, bool select = true)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = name;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + name;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = name; }
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + name;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Add(tabData);
        ApplyOverflowMethod();

        if (select) { SelectTab(tabData); }
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="atIndex">Index to insert this tab at</param>
    public void AddTab(int atIndex)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = name;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + name;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = name; }
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + name;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Insert(atIndex, tabData);
        ApplyOverflowMethod();
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="atIndex">Index to insert this tab at</param>
    /// <param name="select">Selects the tab once its added</param>
    public void AddTab(int atIndex, bool select)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = name;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + name;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = name; }
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + name;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Insert(atIndex, tabData);
        ApplyOverflowMethod();

        if (select) { SelectTab(tabData); }
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="name">Name to set for this tab</param>
    /// <param name="size">Width to set for this tab</param>
    /// <param name="atIndex">Index to insert this tab at</param>
    public void AddTab(string name, float size, int atIndex)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = name;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + name;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = name; }
        btn.GetComponent<LayoutElement>().minWidth = (size <= 0f) ? 150f : size;
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + name;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Insert(atIndex, tabData);
        ApplyOverflowMethod();
    }

    /// <summary>
    /// Add a tab to the Tab Control.
    /// </summary>
    /// <param name="name">Name to set for this tab</param>
    /// <param name="size">Width to set for this tab</param>
    /// <param name="atIndex">Index to insert this tab at</param>
    /// <param name="select">Selects the tab once its added</param>
    public void AddTab(string name, float size, int atIndex, bool select)
    {
        TabData tabData = new TabData();
        tabData.tabTitle = name;

        Button btn = Instantiate(tabTemplate).GetComponent<Button>();
        btn.name = "Tab: " + name;
        Text btnChild = btn.GetComponentInChildren<Text>();
        if (btnChild != null) { btnChild.text = name; }
        btn.GetComponent<LayoutElement>().minWidth = (size <= 0f) ? 150f : size;
        btn.gameObject.SetActive(true);
        int index = tabsContainer.childCount - 1;
        btn.onClick.AddListener(delegate { SwitchTabs(index); });
        btn.transform.SetParent(tabsContainer);

        RectTransform content = Instantiate(containerTemplate).GetComponent<RectTransform>();
        content.name = "Content: " + name;
        content.gameObject.SetActive(false);
        content.SetParent(contentContainer);
        content.transform.localScale = containerTemplate.transform.localScale;
        content.anchoredPosition3D = Vector3.zero;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;

        tabData.tabButton = btn;
        tabData.tabContent = content;

        tabs.Insert(atIndex, tabData);
        ApplyOverflowMethod();

        if (select) { SelectTab(tabData); }
    }

    /// <summary>
    /// Remove a tab by its index.
    /// </summary>
    /// <param name="index">Index of the tab to remove.</param>
    public void RemoveTab(int index)
    {
        if (index < tabs.Count - 1) { tabs.RemoveAt(index); Destroy(tabsContainer.GetChild(index).gameObject); Destroy(contentContainer.GetChild(index).gameObject); }
    }

    /// <summary>
    /// Remove a tab by its name.
    /// </summary>
    /// <param name="name">Name of the tab to remove.</param>
    public void RemoveTab(string name)
    {
        TabData data = FindTab(name);
        if (data != null) { tabs.Remove(data); Destroy(data.tabContent.gameObject); Destroy(data.tabButton.gameObject); }
    }

    /// <summary>
    /// Find a tab by its index.
    /// </summary>
    /// <param name="index">The index of the tab to find.</param>
    /// <returns></returns>
    public TabData FindTab(int index)
    {
        return (index < tabs.Count - 1) ? tabs[index] : null;
    }

    /// <summary>
    /// Find a tab by its name.
    /// </summary>
    /// <param name="name">The name of the tab to find.</param>
    /// <returns></returns>
    public TabData FindTab(string name)
    {
        for (int i = 0; i < tabs.Count; i++) { if (name.ToLower() == tabs[i].tabTitle.ToLower()) { return tabs[i]; } }
        return null;
    }

    /// <summary>
    /// Determines if the tab is locked in the Tab Control.
    /// </summary>
    /// <param name="index">The index of the control to toggle the lock state of.</param>
    /// <param name="unlock">If checked the tab will be locked. If false, it will be unlocked.</param>
    public void ToggleTabLock(int index, bool unlock)
    {
        if (index < tabs.Count - 1)
        {
            tabs[index].tabButton.interactable = unlock;
        }
    }

    /// <summary>
    /// Determines if the tab is locked in the Tab Control.
    /// </summary>
    /// <param name="name">The name of the control to toggle the lock state of.</param>
    /// <param name="unlock">If checked the tab will be locked. If false, it will be unlocked.</param>
    public void ToggleTabLock(string name, bool unlock)
    {
        TabData data = FindTab(name);
        if (data != null)
        {
            data.tabButton.interactable = unlock;
        }
    }

    /// <summary>
    /// Determines if the tab is visible in the Tab Control.
    /// </summary>
    /// <param name="index">Index of the tab to change visibility of.</param>
    /// <param name="show">If checked, the tab will be visible. If set to false, the tab will be hidden.</param>
    public void ToggleTabVisibility(int index, bool show)
    {
        if (index < tabs.Count - 1)
        {
            tabs[index].tabButton.gameObject.SetActive(show);
        }
    }

    /// <summary>
    /// Determines if the tab is visible in the Tab Control.
    /// </summary>
    /// <param name="name">Name of the tab to change visibility of.</param>
    /// <param name="show">If checked, the tab will be visible. If set to false, the tab will be hidden.</param>
    public void ToggleTabVisibility(string name, bool show)
    {
        TabData data = FindTab(name);
        if (data != null)
        {
            data.tabButton.gameObject.SetActive(show);
        }
    }

    /// <summary>
    /// Select a tab by its TabData.
    /// </summary>
    /// <param name="tab">The TabData of the tab.</param>
    public void SelectTab(TabData tab)
    {
        ShowTab(tab);
    }

    /// <summary>
    /// Select a tab by its index.
    /// </summary>
    /// <param name="tabIndex">The index of the tab.</param>
    public void SelectTab(int tabIndex)
    {
        if (tabIndex < tabs.Count - 1) { ShowTab(tabs[tabIndex]); }
    }

    /// <summary>
    /// Select a tab by its name.
    /// </summary>
    /// <param name="tabName">The name of the tab.</param>
    public void SelectTab(string tabName)
    {
        TabData data = FindTab(name);
        if (data != null)
        {
            ShowTab(data);
        }
    }

    /// <summary>
    /// Clear all tabs from the Tab Control.
    /// </summary>
    public void ClearTabs()
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            try { Destroy(tabsContainer.GetChild(i).gameObject); } catch { }
            try { Destroy(contentContainer.GetChild(i).gameObject); } catch { }
        }

        tabs.Clear();
    }

    /// <summary>
    /// Automatically called whenever a tab is added, but can be manually called if modified.
    /// </summary>
    public void ApplyOverflowMethod()
    {
        if (GetComponent<ScrollRect>())
        {
            float tabsWidth = 0f;
            for (int i = 0; i < tabs.Count; i++) { tabsWidth += tabs[i].tabButton.GetComponent<LayoutElement>().minWidth; }
            if (tabsWidth < contentContainer.GetComponent<RectTransform>().rect.width) { tabsWidth = contentContainer.GetComponent<RectTransform>().rect.width; }
            tabsContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(tabsWidth, tabsContainer.GetComponent<RectTransform>().sizeDelta.y);
        }

        if (overflow == TabOverflowMethod.Scale) { tabsContainer.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = true; }
        else { tabsContainer.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = false; }
    }

    /// <summary>
    /// Switch to the indexed tab.
    /// </summary>
    /// <param name="tabIndex">The index to switch to.</param>
    public void SwitchTabs(int tabIndex)
    {
        ShowTab(tabs[tabIndex]);
        if (repositionOverflow) { tabsScrollRect.horizontalNormalizedPosition = (tabIndex + 1) / tabs.Count; }
    }
    #endregion


    void ShowTab(TabData tab)
    {
        if (selectedTab != null)
        {
            selectedTab.tabButton.GetComponent<Image>().color = selectedTab.tabButton.colors.normalColor;
        }

        for (int i = 0; i < tabs.Count; i++) { tabs[i].tabContent.gameObject.SetActive(false); }
        tab.tabContent.gameObject.SetActive(true);
        _selectedTab = tab;
        selectedTab.tabButton.GetComponent<Image>().color = selectedTab.tabButton.colors.pressedColor;
    }
}

public enum TabOverflowMethod { None, Scale, Scroll };