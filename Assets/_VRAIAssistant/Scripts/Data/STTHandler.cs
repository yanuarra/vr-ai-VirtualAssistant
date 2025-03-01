using Newtonsoft.Json;
using OpenAI;
using TMPro;
using UnityEngine;

public class STTHandler : Singleton<STTHandler>
{
    [SerializeField]
    STTResponseData _response;
    [SerializeField]
    TMP_Text _STTText;

    public void SetSTTText(string STTText)
    {
        if (_STTText != null)
            _STTText.text = STTText;
    }

    public void OnGetSTTResponseDone(string result)
    {
        if (StaticData.requestError)
        {
            GlobalValues.Instance.SetIsAIRunning(false);
            return;
        }
        _response = JsonConvert.DeserializeObject<STTResponseData>(result);
        SetSTTText(_response.text);
        ChatBotHandler.Instance.GetResponseData(_response.text);
    }
}
