using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// Calendar UI Tool - Extention Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Extentions/Calendar", 1), DisallowMultipleComponent]
public class Calendar : MonoBehaviour
{
    public DateTime today, targetDate;
    public SelectedButton selected = new SelectedButton();

    [Header("Display")]
    public Button prevMonth;
    public Button nextMonth, currentMonth, currentYear;
    public Transform DOMContainer;

    [Header("Templates")]
    public Button DOM_selected;
    public Button DOM_active;
    public Button DOM_locked;

    [Header("Design")]
    public Sprite background = null;
    
    #region Public Events
    /// <summary>
    /// Called whenever a button is clicked with the LEFT MOUSE BUTTON
    /// </summary>
    public event LeftClicked OnButtonClicked;

    /// <summary>
    /// Called whenever a button is double-clicked with the LEFT MOUSE BUTTON
    /// </summary>
    public event DoubleClicked OnButtonDoubleClicked;

    /// <summary>
    /// Called whenever a button is clicked with the RIGHT MOUSE BUTTON
    /// </summary>
    public event RightClicked OnButtonRightClicked;

    /// <summary>
    /// Called whenever a button is selected, and the previous selection has changed. This can be retrieved from Calendar.selected
    /// </summary>
    public event SelectionChanged OnSelectionChanged;

    /// <summary>
    /// Called whenever the month has been changed to the next or previous one
    /// </summary>
    public event MonthChanged OnMonthChanged;

    /// <summary>
    /// Called whenever the calendar has been updated, such as months being changed
    /// </summary>
    public event CalendarUpdated OnCalendarUpdated;

    /// <summary>
    /// Called whenever a new event is created with Calendar.AddEvent
    /// </summary>
    public event EventCreated OnEventCreated;

    /// <summary>
    /// Called whenever an event is removed with Calendar.RemoveEvent
    /// </summary>
    public event EventRemoved OnEventRemoved;

    public delegate void LeftClicked(ButtonEventData eventData);
    public delegate void DoubleClicked(ButtonEventData eventData);
    public delegate void RightClicked(ButtonEventData eventData);
    public delegate void SelectionChanged(ButtonEventData eventData);
    public delegate void MonthChanged(DateTime previousDate, DateTime currentDate);
    public delegate void CalendarUpdated();
    public delegate void EventCreated(ButtonEvent eventData);
    public delegate void EventRemoved(List<ButtonEvent> eventDataList);
    #endregion

    private enum Months { Janurary = 1, February, March, April, May, June, July, August, September, October, November, December };
    private Dictionary<DateTime, ButtonEventData> events = new Dictionary<DateTime, ButtonEventData>();
    private DateTime firstDOM;

    #region Unity & Helper Functions
    private void Start()
    {
        today = DateTime.Now;
        targetDate = today;

        DOM_active.gameObject.SetActive(false);
        DOM_locked.gameObject.SetActive(false);
        DOM_selected.gameObject.SetActive(false);

        UpdateCalandarDisplay();
    }

    void UpdateCalandarDisplay()
    {
        currentMonth.transform.GetChild(0).GetComponent<Text>().text = ((Months)targetDate.Month).ToString();
        if (targetDate.Month - 1 > 0) { prevMonth.transform.GetChild(0).GetComponent<Text>().text = ((Months)(targetDate.Month - 1)).ToString(); }
        else { prevMonth.transform.GetChild(0).GetComponent<Text>().text = ((Months)12).ToString(); }
        if (targetDate.Month + 1 < 13) { nextMonth.transform.GetChild(0).GetComponent<Text>().text = ((Months)(targetDate.Month + 1)).ToString(); }
        else { nextMonth.transform.GetChild(0).GetComponent<Text>().text = ((Months)1).ToString(); }

        currentYear.transform.GetChild(0).GetComponent<Text>().text = targetDate.Year.ToString();

        GenerateDOM();

        if (background != null) { GetComponent<Image>().sprite = background; }

        if (OnCalendarUpdated != null) { OnCalendarUpdated(); }
    }

    void GenerateDOM()
    {
        for (int i = 0; i < DOMContainer.childCount; i++) { Destroy(DOMContainer.GetChild(i).gameObject); }
        int dom = 1;
        int daysInWeek = DateTime.DaysInMonth(targetDate.Year, targetDate.Month);
        firstDOM = new DateTime(targetDate.Year, targetDate.Month, 1);

        int daysofWeekPrevMonth = DateTime.DaysInMonth(targetDate.Year, targetDate.Month - 1);
        for (int i = 0; i < (int)firstDOM.DayOfWeek; i++)
        {
            Transform btn = (Transform)Instantiate(DOM_locked.gameObject).transform;
            btn.gameObject.SetActive(true);
            btn.name = "Prev_Day " + (i + 1);
            btn.SetParent(DOMContainer);
            btn.GetChild(1).GetComponent<Text>().text = (daysofWeekPrevMonth - ((int)firstDOM.DayOfWeek - i) + 1).ToString();
        }

        for (int i = 0; i < daysInWeek; i++)
        {
            Transform btn;
            bool isToday = (dom == today.Day && targetDate.Month == today.Month);
            if (isToday) { btn = (Transform)Instantiate(DOM_selected.gameObject).transform; }
            else { btn = (Transform)Instantiate(DOM_active.gameObject).transform; }
            btn.gameObject.SetActive(true);

            btn.name = "Day " + (i + 1);
            btn.SetParent(DOMContainer);

            if (btn.GetComponentInChildren<Text>()) { btn.GetComponentInChildren<Text>().text = dom.ToString(); }
            if (dom < daysInWeek) { dom++; }

            DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, (i + 1));
            if (!events.ContainsKey(thisDay)) { events.Add(thisDay, new ButtonEventData()); }

            if (events[thisDay].eventImage != null) { btn.GetChild(0).GetComponent<Image>().sprite = events[thisDay].eventImage; }
            btn.GetComponent<CalendarButton>().SetCalendar(this, events[thisDay]);
            events[thisDay].button = btn.GetComponent<Button>();
        }
    }
    #endregion

    #region Button Functions
    public void BtnChangeMonth(bool prev)
    {
        DateTime dateShift = targetDate;
        if (prev) { targetDate = dateShift.AddMonths(-1); }
        else { targetDate = dateShift.AddMonths(1); }

        UpdateCalandarDisplay();
        if (OnMonthChanged != null) { OnMonthChanged(dateShift, targetDate); }
    }

    public void BtnDayViewEvents(Button btn)
    {
        int day = int.Parse(btn.transform.GetChild(1).GetComponent<Text>().text);
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);

        selected.gameObject = btn;
        selected.date = thisDay;
        selected.eventData = events[thisDay];

        if (OnSelectionChanged != null) { OnSelectionChanged(events[thisDay]); }
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Automatically called by CalendarButton whenever the button recieves click events (left/right), and is sent to the Calendar for processing.
    /// </summary>
    public void SendClickEvents(PointerEventData eventData, ButtonEventData buttonData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (eventData.clickCount < 2) { if (OnButtonClicked != null) { OnButtonClicked(buttonData); } }
            else { if (OnButtonDoubleClicked != null) { OnButtonDoubleClicked(buttonData); } }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (OnButtonRightClicked != null) { OnButtonRightClicked(buttonData); }
        }
    }

    /// <summary>
    /// Refreshes the specified day, updating all event changes.
    /// </summary>
    /// <param name="day">The day to refresh</param>
    public void ReloadDay(int day)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);
        if (!events.ContainsKey(thisDay)) { events.Add(thisDay, new ButtonEventData()); }

        Transform btn = DOMContainer.GetChild((day + (int)firstDOM.DayOfWeek) - 1);
        if (events[thisDay].eventImage != null) { btn.GetChild(0).GetComponent<Image>().sprite = events[thisDay].eventImage; }
    }

    /// <summary>
    /// Gets all the events associated wit the specified day.
    /// </summary>
    /// <param name="day">The day to retrieve events from</param>
    /// <returns></returns>
    public List<ButtonEvent> GetEvents(int day)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);
        return events[thisDay].buttonEvents;
    }

    /// <summary>
    /// Gets the current thumbnail for the specified day.
    /// </summary>
    /// <param name="day">The day to retrieve the thumbnail from</param>
    /// <returns></returns>
    public Sprite GetButtonThumbnail(int day)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);
        return events[thisDay].eventImage;
    }

    /// <summary>
    /// Sets the thumbnail of the specified day to the provided thumbnail.
    /// </summary>
    /// <param name="day">The day to set the thumbnail to</param>
    /// <param name="eventThumbnail">Thumbnail to apply to the specified day</param>
    public void SetEventThumbnail(int day, Sprite eventThumbnail)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);
        events[thisDay].eventImage = eventThumbnail;
    }

    /// <summary>
    /// Adds a set of events to the specified day.
    /// </summary>
    /// <param name="day">The day to add events to</param>
    /// <param name="eventName">Name to set to this event</param>
    /// <param name="eventThumbnail">Thumbnail to set for this event</param>
    /// <param name="useAsThumbnail">If true, the specified thumbnail will become the thumbnail for the specified day</param>
    public void AddEvent(int day, string eventName, Sprite eventThumbnail = null, bool useAsThumbnail = false)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);

        int count = events[thisDay].buttonEvents.Count;
        events[thisDay].buttonEvents.Add(new ButtonEvent(eventName, thisDay, eventThumbnail));
        if (useAsThumbnail) { events[thisDay].eventImage = eventThumbnail; }
        ReloadDay(day);

        if (OnEventCreated != null) { OnEventCreated(events[thisDay].buttonEvents[count]); }
    }

    /// <summary>
    /// Adds a set of events to the specified day. 
    /// </summary>
    /// <param name="day">The day to add events to</param>
    /// <param name="eventData">Data to use for this event - this includes the name and thumbnail</param>
    /// <param name="useAsThumbnail">If true, the specified thumbnail will become the thumbnail for the specified day</param>
    public void AddEvent(int day, ButtonEvent eventData, bool useAsThumbnail = false)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);

        int count = events[thisDay].buttonEvents.Count;
        events[thisDay].buttonEvents.Add(eventData);
        if (useAsThumbnail) { events[thisDay].eventImage = eventData.image; }
        ReloadDay(day);

        if (OnEventCreated != null) { OnEventCreated(events[thisDay].buttonEvents[count]); }
    }

    /// <summary>
    /// Adds a set of events to the specified day.
    /// </summary>
    /// <param name="day">The day to add events to</param>
    /// <param name="eventData"></param>
    /// <param name="buttonThumbnail"></param>
    public void AddEvent(int day, ButtonEvent[] eventData, ButtonEvent buttonThumbnail = null)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);

        int count = events[thisDay].buttonEvents.Count;
        events[thisDay].buttonEvents.AddRange(eventData);
        if (buttonThumbnail != null) { events[thisDay].eventImage = buttonThumbnail.image; }
        ReloadDay(day);

        if (OnEventCreated != null) { OnEventCreated(events[thisDay].buttonEvents[count]); }
    }

    /// <summary>
    /// Removes a specified event from the specified day.
    /// </summary>
    /// <param name="day">The day to remove events from</param>
    /// <param name="eventData">Event to be removed from the specified day</param>
    public void RemoveEvent(int day, ButtonEvent eventData)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);
        
        events[thisDay].buttonEvents.Remove(eventData);
        ReloadDay(day);

        if (OnEventRemoved != null) { OnEventRemoved(events[thisDay].buttonEvents); }
    }

    /// <summary>
    /// Removes a set of events from the specified day.
    /// </summary>
    /// <param name="day">The day to remove events from</param>
    /// <param name="eventData">Array of events to be removed from the specified day</param>
    public void RemoveEvent(int day, ButtonEvent[] eventData)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);
        
        for(int i = 0; i < eventData.Length; i++) { events[thisDay].buttonEvents.Remove(eventData[i]); }
        ReloadDay(day);

        if (OnEventRemoved != null) { OnEventRemoved(events[thisDay].buttonEvents); }
    }

    /// <summary>
    /// Removes all events from the specified day.
    /// </summary>
    /// <param name="day">Day to remove all events from</param>
    public void RemoveAllEvents(int day)
    {
        DateTime thisDay = new DateTime(targetDate.Year, targetDate.Month, day);

        events[thisDay].buttonEvents.Clear();
        ReloadDay(day);

        if (OnEventRemoved != null) { OnEventRemoved(events[thisDay].buttonEvents); }
    }
    #endregion
}
