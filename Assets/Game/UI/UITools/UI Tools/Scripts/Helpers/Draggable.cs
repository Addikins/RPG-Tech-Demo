using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Draggable UI Tool - Extention Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Extentions/Draggable", 0), DisallowMultipleComponent]
public class Draggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler
{
    [Tooltip("The only space dragging of this object is allowed. If left empty, then anywhere is considered drag space.")]
    public RectTransform dragSpace;
    [Tooltip("Prevents the dragging object from leaving the canvas.")]
    public bool screenBound;
    [Tooltip("The canvas to keep the dragging object inside of.")]
    public Canvas canvas;

    private List<GameObject> hoveredElements;
    private RectTransform hovered;
    private Vector3 startPoint;

    private RectTransform rect;
    private RectTransform canvasBounds;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        if(canvas == null) { canvas = GetComponent<MaskableGraphic>().canvas; }
        canvasBounds = canvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (dragSpace != null && hoveredElements.Contains(dragSpace.gameObject))
        {
            for(int i = 0; i < hoveredElements.Count; i++)
            {
                if(hoveredElements[i] == dragSpace.gameObject) { hovered = hoveredElements[i].GetComponent<RectTransform>(); }
            }
        }
        else { hovered = null; }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredElements = eventData.hovered;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragSpace == null) { startPoint = Vector2.zero; }
        else { startPoint = dragSpace.localPosition; }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragSpace == null)
        {
            AllowDrag();
        }
        else
        {
            if (hovered != null && hovered == dragSpace)
            {
                AllowDrag();
            }
        }
    }

    void AllowDrag()
    {
        Vector3 pos = Input.mousePosition;
        Vector3 finalPos;

        pos.z = rect.position.z;
        finalPos = pos - startPoint;

        if (screenBound)
        {
            finalPos.x = Mathf.Clamp(finalPos.x, rect.rect.width / 2f, canvasBounds.rect.width - (rect.rect.width / 2f));
            finalPos.y = Mathf.Clamp(finalPos.y, rect.rect.height / 2f, canvasBounds.rect.height - (rect.rect.height / 2f));
        }

        rect.position = finalPos;
    }
}