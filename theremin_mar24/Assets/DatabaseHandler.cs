using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class LoginRequest
{
    public string email;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public string message;
    public string token;
}

[System.Serializable]
public class PresetResponse
{
    public string message;
    public Preset[] presets;
}


[System.Serializable]
public class SinglePresetResponse
{
    public string message;
    public Preset preset;
}

[System.Serializable]
public class Preset
{
    public string _id;
    public string title;
    public string description;

    public User author;

    public string[] tags;

    public Vote[] votes;

    public ApiScale scale;
    public Waveform waveform;
    public PitchCurve pitchcurve;

    public Comment[] comments;

    public string createdAt;
    public string updatedAt;

    public int __v;
}
[System.Serializable]
public class Vote
{
    public User user;
    public int value;
}
[System.Serializable]
public class User
{
    public string _id;
    public string username;
    public string email;

    public string createdAt;
    public string updatedAt;

    public int __v;
}

[System.Serializable]
public class ApiScale
{
    public string _id;
    public int rootNote;
    public string type;

    public int[] intervals;

    public string createdAt;
    public string updatedAt;

    public int __v;
}

[System.Serializable]
public class Waveform
{
    public string _id;
    public string type;

    public string createdAt;
    public string updatedAt;

    public int __v;
}

[System.Serializable]
public class PitchCurve
{
    public string _id;
    public string type;

    public string createdAt;
    public string updatedAt;

    public int __v;
}

[System.Serializable]
public class Comment
{
    public string content;
}


[System.Serializable]
public class ScaleRequest
{
    public string root;
    public string type;
}

[System.Serializable]
public class CreatePresetRequest
{
    public string title;
    public string description;

    public string[] tags;

    public string waveform;
    public string pitchcurve;

    public ScaleRequest scale;
}
[System.Serializable]
public class RatePresetRequest
{
    public string presetId;
    public string rating;
}
[System.Serializable]
public class CommentOnPresetRequest
{
    public string presetId;
    public string comment;

    public CommentOnPresetRequest(string id, string text)
    {
        this.presetId = id;
        this.comment = text;
    }
}

public class DatabaseHandler : MonoBehaviour
{
    [SerializeField] private AuthManager auth;

    private const string apiUrl = "https://uneccentrically-spinaceous-ed.ngrok-free.dev/api";
    public TextMeshProUGUI outputText;

    public TMP_Text debugText;


    public GameObject loggedInUI;
    public GameObject loggedOutUI;

    void Start()
    {
        //StartCoroutine(GetData());
        if (auth)
        {
            auth.onLoggedIn += togglePanel;
        }
    }

    void togglePanel(bool loggedIn) {
        loggedInUI.SetActive(loggedIn);
        loggedOutUI.SetActive(!loggedIn);
    }



    private string jwtToken;

    public Transform presetDropdownContent;
    public GameObject presetItem;

    public ViewPresetHandler viewPresetHandler;

    public IEnumerator Login(string email, string password) {
        LoginRequest body = new LoginRequest
        {
            email = email,
            password = password
        };

        string bodyJson = JsonUtility.ToJson(body);
        string url = $"{apiUrl}/users/login";

        UnityWebRequest request = new UnityWebRequest( url,"POST");
        request.SetRequestHeader("Content-Type", "application/json");

        //append json
        byte[] json = new System.Text.UTF8Encoding().GetBytes(bodyJson);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(json);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            LoginResponse response =
                JsonUtility.FromJson<LoginResponse>(
                    request.downloadHandler.text
                );

            jwtToken = response.token;
            auth.setToken(jwtToken);

            StartCoroutine(GetPresets());

            debugText.text = "login successful";
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError) {
            debugText.text = $"protocol error: {request.error}";
        }
        else
        {
            debugText.text = $"error: {request.error}";
        }
    }



    public IEnumerator GetPresets()
    {
        debugText.text = $"getting presets...";

        string url = $"{apiUrl}/presets";

        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", $"Bearer {auth.jwtToken}");
        yield return request.SendWebRequest();

        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            PresetResponse response =JsonUtility.FromJson<PresetResponse>(request.downloadHandler.text);
            debugText.text = $"{response.ToString()}";

            //delete everything
            foreach (Transform child in presetDropdownContent)
            {
                Destroy(child.gameObject);
            }

            foreach (Preset preset in response.presets)
            {
                GameObject item =Instantiate(presetItem,presetDropdownContent);
                PresetItem uiItem =item.GetComponent<PresetItem>();
                uiItem.Setup(preset, viewPresetHandler);
            }
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            debugText.text = $"protocol error: {request.error}";
        }
        else
        {
            debugText.text = $"error: {request.error}";
        }
    }
    public IEnumerator getPresetById(string presetId)
    {

        string url = $"{apiUrl}/presets/{presetId}";

        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", $"Bearer {auth.jwtToken}");
        yield return request.SendWebRequest();

        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            SinglePresetResponse response =
                JsonUtility.FromJson<SinglePresetResponse>(
                    request.downloadHandler.text
                );
            debugText.text = $"{response.ToString()}";
            viewPresetHandler.viewPreset(response.preset);
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            debugText.text = $"protocol error: {request.error}";
        }
        else
        {
            debugText.text = $"error: {request.error}";
        }
    }
    public IEnumerator CreatePreset(CreatePresetRequest preset)
    {
        string url = $"{apiUrl}/presets";
        string json =JsonUtility.ToJson(preset);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader( "Content-Type","application/json");
        request.SetRequestHeader("Authorization",$"Bearer {auth.jwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            debugText.text = $"{request.downloadHandler.text}";
        }
        else
        {
            debugText.text = $"{request.downloadHandler.text}";
        }
    }
    public IEnumerator RatePreset(RatePresetRequest preset)
    {
        string url = $"{apiUrl}/presets";
        string json = JsonUtility.ToJson(preset);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type","application/json");
        request.SetRequestHeader("Authorization", $"Bearer {auth.jwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            debugText.text = $"{request.downloadHandler.text}";
        }
        else
        {
            debugText.text = $"{request.downloadHandler.text}";
        }
    }
    public IEnumerator CommentOnPreset(CommentOnPresetRequest preset)
    {
        string url = $"{apiUrl}/presets";
        string json = JsonUtility.ToJson(preset);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type","application/json");
        request.SetRequestHeader("Authorization", $"Bearer {auth.jwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            debugText.text = $"{request.downloadHandler.text}";
        }
        else
        {
            debugText.text = $"{request.downloadHandler.text}";
        }
    }
}
