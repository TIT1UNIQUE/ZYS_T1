using com;
using System.Collections;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem instance;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("debug fast OnClickExitJisawScene");
            Floor5System.instance.OnClickExitJisawScene();
            Floor5System.instance.RevealList();
        }
    }

    public void OnClickTestKey()
    {
        //add item will hide the current displaying tip, so must add item before set tip
        InventorySystem.instance.AddItem(new ItemData(1, "key"));
        TipSystem.instance.ShowText("This is a key, but it is rusty...", true);
    }

    public void OnClickCandle()
    {
        InventorySystem.instance.AddItem(new ItemData(1, "candle"));
        TipSystem.instance.ShowText("I don't like candles", true);
    }

    public void OnClickPistol()
    {
        InventorySystem.instance.AddItem(new ItemData(1, "pistol"));
        TipSystem.instance.ShowText("It has one bullet loaded...", true);
    }
}