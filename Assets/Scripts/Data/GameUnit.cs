using ScriptableObjects;
using System;
using UnityEngine;

[Serializable]
public class BaseGameUnit
{
    public Action updateAction;

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

    public BaseGameUnit()
    {
    }
}

[Serializable]
public class EnemyGameUnit : BaseGameUnit
{
    public int exp { get; private set; }

    public EnemyGameUnit(EnemyProfile enemyProfile) : base()
    {
        maxHp = enemyProfile.hp;
        hp = enemyProfile.hp;
        attack = enemyProfile.damage;
        exp = enemyProfile.exp;
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

    private int _level;
    public int level
    {
        get => _level;
        set { _level = value; updateAction?.Invoke(); }
    }

    public PlayerGameUnit(PlayerProfile profile) : base()
    {
        maxHp = profile.hp;
        hp = profile.hp;
        maxStamina = profile.stamina;
        stamina = profile.stamina;
        attack = profile.attack;
        level = 1;
    }

}