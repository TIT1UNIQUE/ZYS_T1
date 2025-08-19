using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button教程 : MonoBehaviour
{
    public AudioSource as_cat;
    public Animator animator_cat;

    public AudioSource as_victory;
    public CanvasGroup cg_victory;

    public void 猫叫声音()
    {
        as_cat.Play();
    }

    public void 猫叫动画()
    {
        animator_cat.SetTrigger("sound");
    }

    public void 胜利声音()
    {
        as_victory.Play();
    }

    public void 胜利界面()
    {
        cg_victory.blocksRaycasts = true;
        cg_victory.DOFade(1, 2).OnComplete(隐藏界面);
    }

    void 隐藏界面()
    {
        cg_victory.alpha = 0;
        cg_victory.blocksRaycasts = false;
    }
}
