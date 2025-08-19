using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class uiButton学习 : MonoBehaviour
{
    public AudioSource as_cat;
    public AudioSource as_victory;
    public Animator animator_cat;
    public CanvasGroup cg_victory;
    public GameObject g;
    public void OnClick_猫叫声音()
    {
        as_cat.Play();
    }

    public void OnClick_胜利声音()
    {
        as_victory.Play();
    }

    public void OnClick_猫叫动画()
    {
        animator_cat.SetTrigger("sound");
    }

    public void OnClick_胜利界面()
    {
        cg_victory.DOFade(1,2).SetEase( Ease.OutCubic).OnComplete(隐藏界面);
    }

    void 隐藏界面()
    {
        cg_victory.alpha=0;
    }

    private void Update()
    {
        
    }
}
