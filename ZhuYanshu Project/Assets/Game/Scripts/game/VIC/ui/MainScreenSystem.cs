using com;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenSystem : MonoBehaviour
{
    public CanvasGroup cg_Welcome;
    public CanvasGroup cg_Desktop;
    public CanvasGroup cg_Mail;
    public CanvasGroup cg_App;

    public CanvasGroup cg_popup_warning_closeMail;

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

    private void Start()
    {
        ToggleCanvasGroup(cg_Welcome, false);
        ToggleCanvasGroup(cg_Desktop, false);
        ToggleCanvasGroup(cg_Mail, false);
        ToggleCanvasGroup(cg_App, false);
        ToggleCanvasGroup(cg_popup_warning_closeMail, false);
        openningEmailBtn.enabled = false;
        StartCoroutine(DelayActionIE(2, ShowWelcomeScreen));
    }

    void ShowWelcomeScreen()
    {
        ToggleCanvasGroup(cg_Welcome, true, animationDuration_long);

        StartCoroutine(DelayActionIE(2.7f, HideWelcomeScreenAndShowDesktop));
    }

    IEnumerator DelayActionIE(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    void HideWelcomeScreenAndShowDesktop()
    {
        ToggleCanvasGroup(cg_Desktop, true);
        ToggleCanvasGroup(cg_Welcome, false, 1);

        StartCoroutine(DelayActionIE(3, ShowOpeningMail));
    }

    public RectTransform openningEmail_start;
    public RectTransform openningEmail_end;
    public RectTransform openningEmail;

    public Button openningEmailBtn;

    void ShowOpeningMail()
    {
        openningEmail.anchoredPosition = openningEmail_start.anchoredPosition;
        openningEmail.gameObject.SetActive(true);
        openningEmail.DOAnchorPos(openningEmail_end.anchoredPosition, animationDuration_long).OnComplete(
            () => { openningEmailBtn.enabled = true; }
            );
    }

    public void OnClickEmail()
    {
        openningEmail.DOAnchorPos(openningEmail_start.anchoredPosition, animationDuration_short).OnComplete(ShowEmail);
    }

    void ShowEmail()
    {
        ToggleCanvasGroup(cg_Mail, true);
        StartCoroutine(DelayActionIE(5, Show_popup_warning_closeMail));
    }

    public BlinkCanvasGroup bcg;

    void Show_popup_warning_closeMail()
    {
        ToggleCanvasGroup(cg_popup_warning_closeMail, true, animationDuration_short);
        bcg.enabled = true;
    }

    public void OnClickClose_popup_warning_closeMail()
    {
        bcg.enabled = false;
        ToggleCanvasGroup(cg_Mail, false, 0);
        ToggleCanvasGroup(cg_popup_warning_closeMail, false, animationDuration_mid);
    }

    public void OnClick_AppIcon()
    {
        ToggleCanvasGroup(cg_App, true, animationDuration_short);
    }
}
