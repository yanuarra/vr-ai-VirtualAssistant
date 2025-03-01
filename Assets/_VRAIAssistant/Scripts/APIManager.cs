using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : Singleton<APIManager>
{
    public bool isInProcess = false;
    private string _rootUrlChatBot = "https://api.interfacechatbotapplication.chat/app/v1/ask-bot";
    private string _tokenChatBot = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
        "eyJhcHBJZCI6IjIzNjczNGE1LTdmMzctNDg1OC04YzI1LWQxZDg5ZTViMmI4NiIsImlhdCI6MTczODgwODQ4MywiZXhwIjo0ODYzMDEwODgzfQ." +
        "XTj8wETb4cHYsj1f6xPrMglrjrcfHsmSiEOJEU-7Ji0";
    private string _rootUrlSTT = "https://stt-dev.eadds.dev/v1/inference";
      private string _tokenSTT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
        "eyJhcHBJZCI6ImQ0YmViZDQxLThjZDctNDY5Ni1iYThlLWE3MTVkMzg3MGM0OSIsImlhdCI6MTc0MDcxMzE0MiwiZXhwIjo0ODY0OTE1NTQyfQ." +
        "5kjcYY4TMDqXKZGSfWBHfNELxgyGLzSXs3KO7LCIYEE";
    private string _rootUrlTTS = "https://telkom-ai-dag.api.apilogy.id/Text_To_Speech/0.0.2";
    private string _APIKey = "DF5SSD8JChipIsXw1dy6HsEKTljCEbDu";
    
    private Action<string> dataEvent;
    private Action<AudioClip> audioClipEvent;
    private string uri;
    public void SetRootURLChatBot(string url) => _rootUrlChatBot = url;
    public string GetRootURLChatBot()
    {
        return _rootUrlChatBot;
    }
    public string GetTokenChatBot()
    {
        return _tokenChatBot;
    }
    public void SetRootURLSTT(string rootUrl) => _rootUrlSTT = rootUrl;
    public string GetRootURLSTT()
    {
        return _rootUrlSTT;
    }
    public void SetRootURLTTS(string rootUrl) => _rootUrlTTS = rootUrl;
    public string GetRootURLTTS()
    {
        return _rootUrlTTS;
    }

    //Dummy
    private string username = "ilham";
    private string _message = " Tolong beri saya info seputar Bank LPS.\r\n";

    //----PUBLIC----//
    public void PostDataChatBot(string message, Action<string> SetDataEvent) {
        string text = StringExtension.RemoveLineEndings(message);
        string body = "{\"username\":\"" + username + "\",\"message\":\"" + text + "\"}";
        dataEvent = SetDataEvent;
        StartCoroutine(PostDataBodyCoroutine(_rootUrlChatBot, "", body, _tokenChatBot));
    }
     
    public void GetTTSData(string subUri, Action<string> SetDataEvent)
    {
        StartCoroutine(GetDataCoroutine(_rootUrlTTS, subUri, _APIKey, SetDataEvent));
    }

    public void GetTTSAudioResult(string subUri, Action<AudioClip> SetDataEvent)
    {
        StartCoroutine(GetAudioClipCoroutine(_rootUrlTTS, subUri, _APIKey, SetDataEvent));
    }

    public void PostDataTTS(string subUri, string speakerName, string inputText, Action<string> SetDataEvent)
    {
        WWWForm form = new WWWForm();
        form.AddField("speaker_name", speakerName);
        form.AddField("input_text", inputText);
        string body = "{\"speaker_name\":\"" + speakerName + "\",\"input_text\":\"" + inputText + "\"}";
        dataEvent = SetDataEvent;
        StartCoroutine(PostDataFormCoroutine(_rootUrlTTS, subUri, form, body, _APIKey));
    }

    public void PostDataSTT(byte[] audioSourceBinary, Action<string> SetDataEvent)
    {
        dataEvent = SetDataEvent;
        StartCoroutine(PostDataBinaryCoroutine(_rootUrlSTT, "", audioSourceBinary, _tokenSTT));
    }
    
    //----PRIVATE----//
    private IEnumerator GetDataCoroutine(string rootUrl, string subUri,string apiKey, Action<string> SetDataEvent)
    {
        if (isInProcess) yield break;
        isInProcess = true;   
        subUri = !string.IsNullOrEmpty(subUri)? $"/{subUri}" : subUri;
        uri = string.Format("{0}{1}", rootUrl, subUri);

        dataEvent = SetDataEvent;
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        uwr.SetRequestHeader("accept", "application/json");
        uwr.SetRequestHeader("x-api-key", apiKey);
        yield return uwr.SendWebRequest();
        HandleUWRResult(uwr);
    }

    private IEnumerator GetAudioClipCoroutine(string rootUrl, string subUri, string apiKey, Action<AudioClip> SetAudioClipEvent)
    {
        if (isInProcess) yield break;
        isInProcess = true;
        subUri = !string.IsNullOrEmpty(subUri) ? $"/{subUri}" : subUri;
        uri = string.Format("{0}{1}", rootUrl, subUri);

        audioClipEvent = SetAudioClipEvent;
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.WAV);
        uwr.SetRequestHeader("accept", "application/json");
        uwr.SetRequestHeader("x-api-key", apiKey);
        yield return uwr.SendWebRequest();
        HandleUWRAudioClipResult(uwr);
    }

    private IEnumerator PostDataFormCoroutine(string rootUrl, string subUri, WWWForm form, string postData, string apiKey)
    {
        if (isInProcess) yield break;
        isInProcess = true;
        subUri = !string.IsNullOrEmpty(subUri) ? $"/{subUri}" : subUri;
        uri = string.Format("{0}{1}", rootUrl, subUri);
        //string json = JsonUtility.ToJson(form);
        //byte[] rawData = Encoding.UTF8.GetBytes(json);
        //UnityWebRequest uwr = UnityWebRequest.Post(uri, form);
        UnityWebRequest uwr = UnityWebRequest.Post(uri, postData, "application/json");
        //UnityWebRequest uwr = UnityWebRequest.PostWwwForm(uri, json);
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Accept", "application/json");
        uwr.SetRequestHeader("x-api-key", apiKey);

        yield return uwr.SendWebRequest();
        HandleUWRResult(uwr);
    }

    private IEnumerator PostDataBinaryCoroutine(string rootUrl, string subUri, byte[] sourceBinary, string token) {
        /*var formData = new List<IMultipartFormSection>()
        {
            new MultipartFormDataSection("response_format", "verbose_json"),
            new MultipartFormDataSection("language", "indonesian"),
            new MultipartFormDataSection("file", postData),
            new MultipartFormDataSection("to_wav", "true")
        };*/
        if (isInProcess) yield break;
        isInProcess = true;
        subUri = !string.IsNullOrEmpty(subUri) ? $"/{subUri}" : subUri;
        uri = string.Format("{0}{1}", rootUrl, subUri);

        byte[] data = sourceBinary;
        WWWForm form = new WWWForm();
        form.AddField("response_format", "verbose_json");
        form.AddBinaryData("file", data);

        UnityWebRequest uwr = UnityWebRequest.Post(rootUrl, form);
        uwr.SetRequestHeader("accept", "application/json");
        uwr.SetRequestHeader("Api-Key", _APIKey);
        //uwr.SetRequestHeader("Content-Type", "multipart/form-data");
        //uwr.SetRequestHeader("Authorization", "Bearer " + _tokenSTT);

        yield return uwr.SendWebRequest(); 
        HandleUWRResult(uwr);
    }

    private IEnumerator PostDataBodyCoroutine(string rootUrl, string subUri, string postData, string token)
    {
        if (isInProcess) yield break;
        isInProcess = true;
        /*
        var body = new StringContent("{\n    \"username\": \"ilham\",\n    \"message\": \"sebutkan asal usul malin kundang\"\n}", null, "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        var body = @"{" + "\n" +
           @"    ""username"": ""ilham""," + "\n" +
           @"    ""message"": ""sebutkan asal usul malin kundang""" + "\n" +
           @"}";   
        ////FORM
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("message", message);
        var body = new Data();
        body.username = username;
        body.message = message;
        string json = JsonUtility.ToJson(body);
        byte[] rawData = new UTF8Encoding().GetBytes(body);
        UnityWebRequest uwr = new UnityWebRequest(uri, "POST");
        UnityWebRequest uwr = UnityWebRequest.PostWwwForm(uri, json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawData);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Api-Key", _apiKey);
        */
        subUri = !string.IsNullOrEmpty(subUri)? $"/{subUri}" : subUri;
        uri = string.Format("{0}{1}", rootUrl, subUri);

        UnityWebRequest uwr = UnityWebRequest.Post(rootUrl, postData, "application/json");
        if (!string.IsNullOrEmpty (token))
            uwr.SetRequestHeader("Authorization", "Bearer " + token);
        
        yield return uwr.SendWebRequest(); 
        HandleUWRResult(uwr);
        /*switch (uwr.result)
        {
            case UnityWebRequest.Result.Success:
                StaticData.requestError = false;
                print("url : " + uri + ", respond : " + uwr.downloadHandler.text);
                SetDataEvent?.Invoke(uwr.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                if (uwr.responseCode != 200 && uwr.responseCode != 201)
                {
                    StaticData.apiError = true;
                    StaticData.errorMessage = "ProtocolError: " + uwr.error;
                }
                else
                {
                    StaticData.apiError = false;
                }
                Debug.Log("url : " + uri + ", respond : " + uwr.downloadHandler.text);
                StaticData.requestError = true;
                SetDataEvent?.Invoke(StaticData.errorMessage);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                if (uwr.responseCode != 200 && uwr.responseCode != 201)
                {
                    StaticData.apiError = true;
                    StaticData.errorMessage = "ErroDataProcessingErrorr: " + uwr.error;
                }
                else
                {
                    StaticData.apiError = false;
                }
                Debug.Log(UnityWebRequest.Result.DataProcessingError);
                StaticData.requestError = true;
                SetDataEvent?.Invoke(StaticData.errorMessage);
                break;
            case UnityWebRequest.Result.ConnectionError:
                StaticData.apiError = true;
                StaticData.errorMessage = "ConnectionError: " + uwr.error;
                StaticData.requestError = true;
                SetDataEvent?.Invoke(StaticData.errorMessage);
                break;
            default:
                break;
        }*/
    }

    private void HandleUWRResult(UnityWebRequest uwr)
    {
        isInProcess = false;
        switch (uwr.result)
        {
            case UnityWebRequest.Result.Success:
                StaticData.requestError = false;
                print("url : " + uri + ", respond : " + uwr.downloadHandler.text);
                dataEvent?.Invoke(uwr.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                if (uwr.responseCode != 200 && uwr.responseCode != 201)
                {
                    StaticData.apiError = true;
                    StaticData.errorMessage = "ProtocolError: " + uwr.error;
                }
                else
                {
                    StaticData.apiError = false;
                }
                Debug.Log("url : " + uri + ", respond : " + uwr.downloadHandler.text);
                StaticData.requestError = true;
                dataEvent?.Invoke(StaticData.errorMessage);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                if (uwr.responseCode != 200 && uwr.responseCode != 201)
                {
                    StaticData.apiError = true;
                    StaticData.errorMessage = "ErroDataProcessingErrorr: " + uwr.error;
                }
                else
                {
                    StaticData.apiError = false;
                }
                Debug.Log(UnityWebRequest.Result.DataProcessingError);
                StaticData.requestError = true;
                dataEvent?.Invoke(StaticData.errorMessage);
                break;
            case UnityWebRequest.Result.ConnectionError:
                StaticData.apiError = true;
                StaticData.errorMessage = "ConnectionError: " + uwr.error;
                StaticData.requestError = true;
                dataEvent?.Invoke(StaticData.errorMessage);
                break;
            default:
                break;
        }
    }

    private void HandleUWRAudioClipResult(UnityWebRequest uwr)
    {
        isInProcess = false;
        switch (uwr.result)
        {
            case UnityWebRequest.Result.Success:
                StaticData.requestError = false;
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(uwr);
                print("url : " + uri + ", respond : " + myClip.name);
                audioClipEvent?.Invoke(myClip);
                break;
            case UnityWebRequest.Result.ProtocolError:
                if (uwr.responseCode != 200 && uwr.responseCode != 201)
                {
                    StaticData.apiError = true;
                    StaticData.errorMessage = "ProtocolError: " + uwr.error;
                }
                else
                {
                    StaticData.apiError = false;
                }
                Debug.Log("url : " + uri + ", respond : " + StaticData.errorMessage);
                StaticData.requestError = true;
                break;
            case UnityWebRequest.Result.DataProcessingError:
                if (uwr.responseCode != 200 && uwr.responseCode != 201)
                {
                    StaticData.apiError = true;
                    StaticData.errorMessage = "ErroDataProcessingErrorr: " + uwr.error;
                }
                else
                {
                    StaticData.apiError = false;
                }
                Debug.Log("url : " + uri + ", respond : " + StaticData.errorMessage);
                StaticData.requestError = true;
                break;
            case UnityWebRequest.Result.ConnectionError:
                StaticData.apiError = true;
                StaticData.errorMessage = "ConnectionError: " + uwr.error;
                StaticData.requestError = true;
                Debug.Log("url : " + uri + ", respond : " + StaticData.errorMessage);
                break;
            default:
                break;
        }
    }
}