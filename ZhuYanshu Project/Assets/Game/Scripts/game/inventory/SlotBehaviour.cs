using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotBehaviour : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject view;
    public Image img;
    public TextMeshProUGUI numTxt;
    public GameObject rays;

    public ItemData data { get; private set; }

    protected override void Start()
    {
        base.Start();
        SetEmpty();
    }

    public void SetEmpty()
    {
        //Debug.Log("Hide");
        data = null;
        view.SetActive(false);
        ToggleSelected(false);
        ToggleHover(false);
        numTxt.SetText("");
    }

    public void SetItem(ItemData item)
    {
        data = item;
        if (item == null)
        {
            SetEmpty();
            return;
        }

        var proto = InventorySystem.instance.GetPrototype(item.id);
        //Debug.Log("slot " + item.id);
        img.sprite = proto.sp;
        if (item.n <= 1)
            numTxt.SetText("");
        else
            numTxt.SetText(item.n + "");
        view.SetActive(true);
        ToggleHover(false);
        ToggleSelected(false);
    }

    public void ToggleSelected(bool b)
    {
        rays.SetActive(b);
        if (data != null)
            img.color = b ? Color.grey : Color.white;
    }

    void ToggleHover(bool b)
    {
        if (b)
        {
            if (data == null)
            {
                TipSystem.instance.HideText();
                return;
            }

            var proto = InventorySystem.instance.GetPrototype(data.id);
            TipSystem.instance.ShowText(proto.title, false);
        }
        else
        {
            TipSystem.instance.HideText();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleHover(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventorySystem.instance.SelectSlot(this);
    }
}