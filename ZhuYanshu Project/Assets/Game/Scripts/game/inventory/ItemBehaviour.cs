using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour
{
    Image img;
    RectTransform rect;
    // Use this for initialization
    void Start()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayFeedback_key()
    {
        img.color = Color.white;
        img.raycastTarget = false;
        img.DOKill();
        rect.DOKill();

        var v = rect.anchoredPosition.y + 25;

        rect.DOAnchorPosY(v, 0.8f).SetEase(Ease.InBack).OnComplete(
            () => { img.DOFade(0, 0.8f); });
    }
}