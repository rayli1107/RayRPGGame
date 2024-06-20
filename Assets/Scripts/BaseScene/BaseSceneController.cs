using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneController : MonoBehaviour
{
    [SerializeField]
    private string _playerName = "Hero";

    [SerializeField]
    private int _playerStartingHP = 10;

    [SerializeField]
    private string _startingScene;

    [SerializeField]
    private Vector3 _startingPosition;

    [SerializeField]
    private Quaternion _startingRotation;

    [SerializeField]
    private Ray _starting;

    // Start is called before the first frame update
    void Start()
    {
        GlobalGameData.Initialize();
        GlobalGameData.Instance.NextScenePlayerPosition = _startingPosition;
        GlobalGameData.Instance.NextScenePlayerRotation = _startingRotation;
        SceneManager.LoadScene(_startingScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
