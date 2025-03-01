using UnityEngine;

public class NPCAI : MonoBehaviour
{
    Animator _animator;
    bool _isTalking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        StartAnimation(false);
    }

    public void StartAnimation(bool _isTalking)
    {
        this._isTalking = _isTalking;
        _animator.SetBool("talking", this._isTalking);
    }

    public void BeginSpeech()
    {
        StartAnimation(true);
        //spawn dialogue UI

    }
}
