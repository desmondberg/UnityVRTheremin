using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AuthManager", menuName = "Scriptable Objects/AuthManager")]
public class AuthManager : ScriptableObject
{
    public string jwtToken;
    public event Action<bool> onLoggedIn;

    private void Awake()
    {
        
    }

    public void setToken(string token)
    {
        jwtToken = token;
        onLoggedIn?.Invoke(true);
    }
}
