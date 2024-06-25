using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalObject : MonoBehaviour
{
    [SerializeField]
    private bool _useBackground;
    public bool useBackground => _useBackground;

    protected virtual void OnEnable()
    {
        GameUIManager.Instance.RegisterModalItem(this);
    }

    protected virtual void OnDisable()
    {
        GameUIManager.Instance.UnregisterModalItem(this);
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
