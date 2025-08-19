using System.Collections;
using com;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClickFeedbackBehaviour : MonoBehaviour
{
    public Image img;
    public Vector3 mousePos;
    Vector3 _anchoredPos;

    void Start()
    {
        var rect = GetComponent<RectTransform>();
        transform.localScale = Vector3.one * 0.06f;
        rect.DOScale(0.24f, 0.5f);
        img.DOFade(0, 0.5f);

        var p = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = p;
        _anchoredPos = rect.anchoredPosition3D;
        _anchoredPos.z = 0;
        rect.anchoredPosition3D = _anchoredPos;
        //rect.anchoredPosition = _anchoredPos;//万万不能只用这句，会让z变成不正确的数字！

        SoundSystem.instance.Play("click");
        Destroy(gameObject, 1);
    }
}
