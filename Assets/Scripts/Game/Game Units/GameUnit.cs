using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameUnit
{
    public Action updateAction;

    public ClampedIntField HP;
    public ValueField<int> Attack;

    public BaseGameUnit(int hp, int attack)
    {
        HP = new ClampedIntField(hp, onUpdateAction);
        Attack = new ValueField<int>(attack, onUpdateAction);
    }

    protected void onUpdateAction()
    {
        updateAction?.Invoke();
    }
}

public class EnemyGameUnit : BaseGameUnit
{
    public ValueField<int> Exp;
    public ClampedIntField Stagger;
    public ClampedFloatField StaggerDuration;
    public bool IsStaggered => StaggerDuration.value > 0;

    public EnemyGameUnit(EnemyProfile enemyProfile) : base(enemyProfile.hp, enemyProfile.damage)
    {
        Exp = new ValueField<int>(enemyProfile.exp, onUpdateAction);
        Stagger = new ClampedIntField(0, 0, enemyProfile.staggerValue, onUpdateAction);
        StaggerDuration = new ClampedFloatField(0, 0, enemyProfile.staggerDuration, onUpdateAction);
    }

}

public class PlayerGameUnit : BaseGameUnit
{
    public PlayerData playerData { get; private set; }
    public ClampedFloatField Stamina;

    public PlayerGameUnit(PlayerData playerData)
        : base(playerData.HP.value, playerData.Attack.value)
    {
        this.playerData = playerData;
        Stamina = new ClampedFloatField(playerData.Stamina.value, onUpdateAction);
    }

}