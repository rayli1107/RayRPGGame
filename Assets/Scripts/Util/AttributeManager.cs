using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttributeObject
{
    public Action updateCallback;
    public AttributeProfile profile { get; private set; }

    private int _allocatedPoints;
    public int allocatedPoints
    {
        get => _allocatedPoints;
        set { SetAllocatedPoints(value); updateCallback?.Invoke(); }
    }

    public int value => profile.CalculateValue(allocatedPoints);

    private int _currentIntValue;
    public int currentIntValue
    {
        get => _currentIntValue;
        set { SetCurrentValue(value); updateCallback?.Invoke(); }
    }

    private float _currentFloatValue;
    public float currentFloatValue
    {
        get => _currentFloatValue;
        set { SetCurrentValue(value); updateCallback?.Invoke(); }
    }


    public AttributeObject(AttributeProfile profile)
    {
        this.profile = profile;
        allocatedPoints = 0;
    }

    public void SetAllocatedPoints(int points)
    {
        _allocatedPoints = points;
    }

    public void SetCurrentValue(int currentValue)
    {
        _currentIntValue = Mathf.Clamp(currentValue, 0, value);
    }

    public void SetCurrentValue(float currentValue)
    {
        _currentFloatValue = Mathf.Clamp(currentValue, 0, value);
    }
}

public class AttributeManager : MonoBehaviour
{
    [SerializeField]
    private AttributeProfile[] _attributes;

    private Dictionary<string, AttributeProfile> _attributeMap;

    private void Awake()
    {
/*        foreach (AttributeProfile profile in _attributes)
        {
            _attributeMap.Add(profile.shortName, profile);
        }
*/
    }

    public AttributeProfile GetAttributeProfile(string key)
    {
        return _attributeMap[key];
    }
}