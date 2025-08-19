using System.Collections.Generic;
using UnityEngine;
using com;

[CreateAssetMenu]
public class ItemsConfig : ScriptableObject
{
    //global config  not a perticular item
    public List<ItemPrototype> list = new List<ItemPrototype>();
}