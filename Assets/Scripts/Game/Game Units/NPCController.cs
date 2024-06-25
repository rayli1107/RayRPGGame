using ScriptableObjects;
using ScriptedActions;
using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCController : BaseNPCGameUnitController
{
    [field: SerializeField]
    public NPCProfile profile { get; private set; }

    public override Sprite face => profile.face;
    public override string name => profile.name;

    private ScriptedBehaviourEntry[] _behaviourEntries;

    private static int compare(ScriptedBehaviourEntry e1, ScriptedBehaviourEntry e2)
    {
        return e2.priority - e1.priority;
    }
    protected override void Awake()
    {
        base.Awake();
        _behaviourEntries = (ScriptedBehaviourEntry[])profile.behaviourEntries.Clone();
        Array.Sort(_behaviourEntries, compare);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void runAction(ScriptedBehaviourEntry entry, int index)
    {
        Debug.Log("runAction: " + entry + " " + index + " " + entry.actions.Length);
        if (index >= entry.actions.Length)
        {
            return;
        }
        ScriptedActionUtil.Run(
            this, entry.actions[index], () => runAction(entry, index + 1));
    }

    public void RunBehaviour()
    {
        foreach (ScriptedBehaviourEntry entry in _behaviourEntries)
        {
            if (PrerequisiteUtil.Check(entry.prerequisite))
            {
                runAction(entry, 0);
                break;
            }
        }
    }
}
