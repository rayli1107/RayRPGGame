using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableIntField
{
    public Action updateAction;

    [SerializeField]
    private int _value;
    public int value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                updateAction?.Invoke();
            }
        }
    }

    public SerializableIntField(int value, Action updateAction)
    {
        _value = value;
        this.updateAction = updateAction;
    }
}

[Serializable]
public class PlayerData
{
    public Action updateAction;

    [SerializeField] public SerializableIntField Level;
    [SerializeField] public SerializableIntField HP;
    [SerializeField] public SerializableIntField Stamina;
    [SerializeField] public SerializableIntField Attack;
    [SerializeField] public SerializableIntField Exp;

    public PlayerData(PlayerProfile profile)
    {
        Level = new SerializableIntField(1, updateAction);
        HP = new SerializableIntField(profile.hp, updateAction);
        Stamina = new SerializableIntField(profile.stamina, updateAction);
        Attack = new SerializableIntField(profile.attack, updateAction);
        Exp = new SerializableIntField(0, updateAction);
    }
}
