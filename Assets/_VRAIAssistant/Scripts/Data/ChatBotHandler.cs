using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class ChatBotHandler : Singleton<ChatBotHandler>
{
    [SerializeField]
    ChatBotResponseData _response;
    [SerializeField]
    TMP_Text _responseText;

    public void GetResponseData(string sttResult)
    {
        APIManager.Instance.PostDataChatBot(sttResult, OnGetBotResponseDone);
        _responseText.text = "processing . . .";
    }

    private void OnGetBotResponseDone(string result)
    {
        if (StaticData.requestError)
        {
            GlobalValues.Instance.SetIsAIRunning(false);
            return;
        }

        GlobalValues.Instance.SetIsAIRunning(true);
        _response = JsonConvert.DeserializeObject<ChatBotResponseData>(result);
        string responseText = StringExtension.RemoveLineEndings(_response.data.output);
        _responseText.text = responseText;
        TTSHandler.Instance.DoTextToSpeech(responseText);
        
    }
}
