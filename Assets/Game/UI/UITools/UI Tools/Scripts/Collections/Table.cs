using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Table UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Table", 5), DisallowMultipleComponent]
public class Table : MonoBehaviour
{
    [Header("Templates")]
    public GridLayoutGroup header_Template;
    public GridLayoutGroup content_Template;
    public HorizontalLayoutGroup headerContainer_Template;
    public VerticalLayoutGroup contentContainer_Template;
    public LayoutElement contentContainerItemElement_Template;
    public ScrollRect contentContainerScrollView_Template;
    
    [Space]
    [Header("Preferences")]

    [Tooltip("The number of colums to create.")]
    public int columns = 2;
    [Tooltip("Should the contents of a colum exceed its height, that column will become scrollable.")]
    public bool scrollColumns;

    private List<GameObject> col_headers = new List<GameObject>();
    private List<GameObject> col_contents = new List<GameObject>();
    private float tableWidth;

    #region Initalization
    void OnDrawGizmos()
    {
        if (header_Template != null && content_Template != null) { RecalculateWidth(); }
    }

    // Use this for initialization
    void Start()
    {
        headerContainer_Template.gameObject.SetActive(false);
        contentContainer_Template.gameObject.SetActive(false);
        headerContainer_Template.transform.SetParent(transform);
        contentContainer_Template.transform.SetParent(transform);
        ClearTable();

        RecalculateWidth();

        for (int i = 0; i < columns; i++)
        {
            GameObject header = (GameObject)Instantiate(headerContainer_Template.gameObject);
            header.name = "[Header " + (i + 1) + " Container]";
            header.transform.SetParent(header_Template.transform);
            header.SetActive(true);
            col_headers.Add(header);

            if (scrollColumns)
            {
                GameObject content = (GameObject)Instantiate(contentContainer_Template.gameObject);
                GameObject scrollContent = (GameObject)Instantiate(contentContainerScrollView_Template.gameObject);
                content.name = "Column " + (i + 1);
                scrollContent.name = "Column " + (i + 1) + " Scrollable";
                Destroy(content.GetComponent<VerticalLayoutGroup>());
                scrollContent.transform.GetChild(0).GetChild(0).gameObject.AddComponent<VerticalLayoutGroup>();
                scrollContent.transform.GetChild(0).GetChild(0).gameObject.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
                scrollContent.transform.SetParent(content.transform);
                scrollContent.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                scrollContent.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
                scrollContent.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                scrollContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                scrollContent.transform.SetAsFirstSibling();
                content.transform.SetParent(content_Template.transform);
                content.SetActive(true);
                scrollContent.SetActive(true);
                ClearColumn(content.GetComponent<VerticalLayoutGroup>());
                col_contents.Add(scrollContent);
            }
            else
            {
                GameObject content = (GameObject)Instantiate(contentContainer_Template.gameObject);
                content.name = "Column " + (i + 1);
                content.transform.SetParent(content_Template.transform);
                content.SetActive(true);
                ClearColumn(content.GetComponent<VerticalLayoutGroup>());
                col_contents.Add(content);
            }

        }
    }

    void ClearTable()
    {
        for (int i = 1; i < header_Template.transform.childCount; i++) { Destroy(header_Template.transform.GetChild(i).gameObject); }
        for (int i = 1; i < content_Template.transform.childCount; i++) { Destroy(content_Template.transform.GetChild(i).gameObject); }
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Recalculate the total width of the table, which will also re-adjust table columns widths, based on 'columns' count.
    /// </summary>
    public void RecalculateWidth()
    {
        header_Template.constraintCount = columns;
        content_Template.constraintCount = columns;

        tableWidth = GetComponent<RectTransform>().rect.width;
        header_Template.cellSize = new Vector2(tableWidth / columns, header_Template.GetComponent<RectTransform>().rect.height);
        content_Template.cellSize = new Vector2(tableWidth / columns, content_Template.GetComponent<RectTransform>().rect.height);
    }

    /// <summary>
    /// Add a column to the table, with default paramaters.
    /// </summary>
    public void AddColumn()
    {
        columns++;
        RecalculateWidth();

        GameObject header = (GameObject)Instantiate(headerContainer_Template.gameObject);
        header.name = "[Header " + (col_headers.Count + 1) + " Container]";
        header.transform.SetParent(header_Template.transform);
        header.SetActive(true);
        col_headers.Add(header);

        if (scrollColumns)
        {
            GameObject content = (GameObject)Instantiate(contentContainer_Template.gameObject);
            GameObject scrollContent = (GameObject)Instantiate(contentContainerScrollView_Template.gameObject);
            content.name = "Column " + (col_contents.Count + 1);
            scrollContent.name = "Column " + (col_contents.Count + 1) + " Scrollable";
            Destroy(content.GetComponent<VerticalLayoutGroup>());
            scrollContent.transform.GetChild(0).GetChild(0).gameObject.AddComponent<VerticalLayoutGroup>();
            scrollContent.transform.GetChild(0).GetChild(0).gameObject.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
            scrollContent.transform.SetParent(content.transform);
            scrollContent.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            scrollContent.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            scrollContent.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            scrollContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            scrollContent.transform.SetAsFirstSibling();
            content.transform.SetParent(content_Template.transform);
            content.SetActive(true);
            scrollContent.SetActive(true);
            ClearColumn(content.GetComponent<VerticalLayoutGroup>());
            col_contents.Add(scrollContent);
        }
        else
        {
            GameObject content = (GameObject)Instantiate(contentContainer_Template.gameObject);
            content.name = "Column " + (col_contents.Count + 1);
            content.transform.SetParent(content_Template.transform);
            content.SetActive(true);
            ClearColumn(content.GetComponent<VerticalLayoutGroup>());
            col_contents.Add(content);
        }
    }

    /// <summary>
    /// Add a column to the table, with a specified name.
    /// </summary>
    /// <param name="name">The name of this colmn to set, when created.</param>
    public void AddColumn(string name)
    {
        columns++;
        RecalculateWidth();

        GameObject header = (GameObject)Instantiate(headerContainer_Template.gameObject);
        header.name = "[Header " + (col_headers.Count + 1) + " Container]";
        header.GetComponentInChildren<Text>().text = name;
        header.transform.SetParent(header_Template.transform);
        header.SetActive(true);
        col_headers.Add(header);

        if (scrollColumns)
        {
            GameObject content = (GameObject)Instantiate(contentContainer_Template.gameObject);
            GameObject scrollContent = (GameObject)Instantiate(contentContainerScrollView_Template.gameObject);
            content.name = "Column " + (col_contents.Count + 1);
            scrollContent.name = "Column " + (col_contents.Count + 1) + " Scrollable";
            Destroy(content.GetComponent<VerticalLayoutGroup>());
            scrollContent.transform.GetChild(0).GetChild(0).gameObject.AddComponent<VerticalLayoutGroup>();
            scrollContent.transform.GetChild(0).GetChild(0).gameObject.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
            scrollContent.transform.SetParent(content.transform);
            scrollContent.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            scrollContent.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            scrollContent.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            scrollContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            scrollContent.transform.SetAsFirstSibling();
            content.transform.SetParent(content_Template.transform);
            content.SetActive(true);
            scrollContent.SetActive(true);
            ClearColumn(content.GetComponent<VerticalLayoutGroup>());
            col_contents.Add(scrollContent);
        }
        else
        {
            GameObject content = (GameObject)Instantiate(contentContainer_Template.gameObject);
            content.name = "Column " + (col_contents.Count + 1);
            content.transform.SetParent(content_Template.transform);
            content.SetActive(true);
            ClearColumn(content.GetComponent<VerticalLayoutGroup>());
            col_contents.Add(content);
        }
    }

    /// <summary>
    /// Remove the specified column from the table, and all its children.
    /// </summary>
    /// <param name="col">The column to remove.</param>
    public void RemoveColumn(VerticalLayoutGroup col)
    {
        columns--;
        RecalculateWidth();

        int index = col.transform.GetSiblingIndex();
        GameObject header, contents;
        header = col_headers[index];
        contents = col_contents[index];
        col_contents.RemoveAt(index);
        col_headers.RemoveAt(index);
        Destroy(header);
        Destroy(contents);
    }

    /// <summary>
    /// Remove the specified column from the table, and all its children.
    /// </summary>
    /// <param name="col">The column to remove.</param>
    public void RemoveColumn(GameObject col)
    {
        columns--;
        RecalculateWidth();

        int index = col.transform.GetSiblingIndex();
        GameObject header, contents;
        header = col_headers[index];
        contents = col_contents[index];
        col_contents.RemoveAt(index);
        col_headers.RemoveAt(index);
        Destroy(header);
        Destroy(contents);
    }

    /// <summary>
    /// Remove the specified column from the table, and all its children, by the columns index.
    /// </summary>
    /// <param name="col_index">The index of the column to remove.</param>
    public void RemoveColumn(int col_index)
    {
        columns--;
        RecalculateWidth();
        
        GameObject header, contents;
        header = col_headers[col_index];
        contents = col_contents[col_index];
        col_contents.RemoveAt(col_index);
        col_headers.RemoveAt(col_index);
        Destroy(header);
        Destroy(contents);
    }

    /// <summary>
    /// Remove all children of the specified column.
    /// </summary>
    /// <param name="col">The column to clear the contents of.</param>
    public void ClearColumn(VerticalLayoutGroup col)
    {
        for (int i = 0; i < col.transform.childCount; i++)
        {
            if (scrollColumns && i == 0) { i = 1; }
            Destroy(col.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Remove all children of the specified column.
    /// </summary>
    /// <param name="col">The column to clear the contents of.</param>
    public void ClearColumn(GameObject col)
    {
        for (int i = 0; i < col.transform.childCount; i++)
        {
            if (scrollColumns && i == 0) { i = 1; }
            Destroy(col.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Remove all children of the specified column, by the columns index.
    /// </summary>
    /// <param name="col_index">The index of the column to clear the contents of.</param>
    public void ClearColumn(int col_index)
    {
        GameObject col = col_contents[col_index];
        for (int i = 0; i < col.transform.childCount; i++)
        {
            if (scrollColumns && i == 0) { i = 1; }
            Destroy(col.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Add an item to the specified column.
    /// </summary>
    /// <param name="item">The item to add to the column.</param>
    /// <param name="col">The column to add the item to.</param>
    public void AddToColumn(GameObject item, VerticalLayoutGroup col)
    {
        GameObject obj = (GameObject)Instantiate(contentContainerItemElement_Template.gameObject);
        obj.transform.SetParent(col.transform);
        item.transform.SetParent(obj.transform);
        item.transform.position = Vector3.zero;
    }

    /// <summary>
    /// Add an item to the specified column.
    /// </summary>
    /// <param name="item">The item to add to the column.</param>
    /// <param name="col">The column to add the item to.</param>
    public void AddToColumn(GameObject item, GameObject col)
    {
        GameObject obj = (GameObject)Instantiate(contentContainerItemElement_Template.gameObject);
        obj.transform.SetParent(col.transform);
        item.transform.SetParent(obj.transform);
        item.transform.position = Vector3.zero;

        if (obj.transform.parent.name == "Content") { obj.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(obj.transform.parent.GetComponent<RectTransform>().sizeDelta.x, obj.transform.parent.GetChild(0).GetComponent<LayoutElement>().preferredHeight * obj.transform.parent.childCount); }
    }

    /// <summary>
    /// Add an item to the specified column, by the columns index.
    /// </summary>
    /// <param name="item">The item to add to the column.</param>
    /// <param name="col_index">The index of the column to add the item to.</param>
    public void AddToColumn(GameObject item, int col_index)
    {
        GameObject obj = (GameObject)Instantiate(contentContainerItemElement_Template.gameObject);
        obj.transform.SetParent(col_contents[col_index].transform);
        item.transform.SetParent(obj.transform);
        item.transform.position = Vector3.zero;

        if (obj.transform.parent.name == "Content") { obj.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(obj.transform.parent.GetComponent<RectTransform>().sizeDelta.x, obj.transform.parent.GetChild(0).GetComponent<LayoutElement>().preferredHeight * obj.transform.parent.childCount); }
    }

    /// <summary>
    /// Remove an item from its column.
    /// </summary>
    /// <param name="item">The item to remove from the table.</param>
    public void RemoveFromColumn(GameObject item)
    {
        Destroy(item);
    }

    /// <summary>
    /// Remove an item by the items index, from the specified column.
    /// </summary>
    /// <param name="item_index">The index of the item to remove from the column.</param>
    /// <param name="col">The column that the item belongs to.</param>
    public void RemoveFromColumn(int item_index, VerticalLayoutGroup col)
    {
        GameObject item = col.transform.GetChild(item_index).gameObject;
        Destroy(item);
    }

    /// <summary>
    /// Remove an item by the items index, from the specified column
    /// </summary>
    /// <param name="item_index">The index of the item to remove from the column.</param>
    /// <param name="col">The column that the item belongs to.</param>
    public void RemoveFromColumn(int item_index, GameObject col)
    {
        GameObject item = col.transform.GetChild(item_index).gameObject;
        Destroy(item);
    }

    /// <summary>
    /// Remove an item by the items index, from the specified column, by the columns index.
    /// </summary>
    /// <param name="item_index">The index of the item to remove from the column.</param>
    /// <param name="col_index">The index of the column that the item belongs to, for removal.</param>
    public void RemoveFromColumn(int item_index, int col_index)
    {
        GameObject item = col_contents[col_index].transform.GetChild(item_index).gameObject;
        Destroy(item);
    }

    /// <summary>
    /// Get the column that the specified item belongs to.
    /// </summary>
    /// <param name="item">The item to find the column of.</param>
    public GameObject GetColumnOf(GameObject item)
    {
        return col_contents[item.transform.parent.GetSiblingIndex()];
    }

    /// <summary>
    /// Get the header/column index that the specified item belongs to. The header will always be the same as the column contents.
    /// </summary>
    /// <param name="item">The item to find the column of.</param>
    public int GetHeaderColumnIndexOf(GameObject item)
    {
        return item.transform.parent.GetSiblingIndex();
    }

    /// <summary>
    /// Get the header that the specified item belongs to.
    /// </summary>
    /// <param name="item">The item to find the header of.</param>
    public GameObject GetHeaderOf(GameObject item)
    {
        return col_headers[item.transform.parent.GetSiblingIndex()];
    }

    /// <summary>
    /// Get a header by index.
    /// </summary>
    /// <param name="index">The index of the header to retrieve.</param>
    public GameObject GetHeader(int index)
    {
        return col_headers[index];
    }

    /// <summary>
    /// Get a column contents by index.
    /// </summary>
    /// <param name="index">The index of the column to retrieve.</param>
    public GameObject GetColumn(int index)
    {
        return col_contents[index];
    }
    #endregion
}
