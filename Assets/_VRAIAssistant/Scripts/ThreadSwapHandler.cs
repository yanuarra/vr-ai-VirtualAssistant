using System;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;


public class ThreadSwapHandler : MonoBehaviour
{
    public static ThreadSwapHandler instance;
    [SerializeField]
    private TMP_Text _countLabel;    
    private bool _useBackground = false;
    private Action _action;

    private async void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        await ExecuteThreadSwapping(Application.exitCancellationToken);
    }

    private async Awaitable ExecuteThreadSwapping(CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested)
                break;

            await Awaitable.BackgroundThreadAsync();

            if (_useBackground)
            {
                Debug.Log("IsBackground Thread");
                DoAction();
            }

            await Awaitable.MainThreadAsync();

            //if (!_useBackground)
            //{
            //    UnityEngine.Debug.Log("IsMainThreadAsync Thread");
            //    DoAction();
            //}
        }
    }

    public void ToggleUseBackground (bool state) => _useBackground = state;
    public void SetAction (Action action) => _action = action;
    public void UnSetAction () => _action = null;
    public void DoAction() {
        _action?.Invoke();
    }
}
