using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Tooltip UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Extentions/Tooltip", 1), DisallowMultipleComponent]
public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    /// <summary>
    /// Allows any UI object to accept a tooltip.
    /// </summary>

    #region Variables
    [Tooltip("The tooltip prefab to use as a template. Depending on the prefab used, the style, formatting, and preferences below may not all be applicable.")]
    public TooltipModal tooltipTarget;
    [Tooltip("The image to display next to the header of the tooltip. This will have no effect on Tooltip Targets without an icon.")]
    public Sprite icon;
    [Tooltip("The title to display in the header of the tooltip. This will have no effect on Tooltip Targets without a header.")]
    public string header;

    [TextArea(3, 5), Tooltip("The block of text to display in the body of the tooltip.")]
    public string text;

    [Space, Tooltip("Determines if the tooltip will follow the mouse on the object or stay in a fixed position.")]
    public bool followMouse;
    [Tooltip("Determines if the tooltip will only be shown if the object it belongs to is clicked rather than hovered.")]
    public bool displayOnClick;
    [Tooltip("Determines the default fixed positioning of the tooltip, relative to the object.")]
    public TooltipAlignment alignment = TooltipAlignment.Right;
    [Tooltip("Additional positioning to add to the placement of the tooltip. No offset will place the top-left corner of the tooltip to the cursor or object.")]
    public Vector2 offset;

    [Space, Tooltip("Events to fire when the mouse enters the object, and the tooltip is shown.")]
    public UnityEvent onMouseEnter;
    [Tooltip("Events to fire when the mouse leaves the object, and the tooltip is hidden.")]
    public UnityEvent onMouseExit;
    [Tooltip("Events to fire when the mouse clicks the object, and the tooltip is shown. These events will only be fired when displayOnClick is on.")]
    public UnityEvent onMouseClick;

    private static bool isValidated;
    private RectTransform rectTransform;
    #endregion

    private void Awake()
    {
        if (isValidated == false)
        {
            if (tooltipTarget == null) { Debug.LogError("Tooltip Target is not set in the Inspector. A ToolipModal prefab must be assigned for this tooltip to function.", this); return; }
            else { if (!tooltipTarget.gameObject.activeInHierarchy) { tooltipTarget = Instantiate(tooltipTarget, transform.parent); Debug.LogWarning("The prefab TooltipModal was used instead of an instance. An instance of the prefab has been created and assigned."); } }

            if (tooltipTarget.gameObject.activeInHierarchy) { tooltipTarget.HideTooltip(); isValidated = true; }
            else { tooltipTarget.gameObject.SetActive(true); tooltipTarget.HideTooltip(); isValidated = true; }

            rectTransform = GetComponent<RectTransform>();
        }
    }

    #region Mouse Events
    public void OnPointerClick(PointerEventData eventData)
    {
        if (displayOnClick && tooltipTarget != null)
        {
            tooltipTarget.SetTarget(GetComponent<RectTransform>());
            tooltipTarget.ShowTooltip(text, header, icon, followMouse, offset, alignment, rectTransform);
            if (onMouseClick != null) { onMouseClick.Invoke(); }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!displayOnClick && tooltipTarget != null)
        {
            tooltipTarget.SetTarget(GetComponent<RectTransform>());
            tooltipTarget.ShowTooltip(text, header, icon, followMouse, offset, alignment, rectTransform);
            if (onMouseEnter != null) { onMouseEnter.Invoke(); }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipTarget != null)
        {
            tooltipTarget.HideTooltip();
            if (onMouseExit != null) { onMouseExit.Invoke(); }
        }
    }
    #endregion
}

public enum TooltipAlignment { Default, Left, Right, Above, Below };
