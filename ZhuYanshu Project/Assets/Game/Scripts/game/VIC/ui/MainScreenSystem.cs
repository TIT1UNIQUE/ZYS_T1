using Assets.Game.Scripts.game.VIC.ui.Message;
using Assets.Game.Scripts.game.VIC.ui.notif;
using com;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenSystem : MonoBehaviour
{
    public static MainScreenSystem instance;
    public CanvasGroup cg_Welcome;
    public CanvasGroup cg_WelcomeContent;
    public CanvasGroup cg_Desktop;
    public CanvasGroup cg_Mail;
    public CanvasGroup cg_App;

    public float animationDuration_long = 1.5f;
    public float animationDuration_mid = 0.7f;
    public float animationDuration_short = 0.4f;

    public void ToggleCanvasGroup(CanvasGroup cg, bool b, float duration = 0)
    {
        if (b)
        {
            if (duration <= 0)
            {
                cg.alpha = 1;
            }
            else
            {
                cg.DOKill();
                cg.DOFade(1, duration);
            }

            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            if (duration <= 0)
            {
                cg.alpha = 0;
            }
            else
            {
                cg.DOKill();
                cg.DOFade(0, duration);
            }
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public bool testMainGame;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (testMainGame)
        {
            ToggleCanvasGroup(cg_Welcome, false);
            ToggleCanvasGroup(cg_Desktop, true);
            ToggleCanvasGroup(cg_Mail, false);
            ToggleCanvasGroup(cg_App, false);
            openningEmailBtn.enabled = false;
            return;
        }

        ToggleCanvasGroup(cg_Welcome, false);
        ToggleCanvasGroup(cg_Desktop, false);
        ToggleCanvasGroup(cg_Mail, false);
        ToggleCanvasGroup(cg_App, false);
        openningEmailBtn.enabled = false;

        ToggleCanvasGroup(cg_Welcome, true);
        StartCoroutine(DelayActionIE(2, ShowWelcomeScreen));
    }

    void ShowWelcomeScreen()
    {
        com.SoundSystem.instance.Play("ding");

        ToggleCanvasGroup(cg_WelcomeContent, true, animationDuration_long);
        StartCoroutine(DelayActionIE(2.7f, HideWelcomeScreenAndShowDesktop));
    }

    public IEnumerator DelayActionIE(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    void HideWelcomeScreenAndShowDesktop()
    {
        ToggleCanvasGroup(cg_Desktop, true);
        ToggleCanvasGroup(cg_Welcome, false, 1);

        //StartCoroutine(DelayActionIE(3, ShowOpeningMail));
    }

    public void OnClickEmail()
    {
        com.SoundSystem.instance.Play("do");
        ToggleCanvasGroup(cg_Mail, true);
        emailTxt.maxVisibleWords = 6;
        StartCoroutine(ShowEmailWordsIE());
        StartCoroutine(DelayActionIE(6, ShowWarning));
    }

    IEnumerator ShowEmailWordsIE()
    {
        while (emailTxt.maxVisibleWords < 200)
        {
            yield return new WaitForSeconds(0.2f);
            emailTxt.maxVisibleWords = emailTxt.maxVisibleWords + 1;
        }
    }


    public RectTransform warning_pos_start;
    public RectTransform warning_pos_end;
    public RectTransform warningRect;
    public TextMeshProUGUI emailTxt;

    public Button openningEmailBtn;

    public Button openningAppBtn;

    void ShowWarning()
    {
        warningRect.anchoredPosition = warning_pos_start.anchoredPosition;
        warningRect.gameObject.SetActive(true);
        com.SoundSystem.instance.Play("ding");
        warningRect.DOAnchorPos(warning_pos_end.anchoredPosition, animationDuration_long).OnComplete(
            () => { openningEmailBtn.enabled = true; }
            );
    }

    public void OnClickWarning()
    {
        com.SoundSystem.instance.Play("tap");
        warningRect.DOAnchorPos(warning_pos_start.anchoredPosition, animationDuration_short);
        ToggleCanvasGroup(cg_Mail, false, 0);
    }

    public ArrowRingSpawner ars;
    public float openAppDelay = 2.4f;
    public void OnClick_AppIcon()
    {
        StartCoroutine(OpenAppIE());
    }

    IEnumerator OpenAppIE()
    {
        com.SoundSystem.instance.Play("tap");
        openningAppBtn.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.15f, 0.15f, 4, 0.5f);
        openningAppBtn.enabled = false;
        ars.SpawnAllWaves();
        yield return new WaitForSeconds(openAppDelay);
        openningAppBtn.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.25f, 0.2f, 2, 1f);
        yield return new WaitForSeconds(0.2f);
        com.SoundSystem.instance.Play("ding");
        ToggleCanvasGroup(cg_App, true, animationDuration_short);
        MessageSystem.instance.初始化message系统();
    }
}
