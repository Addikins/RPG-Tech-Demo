using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor script used to create each UI Tool prefab, from GameObject > UI top menu
/// </summary>

public class UIToolsMaker : MonoBehaviour
{
    [MenuItem("UI Tools/Combobox (Advanced Dropdown)", menuItem = "GameObject/UI/Combox (Advanced Dropdown)", priority = 0)]
    static void UIToolsDropdown()
    {
        Create(Resources.Load("Dropdown") as GameObject);
    }

    [MenuItem("UI Tools/Radio Button", menuItem = "GameObject/UI/Radio Button", priority = 1)]
    static void UIToolsRadioButton()
    {
        Create(Resources.Load("Radio Button") as GameObject);
    }

    [MenuItem("UI Tools/Option Selector", menuItem = "GameObject/UI/Option Selector", priority = 2)]
    static void UIToolsOptionSelector()
    {
        Create(Resources.Load("Option Selector") as GameObject);
    }

    [MenuItem("UI Tools/Progress Bar", menuItem = "GameObject/UI/Progress Bar", priority = 3)]
    static void UIToolsProgressbar()
    {
        Create(Resources.Load("Progress Bar") as GameObject);
    }
    
    [MenuItem("UI Tools/Listbox", menuItem = "GameObject/UI/Listbox", priority = 4)]
    static void UIToolsListbox()
    {
        Create(Resources.Load("Listbox") as GameObject);
    }

    [MenuItem("UI Tools/Tab Control", menuItem = "GameObject/UI/Tab Control", priority = 5)]
    static void UIToolsTabControl()
    {
        Create(Resources.Load("Tab Control") as GameObject);
    }

    [MenuItem("UI Tools/Table", menuItem = "GameObject/UI/Table", priority = 6)]
    static void UIToolsTabTable()
    {
        Create(Resources.Load("Table") as GameObject);
    }

    [MenuItem("UI Tools/GroupBox", menuItem = "GameObject/UI/Groupbox", priority = 7)]
    static void UIToolsGroupbox()
    {
        Create(Resources.Load("Groupbox") as GameObject);
    }
    
    [MenuItem("UI Tools/Extras/Radio Button Group", menuItem = "GameObject/UI/Extras/Radio Button Group", priority = 8)]
    static void UIToolsRadioGroup()
    {
        Create(Resources.Load("Radio Button Group") as GameObject);
    }

    [MenuItem("UI Tools/Extras/Message Box", menuItem = "GameObject/UI/Extras/Message Box", priority = 9)]
    static void UIToolsMessagebox()
    {
        Create(Resources.Load("Extentions/MessageBox Dialog") as GameObject);
    }

    [MenuItem("UI Tools/Extras/Tooltip", menuItem = "GameObject/UI/Extras/Tooltip", priority = 10)]
    static void UIToolsTooltip()
    {
        Create(Resources.Load("Extentions/Tooltips/Tooltip (+Icon)") as GameObject);
    }

    [MenuItem("UI Tools/Extras/Calendar", menuItem = "GameObject/UI/Extras/Calendar", priority = 11)]
    static void UIToolsCalendar()
    {
        Create(Resources.Load("Extentions/Calendar") as GameObject);
    }

    static void Create(GameObject uiTool)
    {
        GameObject ui = null;
        int count = FindObjectsOfType<GameObject>().Count(obj => ((obj.name.Contains('(')) ? obj.name.Substring(0, obj.name.LastIndexOf('(') - 1) : obj.name) == uiTool.name);

        if (Selection.activeGameObject != null && Selection.activeGameObject.activeInHierarchy)
        {
            if (Selection.activeGameObject.transform.parent != null)
            {
                //spawn on parent
                ui = Instantiate(uiTool, Selection.activeGameObject.transform.parent);
            }
            else
            {
                //spawn on object, add as child
                ui = Instantiate(uiTool, Selection.activeGameObject.transform);
            }
        }
        else
        {
            //spawn in hierarchy (anywhere), no parent
            ui = Instantiate(uiTool, FindObjectOfType<Canvas>().transform);
        }

        ui.name = uiTool.name + ((count > 0) ? " (" + count + ")" : ""); //add count to name
        Selection.activeGameObject = ui; //select object in Hierarchy
    }
}
