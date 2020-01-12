using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/// <summary>
/// Option Selector UI Tool
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Option Selector", 1), DisallowMultipleComponent, Serializable]
public class OptionSelector : MonoBehaviour
{
    #region Common Variables
    [Tooltip("Determines if the up/down will continue from the beginning once it reaches the end, or stop.")]
    public bool loopback;
    public DataType dataType;
    [Tooltip("Determines if 'Best Fit' applies to the text to fit it in its container.")]
    public bool fitText;
    #endregion

    #region Events
    public delegate void OnTypeChangedDelegate(DataType type);
    public delegate void OnValueChangedDelegate(object value);

    /// <summary>
    /// Called every time the dataType is changed. You can subscribe your own calls to this event in Start or Awake.
    /// </summary>
    public event OnTypeChangedDelegate OnTypeChanged;

    /// <summary>
    /// Called every time the up/down arrows are pressed. You can subscribe your own calls to this event in Start or Awake.
    /// </summary>
    public event OnValueChangedDelegate OnValueChanged;
    #endregion

    #region Variable Data Types
    //Numeric
    public int minInt, maxInt;

    //Decimal
    public float minFloat, maxFloat;
    [Tooltip("How much to increase the value by, each iteration.")]
    public float increment = 0.5f;

    //String
    public List<string> stringValues = new List<string>();

    //Enum
    public Enum enumType;

    //Object
    public List<UnityEngine.Object> objectValues = new List<UnityEngine.Object>();
    [Tooltip("Should the Object type also be displayed?")]
    public bool includeType = true;

    //Custom
    public List<object> customValues = new List<object>();

    public int valueIndex { get; set; }
    #endregion

    #region Private (class) Variables
    public enum DataType { Numeric, Decimal, String, Enum, Object, Custom };

    private InputField option;
    private float floatValueIndex;
    private Array enumArray;
    #endregion

    #region Private (class) Functions
    void Awake()
    {
        option = GetComponent<InputField>();
        UpdateSelector();
    }

    public void BtnUp()
    {
        valueIndex++;
        UpdateSelector(1);
    }

    public void BtnDown()
    {
        valueIndex--;
        UpdateSelector(-1);
    }

    void UpdateSelector(int add)
    {
        if (fitText) { option.textComponent.horizontalOverflow = HorizontalWrapMode.Wrap; }

        if (dataType == DataType.Numeric)
        {
            valueIndex = Mathf.Clamp(valueIndex, minInt, maxInt);
            option.contentType = InputField.ContentType.IntegerNumber;
            option.text = (valueIndex).ToString();

            if (OnValueChanged != null) { OnValueChanged(valueIndex); }
        }
        else if (dataType == DataType.Decimal)
        {
            if (add > 0) { floatValueIndex += increment; } else { floatValueIndex -= increment; }
            floatValueIndex = Mathf.Clamp(floatValueIndex, minFloat, maxFloat);
            option.contentType = InputField.ContentType.DecimalNumber;
            option.text = (floatValueIndex).ToString();

            if (OnValueChanged != null) { OnValueChanged(floatValueIndex); }
        }
        else if (dataType == DataType.String)
        {
            valueIndex = Mathf.Clamp(valueIndex, 0, stringValues.Count - 1);
            string value = stringValues[valueIndex];

            option.contentType = InputField.ContentType.Standard;
            option.text = value;

            if (OnValueChanged != null) { OnValueChanged(value); }
        }
        else if (dataType == DataType.Enum)
        {
            valueIndex = Mathf.Clamp(valueIndex, 0, enumArray.Length - 1);
            Enum value = (Enum)enumArray.GetValue(valueIndex);

            option.contentType = InputField.ContentType.Standard;
            option.text = value.ToString();

            if (OnValueChanged != null) { OnValueChanged(value); }
        }
        else if (dataType == DataType.Object)
        {
            valueIndex = Mathf.Clamp(valueIndex, 0, objectValues.Count - 1);
            UnityEngine.Object value = objectValues[valueIndex];

            option.contentType = InputField.ContentType.Standard;
            option.text = (includeType) ? value.ToString() : value.name;

            if (OnValueChanged != null) { OnValueChanged(value); }
        }
        else if (dataType == DataType.Custom)
        {
            valueIndex = Mathf.Clamp(valueIndex, 0, customValues.Count - 1);
            object value = customValues[valueIndex];

            option.contentType = InputField.ContentType.Custom;
            option.text = value.ToString();

            if (OnValueChanged != null) { OnValueChanged(value); }
        }
    }

    void SetRange(Vector2Int range)
    {
        minInt = range.x;
        maxInt = range.y;
    } //Apply int range

    void SetRange(Vector2 range)
    {
        minFloat = range.x;
        maxFloat = range.y;
    } //Apply float range
#endregion

    #region Public Functions (Overloads)
    /// <summary>
    /// Update the Selector to show the relevant data. Automatically called on Awake and every time the Inspector or the dataType is changed.
    /// The value updated will always be "floored" or reset to lowest-index.
    /// </summary>
    public void UpdateSelector()
    {
        valueIndex = -1;
        floatValueIndex = minFloat - increment;
        if (fitText) { option.textComponent.horizontalOverflow = HorizontalWrapMode.Wrap; }

        BtnDown();
    }

    /// <summary>
    /// Set the Enum to be used by the Selector.
    /// </summary>
    /// <param name="enumData">The enum to use. This can be any type of enum data. The order of the data will be the order of the enums values.</param>
    public void SetEnumType(Enum enumData)
    {
        enumType = enumData;
        enumArray = Enum.GetValues(enumType.GetType());
    }

    /// <summary>
    /// Set the Enum to be used by the Selector.
    /// </summary>
    /// <param name="enumData">The enum to use. This can be any type of enum data. The order of the data will be the order of the enums values.</param>
    /// <param name="applyTypeChange">Should the type be auto-changed to DataType.Enum when setting the Enum data as well?</param>
    public void SetEnumType(Enum enumData, bool applyTypeChange = false)
    {
        if (applyTypeChange) { dataType = DataType.Enum; UpdateSelector(); }
        enumType = enumData;
        enumArray = Enum.GetValues(enumType.GetType());
    }

    /// <summary>
    /// Set the Custom datatype to the specified list of object data.
    /// </summary>
    /// <param name="objectData">The object list data that the Selector will use to iterate through.</param>
    public void SetCustomType(List<object> objectData)
    {
        customValues = objectData;
    }

    /// <summary>
    /// Set the Custom data type to the specified list of object data.
    /// </summary>
    /// <param name="objectData">The object list data that the Selector will use to iterate through.</param>
    /// <param name="applyTypeChange">Should the type be auto-changed to DataType.Object when setting the Object data as well?</param>
    public void SetCustomType(List<object> objectData, bool applyTypeChange = false)
    {
        if (applyTypeChange) { dataType = DataType.Object; UpdateSelector(); }
        customValues = objectData;
    }

    /// <summary>
    /// Set the values and data type to a Numeric (int) range.
    /// </summary>
    /// <param name="range">The min (x) and max (y) range for Numeric values.</param>
    public void SetType(Vector2Int range)
    {
        dataType = DataType.Numeric;
        SetRange(range);
    }

    /// <summary>
    /// Set the values and data type to a Decimal (float) range.
    /// </summary>
    /// <param name="range">The min (x) and max (y) range for Decimal values.</param>
    public void SetType(Vector2 range)
    {
        dataType = DataType.Decimal;
        SetRange(range);
    }

    /// <summary>
    /// Set the String data type to the specified list of string data.
    /// </summary>
    /// <param name="stringList">The list of strings that the Selector will use to iterate through.</param>
    public void SetType(List<string> stringList)
    {
        dataType = DataType.String;
        stringValues = stringList;
    }

    /// <summary>
    /// Set the Object data type to the specified list of Unity Object data.
    /// </summary>
    /// <param name="objectList">The list of Unity Object that the Selector will use to iterate through.</param>
    public void SetType(List<UnityEngine.Object> objectList)
    {
        dataType = DataType.Object;
        objectValues = objectList;
    }
    #endregion

    #region Editor Event
    //Used by the Editor to call OnTypeChanged event.
    public void FireEvent()
    {
        if (OnTypeChanged != null) { OnTypeChanged(dataType); }
        if (!IsInvoking("UpdateSelector")) { Invoke("UpdateSelector", 0.01f); } //prevents Layout from attempting to do Repaint jobs, after Unity updates all events
    }
    #endregion
}
