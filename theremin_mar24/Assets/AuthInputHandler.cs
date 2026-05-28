using System.Collections.Generic;
using TMPro;
using UnityEngine;



public static class Validate
{
    public static bool requiredField(string text, string fieldName, List<string> errormsg)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            errormsg.Add($"{fieldName} is required");
            return false;
        }
        return true;
    }
}
public class AuthInputHandler : MonoBehaviour
{
    [SerializeField] private AuthManager auth;

    //login input fields
    public TMP_InputField loginEmail;
    public TMP_InputField loginPass;

    //sign up input fields
    public TMP_InputField signupEmail;
    public TMP_InputField signupUsername;
    public TMP_InputField signupPass;

    private List<string> errorMessages;
    public TMP_Text debugText;

    public DatabaseHandler handler;
    public ToggleCanvasAndTheremin toggler;



    void Start()
    {
        if (auth)
        {
            auth.onLoggedIn += togglePanel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //debugText.text = $"{loginEmail.text}, {loginPass.text}";
    }

    public void signup()
    {
        errorMessages = new List<string>();
        string email = signupEmail.text;
        string username = signupUsername.text;
        string password = signupPass.text;

        if (errorMessages.Count > 0)
        {
            debugText.text = string.Join("\n", errorMessages);
        }
    }
    public void login()
    {
        errorMessages = new List<string>();
        string email = loginEmail.text;
        string password = loginPass.text;
        if (Validate.requiredField(loginEmail.text,"email",errorMessages) && Validate.requiredField(loginPass.text, "password", errorMessages))
        {
            debugText.text = $"sending user:{email} with pwd:{password}";
            StartCoroutine(handler.Login(email, password));
        }
        if (errorMessages.Count > 0)
        {
            debugText.text = string.Join("\n", errorMessages);
        }

    }

    public void togglePanel(bool loggedIn)
    {
        if (loggedIn)
        {
            toggler.Toggle();
        }
    }
}
