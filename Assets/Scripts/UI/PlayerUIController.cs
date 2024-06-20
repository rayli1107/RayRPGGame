using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private Image _iconNotice;
    [SerializeField]
    private Slider _sliderHP;
    [SerializeField]
    private Slider _sliderStamina;
    [SerializeField]
    private Slider _sliderExp;

    private PlayerGameUnit _playerData => GlobalGameData.Instance.PlayerData;

    private void OnEnable()
    {
        _playerData.updateAction += onStatUpdate;
        onStatUpdate();
    }

    private void OnDisable()
    {
        _playerData.updateAction -= onStatUpdate;
    }

    private void onStatUpdate()
    {
        if (_sliderHP.maxValue != _playerData.maxHp)
        {
            RectTransform rt = _sliderHP.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(_playerData.maxHp * 10, rt.sizeDelta.y);
            _sliderHP.maxValue = _playerData.maxHp;
        }
        _sliderHP.value = _playerData.hp;

        if (_sliderStamina.maxValue != _playerData.maxStamina)
        {
            RectTransform rt = _sliderStamina.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(_playerData.maxStamina * 20, rt.sizeDelta.y);
            _sliderStamina.maxValue = _playerData.maxStamina;
        }
        _sliderStamina.value = _playerData.stamina;

        _sliderExp.maxValue = _playerData.nextExp;
        _sliderExp.value = _playerData.exp;
    }

    private void OnTriggerEnter(Collider other)
    {
//        _iconNotice.enabled = true;

        TriggerController triggerController = other.GetComponent<TriggerController>();
        if (triggerController)
        {
            string message = triggerController.message;
            if (message.Length > 0)
            {
                GameUIManager.Instance.ShowQuickMessage(message);
            }
            Debug.Log(triggerController.icon);
            if (triggerController.icon != null)
            {
                _iconNotice.enabled = true;
                _iconNotice.sprite = triggerController.icon;
                Debug.Log(_iconNotice.sprite);
                _iconNotice.color = triggerController.iconColor;
                Debug.Log(_iconNotice.color);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
//        _iconNotice.enabled = false;
        if (other.GetComponent<TriggerController>() != null)
        {
            GameUIManager.Instance.HideQuickMessage();
            _iconNotice.enabled = false;
        }
    }
}
