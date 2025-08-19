using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using com;

public class Floor4System : MonoBehaviour
{
    public static Floor4System instance;

    public ClickSpeak speaker;
    public ClickSpeak wrongAnswer;
    public ClickSpeak goodAnswer;

    public ClickChangeImageQueue icon1;
    public ClickChangeImageQueue icon2;
    public ClickChangeImageQueue icon3;
    public ClickChangeImageQueue icon4;
    public int correctIndex1;
    public int correctIndex2;
    public int correctIndex3;
    public int correctIndex4;

    public RectTransform itemTrans;
    public Vector2 itemEndPos;
    public float itemDropDuration;
    public GameObject btn;

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

    public void OnClickMan()
    {
        speaker.Speak();
    }

    public void OnClickBtn()
    {
        if (!RightAnswer())
        {
            wrongAnswer.Speak();
            return;
        }

        goodAnswer.Speak();
        SoundSystem.instance.Play("change");
        btn.SetActive(false);

        LiftSystem.instance.lockLift = true;
        itemTrans.DOAnchorPos(itemEndPos, itemDropDuration).SetEase(Ease.InCubic).SetDelay(0.1f).OnComplete(
            () =>
            {
                LiftSystem.instance.lockLift = false;
            }
            );
    }

    bool RightAnswer()
    {
        //Debug.Log(icon1.crtIndex);
        //Debug.Log(icon2.crtIndex);
        //Debug.Log(icon3.crtIndex);
        //Debug.Log(icon4.crtIndex);
        if (icon1.crtIndex == correctIndex1
         && icon2.crtIndex == correctIndex2
         && icon3.crtIndex == correctIndex3
         && icon4.crtIndex == correctIndex4)
        {
            return true;
        }

        return false;
    }
}