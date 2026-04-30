using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseHandler : MonoBehaviour
{
    public string apiUrl = "https://uneccentrically-spinaceous-ed.ngrok-free.dev/";
    public TextMeshProUGUI outputText;

    void Start()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            outputText.text = "API Response:\n" + response;
        }
        else
        {
            outputText.text = "API Error:\n" + request.error;
        }
    }
}
