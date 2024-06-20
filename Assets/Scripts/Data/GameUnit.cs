using ScriptableObjects;
using System;
using UnityEngine;

[Serializable]
public class BaseGameUnit
{
    public Action updateAction;

    private string _name;
    public string name
    {
        get => _name;
        set { _name = value; updateAction?.Invoke(); }
    }

    private int _maxHp;
    public int maxHp
    {
        get => _maxHp;
        set {
            _maxHp = value;
            hp = hp;
        }
    }

    private int _hp;
    public int hp
    {
        get => _hp;
        set { _hp = Mathf.Clamp(value, 0, maxHp); updateAction?.Invoke(); }
    }

    private int _attack;
    public int attack
    {
        get => _attack;
        set { _attack = value; updateAction?.Invoke(); }
    }

    public BaseGameUnit(EnemyProfile enemyProfile)
    {
        name = enemyProfile.enemyName;
        maxHp = enemyProfile.hp;
        hp = enemyProfile.hp;
        attack = enemyProfile.damage;
    }

    public BaseGameUnit()
    {
    }
}

[Serializable]
public class PlayerGameUnit : BaseGameUnit
{

    private int _maxStamina;
    public int maxStamina
    {
        get => _maxStamina;
        set { _maxStamina = value; stamina = stamina; }
    }

    private float _stamina;
    public float stamina
    {
        get => _stamina;
        set { _stamina = Mathf.Clamp(value, 0, maxStamina); updateAction?.Invoke(); }
    }

    private int _exp;
    public int exp
    {
        get => _exp;
        set { _exp = value; updateAction?.Invoke(); }
    }

    private int _nextExp;
    public int nextExp
    {
        get => _nextExp;
        set { _nextExp = value; updateAction?.Invoke(); }
    }

    public PlayerGameUnit() : base()
    {
        name = "Hero";
        maxHp = 20;
        hp = 20;
        maxStamina = 20;
        stamina = maxStamina;
        attack = 2;
        exp = 0;
        nextExp = 5;
    }

}