using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using com;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    public ItemsConfig itemsConfig;
    public List<SlotBehaviour> slots;
    private SlotBehaviour _crtSlot;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ClearItems();
        //AddItem("coin");
    }


    public ItemPrototype GetPrototype(string id)
    {
        foreach (var i in itemsConfig.list)
        {
            if (i != null && i.id == id)
                return i;
        }
        return null;
    }

    public void AddItem(ItemData newItem)
    {
        foreach (var s in slots)
        {
            var i = s.data;
            if (i != null && i.id == newItem.id)
            {
                i.n += newItem.n;
                s.SetItem(i);
                SoundSystem.instance.Play(GetPrototype(newItem.id).sfx);
                return;
            }
        }

        foreach (var s in slots)
        {
            if (s.data == null)
            {
                SoundSystem.instance.Play(GetPrototype(newItem.id).sfx);
                s.SetItem(newItem);
                return;
            }
        }

        Debug.LogError("AddItem but no valid slot!");
    }

    public int GetItemCount(string id)
    {
        foreach (var s in slots)
        {
            if (s.data != null && s.data.id == id)
                return s.data.n;
        }

        return 0;
    }

    public void ClearItems()
    {
        foreach (var s in slots)
        {
            s.SetEmpty();
        }
    }

    public bool RemoveItem(ItemData item)
    {
        foreach (var s in slots)
        {
            if (s.data != null && s.data.id == item.id && s.data.n >= item.n)
            {
                s.data.n -= item.n;
                if (s.data.n <= 0)
                {
                    if (_crtSlot == s)
                        _crtSlot = null;
                    s.SetEmpty();
                }
                else
                {
                    s.SetItem(s.data);
                    s.ToggleSelected(_crtSlot == s);
                }

                return true;
            }
        }

        return false;
    }

    public void SelectSlot(SlotBehaviour slot)
    {
        foreach (var s in slots)
        {
            if (s == slot)
            {
                if (s.data != null)
                {
                    _crtSlot = s;
                    s.ToggleSelected(true);
                }
                else
                {
                    _crtSlot = null;
                    s.ToggleSelected(false);
                }

            }
            else
            {
                s.ToggleSelected(false);
            }
        }
    }

    public ItemData GetCurrentItemData()
    {
        if (_crtSlot == null)
            return null;

        return _crtSlot.data;
    }
}