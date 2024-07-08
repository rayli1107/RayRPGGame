using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionUIController : MonoBehaviour
{
    [field: SerializeField]
    private Image _spriteAction;

    [field: SerializeField]
    private Image _imageFill;

    [field: SerializeField]
    private TextMeshProUGUI _labelActionIndex;

    [field: SerializeField]
    private TextMeshProUGUI _labelItemCount;

    private PlayerController _playerController => GameController.Instance.player;

    private PlayerActionController _playerAction;
    public PlayerActionController playerAction
    {
        get => _playerAction;
        set
        {
            if (_playerAction != value)
            {
                _playerAction = value;
                _spriteAction.sprite = playerAction.sprite;
                _labelItemCount.enabled = playerAction.hasStackableCount;
            }
        }
    }

    private int _playerActionIndex;
    public int playerActionIndex
    {
        get => _playerActionIndex;
        set
        {
            if (_playerActionIndex != value)
            {
                _playerActionIndex = value;
                _labelActionIndex.text = _playerActionIndex.ToString();
            }
        }
    }

    private Button _buttonSkill;
    private GameSessionData _gameData => GlobalDataManager.Instance.gameData;
    private void Awake()
    {
        _buttonSkill = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (playerAction != null)
        {
            playerAction.updateAction += onUpdate;
            _gameData.inventory.updateAction += onUpdate;
            onUpdate();
        }
    }

    private void OnDisable()
    {
        if (playerAction != null)
        {
            playerAction.updateAction -= onUpdate;
            _gameData.inventory.updateAction -= onUpdate;
        }
    }


    private void Update()
    {
    }

    private void onUpdate()
    {
        if (playerAction.coolDown > 0)
        {
            _imageFill.fillAmount = playerAction.cooldownRemaining / playerAction.coolDown;
        }
        if (playerAction.hasStackableCount)
        {
            _labelItemCount.text = playerAction.GetStackCount().ToString();
        }
        _buttonSkill.interactable = playerAction.IsAvailable(_playerController);
    }

    public void onClick()
    {
        playerAction.Trigger(_playerController);
    }
}
