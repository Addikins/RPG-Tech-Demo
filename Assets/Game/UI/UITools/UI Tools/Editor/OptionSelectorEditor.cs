using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(OptionSelector))]
public class OptionSelectorEditor : Editor {

    SerializedProperty loopback, dataType, fitText, includeObjectType;
    OptionSelector.DataType dt;

    private void OnEnable()
    {
        //default elements to draw first
        loopback = serializedObject.FindProperty("loopback");
        dataType = serializedObject.FindProperty("dataType");
        fitText = serializedObject.FindProperty("fitText");
        includeObjectType = serializedObject.FindProperty("includeType");

        OptionSelector options = target as OptionSelector;
        dt = options.dataType;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultElements();

        OptionSelector options = target as OptionSelector;
        
        if(dt != options.dataType && Application.isPlaying) { options.FireEvent(); dt = options.dataType; }

        if (options.dataType == OptionSelector.DataType.Numeric)
        {
            options.minInt = EditorGUILayout.IntField("Min", options.minInt);
            options.maxInt = EditorGUILayout.IntField("Max", options.maxInt);
        }
        else if (options.dataType == OptionSelector.DataType.Decimal)
        {
            options.minFloat = EditorGUILayout.FloatField("Min", options.minFloat);
            options.maxFloat = EditorGUILayout.FloatField("Max", options.maxFloat);
            options.increment = EditorGUILayout.FloatField("Increment", options.increment);
        }
        else if (options.dataType == OptionSelector.DataType.String)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stringValues"), true);
        }
        else if (options.dataType == OptionSelector.DataType.Enum)
        {
            EditorGUILayout.LabelField("Set Enum by script: OptionSelector.SetEnumType(Enum);");
        }
        else if (options.dataType == OptionSelector.DataType.Object)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("objectValues"), true);
            EditorGUILayout.PropertyField(includeObjectType);
        }
        else if (options.dataType == OptionSelector.DataType.Custom)
        {
            EditorGUILayout.LabelField("Set custom data by script: OptionSelector.SetCustomType(object);");
        }

        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawDefaultElements()
    {
        EditorGUILayout.PropertyField(loopback);
        EditorGUILayout.PropertyField(dataType);
        EditorGUILayout.PropertyField(fitText);
    }
}
