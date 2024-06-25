using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _prefabPlayer;
    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private CinemachineVirtualCamera _camera;
    [SerializeField]
    private GameUIManager _uiManager;

    public static GameController Instance;

    //    public PlayerController player {get; private set;}
    public PlayerController player => _player;
    public System.Random Random { get; private set; }

    private void Awake()
    {
        Instance = this;
        Random = new System.Random(System.Guid.NewGuid().GetHashCode());
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalGameData.Instance == null)
        {
            SceneUtil.LoadBaseScene();
            return;
        }
        else
        {
            _camera.Follow = player.transform;
            _camera.LookAt = player.transform;
            _player.gameObject.SetActive(true);
            _uiManager.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransitionLevel(
        string sceneName,
        Vector3 position,
        Quaternion rotation)
    {
        GlobalGameData.Instance.NextScenePlayerPosition = position;
        GlobalGameData.Instance.NextScenePlayerRotation = rotation;
        SceneManager.LoadScene(sceneName);
    }
}
