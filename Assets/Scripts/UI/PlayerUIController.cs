using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private Image _playerFace;
    [SerializeField]
    private Slider _sliderHP;
    [SerializeField]
    private Slider _sliderStamina;
    [SerializeField]
    private Slider _sliderExp;

    private PlayerData _playerData;
    public PlayerData playerData
    {
        get => _playerData;
        set
        {
            if (_playerData != null && value == null)
            {
                _playerData.updateAction -= onPlayerDataUpdate;
            }
            _playerData = value;
            if (_playerData != null)
            {
                _playerData.updateAction += onPlayerDataUpdate;
                onPlayerDataUpdate();
            }
        }
    }

    private PlayerController _playerController;
    public PlayerController playerController
    {
        get => _playerController;
        set
        {
            if (_playerController != null && value == null)
            {
                _playerController.playerUnit.updateAction -= onStatUpdate;
                if (_playerFace != null)
                {
                    _playerFace.sprite = null;
                }
            }
            _playerController = value;
            if (_playerController != null)
            {
                _playerController.playerUnit.updateAction += onStatUpdate;
                if (_playerFace != null)
                {
                    _playerFace.sprite = _playerController.face;
                }
                onStatUpdate();
            }
        }
    }

    private void onPlayerDataUpdate()
    {
        if (_sliderExp != null)
        {
            _sliderExp.maxValue = ProgressionUtil.GetNextLevelExp(_playerData.Level.value);
            _sliderExp.value = _playerData.Exp.value;
        }
    }

    private void onStatUpdate()
    {
        PlayerGameUnit playerUnit = _playerController.playerUnit;
        if (_sliderHP != null)
        {
            if (_sliderHP.maxValue != playerUnit.HP.maxValue)
            {
                RectTransform rt = _sliderHP.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(playerUnit.HP.maxValue * 10, rt.sizeDelta.y);
                _sliderHP.maxValue = playerUnit.HP.maxValue;
            }
            _sliderHP.value = playerUnit.HP.value;
        }

        if (_sliderStamina != null)
        {
            if (_sliderStamina.maxValue != playerUnit.Stamina.maxValue)
            {
                RectTransform rt = _sliderStamina.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(playerUnit.Stamina.maxValue * 20, rt.sizeDelta.y);
                _sliderStamina.maxValue = playerUnit.Stamina.maxValue;
            }
            _sliderStamina.value = playerUnit.Stamina.value;
        }

    }
}
