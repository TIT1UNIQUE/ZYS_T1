using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TipSystem : MonoBehaviour
{
    public static TipSystem instance;

    public CanvasGroup cg;
    public TMPro.TextMeshProUGUI text;

    void Awake()
    {
        instance = this;
    }

    public void ShowText(string content, bool autoHide)
    {
        Debug.Log(content);
        text.text = content;
        cg.alpha = 0;
        cg.DOKill();
        var t = cg.DOFade(1, 1.5f);

        if (autoHide)
            cg.DOFade(0, 1.5f).SetDelay(3.3f);
    }

    public void HideText()
    {
        //Debug.Log("h");
        cg.DOFade(0, 1.5f);
    }
}