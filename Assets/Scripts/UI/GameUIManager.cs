using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelQuickMessage;
    [SerializeField]
    private TextMeshProUGUI _textQuickMessage;

    private float _quickMessageHideTime;

    public static GameUIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_panelQuickMessage.gameObject.activeInHierarchy &&
            Time.time >= _quickMessageHideTime)
        {
            HideQuickMessage();
        }
    }

    public void ShowQuickMessage(string message, float duration = Mathf.Infinity)
    {
        _quickMessageHideTime = Time.time + duration;
        _textQuickMessage.text = message;
        _panelQuickMessage.gameObject.SetActive(true);
    }

    public void HideQuickMessage()
    {
        _panelQuickMessage.gameObject.SetActive(false);
    }
}
