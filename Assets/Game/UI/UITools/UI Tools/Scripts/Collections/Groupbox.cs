using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Groupbox UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Groupbox", 1), DisallowMultipleComponent]
public class Groupbox : MonoBehaviour
{

    [Tooltip("(Optional) the text to display in the header of this groupbox. Leave blank to not include a header.")]
    public string header;

    [Tooltip("The alignment positioning of the header to the left, right, or center of the groupbox. Stacks with the Text properties and most RectTransform properties.")]
    public HeaderAlignment alignment = HeaderAlignment.Center;
    [Tooltip("(Optional) additional offset to the alignment of the header.")]
    public Vector2 offset;

    [Space]

    [Tooltip("The Text object of the Header itself. This should exist inside the groubox.")]
    public Text headerComponent;
    [Tooltip("The Transform object that all gameobjects in the groupbox will be validated. This should exist inside the groupbox.")]
    public Transform contentsContainer;

    public enum HeaderAlignment { Left, Center, Right };

    //all children of the contents, stored
    private List<GameObject> contents = new List<GameObject>();

    void Start()
    {
        contents.Clear();
        UpdateHeader();
    }

    #region Public Functions
    /// <summary>
    /// Update the text and position changes of the Header text.
    /// </summary>
    public void UpdateHeader()
    {
        if (headerComponent != null)
        {
            headerComponent.text = header;
            RepositionHeader();
        }
    }

    /// <summary>
    /// Gets all the gameobjects inside the groupbox's "ContentsContainer" child.
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetContents()
    {
        return contents;
    }
    #endregion

    #region Private Functions
    void RepositionHeader()
    {
        RectTransform headerRect = headerComponent.GetComponent<RectTransform>();
        float side = 2f;

        if (alignment == HeaderAlignment.Center)
        {
            headerRect.anchorMin = new Vector2(0.5f, headerRect.anchorMin.y);
            headerRect.anchorMax = new Vector2(0.5f, headerRect.anchorMax.y);
            side = 0f;
        }
        else if (alignment == HeaderAlignment.Left)
        {
            headerRect.anchorMin = new Vector2(0f, headerRect.anchorMin.y);
            headerRect.anchorMax = new Vector2(0f, headerRect.anchorMax.y);
            side = headerRect.rect.width / 2f;
        }
        else //Right
        {
            headerRect.anchorMin = new Vector2(1f, headerRect.anchorMin.y);
            headerRect.anchorMax = new Vector2(1f, headerRect.anchorMax.y);
            side = headerRect.rect.width / -2f;
        }

        headerRect.anchoredPosition = new Vector2(side + offset.x, headerRect.anchoredPosition.y + offset.y);
    }

    private void OnTransformChildrenChanged()
    {
        contents.Clear();
        for (int i = 0; i < contentsContainer.childCount; i++)
        {
            contents.Add(contentsContainer.GetChild(i).gameObject);
        }
    }
    #endregion
}
