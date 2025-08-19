using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using com;

public class Floor2System : MonoBehaviour
{
    public static Floor2System instance;

    public const string candleItemId = "candle";

    public GameObject pistolItem;
    public Transform pistolFrom;
    public Transform pistolTo;

    public float pistolMoveDuration;

    public Image web;

    public ParticleSystem candle1Fire;
    public ParticleSystem candle2Fire;
    public ParticleSystem candle3Fire;
    public ParticleSystem candle4Fire;
    public ParticleSystem fire;

    public Image candle1Img;
    public Image candle2Img;//pistolParent is in 2
    public Image candle3Img;
    public Image candle4Img;

    bool _seqStarted;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartPlay();
        pistolItem.transform.position = pistolFrom.position;
    }

    void StartPlay()
    {

    }

    public void OnClickCandle1Slot()
    {
        CheckClickCandleSlot(candle1Img);
    }

    public void OnClickCandle2Slot()
    {
        CheckClickCandleSlot(candle2Img);
    }

    public void OnClickCandle3Slot()
    {
        CheckClickCandleSlot(candle3Img);
    }

    public void OnClickCandle4Slot()
    {
        CheckClickCandleSlot(candle4Img);
    }

    void CheckClickCandleSlot(Image targetImg)
    {
        if (_seqStarted)
            return;

        if (targetImg.gameObject.activeSelf)
            return;

        var crtSelectedInvItemData = InventorySystem.instance.GetCurrentItemData();
        if (crtSelectedInvItemData == null || crtSelectedInvItemData.id != candleItemId)
            return;

        //remove item, unselect if empty
        InventorySystem.instance.RemoveItem(new ItemData(1, candleItemId));
        SoundSystem.instance.Play("candle put in");
        targetImg.gameObject.SetActive(true);
        targetImg.DOFade(1, 1.5f);

        CheckEventStart();
    }

    void CheckEventStart()
    {
        if (_seqStarted)
            return;

        if (candle1Img.gameObject.activeSelf
            && candle2Img.gameObject.activeSelf
              && candle3Img.gameObject.activeSelf
                && candle4Img.gameObject.activeSelf
            )
        {
            StartCoroutine(Seq());
        }
    }

    IEnumerator Seq()
    {
        _seqStarted = true;

        //lock lift
        LiftSystem.instance.lockLift = true;
        Floor5System.instance.webBurned = true;
        //fade candles
        yield return new WaitForSeconds(0.7f);
        candle1Img.DOKill();
        candle2Img.DOKill();
        candle3Img.DOKill();
        candle4Img.DOKill();
        candle1Img.DOFade(0, 1.5f);
        candle2Img.DOFade(0, 1.5f);
        candle3Img.DOFade(0, 1.5f);
        candle4Img.DOFade(0, 1.5f);

        //all candle burn
        yield return new WaitForSeconds(1.1f);
        candle1Fire.Play(true);
        yield return new WaitForSeconds(0.35f);
        candle2Fire.Play(true);
        yield return new WaitForSeconds(0.35f);
        candle3Fire.Play(true);
        yield return new WaitForSeconds(0.35f);
        candle4Fire.Play(true);

        //big fire burn
        yield return new WaitForSeconds(1.2f);
        fire.Play(true);

        //web fade
        yield return new WaitForSeconds(1.5f);
        web.DOFade(0, 3);

        //fire fade
        candle1Fire.Stop(true);
        candle2Fire.Stop(true);
        candle3Fire.Stop(true);
        candle4Fire.Stop(true);
        fire.Stop(true);

        //gun drop
        yield return new WaitForSeconds(0.35f);
        pistolItem.SetActive(true);
        pistolItem.transform.DOMove(pistolTo.position, pistolMoveDuration).SetEase(Ease.InQuad).OnComplete(
            () =>
            {
                SoundSystem.instance.Play("pistol drop");
            }
            );

        yield return new WaitForSeconds(pistolMoveDuration);
        //unlock lift
        LiftSystem.instance.lockLift = false;
    }
}