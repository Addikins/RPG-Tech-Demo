using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Ingame Cursor UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Extentions/In-game Cursor", 2), DisallowMultipleComponent]
public class IngameCursor : UICursor, IPointerClickHandler
{
    [Tooltip("The source object to use as the UI cursor.")]
    public Image cursor;
    [Tooltip("Size of the cursor.")]
    public float size = 1f;
    [Tooltip("Speed the cursor will constantly maintin when following the exact position of the system cursor. Set to 0 for instant following.")]
    public float followSpeed = 0f;

    [Space]

    [Tooltip("The image to use on the cursor.")]
    public Sprite icon;
    [Tooltip("The array of sprites to use instead of a single icon. The icon must be unset before this array can be used.")]
    public Sprite[] spriteIcons;
    [Tooltip("The array of textures to use instead of a single icon. The icon must be unset before this array can be used.")]
    public Texture2D[] textureIcons;

    [Tooltip("How fast the icon iterates through the provided array.")]
    public float animationSpeed = 1f;
    [Tooltip("The range of the array to use. For example: in an array of 10 elements with a range of [2, 8], means only elements 2 through 8 will be used, and 0, 1 and 9 will be exempt.")]
    public Vector2 range = new Vector2(-1, -1);

    [Tooltip("Hides the system cursor while in-game so only the UI cursor is visible.")]
    public bool hideSystemCursor;
    [Tooltip("Hides the UI cursor while in-game so only the system cursor is visisble")]
    public bool hideGameCursor;

    /// <summary>
    /// The PointerEventData of the last object the UI cursor clicked.
    /// </summary>
    public static PointerEventData clickData;
    /// <summary>
    /// The GameObject of the last object the UI Cursor clicked.
    /// </summary>
    public static GameObject lastClicked;

    private int index;
    private bool set;
    private RectTransform cursorRect;

    #region Public Functions
    /// <summary>
    /// Set the UI cursor to a single sprite.
    /// </summary>
    /// <param name="cursorIcon">The sprite to use as the new UI cursor.</param>
    public void SetCursor(Sprite cursorIcon)
    {
        cursor.sprite = cursorIcon;
        range.x = (range.x > spriteIcons.Length - 1) ? -1 : range.x;
        index = (range.x < 0) ? 0 : (int)range.x;
        set = true;
    }

    /// <summary>
    /// Set the UI cursor to an array of sprites.
    /// </summary>
    /// <param name="cursorIcons">The sprite array to use as the new UI cursor. The cursor will cycle through this array at an anitmationSpeed.</param>
    public void SetCursor(Sprite[] cursorIcons)
    {
        spriteIcons = cursorIcons;
        range.x = (range.x > spriteIcons.Length - 1) ? -1 : range.x;
        index = (range.x < 0) ? 0 : (int)range.x;
        set = true;
    }

    /// <summary>
    /// Set the UI cursor to an array of textures.
    /// </summary>
    /// <param name="cursorIcons">The texture array to use as the new UI cursor. The cursor will cycle through this array at an anitmationSpeed.</param>
    public void SetCursor(Texture2D[] cursorIcons)
    {
        textureIcons = cursorIcons;
        range.x = (range.x > spriteIcons.Length - 1) ? -1 : range.x;
        index = (range.x < 0) ? 0 : (int)range.x;
        set = true;
    }
    #endregion

    #region Private Functions
    void Start()
    {
        if(icon != null) { SetCursor(icon); }
        else if(spriteIcons.Length > 0) { SetCursor(spriteIcons); }
        else if(textureIcons.Length > 0) { SetCursor(textureIcons); }

        cursorRect = cursor.GetComponent<RectTransform>();

        StartCoroutine(AnimateCursor());
    }

    void LateUpdate()
    {
        if (icon != null) { cursor.sprite = icon; }
        if(size < 1) { size = 1f; }
        cursor.rectTransform.sizeDelta = new Vector2(size, size);

        Cursor.visible = !hideSystemCursor;
        cursor.enabled = !hideGameCursor;
    }

    void FixedUpdate()
    {
        if (followSpeed <= 0f) { cursor.transform.position = Input.mousePosition - new Vector3(cursorRect.rect.xMin, cursorRect.rect.yMax); }
        else { cursor.transform.position = Vector3.MoveTowards(cursor.transform.position, Input.mousePosition - new Vector3(cursorRect.rect.xMin, cursorRect.rect.yMax), followSpeed * Time.deltaTime); }
    }

    IEnumerator AnimateCursor()
    {
        while (set)
        {
            if (icon == null)
            {
                if (spriteIcons.Length > 0)
                {
                    cursor.sprite = spriteIcons[index];
                    yield return new WaitForSecondsRealtime(1 / animationSpeed);
                    index++;
                    range.x = (range.x > spriteIcons.Length - 1) ? -1 : range.x;
                    range.y = (range.y > spriteIcons.Length - 1) ? -1 : range.y;
                    if (index > ((range.y < 0) ? spriteIcons.Length - 1 : (int)range.y)) { index = (range.x < 0) ? 0 : (int)range.x; }
                }
                else if (textureIcons.Length > 0)
                {
                    Sprite textureSprite = Sprite.Create(textureIcons[index], new Rect(0,0,textureIcons[index].width, textureIcons[index].height), Vector2.zero);
                    cursor.sprite = textureSprite;
                    yield return new WaitForSecondsRealtime(1 / animationSpeed);
                    index++;
                    range.x = (range.x > textureIcons.Length - 1) ? -1 : range.x;
                    range.y = (range.y > textureIcons.Length - 1) ? -1 : range.y;
                    if (index > ((range.y < 0) ? textureIcons.Length - 1 : (int)range.y)) { index = (range.x < 0) ? 0 : (int)range.x; }
                }
            }
            else { yield return new WaitForSecondsRealtime(1 / animationSpeed); }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickData = eventData;
        lastClicked = clickData.pointerCurrentRaycast.gameObject;
    }
#endregion
}
