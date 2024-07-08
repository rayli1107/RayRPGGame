using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public virtual string message => "";
    public virtual Sprite icon => null;
    public virtual Color iconColor => Color.white;

    protected PlayerController player => GameController.Instance.player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Invoke()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == player.targetCollider)
        {
            player.RegisterTriggerController(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == player.targetCollider)
        {
            player.UnregisterTriggerController(this);
        }
    }

}
