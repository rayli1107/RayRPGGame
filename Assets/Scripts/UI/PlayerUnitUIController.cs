using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitUIController : MonoBehaviour
{
    [SerializeField]
    private Image _iconNotice;
    [SerializeField]
    private Image _iconTalk;

    private PlayerController _player;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_player.GetCurrentNPCTarget() != null)
        {
            _iconTalk.enabled = true;
            _iconNotice.enabled = false;
        }
        else if (_player.hasTriggerController)
        {
            _iconNotice.sprite = _player.currentTriggerController.icon;
            _iconNotice.color = _player.currentTriggerController.iconColor;
            _iconNotice.enabled = true;
            _iconTalk.enabled = false;
        }
        else
        {
            _iconTalk.enabled = false;
            _iconNotice.enabled = false;
        }
    }
}
