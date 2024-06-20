using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    [SerializeField]
    private Image _imageTarget;
    [SerializeField]
    private Slider _sliderHP;

    private BaseGameUnit _gameUnit;
    public BaseGameUnit gameUnit
    {
        get => _gameUnit;
        set
        {
            if (_gameUnit != null)
            {
                _gameUnit.updateAction -= OnStatUpdate;
            }
            _gameUnit = value;
            if (_gameUnit != null)
            {
                _gameUnit.updateAction += OnStatUpdate;
                OnStatUpdate();
            }
        }
    }

    private PlayerController _player => GameController.Instance.player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _player.attackHitBox)
        {
            _imageTarget.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _player.attackHitBox)
        {
            _imageTarget.enabled = false;
        }
    }

    private void OnStatUpdate()
    {
        _sliderHP.minValue = 0;
        _sliderHP.maxValue = gameUnit.maxHp;
        _sliderHP.value = gameUnit.hp;
    }
}
