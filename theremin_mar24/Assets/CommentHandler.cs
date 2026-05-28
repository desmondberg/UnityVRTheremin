using TMPro;
using UnityEngine;

public class CommentHandler : MonoBehaviour
{
    [SerializeField] private AuthManager auth;
    [SerializeField] private ViewPresetHandler preset;
    [SerializeField] private DatabaseHandler handler;


    public TMP_InputField commentField;
    public TMP_Text debugText;

    public void Awake()
    {
        if (auth)
        {
            auth.onLoggedIn += addComment;
        }
    }

    public void addComment(bool loggedIn)
    {
        if (loggedIn)
        {
            if (string.IsNullOrWhiteSpace(commentField.text))
            {
                debugText.text = "comment box is empty";
            }
            else if (!string.IsNullOrWhiteSpace(auth.jwtToken))
            {
                CommentOnPresetRequest request = new(preset.currentPreset._id, commentField.text);
                StartCoroutine(handler.CommentOnPreset(request));
            }
        }
    }
}
