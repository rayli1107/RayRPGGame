using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ValueField<FieldType>
{
    public Action updateAction;

    private FieldType _value;
    public FieldType value
    {
        get => _value;
        set {
            if (!EqualityComparer<FieldType>.Default.Equals(_value, value))
            {
                _value = value;
                updateAction?.Invoke();
            }
        }
    }

    public ValueField(FieldType value, Action updateAction)
    {
        _value = value;
        this.updateAction = updateAction;
    }
}

public abstract class AbstractClampedField
{
    public Action updateAction;

    protected int _minValue { get; private set; }
    public int minValue
    {
        get => _minValue;
        set { unsafeSetMinValue(value); }
    }

    protected int _maxValue { get; private set; }
    public int maxValue
    {
        get => _maxValue;
        set { unsafeSetMaxValue(value); }
    }

    public AbstractClampedField(int minValue, int maxValue, Action updateAction)
    {
        this.updateAction = updateAction;
        _minValue = minValue;
        _maxValue = maxValue;
    }

    protected abstract void clampValue();

    private void unsafeSetMinValue(int value)
    {
        if (_minValue != value) 
        {
            return;
        }

        _minValue = value;
        _maxValue = Mathf.Max(_minValue, _maxValue);
        clampValue();
        updateAction?.Invoke();
    }

    private void unsafeSetMaxValue(int value)
    {
        if (_maxValue != value)
        {
            return;
        }

        _maxValue = value;
        _minValue = Mathf.Min(_minValue, _maxValue);
        clampValue();
        updateAction?.Invoke();
    }
}

public class ClampedIntField : AbstractClampedField
{
    private int _value;
    public int value
    {
        get => _value;
        set { unsafeSetIntValue(value); }
    }

    public ClampedIntField(int value, int minValue, int maxValue, Action updateAction)
        : base(minValue, maxValue, updateAction)
    {
        _value = value;
    }

    public ClampedIntField(int value, Action updateAction)
       : this(value, 0, value, updateAction)
    {
    }

    protected override void clampValue()
    {
        _value = Mathf.Clamp(_value, _minValue, _maxValue);
    }

    private void unsafeSetIntValue(int value)
    {
        if (_value != value)
        {
            _value = value;
            clampValue();
            updateAction?.Invoke();
        }
    }
}

public class ClampedFloatField : AbstractClampedField
{
    private float _value;
    public float value
    {
        get => _value;
        set { unsafeSetFloatValue(value); }
    }

    public ClampedFloatField(float value, int minValue, int maxValue, Action updateAction)
        : base(minValue, maxValue, updateAction)
    {
        _value = value;
    }

    public ClampedFloatField(int value, Action updateAction)
       : this(value, 0, value, updateAction)
    {
    }

    protected override void clampValue()
    {
        _value = Mathf.Clamp(_value, _minValue, _maxValue);
    }

    private void unsafeSetFloatValue(float value)
    {
        if (_value != value)
        {
            _value = value;
            clampValue();
            updateAction?.Invoke();
        }
    }
}