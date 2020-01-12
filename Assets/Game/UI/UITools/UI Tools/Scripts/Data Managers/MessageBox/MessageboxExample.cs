using UnityEngine;
using System.Collections;
using CustomMessageBox;
using UnityEngine.UI;
using System;

/// <summary>
/// Messagebox UI Tool - Example
/// **This file can be safely deleted without affecting asset dependacies. This is only an example script.**
/// Developed by Dibbie.
/// Website: http://www.simpleminded.x10host.com
/// Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// </summary>

[AddComponentMenu("UI Tools/Messagebox", 7), DisallowMultipleComponent]
public class MessageboxExample : MonoBehaviour
{
    public GameObject msgBox;
    public float timer = 5f;

    float defaultTime;
    bool start = false;
    bool afk = false;

    MessageBox box;

    // Use this for initialization
    void Awake()
    {
        defaultTime = timer;
        MessageBox.Initalize(msgBox);
        msgBox.SetActive(false);
    }

    private void Update()
    {
        #region Timer logic
        if (start && box != null)
        {
            timer -= Time.deltaTime;
            if (timer >= 0) { box.UpdateButtonText(MessageBoxCustomButtons.Button1, " (" + timer.ToString("00") + ")"); }
            else { ResetTimer(); box.LockButtonInteraction(MessageBoxCustomButtons.Button1, false); box.UpdateButtonText(MessageBoxCustomButtons.Button1, "Try Again"); }
        }
        else if (afk && box != null)
        {
            timer -= Time.deltaTime;
            if (timer >= 0) { box.UpdateButtonText(MessageBoxCustomButtons.Button1, " (" + timer.ToString("0") + ")"); }
            else { ResetTimer(); print("You were AFK for too long and were logged out."); QuitEditor(); }
        }
        #endregion
    }

    private void ResetTimer()
    {
        timer = defaultTime;
        start = false;
        afk = false;
    }

    private void Start()
    {
        /*COMMENT OUT A SECTION OF CODE BELOW TO SEE AN EXAMPLE OF THE MESSAGE BOX SYSTEM*/

        ///Display a simple message box with just text and a default button, no additional logic or actions
        //box = MessageBox.Show("You have logged in, and recieved your daily reward!", "Every-Day Login event", MessageBoxButtons.OK);

        ///Display a simple message box with a single action to handle logic on both buttons
        box = MessageBox.Show("Do you really want to exit the game?", "Exit Game", MessageBoxButtons.YesNo, ExitGame);

        ///Display a message box with a delegate
        //box = MessageBox.Show("This is a message box...?", "Message Box", MessageBoxButtons.YesNo, delegate { MsgBoxEvent(MessageBox.response); });

        ///Display a message box with an event for each button
        //box = MessageBox.Show("Do you want to continue with this action? It could have consequences, I guess.", "Committing an Action", Yes, Cancel, No);

        ///Display a message box with custom buttons
        //CustomButton answer1 = new CustomButton(null), answer2 = new CustomButton(null), answer3 = new CustomButton(null);
        //answer1.buttonAction = Answer1; answer1.buttonText = "A Facebook Post";
        //answer2.buttonAction = Answer2; answer2.buttonText = "A secret";
        //answer3.buttonAction = Answer3; answer3.buttonText = "A wish";
        //box = MessageBox.Show("IF YOU HAVE ME, YOU WILL WANT TO SHARE ME. IF YOU SHARE ME, YOU WILL NO LONGER HAVE ME. WHAT AM I?", "Riddle Me This...", answer1, answer2, answer3);

        ///Display a message box with a timer and button lock
        //start = true;
        //box = MessageBox.Show("Too many failed login attempts, you can try again in a short bit.", "Anti-Login Spam System", new CustomButton(TryAgain, "Try Again"), new CustomButton(QuitEditor, "Exit"));
        //box.LockButtonInteraction(MessageBoxCustomButtons.Button1, true);

        ///Display a message box with a timer and call an event at 0
        //afk = true;
        //box = MessageBox.Show("You are AFK, and will be auto-kicked if you dont respond soon.", "AFK Timer", new CustomButton(UnsetAFKStatus, "Respond"));
    }

    #region Custom Message Box Buttons - Events
    void Answer1()
    {
        print("HAH! Good meme. Close, but no.");
    }

    void Answer2()
    {
        print("Correct! You often want to tell people secrets, but in doing that, it is no longer a secret if everyone knows it.");
    }

    void Answer3()
    {
        print("I can see how you thought that... Close, but no.");
    }

    void MsgBoxEvent(MessageBoxResponse response)
    {
        print("Message box response: " + response);
    }
    #endregion

    #region Default Message Box Buttons - Events
    void Yes()
    {
        print("Okay, good luck then I guess.");
    }

    void Cancel()
    {
        print("Atta-boi!");
    }

    void No()
    {
        print("Well, no need to be so formal.");
    }
    #endregion

    #region Timer events - MessageBox calls
    void UnsetAFKStatus()
    {
        print("AFK timer disabled.");
    }

    void TryAgain()
    {
        print("Alright, lets try this again... 204th's time the charm, right?");
    }

    void ExitGame()
    {
        if (MessageBox.response == MessageBoxResponse.Yes) { print("..."); Invoke("ActuallyExit", 0.1f); }
        else { print("Fine, be that way..."); }
    }

    void ActuallyExit()
    {
        MessageBox.Show("Thanks for playing this awesome game!", "Thanks Player", MessageBoxButtons.OK, QuitEditor);
    }

#if UNITY_EDITOR
    void QuitEditor()
    {
        if (Application.isEditor) { UnityEditor.EditorApplication.isPlaying = false; }
    }
#endif
    #endregion
}
