using System.Collections;
using UnityEngine;
using DG.Tweening;


public class LetterBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Vector2 startPos;
    public Vector2 endPos;

    [Range(0.5f, 5f)]
    public float duration;
    [HideInInspector]
    public Vector3 startEuler;
    [Range(0.2f, 5f)]
    public float delayTimeMax;
    public bool willMove;

    /// <summary>
    /// method方法，函数 function
    /// </summary>
    private void Start()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
        startEuler = transform.eulerAngles;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (willMove)
            {
                float delayTime = Random.Range(0.1f, delayTimeMax);
                //GetComponent<RectTransform>().anchoredPosition = endPos;//等于号是赋值的意思 assign
                //transform.eulerAngles = Vector3.zero;
                transform.DOKill();
                transform.DORotate(Vector3.zero, duration).SetDelay(delayTime + 1f).SetEase(Ease.OutBounce);
                GetComponent<RectTransform>().DOAnchorPos(endPos, duration).SetDelay(delayTime).SetEase(Ease.InOutCubic);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (willMove)
            {
                float delayTime = Random.Range(0.1f, delayTimeMax);
                transform.DOKill();
                transform.DORotate(startEuler, duration).SetDelay(delayTime + 1f).SetEase(Ease.OutBounce);
                GetComponent<RectTransform>().DOAnchorPos(startPos, duration).SetDelay(delayTime).SetEase(Ease.InOutCubic);
            }
        }
    }
}