using System;
using UnityEngine;

public class GetPresets : MonoBehaviour
{
    [SerializeField] private AuthManager auth;
    [SerializeField] private DatabaseHandler handler;

    public void Awake()
    {
        if (auth)
        {
            auth.onLoggedIn += getPresets;
        }
    }

    public void getPresets(bool loggedIn)
    {
        if (loggedIn)
        {
            if (!string.IsNullOrWhiteSpace(auth.jwtToken))
            {
                StartCoroutine(handler.GetPresets());
            }
        }
    }
}
