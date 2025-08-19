using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using com;
using UnityEngine.Rendering.PostProcessing;

public class Floor1System : MonoBehaviour
{
    public static Floor1System instance;
    public GameObject bg;
    public GameObject woodenFace1;
    public GameObject woodenFace2;

    public int correctIndex1;
    public int correctIndex2;
    public int correctIndex3;
    public int correctIndex4;

    public ClickChangeImageQueue icon1;
    public ClickChangeImageQueue icon2;
    public ClickChangeImageQueue icon3;
    public ClickChangeImageQueue icon4;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartPlay();
    }

    void StartPlay()
    {

    }

    public void CheckIcons()
    {
        if (icon1.crtIndex == correctIndex1
            && icon2.crtIndex == correctIndex2
              && icon3.crtIndex == correctIndex3
                && icon4.crtIndex == correctIndex4
            )
        {
            SoundSystem.instance.Play("change");

            woodenFace2.SetActive(true);
            woodenFace2.transform.DOShakePosition(2, 3, 8);
            woodenFace1.SetActive(false);

            icon1.gameObject.SetActive(false);
            icon2.gameObject.SetActive(false);
            icon3.gameObject.SetActive(false);
            icon4.gameObject.SetActive(false);

            bgWithCrackImg.gameObject.SetActive(true);
            bgWithCrackImg.color = new Color(1, 1, 1, 0);
            bgWithCrackImg.DOFade(1, 2);
        }
    }

    public void OnClickKeyHole()
    {
        Debug.Log("OnClickKeyHole");

        var itemData = InventorySystem.instance.GetCurrentItemData();
        if (itemData != null && itemData.id == "key")
        {
            Debug.Log("has key");
            SoundSystem.instance.Play("change");
            InventorySystem.instance.RemoveItem(new ItemData(1, "key"));
            StartCoroutine(EndGameSeq());
        }
    }

    public RectTransform doorLayer1_r;
    public RectTransform doorLayer2_r;
    public RectTransform doorLayer3_r;
    public RectTransform doorLayer1_l;
    public RectTransform doorLayer2_l;
    public RectTransform doorLayer3_l;
    public float doorMoveEndX_r;
    public float doorMoveEndX_l;

    public RectTransform bikeEndPos;
    public GameObject bike;
    public Image bgWithCrackImg;
    public Image darkness;
    IEnumerator EndGameSeq()
    {
        LiftSystem.instance.lockLift = true;
        bg.SetActive(false);
        doorLayer3_r.gameObject.SetActive(true);
        doorLayer3_l.gameObject.SetActive(true);
        doorLayer2_r.gameObject.SetActive(true);
        doorLayer2_l.gameObject.SetActive(true);
        doorLayer1_r.gameObject.SetActive(true);
        doorLayer1_l.gameObject.SetActive(true);

        bgWithCrackImg.DOFade(0, 3);

        woodenFace2.transform.DOShakePosition(3, 5, 8);
        yield return new WaitForSeconds(1);
        woodenFace2.GetComponent<Image>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        bgWithCrackImg.gameObject.SetActive(false);

        //open doors
        doorLayer3_r.DOAnchorPosX(doorMoveEndX_r, 4);
        doorLayer3_l.DOAnchorPosX(doorMoveEndX_l, 4);
        yield return new WaitForSeconds(1.5f); ;

        doorLayer2_r.DOAnchorPosX(doorMoveEndX_r, 4);
        doorLayer2_l.DOAnchorPosX(doorMoveEndX_l, 4);
        yield return new WaitForSeconds(1.5f);

        doorLayer1_r.DOAnchorPosX(doorMoveEndX_r, 4);
        doorLayer1_l.DOAnchorPosX(doorMoveEndX_l, 4);
        yield return new WaitForSeconds(3);

        bike.GetComponent<RectTransform>().DOAnchorPosX(bikeEndPos.anchoredPosition.x, 7);
        bike.transform.DOShakeRotation(7, 1, 18, 70, false);
        yield return new WaitForSeconds(6);

        darkness.gameObject.SetActive(true);
        darkness.DOFade(1, 5);
        Floor5System.instance.vignette.DOColor(Color.white, 2);
        //LiftSystem.instance.lockLift = false;
    }
}