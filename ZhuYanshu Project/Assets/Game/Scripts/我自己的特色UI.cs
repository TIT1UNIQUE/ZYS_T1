using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 我自己的特色UI : UIBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ICancelHandler
{

    public Sprite sp1;
    public Image image;

    protected override void Start()
    {
        base.Start();
        不变红();
    }

    public void OnCancel(BaseEventData eventData)
    {
        不变红();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.sprite = sp1;
        // throw new System.NotImplementedException("没实现啊");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        变红();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        不变红();
    }

    void 变红()
    {
        image.color = Color.red;
    }

    void 不变红()
    {
        image.color = Color.white;
    }
}
