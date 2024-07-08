using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform _panelQuickMessage;

    [SerializeField]
    private TextMeshProUGUI _textQuickMessage;

    [SerializeField]
    private TextMeshProUGUI _textCoin;

    [SerializeField]
    private RectTransform _panelModalBackground;

    [SerializeField]
    private RectTransform _panelPlayerMenu;

    [SerializeField]
    private MessageBoxController _messageBoxController;

    [SerializeField]
    private RectTransform _playerActionHotkeyPanel;

    [SerializeField]
    private PlayerActionUIController _prefabPlayerActionUIController;

    [field: SerializeField]
    public PlayerUIController PlayerHeader { get; private set; }

    public static GameUIManager Instance;

    private List<ModalObject> _modalObjects;
    private float _quickMessageHideTime;
    private float _timeScale;

    private void Awake()
    {
        if (GlobalDataManager.Instance == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Instance = this;
        _modalObjects = new List<ModalObject>();

        for (int i = 0; i < PlayerActionManager.Instance.playerHotkeyActions.Length; ++i)
        {
            PlayerActionUIController controller = Instantiate(
                _prefabPlayerActionUIController, _playerActionHotkeyPanel.transform);
            controller.playerActionIndex = i + 1;
            controller.playerAction = PlayerActionManager.Instance.playerHotkeyActions[i];
            controller.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        if (GlobalDataManager.Instance != null)
        {
            GlobalDataManager.Instance.gameData.inventory.updateAction += onUpdate;
        }
    }

    private void OnDisable()
    {
        if (GlobalDataManager.Instance != null)
        {
            GlobalDataManager.Instance.gameData.inventory.updateAction -= onUpdate;
        }
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

    public void RegisterModalItem(ModalObject modalObject)
    {
        if (_modalObjects.Count == 0)
        {
            _timeScale = Time.timeScale;
            Time.timeScale = 0;
            GameController.Instance.player.playerInput.enabled = false;
        }
        _modalObjects.Add(modalObject);
        _panelModalBackground.gameObject.SetActive(
            _modalObjects.Exists(m => m.useBackground));
    }

    public void UnregisterModalItem(ModalObject modalObject)
    {
        _modalObjects.Remove(modalObject);
        _panelModalBackground.gameObject.SetActive(
            _modalObjects.Exists(m => m.useBackground));
        if (_modalObjects.Count == 0)
        {
            Time.timeScale = _timeScale;
            GameController.Instance.player.playerInput.enabled = true;
        }
    }

    public void ShowMessageBox(
        BaseGameUnitController gameUnit,
        string message,
        MessageBoxHandler handler=null)
    {
        _messageBoxController.gameUnit = gameUnit;
        _messageBoxController.message = message;
        _messageBoxController.messageBoxHandler = handler;
        _messageBoxController.gameObject.SetActive(true);
    }

    private void onUpdate()
    {
        _textCoin.text = GlobalDataManager.Instance.gameData.inventory.coins.ToString();
    }
}
