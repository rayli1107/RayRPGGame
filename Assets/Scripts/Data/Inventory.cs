using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStack
{
    [field: SerializeField]
    public string itemId { get; private set; }

    public int itemCount;

    public ItemStack(string itemId, int itemCount)
    {
        this.itemId = itemId;
        this.itemCount = itemCount;
    }
}

[Serializable]
public class Inventory
{
    [HideInInspector]
    public Action updateAction;

    [field: SerializeField]
    private int _coins;
    public int coins
    {
        get => _coins;
        set
        {
            _coins = value;
            updateAction?.Invoke();
        }
    }

    [field: SerializeField]
    private List<ItemStack> _items;

    public Inventory()
    {
        coins = 0;
        _items = new List<ItemStack>();
    }

    public void AddItem(string itemId, int count)
    {
        ItemStack stack = _items.Find(s => s.itemId == itemId);
        if (stack == null)
        {
            _items.Add(new ItemStack(itemId, count));
        }
        else
        {
            stack.itemCount += count;
        }
        updateAction?.Invoke();
    }

    public int RemoveItem(string itemId, int count)
    {
        ItemStack stack = _items.Find(s => s.itemId == itemId);
        int removed = Mathf.Min(stack.itemCount, count);
        stack.itemCount -= removed;
        if (stack.itemCount <= 0)
        {
            _items.Remove(stack);
        }

        if (removed > 0)
        {
            updateAction?.Invoke();
        }

        return removed;
    }

    public int GetItemCount(string itemId)
    {
        ItemStack stack = _items.Find(s => s.itemId == itemId);
        return stack == null ? 0 : stack.itemCount;
    }
}
