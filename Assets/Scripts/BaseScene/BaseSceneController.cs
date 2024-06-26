using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneController : MonoBehaviour
{
    [field: SerializeField]
    private PlayerProfile _playerProfile;

    [SerializeField]
    private string _startingScene;

    [SerializeField]
    private Vector3 _startingPosition;

    [SerializeField]
    private Quaternion _startingRotation;

    public static BaseSceneController Instance;
    private bool _started;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _started = false;
    }

    private void Update()
    {
        if (!_started &&
            QuestManager.Instance.initialized &&
            GlobalDataManager.Instance.initialized)
        {
            _started = true;
            GlobalDataManager.Instance.NextScenePlayerPosition = _startingPosition;
            GlobalDataManager.Instance.NextScenePlayerRotation = _startingRotation;
            SceneManager.LoadScene(_startingScene);
        }
    }
}
