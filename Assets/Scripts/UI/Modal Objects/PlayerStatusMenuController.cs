using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusMenuController : MonoBehaviour
{
    [SerializeField]
    private Image _playerFace;

    [SerializeField]
    private TextMeshProUGUI _labelName;
    [SerializeField]
    private TextMeshProUGUI _labelLevel;
    [SerializeField]
    private TextMeshProUGUI _labelExp;

    [SerializeField]
    private Button _buttonAddHP;
    [SerializeField]
    private Button _buttonRemoveHP;
    [SerializeField]
    private TextMeshProUGUI _labelHP;

    [SerializeField]
    private Button _buttonAddStamina;
    [SerializeField]
    private Button _buttonRemoveStamina;
    [SerializeField]
    private TextMeshProUGUI _labelStamina;

    [SerializeField]
    private Button _buttonAddAttack;
    [SerializeField]
    private Button _buttonRemoveAttack;
    [SerializeField]
    private TextMeshProUGUI _labelAttack;


    private PlayerController _playerController => GameController.Instance.player;
    private PlayerGameUnit _playerData => GlobalGameData.Instance.gameData.playerData;

    private int _allocatedHP;
    private int _allocatedStamina;
    private int _allocatedAttack;
    private int _spentExp;
    private int _actualHP => _playerData.maxHp + _allocatedHP * ProgressionUtil.statHpMultiplier;
    private int _actualStamina => _playerData.maxStamina + _allocatedStamina * ProgressionUtil.statStaminaMultiplier;
    private int _actualAttack => _playerData.attack + _allocatedAttack * ProgressionUtil.statAttackMultiplier;
    private int _actualLevel => _playerData.level + _allocatedAttack + _allocatedHP + _allocatedStamina;
    private int _actualExp => _playerData.exp - _spentExp;

    private void OnEnable()
    {
        if (_playerFace != null)
        {
            _playerFace.sprite = _playerController.face;
        }

        if (_labelName != null)
        {
            _labelName.text = _playerController.name;
        }

        _allocatedHP = 0;
        _allocatedStamina = 0;
        _allocatedAttack = 0;
        _spentExp = 0;
        updateStatus();
    }

    private void OnDisable()
    {
        _playerData.maxHp = _actualHP;
        _playerData.hp = _actualHP;
        _playerData.maxStamina = _actualStamina;
        _playerData.stamina = _actualStamina;
        _playerData.attack = _actualAttack;
        _playerData.level = _actualLevel;
        _playerData.exp = _actualExp;
    }

    private void updateStatus()
    {
        int nextLevelExp = ProgressionUtil.GetNextLevelExp(_actualLevel);

        if (_labelLevel != null) _labelLevel.text = "Level: " + _actualLevel;
        if (_labelExp != null) _labelExp.text = "Exp: " + _actualExp + " / " + nextLevelExp;

        bool canAdd = _actualExp >= nextLevelExp;
        if (_buttonAddHP != null) _buttonAddHP.gameObject.SetActive(canAdd);
        if (_buttonAddStamina != null) _buttonAddStamina.gameObject.SetActive(canAdd);
        if (_buttonAddAttack != null) _buttonAddAttack.gameObject.SetActive(canAdd);
        if (_buttonRemoveHP != null) _buttonRemoveHP.gameObject.SetActive(_allocatedHP > 0);
        if (_buttonRemoveStamina != null) _buttonRemoveStamina.gameObject.SetActive(_allocatedStamina > 0);
        if (_buttonRemoveAttack != null) _buttonRemoveAttack.gameObject.SetActive(_allocatedAttack > 0);

        if (_labelHP != null) _labelHP.text = _actualHP.ToString();
        if (_labelStamina != null) _labelStamina.text = _actualStamina.ToString();
        if (_labelAttack != null) _labelAttack.text = _actualAttack.ToString();
    }

    private void updateExp(bool increase)
    {
        if (increase)
        {
            _spentExp += ProgressionUtil.GetNextLevelExp(_actualLevel);
        }
        else
        {
            _spentExp -= ProgressionUtil.GetPreviousLevelExp(_actualLevel);
        }
    }

    public void ChangeHP(bool increase)
    {
        updateExp(increase);
        _allocatedHP += increase ? 1 : -1;
        updateStatus();
    }

    public void ChangeStamina(bool increase)
    {
        updateExp(increase);
        _allocatedStamina += increase ? 1 : -1;
        updateStatus();
    }

    public void ChangeAttack(bool increase)
    {
        updateExp(increase);
        _allocatedAttack += increase ? 1 : -1;
        updateStatus();
    }
}
