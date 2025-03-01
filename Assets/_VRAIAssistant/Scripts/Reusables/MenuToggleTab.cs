using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Telkom
{
    public class MenuToggleTab : MonoBehaviour
    {
        [SerializeField]
        private Image _imageToggle;
        [SerializeField]
        private Sprite _spriteOff;
        [SerializeField]
        private Sprite _spriteOn;
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private UnityEvent<bool> _eventOnChangeToggle;
        private bool _isOn;

        public void SetToggle()
        {
            if (_isOn)
            {
                SetDisable();
            } else
            {
                SetEnable();
            }
        }

        public void SetEnableDisable(bool isEnable)
        {
            if (isEnable)
            {
                SetEnable();
            }
            else
            {
                SetDisable();
            }
        }

        public void SetEnable()
        {
            SetEnableVisual();
            _eventOnChangeToggle?.Invoke(true);
        }

        public void SetDisable()
        {
            SetDisableVisual();
            _eventOnChangeToggle?.Invoke(false);
        }

        public void SetEnableVisual()
        {
            _imageToggle.sprite = _spriteOn;
            _text.text = "ON";
            _isOn = true;
        }

        public void SetDisableVisual()
        {
            _imageToggle.sprite = _spriteOff;
            _text.text = "OFF";
            _isOn = false;
        }
    }
}
