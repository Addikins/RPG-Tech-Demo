using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tooltip UI Tool - Modal
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[DisallowMultipleComponent]
public class TooltipModal : MonoBehaviour
{

    /// <summary>
    /// Sets up the paramaters on the Tooltip prefab itself.
    /// </summary>

    public Image icon;
    public Text header, body;

    private RectTransform target;
    private bool followMouse;
    private RectTransform self;
    private Vector3 size, offset;
    private TooltipAlignment alignment;

    void Awake()
    {
        self = GetComponent<RectTransform>();
    }

    void Update()
    {
        SetPosition();
    }

    void SetPosition()
    {
        if (followMouse)
        {
            transform.position = Input.mousePosition + offset;
        }
        else
        {
            if (target != null)
            {
                if (alignment == TooltipAlignment.Default) { size = Vector2.zero; }
                else if (alignment == TooltipAlignment.Above) { size = new Vector2(0f, target.rect.height / 2f); }
                else if (alignment == TooltipAlignment.Below) { size = new Vector2(0f, -target.rect.height / 2f); }
                else if (alignment == TooltipAlignment.Left) { size = new Vector2(-target.rect.width / 2f, target.rect.height / 2f); }
                else if (alignment == TooltipAlignment.Right) { size = new Vector2(target.rect.width / 2f, target.rect.height / 2f); }

                transform.position = target.position + size + offset;
            }
            else { transform.position = offset; }
        }
    }

    #region Public Functions
    /// <summary>
    /// Update the data of the tooltip with the provided source data.
    /// </summary>
    /// <param name="source">Tooltip object to pull the data from.</param>
    public void UpdateTooltip(Tooltip source)
    {
        if (header != null) { header.text = source.header; }
        if (body != null) { body.text = source.text; }
        if (icon != null)
        {
            icon.sprite = source.icon;
            icon.enabled = (source.icon != null);
        }

        followMouse = source.followMouse;
        alignment = source.alignment;
        target = source.GetComponent<RectTransform>();

        if (source.offset == Vector2.zero) { offset = Vector2.zero; }
        else
        {
            if (alignment == TooltipAlignment.Default) { offset = (target != null) ? (target.position / 2f) - (Input.mousePosition / 2f) : Vector3.zero; }
            else if (alignment == TooltipAlignment.Above) { offset = new Vector2(0f, self.rect.height / 2f) + source.offset; }
            else if (alignment == TooltipAlignment.Below) { offset = new Vector2(0f, -self.rect.height / 2f) + source.offset; }
            else if (alignment == TooltipAlignment.Left) { offset = new Vector2(-self.rect.width / 2f, -self.rect.height / 2f) + source.offset; }
            else if (alignment == TooltipAlignment.Right) { offset = new Vector2(self.rect.width / 2f, -self.rect.height / 2f) + source.offset; }
        }
    }

    /// <summary>
    /// Update the title header text of the tooltip.
    /// </summary>
    /// <param name="newText">The new text to use in replacement of the previous.</param>
    public void UpdateHeader(string newText)
    {
        if (header != null) { header.text = newText; }
    }

    /// <summary>
    /// Update the body text of the tooltip.
    /// </summary>
    /// <param name="newText">The new text to use in replacement of the previous.</param>
    public void UpdateBody(string newText)
    {
        if (body != null) { body.text = newText; }
    }

    /// <summary>
    /// Update the icon image of the tooltip.
    /// </summary>
    /// <param name="newImage">The new sprite image to use in replacement of the previous.</param>
    public void UpdateImage(Sprite newImage)
    {
        if (icon != null) { icon.sprite = newImage; }
    }

    /// <summary>
    /// Set the target this tooltip should appear on. This is only relevant when "followMouse" is disabled, for better accuracy.
    /// </summary>
    /// <param name="targetObject">The object to target the tooltips positioning on.</param>
    public void SetTarget(RectTransform targetObject) { target = targetObject; }

    /// <summary>
    /// Display a tooltip with specified paramaters for positioning.
    /// </summary>
    /// <param name="text">Body text of the tooltip</param>
    /// <param name="title">Header title of the tooltip</param>
    /// <param name="image">Icon sprite image of the tooltip, displayed next to the header</param>
    /// <param name="updatePosition">Should the position of the tooltip be updated to the mouse position per frame?</param>
    /// <param name="mouseOffset">Offset value from its origin point (mouse or object, depending on upatePosition)</param>
    /// <param name="alignmentPosition">Position alignment in relation to the origin point of the object</param>
    /// <param name="targetObject">Object to be displaying this tooltip on</param>
    public void ShowTooltip(string text, string title = "", Sprite image = null, bool updatePosition = false, Vector2 mouseOffset = new Vector2(), TooltipAlignment alignmentPosition = TooltipAlignment.Default, RectTransform targetObject = null)
    {
        if (header != null) { header.text = title; }
        if (body != null) { body.text = text; }
        if (icon != null)
        {
            icon.sprite = image;
            icon.enabled = (image != null);
        }

        followMouse = updatePosition;
        alignment = alignmentPosition;
        target = targetObject;

        if (alignment == TooltipAlignment.Default) { offset = (target != null) ? (target.position / 2f) - (Input.mousePosition / 2f) : Vector3.zero; }
        else if (alignment == TooltipAlignment.Above) { offset = new Vector2(0f, self.rect.height / 2f) + mouseOffset; }
        else if (alignment == TooltipAlignment.Below) { FlipOffset(ref mouseOffset); offset = new Vector2(0f, -self.rect.height / 2f) + mouseOffset; }
        else if (alignment == TooltipAlignment.Left) { FlipOffset(ref mouseOffset); offset = new Vector2(-self.rect.width / 2f, -self.rect.height / 2f) + mouseOffset; }
        else if (alignment == TooltipAlignment.Right) { offset = new Vector2(self.rect.width / 2f, -self.rect.height / 2f) + mouseOffset; }

        SetPosition();
        gameObject.SetActive(true);
        gameObject.GetComponent<ContentSizeFitter>().SetLayoutVertical();
    }

    /// <summary>
    /// Hide the tooltip display.
    /// </summary>
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Simply flips the offset of negative directions so positive values are treated as negatives for opposite directions.
    /// </summary>
    /// <param name="targetOffset">Reference to the offset to affect</param>
    void FlipOffset(ref Vector2 targetOffset)
    {
        if(alignment == TooltipAlignment.Left) { if (targetOffset.x > 0f) { targetOffset.x = -targetOffset.x; } }
        else if (alignment == TooltipAlignment.Below) { if (targetOffset.y > 0f) { targetOffset.y = -targetOffset.y; } }
    }
    #endregion
}
