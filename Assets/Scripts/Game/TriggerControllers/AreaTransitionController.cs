using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransitionController : TriggerController
{
    [SerializeField]
    private Sprite _icon;
    public override Sprite icon => _icon;

    [SerializeField]
    private Color _iconColor;
    public override Color iconColor => _iconColor;

    [SerializeField]
    private string _nextSceneName;
    [SerializeField]
    private string _nextSceneFullName;

    [SerializeField]
    private Vector3 _nextScenePosition;
    [SerializeField]
    private Quaternion _nextSceneRotation;

    public override string message => "To " + _nextSceneFullName;

    public override void Invoke()
    {
        GameController.Instance.TransitionLevel(
            _nextSceneName, _nextScenePosition, _nextSceneRotation);
    }
}
