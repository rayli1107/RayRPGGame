using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [field: SerializeField]
    public Sprite sprite { get; private set; }


    [field: SerializeField]
    public int staminaCost { get; private set; }

    [field: SerializeField]
    public float coolDown { get; private set; }

    private bool _isTriggered;

    public void Trigger()
    {
        _isTriggered = true;
    }

    public bool GetAndResetTrigger()
    {
        bool result = _isTriggered;
        _isTriggered = false;
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
