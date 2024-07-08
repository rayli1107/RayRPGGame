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
    [SerializeField]
    private Slider _sliderStagger;
    [SerializeField]
    private Slider _sliderStaggerDuration;

    private EnemyGameUnit _gameUnit;
    public EnemyGameUnit gameUnit
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

    private void OnTriggerEnter(Collider other)
    {
        if (other == _player.targetCollider)
        {
            _imageTarget.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _player.targetCollider)
        {
            _imageTarget.enabled = false;
        }
    }

    private void OnStatUpdate()
    {
        if (_sliderHP != null)
        {
            _sliderHP.minValue = 0;
            _sliderHP.maxValue = gameUnit.HP.maxValue;
            _sliderHP.value = gameUnit.HP.value;
        }

        bool isStaggered = gameUnit.IsStaggered;

        if (_sliderStagger != null)
        {
            _sliderStagger.gameObject.SetActive(!isStaggered);
            if (!isStaggered)
            {
                _sliderStagger.minValue = gameUnit.Stagger.minValue;
                _sliderStagger.maxValue = gameUnit.Stagger.maxValue;
                _sliderStagger.value = gameUnit.Stagger.value;
            }
        }

        if (_sliderStaggerDuration != null)
        {
            _sliderStaggerDuration.gameObject.SetActive(isStaggered);
            if (isStaggered)
            {
                _sliderStaggerDuration.minValue = gameUnit.StaggerDuration.minValue;
                _sliderStaggerDuration.maxValue = gameUnit.StaggerDuration.maxValue;
                _sliderStaggerDuration.value = gameUnit.StaggerDuration.value;
            }
        }
    }
}
