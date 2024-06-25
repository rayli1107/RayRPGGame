using System.Collections.Generic;
using UnityEngine;

public abstract class SerializedHashSet<TValue> : HashSet<TValue>, ISerializationCallbackReceiver
{
    [SerializeField, HideInInspector]
    private List<TValue> _values;

    public SerializedHashSet() : base()
    {
        _values = new List<TValue>();
    }

    public void OnAfterDeserialize()
    {
        Clear();
        _values.ForEach(e => Add(e));
    }

    public void OnBeforeSerialize()
    {
        _values.Clear();
        foreach (TValue item in this)
        {
            _values.Add(item);
        }
    }
}

public abstract class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField, HideInInspector]
    private List<TKey> _keys;

    [SerializeField, HideInInspector]
    private List<TValue> _values;

    public SerializedDictionary() : base()
    {
        _keys = new List<TKey>();
        _values = new List<TValue>();
    }

    public void OnAfterDeserialize()
    {
        Clear();
        for (int i = 0; i < _keys.Count && i < _values.Count; ++i)
        {
            this[_keys[i]] = _values[i];
        }
    }

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (KeyValuePair<TKey, TValue> kv in this)
        {
            _keys.Add(kv.Key);
            _values.Add(kv.Value);
        }
    }
}