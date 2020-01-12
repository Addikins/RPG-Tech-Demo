using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Game Cursor UI Tool (not a UI element)
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Extentions/Game Cursor", 3), DisallowMultipleComponent]
public class GameCursor : UICursor
{
    [Tooltip("Auto - will set the size to match a 'System' cursor size.\nForce Software - will set the size to match the import settings of the texture.")]
    public CursorMode cursorType;

    [Tooltip("Determines if the Editor will auto-update Player Settings cursor information, whenever anything is changed.")]
    public bool ApplyInEditor;

    [Space]

    [Header("Static Cursor")]
    [Tooltip("Set a static texture to use as the cursor. Doing so will use the static cursor instead of any animated settings.")]
    public Texture2D cursor;
    [Tooltip("Set the hotspot where the cursor starts to register as a pointer.")]
    public Vector2 hotspot = Vector2.zero;

    [Space]
    [Header("Animated Cursor")]

    [Tooltip("Optionally load the Animated Cursor array with textures from a Resource folder. Specify the name of the folder within Resources these textures exist.\nFor example: If you had 5 textures inside 'Assets > Resources > Game Cursors', youd specify 'Game Cursors'.")]
    public string loadFromResources;
    [Tooltip("The textures to animate the cursor with.")]
    public Texture2D[] animatedCursor;
    [Tooltip("Optionally set the range of the provided texture array, to use.\nThe first value represents the 'min' range, and the second represents the 'max' range.\nFor example: If you had 10 textures set, and you set the range to [2, 8], only the textures from index 2, up to index 8 will play, skipping 0, 1 and 9.\nSet to default [-1,-1] to use all textures and ignore any range settings.")]
    public Vector2 range = new Vector2(-1, -1);
    [Tooltip("The speed to play the animation by. 1 is default normal speed.\nAnything higher than 1 is faster, anything less than 1 is slower than the default speed.")]
    public float animationSpeed = 1f;


    private int step = 0;

    #region Public Functions
    /// <summary>
    /// Set the cursor to an animated array of Texture2D.
    /// </summary>
    /// <param name="animation">Array of Texture2D to animate the cursor with.</param>
    /// <param name="initialSpeed">Speed to animate the cursor by.</param>
    public void SetAnimatedCursor(Texture2D[] animation, float initialSpeed)
    {
        animatedCursor = animation;
        animationSpeed = initialSpeed;
        range.x = (range.x > animation.Length - 1) ? -1 : range.x;
        step = (range.x < 0) ? 0 : (int)range.x;

        StopCoroutine(AnimateCursor());
        if (animation.Length > 0) { StartCoroutine(AnimateCursor()); }
    }

    /// <summary>
    /// Set the cursor to an animated array from the Resources folder.
    /// </summary>
    /// <param name="resourceAnimation">Path to the folder starting in Assets/Resources.</param>
    /// <param name="initialSpeed">Speed to animate the cursor by.</param>
    public void SetAnimatedCursor(string resourceAnimation, float initialSpeed)
    {
        Texture2D[] animation = Resources.LoadAll<Texture2D>(resourceAnimation);
        animatedCursor = animation;
        animationSpeed = initialSpeed;
        range.x = (range.x > animation.Length - 1) ? -1 : range.x;
        step = (range.x < 0) ? 0 : (int)range.x;

        StopCoroutine(AnimateCursor());
        if (animation.Length > 0) { StartCoroutine(AnimateCursor()); }
    }
    #endregion

    private void OnValidate()
    {
        if (ApplyInEditor)
        {
            Cursor.SetCursor(cursor, hotspot, cursorType);

            if (!string.IsNullOrEmpty(loadFromResources))
            { SetAnimatedCursor(loadFromResources, animationSpeed); }
            else { SetAnimatedCursor(animatedCursor, animationSpeed); }
        }
    }
    
    void Start()
    {
        if (!string.IsNullOrEmpty(loadFromResources))
        { SetAnimatedCursor(loadFromResources, animationSpeed); }
        else { SetAnimatedCursor(animatedCursor, animationSpeed); }
    }

    void FixedUpdate()
    {
        if(cursor != null) { Cursor.SetCursor(cursor, Vector2.zero, cursorType); }
    }

    IEnumerator AnimateCursor()
    {
        while (true)
        {
            if (cursor == null)
            {
                Cursor.SetCursor(animatedCursor[step], hotspot, cursorType);
                yield return new WaitForSecondsRealtime(1 / animationSpeed);
                step++;
                range.x = (range.x > animatedCursor.Length - 1) ? -1 : range.x;
                range.y = (range.y > animatedCursor.Length - 1) ? -1 : range.y;
                if (step > ((range.y < 0) ? animatedCursor.Length - 1 : (int)range.y)) { step = (range.x < 0) ? 0 : (int)range.x; }
            }
            else { yield return new WaitForSecondsRealtime(1 / animationSpeed); }
        }
    }
}

[DisallowMultipleComponent]
public class UICursor : MonoBehaviour
{
    //empty class used for Cursor's to prevent multiple types of cursors being used.
}