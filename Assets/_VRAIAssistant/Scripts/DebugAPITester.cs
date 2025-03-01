using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugAPITester : MonoBehaviour
{
    [SerializeField]
    Button _btnPostData;
    [SerializeField]
    TMP_InputField _textField;

    private void Start()
    {
        if (_btnPostData ==null)
            _btnPostData = GetComponentInChildren<Button>();
        if (_textField == null)
            _textField = GetComponentInChildren<TMP_InputField>();
        _btnPostData.onClick.RemoveAllListeners();
        _btnPostData.onClick.AddListener(delegate {
            //BotResponseDataHandler.Instance.GetResponseData("sad" );
        });
    }
}
