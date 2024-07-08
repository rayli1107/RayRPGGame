using ScriptableObjects;
using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [field: SerializeField]
    public Collider attackHitBox { get; private set; }

    [field: SerializeField]
    public bool isShield { get; private set; }

    [field: SerializeField]
    public bool isTwoHanded { get; private set; }
}
