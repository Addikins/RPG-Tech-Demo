using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Calendar UI Tool - Data Class
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

public class ButtonEventData
{
    public Sprite eventImage;
    public List<ButtonEvent> buttonEvents = new List<ButtonEvent>();

    /// <summary>
    /// Automatically set when Calendar.OnCalendarUpdated
    /// </summary>
    public Button button;

    public ButtonEventData()
    {
        buttonEvents = new List<ButtonEvent>();
    }
};

public class ButtonEvent
{
    public string name;
    public DateTime time;
    public Sprite image;
    public bool useAsPreviewPicture;

    public ButtonEvent(string eventName, DateTime eventDate)
    {
        name = eventName;
        time = eventDate;
    }

    public ButtonEvent(string eventName, DateTime eventDate, Sprite eventImage)
    {
        name = eventName;
        time = eventDate;
        image = eventImage;
    }
};

public class SelectedButton
{
    public Button gameObject;
    public DateTime date;
    public ButtonEventData eventData;
};
