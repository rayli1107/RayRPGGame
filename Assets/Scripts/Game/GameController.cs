using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [field: SerializeField]
    public PlayerController player { get;  private set; }

    [field: SerializeField]
    public CinemachineVirtualCamera virtualCamera { get; private set; }

    public static GameController Instance;

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
        if (GlobalDataManager.Instance == null)
        {
            SceneUtil.LoadBaseScene();
            return;
        }

        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
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

        GlobalDataManager.Instance.NextScenePlayerPosition = position;
        GlobalDataManager.Instance.NextScenePlayerRotation = rotation;
        SceneManager.LoadScene(sceneName);
    }
}
