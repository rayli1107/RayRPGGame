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

    private PlayerGameUnit _playerData => GlobalDataManager.Instance.gameData.playerData;
    private PlayerController _player => GameController.Instance.player;

    private void OnEnable()
    {
        _playerData.updateAction += onStatUpdate;
        onStatUpdate();
    }

    private void OnDisable()
    {
        _playerData.updateAction -= onStatUpdate;
    }

    private void Update()
    {
        if (_playerFace.sprite == null && _player != null)
        {
            _playerFace.sprite = _player.face;
        }
    }

    private void onStatUpdate()
    {
        if (_sliderHP != null)
        {
            if (_sliderHP.maxValue != _playerData.maxHp)
            {
                RectTransform rt = _sliderHP.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(_playerData.maxHp * 10, rt.sizeDelta.y);
                _sliderHP.maxValue = _playerData.maxHp;
            }
            _sliderHP.value = _playerData.hp;
        }

        if (_sliderStamina != null)
        {
            if (_sliderStamina.maxValue != _playerData.maxStamina)
            {
                RectTransform rt = _sliderStamina.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(_playerData.maxStamina * 20, rt.sizeDelta.y);
                _sliderStamina.maxValue = _playerData.maxStamina;
            }
            _sliderStamina.value = _playerData.stamina;
        }

        if (_sliderExp != null)
        {
            _sliderExp.maxValue = ProgressionUtil.GetNextLevelExp(_playerData.level);
            _sliderExp.value = _playerData.exp;
        }
    }
}
