using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Message Box UI Tool - Data Class
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

namespace CustomMessageBox
{
    public enum MessageBoxResponse { None, Yes, No, Cancel, OK, Custom };
    public enum MessageBoxButtons { None, YesNo, YesNoCancel, OK, Custom };
    public enum MessageBoxCustomButtons { Button1, Button2, Button3 };

    public class CustomButton
    {
        public Action buttonAction;
        public string buttonText;
        public ButtonLocation location;

        public enum ButtonLocation { Default, Left, Right, Center };

        public CustomButton(Action listenEvent, string buttonTxt = "", ButtonLocation buttonLocation = ButtonLocation.Default)
        { buttonAction = listenEvent; buttonText = buttonTxt; location = buttonLocation; }
    };

    public class MessageBox
    {
        /// <summary>
        /// The response returned after the event has fired on the button.
        /// </summary>
        public static MessageBoxResponse response;

        /// <summary>
        /// The GameObject this MessageBox logic is being applied to.
        /// </summary>
        public static GameObject gameObject;

        static Action BtnActionYes, BtnActionNo, BtnActionCancel, BtnActionOK;
        static GameObject msgBox;
        static Text header, body;
        static Button btnYes, btnNo, btnCancel, btnOK;

        static Button btnCustom1, btnCustom2, btnCustom3;
        
        public static void Initalize(GameObject static_MsgBox)
        {
            msgBox = (GameObject)MonoBehaviour.Instantiate(static_MsgBox, static_MsgBox.transform.position, static_MsgBox.transform.rotation);
            msgBox.transform.SetParent(static_MsgBox.transform.parent);
            msgBox.GetComponent<RectTransform>().sizeDelta = static_MsgBox.GetComponent<RectTransform>().sizeDelta;
            msgBox.name = "MessageBox Dialog " + static_MsgBox.transform.parent.childCount;
            header = msgBox.transform.GetChild(0).GetComponent<Text>();
            body = msgBox.transform.GetChild(1).GetComponent<Text>();

            btnYes = msgBox.transform.GetChild(2).GetComponent<Button>();
            btnNo = msgBox.transform.GetChild(3).GetComponent<Button>();
            btnCancel = msgBox.transform.GetChild(4).GetComponent<Button>();
            btnOK = msgBox.transform.GetChild(5).GetComponent<Button>();

            msgBox.SetActive(false);
        }
        
        #region Public Functions
        /// <summary>
        /// Update the text of a specified button on the message box display.
        /// </summary>
        /// <param name="btn">The message box button to affect</param>
        /// <param name="newText">The text to set to this button</param>
        public void UpdateButtonText(MessageBoxCustomButtons btn, string newText)
        {
            if (btn == MessageBoxCustomButtons.Button1)
            {
                if (btnCustom1.gameObject.activeSelf) { btnCustom1.gameObject.transform.GetChild(0).GetComponent<Text>().text = newText; }
            }
            else if (btn == MessageBoxCustomButtons.Button2)
            {
                if (btnCustom2.gameObject.activeSelf) { btnCustom2.gameObject.transform.GetChild(0).GetComponent<Text>().text = newText; }
            }
            else if (btn == MessageBoxCustomButtons.Button3)
            {
                if (btnCustom3.gameObject.activeSelf) { btnCustom3.gameObject.transform.GetChild(0).GetComponent<Text>().text = newText; }
            }
        }

        /// <summary>
        /// Set the lock state of a specified button on the message box display.
        /// </summary>
        /// <param name="btn">The message box button to affect</param>
        /// <param name="lockButton">Should this button be locked/unclickable or unlocked/clickable</param>
        public void LockButtonInteraction(MessageBoxCustomButtons btn, bool lockButton = true)
        {
            if (btn == MessageBoxCustomButtons.Button1)
            {
                if (btnCustom1.gameObject.activeSelf) { btnCustom1.interactable = !lockButton; }
            }
            else if (btn == MessageBoxCustomButtons.Button2)
            {
                if (btnCustom2.gameObject.activeSelf) { btnCustom2.interactable = !lockButton; }
            }
            else if (btn == MessageBoxCustomButtons.Button3)
            {
                if (btnCustom3.gameObject.activeSelf) { btnCustom3.interactable = !lockButton; }
            }
        }

        /// <summary>
        /// Get the center of the screen where this message box should sit, taking in account its size.
        /// </summary>
        public Vector2 GetCenter()
        {
            float width = (msgBox.transform.parent.GetComponent<RectTransform>().rect.xMax / 2f) - msgBox.GetComponent<RectTransform>().rect.width / 2f;
            float height = (msgBox.transform.parent.GetComponent<RectTransform>().rect.yMax / 2f) - msgBox.GetComponent<RectTransform>().rect.height / 2f;
            msgBox.transform.localPosition = new Vector2(width, height);

            return msgBox.transform.localPosition;
        }

        /// <summary>
        /// Get or set the position of this message box on the Canvas.
        /// </summary>
        public Vector2 Position
        {
            get { return msgBox.transform.localPosition; }
            set { msgBox.transform.localPosition = value; }
        }

        /// <summary>
        /// Get or set the parent of this message box on the Canvas.
        /// </summary>
        public Transform Parent
        {
            get { return msgBox.transform.parent; }
            set { msgBox.transform.SetParent(value); }
        }
        #endregion

        #region "Show" Overloads (Static Functions)
        /// <summary>
        /// Actually show the message box.
        /// </summary>
        /// <param name="message">Text to display in the body of the box</param>
        /// <param name="title">Text to display in the header of the box</param>
        /// <param name="response">Buttons to display on this box</param>
        /// <returns></returns>
        public static MessageBox Show(string message, string title = "", MessageBoxButtons response = MessageBoxButtons.OK)
        {
            ToggleMessageBoxDisplay(true, message, title, response);

            if (response == MessageBoxButtons.OK) { BtnActionOK = DefaultActionCancel; }
            else if (response == MessageBoxButtons.YesNo) { BtnActionYes = DefaultActionCancel; BtnActionNo = DefaultActionCancel; }
            else if (response == MessageBoxButtons.YesNoCancel) { BtnActionYes = DefaultActionCancel; BtnActionNo = DefaultActionCancel; BtnActionCancel = DefaultActionCancel; }

            gameObject = msgBox;
            return new MessageBox();
        }

        /// <summary>
        /// Actually show the message box.
        /// </summary>
        /// <param name="message">Text to display in the body of the box</param>
        /// <param name="title">Text to display in the header of the box</param>
        /// <param name="response">Buttons to display on this box</param>
        /// <param name="listenEvent">Event to fire when the response button is clicked</param>
        /// <returns></returns>
        public static MessageBox Show(string message, string title = "", MessageBoxButtons response = MessageBoxButtons.OK, Action listenEvent = null)
        {
            if (listenEvent == null) { listenEvent = BtnActionOK; }

            ToggleMessageBoxDisplay(true, message, title, response);

            if (response == MessageBoxButtons.OK) { BtnActionOK = listenEvent; BtnActionOK += DefaultActionCancel; }
            else if (response == MessageBoxButtons.YesNo) { BtnActionYes = listenEvent; BtnActionYes += DefaultActionCancel; BtnActionNo = listenEvent; BtnActionNo += DefaultActionCancel; }
            else if (response == MessageBoxButtons.YesNoCancel) { BtnActionYes = listenEvent; BtnActionYes += DefaultActionCancel; BtnActionNo = listenEvent; BtnActionNo += DefaultActionCancel; BtnActionCancel = listenEvent; BtnActionCancel += DefaultActionCancel; }

            gameObject = msgBox;
            return new MessageBox();
        }

        /// <summary>
        /// Actually show the message box.
        /// </summary>
        /// <param name="message">Text to display in the body of the box</param>
        /// <param name="title">Text to display in the header of the box</param>
        /// <param name="option1">Event to fire when the first (center) button is clicked</param>
        /// <param name="option2">Event to fire when the first (left) button is clicked</param>
        /// <param name="option3">Event to fire when the first (right) button is clicked</param>
        /// <returns></returns>
        public static MessageBox Show(string message, string title = "", Action option1 = null, Action option2 = null, Action option3 = null)
        {
            int options = 0;

            if (option1 == null) { option1 = BtnActionOK; }
            options = ((option1 != null) ? 1 : 0) + ((option2 != null) ? 1 : 0) + ((option3 != null) ? 1 : 0);
            ToggleMessageBoxDisplay(true, message, title, MessageBoxButtons.Custom, options);

            if (option1 != null && option2 == null && option3 == null) { BtnActionOK = option1; BtnActionOK += DefaultActionCancel; }
            else if (option1 != null && option2 != null && option3 == null) { BtnActionYes = option1; BtnActionYes += DefaultActionCancel; BtnActionNo = option2; BtnActionNo += DefaultActionCancel; }
            else if (option1 != null && option2 != null && option3 != null) { BtnActionYes = option1; BtnActionYes += DefaultActionCancel; BtnActionNo = option2; BtnActionNo += DefaultActionCancel; BtnActionCancel = option3; BtnActionCancel += DefaultActionCancel; }

            gameObject = msgBox;
            return new MessageBox();
        }

        /// <summary>
        /// Actually show the message box.
        /// </summary>
        /// <param name="message">Text to display in the body of the box</param>
        /// <param name="title">Text to display in the header of the box</param>
        /// <param name="option1">Details for the first (center) custom button option (optional)</param>
        /// <param name="option2">Details for the second (left) custom button option (optional)</param>
        /// <param name="option3">Details for the third (right) custom button option (optional)</param>
        /// <returns></returns>
        public static MessageBox Show(string message, string title = "", CustomButton option1 = null, CustomButton option2 = null, CustomButton option3 = null)
        {
            int options = 0;
            btnCustom1 = null;
            btnCustom2 = null;
            btnCustom3 = null;

            if (option1 == null) { option1.buttonAction = BtnActionOK; option1.buttonText = "OK"; }
            options = ((option1 != null) ? 1 : 0) + ((option2 != null) ? 1 : 0) + ((option3 != null) ? 1 : 0);



            ToggleMessageBoxDisplay(true, message, title, MessageBoxButtons.Custom, options);

            if (option1 != null && option2 == null && option3 == null) { btnCustom1 = btnOK; SetButtonText(btnOK, option1.buttonText, option1.location); BtnActionOK = option1.buttonAction; BtnActionOK += DefaultActionCancel; }
            else if (option1 != null && option2 != null && option3 == null) { btnCustom1 = btnYes; btnCustom2 = btnNo; SetButtonText(btnYes, option1.buttonText, option1.location); SetButtonText(btnNo, option2.buttonText, option2.location); BtnActionYes = option1.buttonAction; BtnActionYes += DefaultActionCancel; BtnActionNo = option2.buttonAction; BtnActionNo += DefaultActionCancel; }
            else if (option1 != null && option2 != null && option3 != null) { btnCustom1 = btnYes; btnCustom2 = btnCancel; btnCustom3 = btnNo; SetButtonText(btnYes, option1.buttonText, option1.location); SetButtonText(btnNo, option3.buttonText, option3.location); SetButtonText(btnCancel, option2.buttonText, option2.location); BtnActionYes = option1.buttonAction; BtnActionYes += DefaultActionCancel; BtnActionNo = option3.buttonAction; BtnActionNo += DefaultActionCancel; BtnActionCancel = option2.buttonAction; BtnActionCancel += DefaultActionCancel; }

            gameObject = msgBox;
            return new MessageBox();
        }
#endregion

        /// <summary>
        /// Called by MessageResponse on each button when clicked.
        /// </summary>
        /// <param name="actionResponse">Response of button clicked</param>
        public static void InvokeAction(MessageBoxResponse actionResponse)
        {
            response = actionResponse;
            if (actionResponse == MessageBoxResponse.Yes) { if(BtnActionYes != null) BtnActionYes.Invoke(); }
            else if (actionResponse == MessageBoxResponse.No) { if (BtnActionNo != null) BtnActionNo.Invoke(); }
            else if (actionResponse == MessageBoxResponse.Cancel) { if (BtnActionCancel != null) BtnActionCancel.Invoke(); }
            else if (actionResponse == MessageBoxResponse.OK) { if (BtnActionOK != null) BtnActionOK.Invoke(); }
        }

        #region Helper Functions
        static void SetButtonText(Button btn, string txt, CustomButton.ButtonLocation loc = CustomButton.ButtonLocation.Default)
        {
            if (btn == null) { return; }
            if (string.IsNullOrEmpty(txt))
            {
                if (btn == btnOK) { txt = "OK"; }
                else if (btn == btnYes) { txt = "Yes"; }
                else if (btn == btnNo) { txt = "No"; }
                else if (btn == btnCancel) { txt = "Cancel"; }
            }

            btn.gameObject.transform.GetChild(0).GetComponent<Text>().text = txt;

            float maxWidth = msgBox.GetComponent<RectTransform>().rect.xMax;
            float btnWidth = btn.GetComponent<RectTransform>().rect.width;

            if (btn == btnOK) { btn.transform.localPosition = new Vector2((maxWidth / 2f) - btnWidth, btn.transform.localPosition.y); }
            if (btn == btnYes) { btn.transform.localPosition = new Vector2(-btnWidth - 25f, btn.transform.localPosition.y); }
            if (btn == btnNo) { btn.transform.localPosition = new Vector2(btnWidth + 25f, btn.transform.localPosition.y); }
            if (btn == btnCancel) { btn.transform.localPosition = new Vector2((maxWidth / 2f) - btnWidth, btn.transform.localPosition.y); }

            if (loc == CustomButton.ButtonLocation.Left)
            {
                if (btn == btnOK) { btn.transform.localPosition = new Vector2(-btnWidth - 25f, btn.transform.localPosition.y); }
                if (btn == btnYes) { btn.transform.localPosition = new Vector2(-btnWidth - 25f, btn.transform.localPosition.y); }
                if (btn == btnNo) { btn.transform.localPosition = new Vector2(-btnWidth - 25f, btn.transform.localPosition.y); }
                if (btn == btnCancel) { btn.transform.localPosition = new Vector2(-btnWidth - 25f, btn.transform.localPosition.y); }
            }
            else if (loc == CustomButton.ButtonLocation.Right)
            {
                if (btn == btnOK) { btn.transform.localPosition = new Vector2(btnWidth + 25f, btn.transform.localPosition.y); }
                if (btn == btnYes) { btn.transform.localPosition = new Vector2(btnWidth + 25f, btn.transform.localPosition.y); }
                if (btn == btnNo) { btn.transform.localPosition = new Vector2(btnWidth + 25f, btn.transform.localPosition.y); }
                if (btn == btnCancel) { btn.transform.localPosition = new Vector2(btnWidth + 25f, btn.transform.localPosition.y); }
            }
            else if (loc == CustomButton.ButtonLocation.Center)
            {
                if (btn == btnOK) { btn.transform.localPosition = new Vector2((maxWidth / 2f) - btnWidth, btn.transform.localPosition.y); }
                if (btn == btnYes) { btn.transform.localPosition = new Vector2((maxWidth / 2f) - btnWidth, btn.transform.localPosition.y); }
                if (btn == btnNo) { btn.transform.localPosition = new Vector2((maxWidth / 2f) - btnWidth, btn.transform.localPosition.y); }
                if (btn == btnCancel) { btn.transform.localPosition = new Vector2((maxWidth / 2f) - btnWidth, btn.transform.localPosition.y); }
            }
        }

        static void ToggleMessageBoxDisplay(bool show, string msg = "", string title = "", MessageBoxButtons response = MessageBoxButtons.None, int options = 0)
        {
            msgBox.SetActive(show);
            header.text = title;
            body.text = msg;

            btnYes.gameObject.SetActive(false);
            btnNo.gameObject.SetActive(false);
            btnCancel.gameObject.SetActive(false);
            btnOK.gameObject.SetActive(false);

            if (response == MessageBoxButtons.YesNo) { btnYes.gameObject.SetActive(true); btnNo.gameObject.SetActive(true); }
            if (response == MessageBoxButtons.YesNoCancel) { btnYes.gameObject.SetActive(true); btnNo.gameObject.SetActive(true); btnCancel.gameObject.SetActive(true); }
            if (response == MessageBoxButtons.OK) { btnOK.gameObject.SetActive(true); }
            if (response == MessageBoxButtons.Custom) { if (options == 1) { btnOK.gameObject.SetActive(true); } else if (options == 2) { btnYes.gameObject.SetActive(true); btnNo.gameObject.SetActive(true); } else if (options == 3) { btnYes.gameObject.SetActive(true); btnNo.gameObject.SetActive(true); btnCancel.gameObject.SetActive(true); } }
        }

        static void DefaultActionCancel()
        {
            ToggleMessageBoxDisplay(false);
        }
        #endregion
    }
}
