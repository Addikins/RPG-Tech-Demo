using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Calendar UI Tool - Data Class
/// Placed on each button of a Calendar.
/// 
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

public class CalendarButton : MonoBehaviour, IPointerClickHandler {

    protected Calendar calendar;
    protected ButtonEventData data;

    public void SetCalendar(Calendar targetCalendar, ButtonEventData eventData)
    {
        calendar = targetCalendar;
        data = eventData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        calendar.SendClickEvents(eventData, data);
    }
}
