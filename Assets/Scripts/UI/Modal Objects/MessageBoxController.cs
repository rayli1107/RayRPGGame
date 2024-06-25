using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public delegate void MessageBoxHandler();

public class MessageBoxController : MonoBehaviour
{
    [SerializeField]
    private Image _imageFace;
    [SerializeField]
    private TextMeshProUGUI _textName;
    [SerializeField]
    private TextMeshProUGUI _textMessage;

    private BaseGameUnitController _gameUnit;
    public BaseGameUnitController gameUnit
    {
        get => _gameUnit;
        set
        {
            _gameUnit = value;
            _imageFace.sprite = _gameUnit.face;
            _textName.text = _gameUnit.name;
        }
    }

    public string message
    {
        get => _textMessage.text;
        set { _textMessage.text = value; }
    }

    public MessageBoxHandler messageBoxHandler;

    public PlayerInput playerInput { get; private set; }
    public InputAction actionConfirm { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        actionConfirm = playerInput.actions["Confirm"];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (actionConfirm.WasReleasedThisFrame())
        {
            gameObject.SetActive(false);
            messageBoxHandler?.Invoke();
            messageBoxHandler = null;
        }
    }
}
