using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using com;

public class Floor5System : MonoBehaviour
{
    public static Floor5System instance;
    public RectTransform jisaw;
    public RectTransform jisawStartPos;
    public RectTransform jisawEndPos;

    public GameObject jisawScene;
    public GameObject[] words;

    bool _canEnterJisawScene;
    bool _canRevealLift;
    bool _hasRevealedLift;

    public Image liftImg;
    public Image doorRightImg;
    public Image doorLeftImg;
    public Image bgImg;

    public GameObject liftControlPanelButton;
    public GameObject liftButtons;
    public GameObject liftBlinker;
    public GameObject exitJisawViewBtn;
    public GameObject bigJisawView;

    public bool webBurned;
    public string pistolId = "pistol";
    public Image jisawImg;
    public Image vignette;
    public GameObject jisawButton;

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
        webBurned = false;
        liftImg.color = Color.black;
        liftControlPanelButton.SetActive(false);
        liftBlinker.SetActive(false);
        jisawScene.SetActive(false);
        liftButtons.SetActive(false);
        exitJisawViewBtn.SetActive(false);

        _canEnterJisawScene = false;
        _canRevealLift = false;
        _hasRevealedLift = false;

        var sequence = DOTween.Sequence(); //Sequence生成
        //Tweenをつなげる
        sequence.AppendCallback(() =>
           {
               jisaw.transform.DOKill();
               jisaw.position = jisawStartPos.position;
           })
            .AppendInterval(1.0f)
            .Append(bgImg.DOColor(Color.white, 1.5f))
            .Append(jisaw.DOAnchorPosX(jisawEndPos.anchoredPosition.x, 7))
            .Join(jisaw.DOShakeRotation(7, 1, 18, 70, false))
            .AppendCallback(() =>
            {
                SoundSystem.instance.Play("trigger");
                _canEnterJisawScene = true;
            })
            .Play();

        /* sequence.Append()
                 .Append(transform.DOMoveX(-7, 1))
                 .Append(transform.DOMoveX(0, 1))
                 //Joinで並列動作するTweenを追加
                 .Join(transform.DORotate(new Vector3(0, 180), 1.5f, RotateMode.WorldAxisAdd)) //0 ~ 1.5
                 .AppendInterval(0.25f)
                 .AppendCallback(() =>
                 {
                     targetRenderer.material.color = Color.cyan;
                 })
                 //前に追加する
                 sequence.PrependInterval(0.5f);
                 sequence.PrependCallback(() => { targetRenderer.material.color = Color.red; });
                 sequence.Prepend(transform.DORotate(new Vector3(0, 0, 180), 1, RotateMode.WorldAxisAdd));

         sequence.Play();
        */
    }

    public void OnClickJiSawScene()
    {
        if (!_canEnterJisawScene)
            return;

        if (webBurned)
        {
            TryKillJisaw();
            return;
        }

        jisawScene.SetActive(true);
        _canEnterJisawScene = false;
        StartJiSawScene_SpeakWords();
    }

    void TryKillJisaw()
    {
        var crtSelectedInvItemData = InventorySystem.instance.GetCurrentItemData();
        if (crtSelectedInvItemData == null || crtSelectedInvItemData.id != pistolId)
            return;

        InventorySystem.instance.RemoveItem(new ItemData(1, pistolId));
        StartCoroutine(KillSeq());
    }

    IEnumerator KillSeq()
    {
        LiftSystem.instance.lockLift = true;
        SoundSystem.instance.Play("shoot");
        jisawImg.rectTransform.DOShakeAnchorPos(3, 10, 10);
        jisawImg.DOColor(Color.red, 0.5f).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(0.5f);
        vignette.DOColor(new Color(0.32f, 0, 0, 1), 0.5f);
        jisawImg.DOColor(Color.white, 0.5f).SetEase(Ease.OutBounce);
        jisawImg.DOFade(0, 1.5f);

        yield return new WaitForSeconds(1.5f);
        jisawButton.SetActive(false);
        jisawImg.gameObject.SetActive(false);
        LiftSystem.instance.lockLift = false;
    }

    public void StartJiSawScene_SpeakWords()
    {
        exitJisawViewBtn.SetActive(false);

        foreach (var w in words)
        {
            w.transform.DOKill();
            w.SetActive(false);
        }

        jisawScene.SetActive(true);
        bigJisawView.transform.DOKill();
        bigJisawView.transform.DOShakePosition(12, 4);

        var sequence = DOTween.Sequence();
        foreach (var w in words)
        {
            sequence.AppendInterval(0.2f);
            sequence.AppendCallback(() =>
            {
                w.SetActive(true);
                w.transform.DOKill();
                w.transform.DOShakePosition(2, 7);
                SoundSystem.instance.Play("show word");
            });
        }
        sequence.AppendCallback(() =>
        {
            _canRevealLift = true;
            //Debug.Log("canRevealLift");
            exitJisawViewBtn.SetActive(true);
        });

        sequence.Play();
    }
    public void OnClickExitJisawScene()
    {
        _canEnterJisawScene = true;
        foreach (var w in words)
            w.SetActive(false);
        SoundSystem.instance.Play("btn ok");
        jisawScene.SetActive(false);

        if (_canRevealLift && !_hasRevealedLift)
            RevealList();
    }

    public void RevealList()
    {
        doorRightImg.DOColor(Color.white, 3.0f);
        doorLeftImg.DOColor(Color.white, 3.0f);

        liftImg.DOColor(Color.white, 2.7f).OnComplete(() =>
        {
            //SoundSystem.instance.Play("ding");
            liftBlinker.SetActive(true);
            liftButtons.SetActive(true);
            liftControlPanelButton.SetActive(true);
        });
    }
}